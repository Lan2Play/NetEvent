
Developers start guide
==================================================

We are glad you want to help us on NetEvent!
Some things you should think of before you start implementing new features or fixing bugs:

- Can another feature thats already implemented be expanded? yes? then go for that instead of Building complete new stuff!
- Does the addition / change might affect other usecases than your own? Build your changes with legacy support in mind!
- Try to follow the coding Style which is used within NetEvent, just look around in our code to see which case is handled mostly in which manner
- Why i shouldn't join the NetEvent discord developer channel for discussion?

To start a new code contribution please:

- open an issue to announce that you are working on a feature/change to get thoughts from the other developers and to prevent incompatibillities
- make your fork NetEvent (if you are not already a acknowledged developer who can create branches on our repository)
- make a new branch with the Name ``feature/examplefeature`` 
- as soon as you have code, please open a draft pull request. 

Before you want to change your draft pull request to a finalized pull request to main you should ask yourself some questions:

- Have i tried to update a running version from NetEvent with data to the one with my changes? Are the changes update proof?
- Have i implemented all strings with localised variables? See :doc:`/develop/translation`!
- Have i implemented the users help features inside the client?
- Have i changed the admin documentation on the affected parts?
- Have i changed the developer documentation on the affected parts?
- Have i changed the ``README.md`` on the affected parts?
- Have i written all ne needed tests for my code?
- Does my pull request have a good speaking name that represents my changes in the changelog?
- Do i have merged the current main branch into my feature branch?

What will happen after you have converted to the finalized pull request:

- Someone of the core developer team assigns a specific label to your pull request, then our sonarcloud code analysis will run on your code. Afterwards please fix the things sonarcloud complains about.
- When your code passes the sonarcloud analysis, someone of the core developer team will review your code and will help you to find missing things or bugs.
- As soon as the review is done, your code will be merged to main and will get available for all users in the next release. Currently we dont have a fixed release cycle.

Development Prerequisites
----------------------------------------
- Download and install .Net6 SDK https://dotnet.microsoft.com/download/dotnet/6.0
- Install EntityFramework with ``make install-ef`` or ``dotnet tool install --global dotnet-ef``
- Install your IDE of choise (Visual Studio or VSCode works best)

Run NetEvent locally
----------------------------------------
with make
^^^^^^^^^^^^^^^^^^^
.. code-block:: bash

    make

or

.. code-block:: bash

    make dev

without make 
^^^^^^^^^^^^^^^^^^^

.. code-block:: bash

    dotnet run --project NetEvent/Server


Digging deeper
----------------------------------------
If you want to know more about our development thoughts, you might want to take a look into:

- :doc:`/develop/database`
- :doc:`/develop/folderstructure`
- :doc:`/develop/models`
- :doc:`/develop/services`
- :doc:`/develop/authenticationsystem`
- :doc:`/develop/permissions`
- :doc:`/develop/cicd`
- :doc:`/develop/release`


code analysis
----------------------------------------
we do our code analysis on `sonarcloud`_


.. _sonarcloud: https://sonarcloud.io/project/overview?id=Lan2Play_NetEvent


