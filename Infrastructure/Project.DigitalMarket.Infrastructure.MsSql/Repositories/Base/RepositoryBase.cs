using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Repositories.Base;
using Project.DigitalMarket.Infrastructure.MsSql.Data;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base
{
    /// <summary>
    /// Lớp triển khai các hàm CRUD cơ bản bằng Entity Framework Core
    /// </summary>
    /// <typeparam name="TEntity">Thực thể Database (Entity)</typeparam>
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ILazyloadProvider _lazyloadProvider;
        internal DigitalMarketDbContext _context => _lazyloadProvider.LazyGetRequiredService<DigitalMarketDbContext>();
        protected DbSet<TEntity> _dbSet => _context.Set<TEntity>();

        protected RepositoryBase(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách bản ghi và tắt tracking (AsNoTracking) để tối ưu hiệu năng
        /// </summary>
        protected virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        /// <summary>
        /// Lấy danh sách bản ghi thỏa mãn điều kiện nhất định, tắt tracking
        /// </summary>
        protected virtual IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Tìm một bản ghi theo Id (Khóa chính)
        /// </summary>
        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Thêm bản ghi mới
        /// </summary>
        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Thêm nhiều bản ghi mới cùng lúc
        /// </summary>
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi (đánh dấu state là Modified)
        /// </summary>
        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Cập nhật thông tin của nhiều bản ghi
        /// </summary>
        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Xóa bản ghi (đánh dấu state là Deleted)
        /// </summary>
        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Xác nhận và lưu trữ toàn bộ thay đổi xuống Database
        /// </summary>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
