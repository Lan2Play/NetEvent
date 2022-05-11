
Database
==================================================

Currently we use sqlite for development and our production container setup uses postgres. If you implement changes in the models, you have to create database migrations. 

create new migrations for the database
----------------------------------------
Switch into the repository and execute 

with make
^^^^^^^^^^^^^^^^^^^
.. code-block:: bash

    make migration name=MigrationName

without make 
^^^^^^^^^^^^^^^^^^^

.. code-block:: bash

    dotnet ef migrations add MigrationName --project NetEvent/Server --context PsqlApplicationDbContext --output-dir Migrations/Psql -- --DBProvider psql
    dotnet ef migrations add MigrationName --project NetEvent/Server --context SqliteApplicationDbContext --output-dir Migrations/Sqlite
