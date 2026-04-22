using System.Linq.Expressions;

namespace Project.DigitalMarket.Domain.Repositories.Base
{
    /// <summary>
    /// Interface cấu trúc các hàm CRUD cơ bản cho Repository Pattern
    /// </summary>
    /// <typeparam name="TEntity">Loại Entity, yêu cầu là một class</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// Lấy một bản ghi theo khóa chính (Id)
        /// </summary>
        /// <param name="id">Giá trị của khóa chính</param>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        /// Thêm mới một bản ghi vào database
        /// </summary>
        /// <param name="entity">Thực thể cần thêm</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Thêm mới nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="entities">Danh sách thực thể cần thêm</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Cập nhật thông tin một bản ghi
        /// </summary>
        /// <param name="entity">Thực thể mang dữ liệu mới</param>
        void Update(TEntity entity);

        /// <summary>
        /// Cập nhật thông tin nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="entities">Danh sách thực thể mang dữ liệu mới</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Xóa một bản ghi khỏi database
        /// </summary>
        /// <param name="entity">Thực thể cần xóa</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="entities">Danh sách thực thể cần xóa</param>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Lưu các thay đổi vào database
        /// </summary>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        Task<int> SaveChangesAsync();
    }
}
