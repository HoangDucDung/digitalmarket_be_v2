# ============================================
# Dockerfile - Digitalmarket Auth API (Microservice)
# Build context: root của solution (../../..)
# Usage: docker build -f Presentation/API/Digitalmarket.Controller.Auth/Dockerfile -t digitalmarket-auth .
# ============================================

# ── Stage 1: Build ──
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution + tất cả .csproj để tận dụng Docker layer cache
COPY digitalmarket_be_v2.sln ./

# Core
COPY Core/Project.DigitalMarket.Domain/Project.DigitalMarket.Domain.csproj Core/Project.DigitalMarket.Domain/
COPY Core/Project.DigitalMarket.Domain.Share/Project.DigitalMarket.Domain.Share.csproj Core/Project.DigitalMarket.Domain.Share/
COPY Core/Project.DigitalMarket.Application/Project.DigitalMarket.Application.csproj Core/Project.DigitalMarket.Application/
COPY Core/Project.DigitalMarket.Application.Contract/Project.DigitalMarket.Application.Contract.csproj Core/Project.DigitalMarket.Application.Contract/

# Extension
COPY Extension/Project.DigitalMarket.Extension/Project.DigitalMarket.Extension.csproj Extension/Project.DigitalMarket.Extension/

# Infrastructure
COPY Infrastructure/Project.DigitalMarket.Infrastructure.MsSql/Project.DigitalMarket.Infrastructure.MsSql.csproj Infrastructure/Project.DigitalMarket.Infrastructure.MsSql/
COPY Infrastructure/Project.DigitalMarket.Infrastructure.Mail/Project.DigitalMarket.Infrastructure.Mail.csproj Infrastructure/Project.DigitalMarket.Infrastructure.Mail/

# Lib
COPY Lib/Project.DigitalMarket.Libs/Project.DigitalMarket.Libs.csproj Lib/Project.DigitalMarket.Libs/
COPY Lib/Project.DigitalMarket.Host.Base/Project.DigitalMarket.Host.Base.csproj Lib/Project.DigitalMarket.Host.Base/

# Presentation / API
COPY Presentation/API/Digitalmarket.Controller.Base/Digitalmarket.Controller.Base.csproj Presentation/API/Digitalmarket.Controller.Base/
COPY Presentation/API/Digitalmarket.Controller.Auth/Digitalmarket.Controller.Auth.csproj Presentation/API/Digitalmarket.Controller.Auth/
COPY Presentation/API/Digitalmarket.Controller.Business/Digitalmarket.Controller.Business.csproj Presentation/API/Digitalmarket.Controller.Business/

# Restore NuGet (cached layer)
RUN dotnet restore Presentation/API/Digitalmarket.Controller.Auth/Digitalmarket.Controller.Auth.csproj

# Copy toàn bộ source
COPY . .

# ── Stage 2: Publish ──
FROM build AS publish
RUN dotnet publish Presentation/API/Digitalmarket.Controller.Auth/Digitalmarket.Controller.Auth.csproj \
    -c Release -o /app/publish --no-restore

# ── Stage 3: Runtime ──
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

# Copy Config
COPY Config/ /app/Config/

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Digitalmarket.Controller.Auth.dll"]
