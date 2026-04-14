using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Infrastructure.MsSql.Data;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class FileController(ILazyloadProvider lazyloadProvider) : DigitalBaseController<FileController>(lazyloadProvider)
    {
        protected DigitalMarketDbContext _context => _lazyloadProvider.LazyGetRequiredService<DigitalMarketDbContext>();

        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { error = "No files" });

            var results = new List<object>();

            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());

                var entity = new FileEntity
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Base64Data = base64,
                    FileSize = file.Length
                };

                _context.Files.Add(entity);
                await _context.SaveChangesAsync();

                results.Add(new
                {
                    id = entity.Id,
                    fileName = entity.FileName,
                    size = entity.FileSize,
                    url = $"{Request.Scheme}://{Request.Host}/api/files/{entity.Id}"
                });
            }

            return Ok(results);
        }
        // Lấy file gốc (binary) - dùng cho download hoặc hiển thị ảnh
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null) return NotFound();

            var bytes = Convert.FromBase64String(file.Base64Data);
            return File(bytes, file.ContentType, file.FileName);
        }

        // Lấy info + base64 (cho Vue.js dùng trực tiếp)
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetFileInfo(Guid id)
        {
            var file = await _context.Files
                .Select(f => new { f.Id, f.FileName, f.ContentType, f.FileSize, f.UploadedAt, f.Base64Data })
                .FirstOrDefaultAsync(f => f.Id == id);

            if (file == null) return NotFound();

            return Ok(new
            {
                file.Id,
                file.FileName,
                file.ContentType,
                file.FileSize,
                file.UploadedAt,
                dataUrl = $"data:{file.ContentType};base64,{file.Base64Data}"
            });
        }

    }
}
