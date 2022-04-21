# Developers quick start Guide!
Download and Install .Net6 SDK https://dotnet.microsoft.com/download/dotnet/6.0

Install EntityFramework 
`dotnet tool install --global dotnet-ef`

Switch into NetEvent\Server folder

Create DB
`dotnet ef database update`


# create migrations for the Database
Switch into NetEvent\Server folder

`dotnet ef migrations add InitialCreate --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql`
`dotnet ef migrations add InitialCreate --context SqliteApplicationDbContext --output-dir Migrations/Sqlite`