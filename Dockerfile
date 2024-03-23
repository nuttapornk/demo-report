FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY Report/ ./
#COPY libs/ ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
EXPOSE 80
WORKDIR /app

COPY --from=build /app/out .
COPY libs/libwkhtmltox/v0.12.4/64bit/* ./

RUN sed -i s/deb.debian.org/archive.debian.org/g /etc/apt/sources.list
RUN sed -i 's|security.debian.org|archive.debian.org/|g' /etc/apt/sources.list
RUN sed -i '/stretch-updates/d' /etc/apt/sources.list

RUN apt-get update
RUN apt-get install wget libgdiplus -y

ENTRYPOINT ["dotnet", "Report.dll"]