# 🚀 QUICK REFERENCE - Both APIs

## Build Commands

```powershell
# Business API
.\build-docker.ps1                                    # Build latest
.\build-docker.ps1 -ImageTag "1.0.0"                # Build v1.0.0
.\build-docker.ps1 -Run                              # Build & Run

# Auth API
.\build-auth-docker.ps1                              # Build latest
.\build-auth-docker.ps1 -ImageTag "1.0.0"           # Build v1.0.0
.\build-auth-docker.ps1 -Run -Port 8081             # Build & Run on 8081
```

## Run Both APIs

```powershell
# Terminal 1
.\build-docker.ps1 -Run
# → Business API on http://localhost:8080

# Terminal 2 (new PowerShell window)
.\build-auth-docker.ps1 -Run -Port 8081
# → Auth API on http://localhost:8081
```

## Docker Commands

```powershell
# View both images
docker images | grep digitalmarket

# Check running containers
docker ps | grep digitalmarket

# View logs
docker logs -f business-api
docker logs -f auth-api

# Stop both
docker stop business-api auth-api

# Remove both
docker rm business-api auth-api
```

## Direct Docker Build (if not using scripts)

```powershell
# Business
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:latest .

# Auth
docker build -f Presentation/API/Digitalmarket.Controller.Auth/Dockerfile -t digitalmarket-auth:latest .
```

## Direct Docker Run (if not using scripts)

```powershell
# Business on 8080
docker run -d -p 8080:8080 -v ./Config:/Config --name business-api digitalmarket-business:latest

# Auth on 8081
docker run -d -p 8081:8080 -v ./Config:/Config --name auth-api digitalmarket-auth:latest
```

## Access APIs

```
Business API: http://localhost:8080
Auth API:     http://localhost:8081
```

## Status Check

```powershell
# See all containers
docker ps -a

# See image sizes
docker images | grep digitalmarket

# Check container logs
docker logs business-api | head -20
docker logs auth-api | head -20

# Monitor in real-time
docker stats business-api auth-api
```

## Cleanup

```powershell
# Stop all
docker stop $(docker ps -q)

# Remove all stopped containers
docker container prune

# Remove all dangling images
docker image prune

# Full cleanup
docker system prune -a
```

## Troubleshooting

```powershell
# Container not starting?
docker logs business-api
docker logs auth-api

# Port already in use?
.\build-auth-docker.ps1 -Run -Port 9090

# Want to restart?
docker restart business-api
docker restart auth-api

# Interactive shell?
docker exec -it business-api sh
docker exec -it auth-api sh
```

## Features

✅ Auto-detect dependencies (add/remove references = no rebuild!)  
✅ Optimized caching (fast rebuilds)  
✅ Security hardened (non-root user)  
✅ Config auto-included + volume mount support  
✅ Health checks integrated  
✅ Production-ready  

## Documentation

- `BOTH_APIS_READY.md` - Full overview
- `DOCKER_README.md` - Getting started
- `DOCKER_BUILD_GUIDE.md` - Detailed guide
- `DOCKER_CHEATSHEET.md` - All commands

---

**Status**: ✅ READY TO USE  
**Both APIs**: 🟢 OPERATIONAL
