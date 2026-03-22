# Hướng Dẫn Sử Dụng Source Code Digital Market BE v2

Tài liệu này hướng dẫn cách cấu hình, chạy và sử dụng Source Code của dự án Digital Market BE v2.

## 1. Cấu Trúc Dự Án (Clean Architecture)

Dự án được xây dựng theo chuẩn **Clean Architecture** để đảm bảo tính mở rộng và dễ bảo trì:
- **Core (Application & Domain)**: Chứa các interface (Contract), DTOs, Entities và logic nghiệp vụ lõi (Services/Managers). Không phụ thuộc vào bất kỳ framework bên ngoài nào.
- **Infrastructure (MsSql & Mail)**: File hạ tầng kết nối ra bên ngoài.
  - `MsSql`: Entity Framework Core, DbContext, Migrations và Repositories.
  - `Mail`: Triển khai các dịch vụ ngoại vi như gửi Email qua SMTP (sử dụng MailKit).
- **Presentation (API)**: Cung cấp các Endpoint RESTful API thông qua Controller. Đây là Gateway cho Client.

## 2. Quy Chuẩn Code (Coding Conventions)

Để thống nhất cách phát triển tính năng mới, chuẩn hóa kiến trúc DDD và Clean Architecture, đội ngũ cần tuân thủ các quy tắc sau:
- **Mô Hình DDD & Clean Architecture**:
  - **Luồng xử lý**: Controller => `Core/Project.DigitalMarket.Application` => `Core/Project.DigitalMarket.Domain`.
  - **Application (Business Logic)**: Chứa các nghiệp vụ business của dự án. Khai báo Interface tại `Project.DigitalMarket.Application.Contract` và thực thi (Implement) tại `Project.DigitalMarket.Application`.
  - **Domain (Core Entity Logic)**: Chứa các nghiệp vụ lõi xoay quanh Entity. Khai báo Interface và thực thi tại `Core/Project.DigitalMarket.Domain` (thư mục Services hoặc Managers).
  - **Thao tác Database**: Khai báo Interface tại `Repositories` (thuộc Domain), thực thi (Query thực tế) tại `Infrastructure/Project.DigitalMarket.Infrastructure.MsSql`.
- **Khai Báo Cấu Hình (Config)**: 
  - Thêm class ánh xạ tại `Lib/Project.DigitalMarket.Host.Base`.
  - Project `Core/Project.DigitalMarket.Domain.Share` khai báo các interface cấu hình.
- **Khởi Tạo Service**: 
  - Sử dụng **`ILazyloadProvider`** (phương thức `LazyGetRequiredService<T>()`) để resolve động các service thay vì inject qua constructor.

## 3. Yêu Cầu Hệ Thống

- **.NET 8.0 SDK** (hoặc mới hơn).
- **SQL Server** (LocalDB hoặc SQL Server độc lập).
- Môi trường IDE: Visual Studio 2022, Rider hoặc VS Code.

## 4. Cấu Hình Dự Án

Các cấu hình chính nằm trong thư mục `Config/` (được nạp tự động qua `Program.cs`):

### `Config/connection.json`
Định nghĩa chuỗi kết nối Database.
```json
{
  "ConnectionString": {
    "SqlServer": "Server=YOUR_SERVER;Database=DigitalMarket;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
  }
}
```

### `Config/auth.json`
Chứa thông tin cấu hình JWT (Json Web Token).
```json
{
  "AuthConfig": {
    "SecretKey": "chuoi-bi-mat-dai-hon-32-ky-tu",
    "Issuer": "Issuer-Name",
    "Audience": "Audience-Name",
    "ExpiresTime": 6000,
    "RefreshTokenTime": 60000
  }
}
```

### `Config/Email.json`
Cấu hình giao thức gửi Email SMTP (Dùng cho kích hoạt tài khoản, 2FA, OTP).
```json
{
  "EmailConfig": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your_email@gmail.com",
    "SmtpPass": "password_ung_dung_gmail",
    "FromEmail": "no-reply@digitalmarket.com",
    "FromName": "Digital Market Admin"
  }
}
```
*Lưu ý: Đối với Gmail, bạn cần bật Xác minh 2 bước và tạo "Mật khẩu ứng dụng" (App Password) để gắn vào `SmtpPass`.*

## 5. Chạy Migrations và Cập Nhật Database

Mở Terminal / Package Manager Console tại thư mục root của solution:

```bash
# 1. Trỏ vào thư mục MsSql infrastructure
cd Infrastructure/Project.DigitalMarket.Infrastructure.MsSql

# 2. Add Migration mới (nếu bạn có chỉnh sửa Entities)
dotnet ef migrations add InitialCreate --startup-project ../../Presentation/API/Digitalmarket.Controller.Auth

# 3. Cập nhật Database
dotnet ef database update --startup-project ../../Presentation/API/Digitalmarket.Controller.Auth
```

## 6. Chạy Dự Án

- Mở solution `digitalmarket_be_v2.sln` bằng Visual Studio.
- Thiết lập project `Digitalmarket.Controller.Auth` làm **Startup Project**.
- Nhấn `F5` hoặc nút Run để khởi chạy.
- Trình duyệt sẽ tự động mở Swagger UI (vd: `https://localhost:71xx/swagger`) để bạn test các endpoint API.

## 7. Tính Năng Chính
- **Auth Flow**: Hỗ trợ Đăng ký (Register), Đăng nhập (Login), Xác thực Email (Verify), và cấu hình/sử dụng xác thực 2 bước (2FA).
- Cấu trúc module hoá chặt chẽ giúp dễ dàng scale ứng dụng ra Microservices nếu cần trong tương lai.
