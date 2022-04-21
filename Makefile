
# create migration usage make migration name=migrationnamehere
migration:
	dotnet ef migrations add $(name) --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
	dotnet ef migrations add $(name) --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql

# create initial migrations | Attention: Do not use this until you know what you are doing!
force-initial-migration:
	rm -rf NetEvent/Server/Migrations/Psql
	rm -rf NetEvent/Server/Migrations/Sqlite
	dotnet ef migrations add InitialCreate --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
	dotnet ef migrations add InitialCreate --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql