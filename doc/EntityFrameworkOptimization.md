# Entity Framework Core Optimization Guidelines

Tài liệu này tóm tắt các chiến lược tối ưu hóa khi làm việc với Entity Framework Core để cải thiện hiệu suất RAM, CPU và tốc độ truy vấn.

## 1. Sử dụng `.AsNoTracking()`

- **Mục đích**: Tắt bộ theo dõi thay đổi (Change Tracker) của EF Core.
- **Lợi ích**: Giảm đáng kể mức chiếm dụng RAM và CPU khi thực hiện các tác vụ chỉ đọc (Read-only). Khi không cần cập nhật dữ liệu, việc bỏ qua bộ đệm theo dõi giúp tăng tốc độ xử lý câu lệnh.

## 2. Sử dụng `IQueryable<T>` và Deferred Execution

- **Cơ chế**: Tận dụng cơ chế trì hoãn thực thi (Deferred Execution).
- **Lợi ích**: Thay vì lấy toàn bộ dữ liệu về RAM (In-memory filtering), EF Core sẽ dịch câu lệnh sang SQL và chỉ gửi những gì thực sự cần thiết xuống database. Quá trình lọc dữ liệu diễn ra trực tiếp tại SQL Server.

## 3. Nhóm các câu lệnh với `SaveChangesAsync()`

- **Cơ chế**: Giải thích về gom nhóm (Batching) các câu lệnh `INSERT`, `UPDATE`, `DELETE` thành một khối duy nhất.
- **Lợi ích**: Giảm thiểu số lượng Round-trip (kết nối mạng) giữa ứng dụng và SQL Server, giúp thực hiện nhiều thay đổi trong một giao dịch duy nhất một cách hiệu quả.

## 4. Tận dụng Generic Type Constraints

- **Cách dùng**: Sử dụng `where TEntity : class` trong các Repository hoặc Service.
- **Lợi ích**: Giúp code an toàn hơn, tránh các lỗi kiểu dữ liệu ngay từ lúc biên dịch (Compile-time) và cho phép trình biên dịch tối ưu hóa các thao tác trên Object.

## 5. Chiến lược Soft Delete (Xóa mềm)

- **Cơ chế**: Kết hợp giữa thuộc tính `BaseEntity.IsDeleted` và các hàm lọc dữ liệu như `GetByCondition`.
- **Lợi ích**: Bảo vệ toàn vẹn dữ liệu, cho phép khôi phục khi cần thiết và ngăn chặn việc mất dữ liệu vĩnh viễn trong database. SQL queries sẽ tự động lọc ra các bản ghi có `IsDeleted = true`.
