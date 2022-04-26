# Developers quickstart guide!

* Download and install .Net6 SDK https://dotnet.microsoft.com/download/dotnet/6.0
* Clone the repository https://github.com/Lan2Play/NetEvent.git
* Install EntityFramework with `dotnet tool install --global dotnet-ef`
* Switch into the `NetEvent\NetEvent\Server` folder and create the database with `dotnet ef database update`


# create new migrations for the database
Switch into the repository and execute 

## with make

`make migration name=MigrationName`

## without make 

`dotnet ef migrations add MigrationName --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql`

`dotnet ef migrations add MigrationName --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite`


# code analysis

we do our code analysis on [sonarcloud](https://sonarcloud.io/project/overview?id=Lan2Play_NetEvent)