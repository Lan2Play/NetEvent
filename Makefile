
#run NetEvent clean dev instance
run-dev-clean: remove-db run-dev

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

# create initial migrations and delete the database | Attention: Do not use this until you know what you are doing!
force-initial-migration: force-initial-migration-without-db-remove remove-db

# create initial migrations | Attention: Do not use this until you know what you are doing!
force-initial-migration-without-db-remove:
ifeq ($(OS),Windows_NT)
	if exist NetEvent\Server\Migrations\Psql rmdir /S/Q NetEvent\Server\Migrations\Psql
	if exist NetEvent\Server\Migrations\Sqlite rmdir /S/Q NetEvent\Server\Migrations\Sqlite
else
	rm -rf NetEvent/Server/Migrations/Psql
	rm -rf NetEvent/Server/Migrations/Sqlite
endif
	dotnet ef migrations add Initial --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
	dotnet ef migrations add Initial --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql

#remove development database
remove-db:
ifeq ($(OS),Windows_NT)
	if exist NetEvent\Server\netevent.db del /f/q NetEvent\Server\netevent.db
	if exist NetEvent\Server\netevent.db-shm del /f/q NetEvent\Server\netevent.db-shm
	if exist NetEvent\Server\netevent.db-wal del /f/q NetEvent\Server\netevent.db-wal
else
	rm -rf NetEvent/Server/netevent.db
	rm -rf NetEvent/Server/netevent.db-shm
	rm -rf NetEvent/Server/netevent.db-wal
endif

# Make Documentation
docs-html:
ifeq ($(OS),Windows_NT)
	echo you currently need docker on linux to build the documentation
else
	docker run --rm -v $(shell dirname $(realpath $(lastword $(MAKEFILE_LIST))))/Docs:/docs -e USERID=$(shell id -u ${USER}) -e GROUPID=$(shell id -g ${USER}) lan2play/docker-sphinxbuild:latest
endif