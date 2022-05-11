#run NetEvent dev instance
run-dev:
	dotnet run --project NetEvent/Server

#install dotnet ef
install-ef:
	dotnet tool install --global dotnet-ef

# create migration usage make migration name=migrationnamehere
migration:
	dotnet ef migrations add $(name) --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
	dotnet ef migrations add $(name) --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql

# create initial migrations | Attention: Do not use this until you know what you are doing!
force-initial-migration:
ifeq ($(OS),Windows_NT)
	rmdir /S/Q NetEvent\Server\Migrations\Psql
	rmdir /S/Q NetEvent\Server\Migrations\Sqlite
else
	rm -rf NetEvent/Server/Migrations/Psql
	rm -rf NetEvent/Server/Migrations/Sqlite
endif
	dotnet ef migrations add InitialCreate --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
	dotnet ef migrations add InitialCreate --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql

# Make Documentation
docs-html:
ifeq ($(OS),Windows_NT)
	echo you currently need docker on linux to build the documentation
else
	docker run --rm -v $(shell dirname $(realpath $(lastword $(MAKEFILE_LIST))))/Docs:/docs -e USERID=$(shell id -u ${USER}) -e GROUPID=$(shell id -g ${USER}) lan2play/docker-sphinxbuild:latest
endif