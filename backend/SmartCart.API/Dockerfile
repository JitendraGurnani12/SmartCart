# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "SmartCart.API.dll"]

# Docker file understanding
# FROM mcr.microsoft.com/dotnet/sdk:8.0 ----->Downloads .NET SDK image, it means Mini  computer with .NET installed
# ############   
# WORKDIR /src ------>current folder inside container
# COPY . . ----> copies our project inside the container
# RUN dotnet restore------->RUN dotnet restore
# dotnet publish--->Creates optimized production files
# ENTRYPOINT ["dotnet", "DockerDemoApi.dll"]--->Starts application when container runs
#  
# command for create docker image from docker build command by using docker file 
# ------docker build -t demo-api .
#  in command this dot . means--> Current folder contains Dockerfile
# then we use command to create container 
# -----docker run -d -p 5292:8080 imagename .
