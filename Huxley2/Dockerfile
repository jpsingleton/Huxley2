FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore --runtime alpine-x64

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out \
  --no-restore \
  --runtime alpine-x64 \
  --self-contained true \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true


# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["./Huxley2"]
