# Docker Commands Cheat Sheet - Digitalmarket Business API

## Build Commands

```powershell
# Using PowerShell script (recommended)
.\build-docker.ps1
.\build-docker.ps1 -ImageTag "1.0.0"
.\build-docker.ps1 -ImageTag "latest" -Run
.\build-docker.ps1 -Run -Port 9000

# Direct docker build
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:latest .
docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business:1.0.0 .
```

## Run Commands

```powershell
# Basic run
docker run -d -p 8080:8080 --name business-api digitalmarket-business:latest

# With Config volume
docker run -d -p 8080:8080 \
  -v C:\path\to\Config:/Config \
  --name business-api digitalmarket-business:latest

# With custom port
docker run -d -p 9000:8080 --name business-api digitalmarket-business:latest

# With environment variables
docker run -d -p 8080:8080 \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  -e "ConnectionStrings__DefaultConnection=Server=.;Database=digitalmarket;" \
  --name business-api digitalmarket-business:latest

# Interactive (useful for debugging)
docker run -it -p 8080:8080 --name business-api digitalmarket-business:latest

# Detached with auto-remove
docker run -d --rm -p 8080:8080 --name business-api digitalmarket-business:latest
```

## Container Management

```powershell
# View logs
docker logs business-api
docker logs -f business-api          # Follow logs (real-time)
docker logs --tail 50 business-api   # Last 50 lines

# Check container status
docker ps                            # Running containers
docker ps -a                         # All containers
docker ps | grep business-api        # Find business-api

# Check container stats (CPU, memory)
docker stats business-api
docker stats business-api --no-stream

# Stop/Start container
docker stop business-api
docker start business-api
docker restart business-api

# Remove container
docker rm business-api
docker rm $(docker ps -aq)           # Remove all containers

# Interactive shell in container
docker exec -it business-api sh
docker exec -it business-api bash
docker exec business-api ls -la /Config

# Copy files from/to container
docker cp business-api:/Config/connection.json ./connection.json.backup
docker cp ./connection.json business-api:/Config/
```

## Image Management

```powershell
# View images
docker images
docker images | grep digitalmarket-business

# Image size
docker images --format "table {{.Repository}}\t{{.Tag}}\t{{.Size}}"

# Inspect image
docker inspect digitalmarket-business:latest

# Tag image
docker tag digitalmarket-business:latest digitalmarket-business:1.0.0
docker tag digitalmarket-business:latest myregistry.azurecr.io/digitalmarket-business:latest

# Push to registry
docker push myregistry.azurecr.io/digitalmarket-business:1.0.0

# Remove image
docker rmi digitalmarket-business:latest
docker rmi $(docker images -q)       # Remove all images

# Build with no cache
docker build --no-cache -t digitalmarket-business:latest -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile .
```

## Network & Debugging

```powershell
# Test endpoint
curl http://localhost:8080
curl http://localhost:8080/health
curl http://localhost:8080/api/test-logs

# View network
docker network ls
docker network inspect bridge

# Port mapping
docker port business-api

# View events
docker events --filter "container=business-api"
```

## Docker Compose

```yaml
# docker-compose.yml
version: '3.8'
services:
  business-api:
    image: digitalmarket-business:latest
    ports:
      - "8080:8080"
    volumes:
      - ./Config:/Config
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=digitalmarket;User Id=sa;Password=YourPassword;
```

```powershell
# Compose commands
docker-compose up -d                 # Start
docker-compose down                  # Stop & remove
docker-compose logs -f              # View logs
docker-compose ps                   # Status
```

## Cleanup

```powershell
# Remove stopped containers
docker container prune

# Remove dangling images
docker image prune

# Remove unused volumes
docker volume prune

# Full cleanup (containers, images, networks, volumes)
docker system prune -a

# Check disk usage
docker system df
```

## Troubleshooting

```powershell
# View container processes
docker top business-api

# Inspect container details
docker inspect business-api
docker inspect --format '{{.State.Status}}' business-api

# Health status
docker inspect --format '{{.State.Health.Status}}' business-api

# View exposed ports
docker port business-api

# Check network connectivity
docker exec business-api ping 8.8.8.8
docker exec business-api curl http://localhost:8080/health

# Export container to image
docker commit business-api digitalmarket-business-snapshot:latest

# Save image to file
docker save digitalmarket-business:latest -o business-api.tar

# Load image from file
docker load -i business-api.tar
```

## Environment Variables (at runtime)

```powershell
docker run -d -p 8080:8080 \
  -e "ASPNETCORE_ENVIRONMENT=Production" \
  -e "ASPNETCORE_URLS=http://+:8080" \
  -e "ConnectionStrings__DefaultConnection=Server=...;Database=...;" \
  -e "Logging__LogLevel__Default=Information" \
  --name business-api digitalmarket-business:latest
```

## Common Issues

```powershell
# Port already in use
docker run -d -p 9000:8080 --name business-api digitalmarket-business:latest

# Container exits immediately
docker logs business-api  # Check error

# Out of disk space
docker system prune -a

# Permission denied
# Run PowerShell as Administrator

# Cannot connect to Docker daemon
# Ensure Docker Desktop is running
```

---

**Tip**: Save this file for quick reference during development and deployment!
