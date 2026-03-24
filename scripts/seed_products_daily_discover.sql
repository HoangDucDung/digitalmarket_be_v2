/*
Seed dữ liệu mẫu cho bảng Products phục vụ API daily-discover.
Chạy script này sau khi đã migrate tạo bảng Products.
*/

SET NOCOUNT ON;

DECLARE @now DATETIME2 = SYSUTCDATETIME();

;WITH SeedData AS
(
    SELECT *
    FROM (VALUES
        ('ao-thun-nam-basic', N'Ao thun nam basic cotton 100%', 'https://cdn.example.com/p1.jpg', N'Shop Basic VN', N'Ho Chi Minh', 'VND', 149000.00, 99000.00, 34, 1800, CAST(4.80 AS DECIMAL(3,2)), 'daily_discover_main', 'Active', 1),
        ('quan-jean-nu-ong-rong', N'Quan jean nu ong rong form Han', 'https://cdn.example.com/p2.jpg', N'Shop Denim House', N'Ha Noi', 'VND', 399000.00, 329000.00, 18, 920, CAST(4.70 AS DECIMAL(3,2)), 'fashion', 'Active', 1),
        ('tai-nghe-bluetooth-5-3', N'Tai nghe bluetooth 5.3 pin 40h', 'https://cdn.example.com/p3.jpg', N'Gadget Pro', N'Da Nang', 'VND', 690000.00, 499000.00, 28, 2500, CAST(4.90 AS DECIMAL(3,2)), 'electronics', 'Active', 1),
        ('may-say-toc-mini', N'May say toc mini gap gon du lich', 'https://cdn.example.com/p4.jpg', N'HomeCare Official', N'Can Tho', 'VND', 320000.00, 249000.00, 22, 670, CAST(4.60 AS DECIMAL(3,2)), 'home_living', 'Active', 0),
        ('sua-rua-mat-tra-xanh', N'Sua rua mat tra xanh diu nhe 120ml', 'https://cdn.example.com/p5.jpg', N'Beauty Hub', N'Ho Chi Minh', 'VND', 189000.00, 139000.00, 26, 4300, CAST(4.85 AS DECIMAL(3,2)), 'beauty', 'Active', 1),
        ('sach-tu-duy-phan-bien', N'Sach Tu duy phan bien cho nguoi moi bat dau', 'https://cdn.example.com/p6.jpg', N'Book Store 247', N'Ha Noi', 'VND', 120000.00, 89000.00, 25, 540, CAST(4.75 AS DECIMAL(3,2)), 'books', 'Active', 0),
        ('giay-the-thao-runner-x', N'Giay the thao Runner X dem hoi sieu nhe', 'https://cdn.example.com/p7.jpg', N'Sport City', N'Ho Chi Minh', 'VND', 990000.00, 799000.00, 19, 1100, CAST(4.65 AS DECIMAL(3,2)), 'fashion', 'Active', 1),
        ('ban-phim-co-rgb', N'Ban phim co RGB switch do hot-swap', 'https://cdn.example.com/p8.jpg', N'PC Gear VN', N'Ha Noi', 'VND', 1290000.00, 999000.00, 23, 860, CAST(4.78 AS DECIMAL(3,2)), 'electronics', 'Active', 0)
    ) AS X
    (
        Slug, Name, ImageUrl, ShopName, ShopLocation, Currency, OriginalPrice, SalePrice, DiscountPercent, SoldCount, RatingAverage, CategoryBundle, Status, IsFeatured
    )
)
INSERT INTO [dbo].[Products]
(
    [Id],
    [SellerId],
    [CategoryId],
    [BrandId],
    [Name],
    [Slug],
    [ImageUrl],
    [ShopName],
    [ShopLocation],
    [Currency],
    [OriginalPrice],
    [SalePrice],
    [DiscountPercent],
    [SoldCount],
    [RatingAverage],
    [CategoryBundle],
    [Status],
    [PublishedAt],
    [IsActive],
    [IsFeatured],
    [CreatedAt],
    [CreatedBy],
    [UpdatedAt],
    [UpdatedBy],
    [IsDeleted]
)
SELECT
    NEWID() AS Id,
    NEWID() AS SellerId,
    NULL AS CategoryId,
    NULL AS BrandId,
    s.Name,
    s.Slug,
    s.ImageUrl,
    s.ShopName,
    s.ShopLocation,
    s.Currency,
    CAST(s.OriginalPrice AS DECIMAL(18,2)),
    CAST(s.SalePrice AS DECIMAL(18,2)),
    s.DiscountPercent,
    s.SoldCount,
    s.RatingAverage,
    s.CategoryBundle,
    s.Status,
    DATEADD(DAY, -1, @now) AS PublishedAt,
    1 AS IsActive,
    s.IsFeatured,
    @now AS CreatedAt,
    'seed-script' AS CreatedBy,
    NULL AS UpdatedAt,
    NULL AS UpdatedBy,
    0 AS IsDeleted
FROM SeedData s
WHERE NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Products] p
    WHERE p.[Slug] = s.[Slug]
);

SELECT @@ROWCOUNT AS InsertedRows;
