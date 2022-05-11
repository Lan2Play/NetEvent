
Developers quickstart guide
==================================================

* Download and install .Net6 SDK https://dotnet.microsoft.com/download/dotnet/6.0
* Clone the repository https://github.com/Lan2Play/NetEvent.git
* Install EntityFramework with ``make install-ef`` or ``dotnet tool install --global dotnet-ef``

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


code analysis
----------------------------------------
we do our code analysis on `sonarcloud`_


.. _sonarcloud: https://sonarcloud.io/project/overview?id=Lan2Play_NetEvent