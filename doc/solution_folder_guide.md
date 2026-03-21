# Hướng dẫn: Điều hướng Project vào Solution Folder trong Visual Studio

## 1. Solution Folder là gì?

Solution Folder là **folder ảo** trong file `.sln` để **tổ chức giao diện** trong Solution Explorer. Nó **KHÔNG** tạo folder thực trên ổ đĩa.

```
Solution Explorer (VS2022)          Folder thực trên ổ đĩa
├── Core/                           ├── Core/
│   └── Project.Domain              │   └── Project.Domain/
├── Infrastructure/                 ├── Infrastructure/
│   └── Project.Infrastructure      │   └── Project.Infrastructure/
└── Presentation/                   └── Presentation/
    └── API/                            └── API/
        └── Controller.Auth                 └── Controller.Auth/
```

## 2. Cơ chế hoạt động trong file .sln

File `.sln` có **3 phần chính** liên quan đến việc tổ chức project:

### 2.1. Khai báo Solution Folder

```
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "TênFolder", "TênFolder", "{GUID-CỦA-FOLDER}"
EndProject
```

- GUID `2150E333-8FDC-42A3-9474-1A3956D46DE8` = **luôn cố định**, đánh dấu đây là Solution Folder
- GUID cuối cùng = **ID duy nhất** của folder này

### 2.2. Khai báo C# Project

```
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TênProject", "đường\dẫn\tới\file.csproj", "{GUID-CỦA-PROJECT}"
EndProject
```

- GUID `FAE04EC0-301F-11D3-BF4B-00C04F79EFBC` = **luôn cố định**, đánh dấu đây là C# project
- GUID cuối = **ID duy nhất** của project

### 2.3. NestedProjects — Quy định project nằm trong folder nào

```
GlobalSection(NestedProjects) = preSolution
    {GUID-CỦA-PROJECT} = {GUID-CỦA-FOLDER-CHA}
EndGlobalSection
```

**Đây là phần quan trọng nhất!** Mỗi dòng mapping:

- **Bên trái** `=` : GUID của project (hoặc sub-folder)
- **Bên phải** `=` : GUID của folder cha mà nó nằm trong

## 3. Ví dụ thực tế trong project DigitalMarket

```
// Khai báo folder "Infrastructure" với GUID {7DC331D0-...}
Project("{2150E333-...}") = "Infrastructure", "Infrastructure", "{7DC331D0-3EC4-40A6-69FB-1CBEB2E0D1EA}"
EndProject

// Khai báo project Infrastructure với GUID {24ECFF31-...}
Project("{FAE04EC0-...}") = "Project.DigitalMarket.Infrastructure", "Infrastructure\...\....csproj", "{24ECFF31-CDEA-4317-9119-6D34B3F585B5}"
EndProject

// ĐẶT project VÀO folder:
GlobalSection(NestedProjects) = preSolution
    {24ECFF31-...} = {7DC331D0-...}   ← Infrastructure project nằm trong Infrastructure folder
EndGlobalSection
```

## 4. Cách thêm project vào solution folder

### Cách 1: Dùng CLI (dotnet sln)

```bash
# Thêm project vào solution (đặt ở root)
dotnet sln add "path/to/Project.csproj"

# Thêm project vào solution folder cụ thể
dotnet sln add "path/to/Project.csproj" --solution-folder "Infrastructure"
```

> ⚠️ **Lưu ý:** `dotnet sln add` không có `--solution-folder` sẽ đặt project ở **root** hoặc **folder sai**. Cần dùng flag `--solution-folder` hoặc sửa tay file `.sln`.

### Cách 2: Dùng Visual Studio 2022

1. Click chuột phải vào **Solution Folder** trong Solution Explorer
2. Chọn **Add → Existing Project...**
3. Chọn file `.csproj`

### Cách 3: Sửa tay file .sln

1. Mở file `.sln` bằng text editor
2. Tìm GUID của project và GUID của folder đích
3. Thêm/sửa dòng trong `GlobalSection(NestedProjects)`:
   ```
   {GUID-PROJECT} = {GUID-FOLDER-ĐÍCH}
   ```

## 5. Lỗi thường gặp

| Lỗi                         | Nguyên nhân                                     | Cách sửa                   |
| --------------------------- | ----------------------------------------------- | -------------------------- |
| Project không hiện trong VS | Thiếu dòng trong `NestedProjects` hoặc GUID sai | Kiểm tra GUID mapping      |
| Project nằm sai folder      | `NestedProjects` trỏ sai GUID folder            | Sửa GUID bên phải `=`      |
| Folder hiện nhưng rỗng      | Project khai báo rồi nhưng chưa nest            | Thêm dòng `NestedProjects` |

## 6. Tóm tắt GUID cần nhớ

| Loại            | GUID cố định                             |
| --------------- | ---------------------------------------- |
| Solution Folder | `{2150E333-8FDC-42A3-9474-1A3956D46DE8}` |
| C# Project      | `{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}` |
