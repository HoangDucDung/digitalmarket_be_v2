# ============================================
# Dockerfile - Digitalmarket Business API (Microservice)
# Build context: root của solution (../../..)
# Usage: docker build -f Presentation/API/Digitalmarket.Controller.Business/Dockerfile -t digitalmarket-business .
# ============================================

# ── Stage 1: Restore ──
# Separate restore stage để tận dụng Docker layer cache
# Khi code thay đổi nhưng dependencies không đổi, restore layer được cache
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src

# Copy solution file
COPY digitalmarket_be_v2.sln ./

# Copy toàn bộ .csproj files (recursive copy để tìm tất cả projects)
# Điều này đảm bảo dotnet restore có thể resolve tất cả dependencies
# Không cần thêm/xóa .csproj paths khi thêm/xóa references
COPY Core/ ./Core/
COPY Extension/ ./Extension/
COPY Infrastructure/ ./Infrastructure/
COPY Lib/ ./Lib/
COPY Presentation/ ./Presentation/

# Restore NuGet packages
RUN dotnet restore Presentation/API/Digitalmarket.Controller.Business/Digitalmarket.Controller.Business.csproj

# ── Stage 2: Build ──
FROM restore AS build
WORKDIR /src

# Copy toàn bộ source code (bao gồm tất cả files)
COPY . .

# Ensure Config folder exists for runtime
RUN mkdir -p /src/Config

# Build project (Release configuration để tối ưu performance)
RUN dotnet build Presentation/API/Digitalmarket.Controller.Business/Digitalmarket.Controller.Business.csproj \
    -c Release --no-restore

# ── Stage 3: Publish ──
FROM build AS publish
WORKDIR /src

# Publish project (without dependencies vì đã build)
RUN dotnet publish Presentation/API/Digitalmarket.Controller.Business/Digitalmarket.Controller.Business.csproj \
    -c Release -o /app/publish --no-build --no-restore

# ── Stage 4: Runtime ──
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Metadata
LABEL maintainer="DigitalMarket Team"
LABEL description="Digitalmarket Business API (Microservice)"

EXPOSE 8080

# Thiết lập environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published application to a nested directory structure to match Config path resolution
# The app looks for Config at: ../../../Config from current working directory
# So we place app at: /work/app/publish and Config at /Config
COPY --from=publish --chown=app:app /app/publish /work/app/

# Copy Config directory from build stage (contains configuration files)
COPY --from=build --chown=app:app /src/Config /Config

# Set working directory to where the published app is
WORKDIR /work/app

# Chạy dưới non-root user (security best practice)
USER app

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "Digitalmarket.Controller.Business.dll"]
