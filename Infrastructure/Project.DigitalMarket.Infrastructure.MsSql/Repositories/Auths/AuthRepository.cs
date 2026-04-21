using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Auths
{
    internal sealed class AuthRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<UserEntity>(lazyloadProvider), IAuthRepository
    {
        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserEntity?> GetUserByUserIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> RegisterAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveRefreshTokenAsync(Guid userId, string refreshToken)
        {
            // Tạm thời chỉ return true nếu repo chưa hỗ trợ lưu refresh token thực sự (cần Add column)
            // Hoặc thực hiện logic lưu token tại đây.
            return true;
        }
    }
}
