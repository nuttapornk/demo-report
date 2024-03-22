FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY Report/ ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
EXPOSE 80
WORKDIR /app

COPY --from=build /app/out .

RUN sed -i s/deb.debian.org/archive.debian.org/g /etc/apt/sources.list
RUN sed -i 's|security.debian.org|archive.debian.org/|g' /etc/apt/sources.list
RUN sed -i '/stretch-updates/d' /etc/apt/sources.list

RUN apt-get update
RUN apt-get install wget libgdiplus -y && \
    wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dll && \
    wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.dylib && \
    wget -P /app https://github.com/rdvojmoc/DinkToPdf/raw/master/v0.12.4/64%20bit/libwkhtmltox.so


#COPY ./font/* ./
#RUN mkdir -p /usr/share/fonts/truetype/
#RUN install -m644 *.ttf /usr/share/fonts/truetype/
#RUN install -m644 *.otf /usr/share/fonts/truetype/
#RUN rm -rf /var/cache/apk/*

ENTRYPOINT ["dotnet", "Report.dll"]