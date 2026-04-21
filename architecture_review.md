# Đánh Giá Kiến Trúc Dự Án: DigitalMarket BE v2

Dựa trên việc đọc mã nguồn, cấu hình project (`.csproj`) và luồng Dependency Injection (DI) của toàn bộ dự án, tôi xin đưa ra đánh giá tổng thể như sau:

Dự án đang hướng đến việc xây dựng theo mô hình **Clean Architecture** kết hợp với **Microservices** (Tách biệt hai dịch vụ độc lập là `Auth` và `Business`). Tuy nhiên, trong quá trình triển khai, dự án đã vi phạm một số nguyên tắc cốt lõi của cả Clean Architecture và thiết kế Microservices.

Dưới đây là 5 điểm chưa hợp lý lớn nhất kèm theo giải thích và phương án khắc phục:

---

## 1. Vi phạm nghiêm trọng: Rò rỉ Infrastructure vào tầng Domain (Domain Dependency Injection Anti-Pattern)

> [!WARNING]
> Tầng **Domain** đang bị phụ thuộc vào Entity Framework Core (Database) - đi ngược lại triết lý cốt lõi của Clean Architecture.

**Giải thích chi tiết:**
- Trong Clean Architecture, tầng `Domain` là trung tâm của kiến trúc, nó chỉ chứa logic nghiệp vụ thuần túy và **tuyệt đối không** được phụ thuộc vào công nghệ truy xuất dữ liệu như ORM (Entity Framework) hay Framework web. 
- Tuy nhiên, file `Project.DigitalMarket.Domain.csproj` lại đang trực tiếp `PackageReference` thư viện `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.
- Bên trong lớp `ProductManager` (thuộc tầng Domain), code đang sử dụng các hàm của EF Core như `EF.Functions.Like()` và gọi `.ToListAsync()`. Điều này xảy ra do Design Pattern bị lỗi: `IProductRepository` đang trả về một `IQueryable<T>`. Trả về `IQueryable` cho phép các tầng phía trên (Domain/Application) tự ý viết các câu queries (chẳng hạn như `.Where()`), khiến toàn bộ business logic bị trói buộc chết vào Entity Framework.

**Phương án khắc phục:**
1. Xóa `Microsoft.AspNetCore.Identity.EntityFrameworkCore` khỏi project `Domain`. Di chuyển các Entity liên quan đến Identity sang project `Infrastructure`.
2. Sửa lại các interface Repository: **Tuyệt đối không** trả về `IQueryable<T>`. Hãy trả về `IEnumerable<T>`, `IReadOnlyList<T>` hoặc `Task<List<T>>`.
3. Trong các hàm của Repository, hãy nhận vào các tham số đại diện cho tiêu chí lọc (Ví dụ: truyền vào `Limit`, `Offset`, `Keyword`) và xử lý câu `Where` ngay trong `Infrastructure.MsSql`.

---

## 2. Dependency Leakage: Web API (Presentation) gọi trực tiếp Infrastructure (done)

> [!IMPORTANT]
> Tầng Web API đang tham chiếu mạnh tới Infrastructure làm phá vỡ sự cô lập của kiến trúc.

**Giải thích chi tiết:**
- Theo lý thuyết, Presentation -> Application -> Domain. Infrastructure là vòng ngoài cùng giao tiếp với file/DB. Tầng Application (và Presentation) chỉ thông qua các Interface chứa trong Domain/Application để nói chuyện với DB mà không hề biết lớp thực thi (về sau).
- Hiện tại project `Digitalmarket.Controller.Base` (thuộc tầng Presentation) lại có `<ProjectReference>` trực tiếp đến `Project.DigitalMarket.Infrastructure.MsSql` và `Project.DigitalMarket.Infrastructure.Mail`. Sự liên kết này phục vụ cho cục `AppCoreFactory` thực hiện Dependency Injection (DI).
- Việc Dependency như vậy khiến project `Controller.Base` kéo theo thư viện Infrastructure, làm cho tất cả các class Controller kế thừa base có khả năng truy suất trực tiếp `DbContext` (hay các Entity Db), vượt mặt được Application layer.

**Phương án khắc phục:**
1. Chuyển phần đăng ký DI của Database, Mail... từ `Controller.Base` sang một Extension Methods (như `AddInfrastructureLayer()`) thuộc dự án `Infrastructure`.
2. Ở file `Program.cs` của tầng API cao nhất chỉ gọi các ServiceCollection Extension mà tuyệt đối không đưa các project Infrastructure vào project Shared UI / Share Web Base.

---

## 3. Kiến trúc Microservices lai tạp Monolith (Coupled Microservices)

> [!CAUTION]
> Dịch vụ Auth và Business được build riêng nhưng đang chia sẻ chung một "Nồi lẩu" Dependency Injection.

**Giải thích chi tiết:**
- Bạn có 2 API là `Digitalmarket.Controller.Auth` và `Digitalmarket.Controller.Business` (cả 2 đều có file `Program.cs` riêng rẽ). Cách tách API này cho thấy tư duy thiết kế Microservices.
- Nhưng cả hai dự án này đều dùng chung class `AppCoreFactory` nằm trong thư viện `Controller.Base`.
- Trong `AppCoreFactory.cs`, chứa cả hàm `UseAppAuthenFactory()` và `UseAppBussinessFactory()`.
- Hậu quả: Khi bạn chạy Microservice `Auth` hoặc `Business`, chúng có thể gọi lẫn vào logic khởi tạo DI của nhau, làm tính Module/Microservice không triệt để. Code của microservice này đang tham chiếu logic/repository của microservice kia. Điều này tạo nên kiến trúc "Distributed Monolith".

**Phương án khắc phục:**
1. Gỡ bỏ `AppCoreFactory` khỏi `Controller.Base`.
2. Chuyển khai báo DI của Authen trực tiếp về Setup trong `Program.cs` của API `Digitalmarket.Controller.Auth`.
3. Chuyển khai báo DI của Business trực tiếp về Setup trong `Program.cs` của API `Digitalmarket.Controller.Business`.

---

## 4. Over-layering: Tạo ra quá nhiều lớp trung gian dư thừa (done)

> [!NOTE]
> Flow luân chuyển thay vì 3 lớp đang bị tách nhỏ thành 4 lớp: Controller -> Service -> Manager -> Repository.

**Giải thích chi tiết:**
- Lấy ví dụ ở tính năng Product: Controller gọi vào `ProductService` (trong Application). `ProductService` sau khi xử lý mapping `Dto` xong lại ném Object vào `ProductManager` (nằm trong Domain). `ProductManager` cuối cùng mới là thằng gọi `ProductRepository` của database.
- Tầng Domain của bạn chứa `ProductManager` nhưng `ProductManager` này đang code giống hệt Application Service: gọi Database Repository, filter data, lấy result, tính discount. Lớp này hoàn toàn không giữ vai trò là "Domain Service" như lý thuyết DDD mà chỉ là lớp thao túng DB cồng kềnh.
- Cấu trúc làm dự án phình to ra, làm 1 tác vụ nhỏ Developer cũng phải setup 4 files và mapping DTO 3 lần thay vì 2.

**Phương án khắc phục:**
1. Gộp logic của `ProductManager` vào luôn `ProductService` thuộc tần Application. Dùng Application Service để điều phối (Orchestration) và tương tác với Databse thông qua Repository Interface.
2. Tầng `Domain` chỉ nên giữ thuần Entity Models và các Pure Logic Classes (như xử lý chuỗi Slug) hoặc Domain Event Models.

---

## 5. Sai định dạng loại dự án (SDK Type) cho project Library

> [!WARNING]
> Project sinh ra để làm thư viện dùng chung nhưng lại mang cấu hình SDK Web Server.

**Giải thích chi tiết:**
- Dự án `Digitalmarket.Controller.Base` đơn giản chỉ là một "Shared Library" chứa các Class ControllerBase, API config dùng chung cho 2 web API thực sự (`Auth` và `Business`) tham chiếu tới.
- Nhưng trong `Digitalmarket.Controller.Base.csproj` lại có định nghĩa `<Project Sdk="Microsoft.NET.Sdk.Web">` và bên trong có file `Program.cs`. ASP.NET sẽ hiểu dự án Base cũng là 1 WebApp. Hậu quả là có rủi ro các API endpoints sinh ra lặp nhau hoặc routing trỏ nhầm.

**Phương án khắc phục:**
1. Đổi dòng đầu của `.csproj` từ `<Project Sdk="Microsoft.NET.Sdk.Web">` thành `<Project Sdk="Microsoft.NET.Sdk">` trong `Controller.Base`.
2. Bổ sung thẻ `<FrameworkReference Include="Microsoft.AspNetCore.App" />` để có thể sử dụng các class như `ControllerBase`.
3. Xóa file `Program.cs` và các file `appsettings.json` trong dự án Base đi vì Library Base bản chất không phải là app chạy độc lập.
