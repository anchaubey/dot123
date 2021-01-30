# Run from the 3.1 LTS SDK Image
FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /Device

# Copy the full source tree  
COPY ./RealWear.DeviceManagement ./
# Restore the CSPROJ nuget packages
RUN dotnet restore

# Publish code (Release build)
RUN dotnet publish --configuration Release --output dist

#RUN dotnet publish -c Release
# Run from 3.1 LTS image
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
#EXPOSE 80
# Pull in the compiled binaries
COPY --from=build /Device/dist/ ./
COPY --from=build /Device ./
# Run the application when the container starts
ENTRYPOINT [ "dotnet", "RealWear.DeviceManagement.Service.dll" ]
