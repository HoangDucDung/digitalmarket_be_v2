# Hướng Dẫn Sử Dụng Base CRUD Repository với Entity Framework Core

Tài liệu này hướng dẫn cách sử dụng kiến trúc Base CRUD sử dụng Generic Repository Pattern bằng Entity Framework Core trong dự án `Project.DigitalMarket`.

## 1. Cấu trúc thư mục

Kiến trúc Base CRUD được chia thành 2 layer chính:

- **Domain Layer (`Project.DigitalMarket.Domain`)**: Chứa định nghĩa các thực thể (Entities) và Interfaces (Hợp đồng/Hàng rào giao tiếp). Không phụ thuộc vào Entity Framework.
  - `BaseEntity.cs`: Chứa các trường chung như `Id`, `CreatedAt`, v.v.
  - `IRepositoryBase.cs`: Interface định nghĩa các hàm CRUD chuẩn.
- **Infrastructure Layer (`Project.DigitalMarket.Infrastructure`)**: Chứa logic query Database thực tế. Phụ thuộc vào Entity Framework Core.
  - `RepositoryBase.cs`: Class thực thi các hàm được định nghĩa ở `IRepositoryBase` thông qua `DigitalMarketDbContext`.

---

## 2. Cách tạo một bảng/Entity mới

Khi bạn tạo một bảng mới trong hệ thống (ví dụ: `Product`), bạn làm theo các bước sau:

**Bước 1:** Tạo Entity và kế thừa `BaseEntity` (trong project `Domain`).

```csharp
using Project.DigitalMarket.Domain.Entities.Base;

namespace Project.DigitalMarket.Domain.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
```

_(Ghi chú: Nếu là Entity dùng cho MS Identity như User, role thì không cần kế thừa `BaseEntity` vì chúng đã có các class IdentityUser riêng)._

**Bước 2:** Định nghĩa interface Repository (trong project `Domain`).

```csharp
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Products
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        // Có thể bổ sung thêm hàm đặc thù cho Product ở đây nếu CRUD cơ bản không đáp ứng đủ
        Task<ProductEntity?> GetProductByNameAsync(string name);
    }
}
```

**Bước 3:** Triển khai Repository (trong project `Infrastructure`).

```csharp
using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Products;
using Project.DigitalMarket.Infrastructure.Data;
using Project.DigitalMarket.Infrastructure.Repositories.Base;

namespace Project.DigitalMarket.Infrastructure.Repositories.Products
{
    public class ProductRepository : RepositoryBase<ProductEntity>, IProductRepository
    {
        public ProductRepository(DigitalMarketDbContext context) : base(context)
        {
        }

        public async Task<ProductEntity?> GetProductByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}
```

**Bước 4:** Thêm DbSet vào `DigitalMarketDbContext.cs`

```csharp
public DbSet<ProductEntity> Products { get; set; }
```

**Bước 5:** Đăng ký Dependency Injection (DI) (trong `Startup` hoặc file Factory, vd `AppCoreFactory.cs`).

```csharp
services.AddScoped<IProductRepository, ProductRepository>();
```

---

## 3. Cách gọi hàm lấy dữ liệu trong Service / Manager

Nhờ việc kế thừa `RepositoryBase`, bạn không cần phải viết lại các hàm Thêm, Xóa, Sửa nữa.
Ví dụ lấy tất cả Product hoặc Thêm Product:

```csharp
public class ProductManager
{
    private readonly IProductRepository _productRepository;

    public ProductManager(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task CreateProductAsync(ProductEntity product)
    {
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync(); // Update xuống Database
    }

    public IQueryable<ProductEntity> GetActiveProducts()
    {
         // Gọi hàm GetByCondition đã có sẵn
         return _productRepository.GetByCondition(x => !x.IsDeleted);
    }
}
```