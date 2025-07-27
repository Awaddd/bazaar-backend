# Use official .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set working directory inside container
WORKDIR /app

# Copy published output
COPY out/ ./

# Expose default port
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "Commerce.dll"]
