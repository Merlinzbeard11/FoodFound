FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/SeatEats.Domain/*.csproj ./SeatEats.Domain/
COPY src/SeatEats.Application/*.csproj ./SeatEats.Application/
COPY src/SeatEats.Web/*.csproj ./SeatEats.Web/
RUN dotnet restore SeatEats.Web/SeatEats.Web.csproj

COPY src/ .
RUN dotnet publish SeatEats.Web/SeatEats.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
CMD ["sh", "-c", "dotnet SeatEats.Web.dll --urls http://+:${PORT:-8080}"]
