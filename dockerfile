FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY BlazorMud/*.csproj ./BlazorMud/
COPY BlazorMud.BusinessLogic/*.csproj ./BlazorMud.BusinessLogic/
COPY BlazorMud.Contracts/*.csproj ./BlazorMud.Contracts/
COPY BlazorMud.DataAccess/*.csproj ./BlazorMud.DataAccess/

WORKDIR /app/BlazorMud
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY BlazorMud/. ./BlazorMud/
COPY BlazorMud.BusinessLogic/. ./BlazorMud.BusinessLogic/
COPY BlazorMud.Contracts/. ./BlazorMud.Contracts/
COPY BlazorMud.DataAccess/. ./BlazorMud.DataAccess/

WORKDIR /app/BlazorMud
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app/BlazorMud/out ./
ENTRYPOINT ["dotnet", "BlazorMud.dll"]