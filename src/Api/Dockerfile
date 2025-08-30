#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ./Api/ ./Api/
COPY ./Model/ ./Model/
COPY ./Service/ ./Service/
COPY ./Repository/ ./Repository/

RUN cd Api/ && dotnet publish -c $BUILD_CONFIGURATION -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS: "http://+:8080"

EXPOSE 8080

COPY --from=build /app ./
# Start the application
ENTRYPOINT ["dotnet", "Api.dll"]
