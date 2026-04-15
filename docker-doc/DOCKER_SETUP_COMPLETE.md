# Digitalmarket Business API - Docker Setup

## ✅ Build Status
Image successfully built and tested: `digitalmarket-business:latest`

## 🚀 Quick Start

### Build Image
```powershell
# From solution root directory
cd C:\Users\MIC\OneDrive\Documents\Code\hddung\digitalmarket_be_v2

# Build using script
.\build-docker.ps1

# Or build directly
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:latest .
```

### Run Container
```powershell
# Simple run
docker run -d -p 8080:8080 -v C:\path\to\Config:/Config --name business-api digitalmarket-business:latest

# Using script
.\build-docker.ps1 -Run

# With custom port
.\build-docker.ps1 -Run -Port 9000
```

### Access Application
```
http://localhost:8080
```

## 📋 Dockerfile Features

- **Multi-Stage Build** (4 stages): restore → build → publish → runtime
- **Optimized Cache**: Restore layer cached separately
- **Flexible**: Add/remove references without modifying Dockerfile
- **Secure**: Runs as non-root user
- **Lean**: 365MB runtime image (aspnet:8.0 base)
- **Config Ready**: Auto-includes Config folder
- **Health Check**: Built-in health check endpoint
- **Volume Mount**: Supports Config override at runtime

## 📦 Image Size
```
digitalmarket-business:latest   365MB
```

## 🔧 Configuration

### Config Folder
The application requires a `Config/` folder with:
- `connection.json` - Database connection string
- `elastic.json` - Elasticsearch configuration
- Other configuration files as needed

Auto-included from source, can be overridden at runtime:
```powershell
docker run -d -p 8080:8080 -v /host/Config:/Config --name business-api digitalmarket-business:latest
```

## 📝 Files Changed/Created

1. **Presentation/API/Digitalmarket.Controller.Business/Dockerfile**
   - Completely rewritten with multi-stage approach
   - Auto-flexible for add/remove references
   - Includes Config folder support

2. **DOCKER_BUILD_GUIDE.md**
   - Comprehensive build guide
   - Usage examples
   - Troubleshooting

3. **build-docker.ps1**
   - PowerShell helper script
   - Supports build and run
   - Config folder mount support

## 🎯 Key Points

✅ **No More Manual Updates**: When you add/remove NuGet references, Dockerfile automatically handles it  
✅ **Fast Rebuilds**: Restore layer caching means faster subsequent builds  
✅ **Production Ready**: Security hardened, non-root user, health checks  
✅ **Volume Ready**: Config can be mounted from host or container  

## 🔍 Testing

All tests passed:
- ✅ Image builds without errors
- ✅ Container starts and runs successfully
- ✅ Application logs show "ready and running"
- ✅ Health check responds correctly
- ✅ Port 8080 accessible
- ✅ Config folder properly resolved

## 📚 Additional Resources

- Full guide: See `DOCKER_BUILD_GUIDE.md`
- PowerShell script: Use `.\build-docker.ps1 -?` for help
- Docker docs: https://docs.docker.com

## 🎓 What Changed from Original

| Aspect | Before | After |
|--------|--------|-------|
| Project References | Hard-coded | Auto-detected |
| Cache Optimization | Limited | Separate restore layer |
| Add/Remove References | Requires Dockerfile edit | Automatic |
| Config Handling | Manual | Auto-included |
| Security | Root user | Non-root (app:app) |
| Health Check | None | Integrated |
| Runtime Size | Similar | Lean aspnet:8.0 |

## 💡 Tips

1. **First build takes longer** (~2-3 minutes) - dependencies are downloaded
2. **Subsequent builds are faster** (~10-20 seconds) - restore layer is cached
3. **Monitor container**:
   ```powershell
   docker logs -f business-api
   docker stats business-api
   ```

4. **Use version tags in production**:
   ```powershell
   .\build-docker.ps1 -ImageTag "1.0.0"
   docker push myregistry.azurecr.io/digitalmarket-business:1.0.0
   ```

---

**Status**: ✅ Ready for Production  
**Date**: 2025-03-28  
**Tested**: Docker Build, Container Run, App Startup
