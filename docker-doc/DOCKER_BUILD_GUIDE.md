# Docker Build Guide - Digitalmarket Business API

## Quick Start

### 1. Build Image (từ root của solution)
```powershell
# Build image
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:latest .

# Hoặc với tag version
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:1.0.0 .
```

### 2. Run Container
```powershell
# Run với port default (8080)
docker run -d -p 8080:8080 --name business-api digitalmarket-business:latest

# Run với custom Config folder (mount volume)
docker run -d -p 8080:8080 \
  -v C:\path\to\Config:/Config \
  --name business-api digitalmarket-business:latest

# Run với environment variables
docker run -d -p 8080:8080 \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  -e "ConnectionStrings__DefaultConnection=Server=your-server;Database=your-db;User Id=sa;Password=your-password;" \
  --name business-api digitalmarket-business:latest
```

### 3. Test Container
```powershell
# Check logs
docker logs business-api

# Check container status
docker ps | grep business-api

# Test endpoint
curl http://localhost:8080/health
# or
curl http://localhost:8080/api/test-logs

# Stop container
docker stop business-api

# Remove container  
docker rm business-api
```

## Dockerfile Architecture

### Multi-Stage Build (4 stages)
1. **restore** - Copy .csproj files và restore dependencies (tối ưu cache)
2. **build** - Copy full source, build project (Release configuration)
3. **publish** - Publish project thành executable
4. **runtime** - ASP.NET runtime slim, chỉ chứa published files + Config

### Key Features
✅ **Flexible** - Khi add/remove references, không cần sửa Dockerfile  
✅ **Optimized Cache** - Restore layer riêng biệt, code changes không invalidate restore cache  
✅ **Secure** - Chạy dưới non-root user (app:app)  
✅ **Health Check** - Tích hợp health check endpoint  
✅ **Lean Runtime** - Dùng aspnet:8.0 runtime (không cần SDK)  
✅ **Config Aware** - Tự động copy Config folder từ source  
✅ **Volume Ready** - Hỗ trợ mount Config folder custom tại runtime  

### Directory Structure
```
/work/app/           ← Application DLL and dependencies
/Config/             ← Configuration files (connection.json, elastic.json, etc)
```

## Troubleshooting

### Build Failed: "project not found"
→ Đảm bảo build context là **root của solution** (nơi có `digitalmarket_be_v2.sln`)
```powershell
# ✅ Đúng
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business .

# ❌ Sai
cd Presentation/API/Digitalmarket.Controller.Business
docker build -f Dockerfile -t digitalmarket-business .
```

### Build Failed: "NuGet restore error"
→ Kiểm tra:
- Internet connection
- NuGet server availability
- Local build trước: `dotnet restore`

```powershell
dotnet restore
```

### Container starts but exits immediately
→ Kiểm tra logs
```powershell
docker logs business-api
```

### "DirectoryNotFoundException: /Config/"
→ Config folder được copy vào image tự động  
→ Nếu cần custom Config, mount volume:
```powershell
docker run -d -p 8080:8080 \
  -v C:\Users\MIC\OneDrive\Documents\Code\hddung\digitalmarket_be_v2\Config:/Config \
  --name business-api digitalmarket-business
```

### Health check fails
→ Kiểm tra API endpoint trong code, hoặc disable health check khi test:
```powershell
docker run -d -p 8080:8080 --no-healthcheck --name business-api digitalmarket-business:latest
```

## Performance Tips

1. **Local Build Test** trước Docker:
```powershell
dotnet restore
dotnet build -c Release
dotnet publish -c Release -o .\publish
```

2. **Check Image Size**:
```powershell
docker images | grep digitalmarket-business
```

3. **Layer Caching**:
   - Đợi restore layer cache (lần đầu sẽ lâu ~1-2 phút)
   - Những lần sau (nếu dependencies không đổi) chỉ rebuild code layers (~10 segundos)

4. **Monitor Container**:
```powershell
docker stats business-api
```

## Advanced Usage

### Run with Docker Compose
```yaml
version: '3.8'
services:
  business-api:
    image: digitalmarket-business:latest
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=digitalmarket;User Id=sa;Password=YourPassword;
    volumes:
      - ./Config:/Config
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=YourPassword
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
```

Run with compose:
```powershell
docker-compose up -d
```

### Push to Container Registry
```powershell
# Tag for registry
docker tag digitalmarket-business:latest myregistry.azurecr.io/digitalmarket-business:1.0.0

# Push
docker push myregistry.azurecr.io/digitalmarket-business:1.0.0
```

## Environment Variables (Production)
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
ConnectionStrings__DefaultConnection=<your-connection-string>
Logging__LogLevel__Default=Information
```

## Update Notes

**Current Version**: v2.0
- ✅ Flexible Dockerfile (add/remove references = no rebuild needed)
- ✅ Multi-stage optimized build
- ✅ Config folder auto-included
- ✅ Health check integrated
- ✅ Non-root user security
- ✅ Volume mount support for Config override
