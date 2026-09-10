param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

dotnet ef migrations add $MigrationName `
    --project src/RealTimeOpsPortal.Infrastructure `
    --startup-project src/RealTimeOpsPortal.Api `
    --output-dir Persistence/Migrations
