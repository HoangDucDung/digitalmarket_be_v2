# Thiết kế Cơ sở Dữ liệu & Luồng Xác thực (Auth Flow) - Marketplace Sản phẩm số

Tài liệu này chi tiết cấu trúc bảng dữ liệu người dùng và luồng xác thực cho nền tảng Digital Marketplace.

---

## 1. Cấu trúc Bảng dữ liệu (Database Schema)

### 1.1. Bảng `Users` (Thông tin định danh)
Lưu trữ thông tin cơ bản và công khai của người dùng.

| Trường | Kiểu dữ liệu | Mô tả | Ràng buộc |
| :--- | :--- | :--- | :--- |
| `Id` | `UUID` / `BIGINT` | Khóa chính | PK |
| `Username` | `VARCHAR(50)` | Tên đăng nhập / Slug profile | Unique, Index |
| `Email` | `VARCHAR(100)` | Email liên hệ & đăng nhập | Unique, Index |
| `FullName` | `NVARCHAR(100)` | Tên đầy đủ hiển thị | |
| `AvatarUrl` | `VARCHAR(255)` | Ảnh đại diện | |
| `Bio` | `NVARCHAR(500)` | Giới thiệu ngắn | |
| `CreatedAt` | `DATETIME` | Ngày tạo tài khoản | Default: Now |

### 1.2. Bảng `UserCredentials` (Xác thực)
Tách biệt để bảo mật, chỉ truy cập khi cần đăng nhập/đổi pass.

| Trường | Kiểu dữ liệu | Mô tả | Ràng buộc |
| :--- | :--- | :--- | :--- |
| `UserId` | `FK` | Tham chiếu tới `Users.Id` | PK, FK |
| `PasswordHash` | `VARCHAR(255)` | Mật khẩu đã băm (BCrypt/Argon2) | |
| `IsEmailVerified`| `BOOLEAN` | Trạng thái xác thực Email | Default: False |
| `TwoFactorSecret`| `VARCHAR(255)` | Khóa bí mật cho 2FA | Nullable |
| `LastLoginAt` | `DATETIME` | Lần đăng nhập cuối | |

### 1.3. Bảng `UserKycProfiles` (Pháp lý - Cho Seller)
Dành cho người dùng muốn bán hàng hoặc rút tiền.

| Trường | Kiểu dữ liệu | Mô tả | Ghi chú |
| :--- | :--- | :--- | :--- |
| `UserId` | `FK` | Tham chiếu `Users.Id` | PK, FK |
| `DocNumber` | `VARCHAR(50)` | Số CCCD/Passport | Mã hóa (At rest) |
| `FrontImgUrl` | `VARCHAR(255)` | Ảnh mặt trước | Private Storage |
| `BackImgUrl` | `VARCHAR(255)` | Ảnh mặt sau | Private Storage |
| `Status` | `VARCHAR(20)` | `Pending`, `Approved`, `Rejected` | |

### 1.4. Bảng `Roles` & `Permissions` (Phân quyền)
- **Roles**: `Admin`, `Seller`, `Buyer`.
- **UserRoles**: Bảng trung gian n-n giữa `Users` và `Roles`.

---

## 2. Luồng Xác thực (Authentication Flow)

### 2.1. Đăng ký tài khoản (Sign Up)
1. **Client**: Gửi `Email`, `Username`, `Password`.
2. **Server**:
   - Kiểm tra trùng lặp `Email`/`Username`.
   - Hash mật khẩu bằng `BCrypt`.
   - Lưu vào bảng `Users` và `UserCredentials`.
   - Gán Role mặc định là `Buyer`.
   - Tạo Verify Token và gửi Email xác nhận.
3. **Client**: Nhận mã qua Email và confirm.

### 2.2. Đăng nhập (Sign In) & JWT
Sử dụng Access Token (ngắn hạn) và Refresh Token (dài hạn).

1. **Client**: Gửi `Email`/`Password`.
2. **Server**:
   - Truy vấn `UserCredentials` theo `Email`.
   - Kiểm tra `PasswordHash`.
   - Nếu đúng: 
     - Tạo **Access Token** (Chứa: `UserId`, `Role`, `Email`) - Hết hạn sau 15-60p.
     - Tạo **Refresh Token** (Lưu vào DB/Redis) - Hết hạn sau 7-30 ngày.
   - Trả về cặp Token cho Client.
3. **Client**: Lưu Token vào `LocalStorage` hoặc `HttpOnly Cookie`.

### 2.3. Luồng làm mới Token (Refresh Token)
Khi Access Token hết hạn (401 Unauthorized):
1. **Client**: Gửi Refresh Token lên endpoint `/v1/auth/refresh`.
2. **Server**:
   - Kiểm tra Refresh Token trong DB/Redis có tồn tại và còn hạn không.
   - Nếu hợp lệ: Cấp Access Token mới.
   - Nếu không: Yêu cầu đăng nhập lại (Logout).

---

## 3. Quy tắc Bảo mật Dữ liệu (PII Security)

1. **Mã hóa (Encryption)**: Số thẻ ngân hàng, số định danh cá nhân (CCCD) phải được mã hóa AES-256 trước khi lưu xuống Database.
2. **Hiding ID**: Không dùng ID tự tăng (`1, 2, 3...`) ra bên ngoài API. Sử dụng `UUID` hoặc `HashID`.
3. **Soft Delete**: Không bao giờ xóa bản ghi người dùng khỏi DB. Sử dụng `IsDeleted` hoặc bảng `UserStatuses` để vô hiệu hóa tài khoản.
4. **Rate Limiting**: Giới hạn số lần thử login sai từ một IP để chống Brute-force.
