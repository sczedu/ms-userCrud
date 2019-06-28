FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ms-userCrud/ms-userCrud.csproj", "ms-userCrud/"]
RUN dotnet restore "ms-userCrud/ms-userCrud.csproj"
COPY . .
WORKDIR "/src/ms-userCrud"
RUN dotnet build "ms-userCrud.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ms-userCrud.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ms-userCrud.dll"]