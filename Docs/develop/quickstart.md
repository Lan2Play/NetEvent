# Developers quick start Guide!
Download and Install .Net6 SDK https://dotnet.microsoft.com/download/dotnet/6.0

Install EntityFramework 
`dotnet tool install --global dotnet-ef`

Switch into NetEvent\Server folder

Create DB
`dotnet ef database update`


# create migrations for the Database
Switch into the repository and execute 

## with make

`make migration name=MigrationName`

## without make 

`dotnet ef migrations add MigrationName --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql`

`dotnet ef migrations add MigrationName --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite`