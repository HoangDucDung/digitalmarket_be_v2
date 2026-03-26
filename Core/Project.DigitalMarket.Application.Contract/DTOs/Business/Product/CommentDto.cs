using System;
using System.Collections.Generic;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class CreateCommentReqDto
    {
        public Guid ProductId { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class CommentResDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserAvatar { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public List<string>? ImageUrls { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
