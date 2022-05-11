Admins quickstart guide
==================================================

Running NetEvent with docker
-------------------------------

Prerequirements
^^^^^^^^^^^^^^^^^^^
- `Docker`_
- `Docker-compose`_

.. _Docker: https://docs.docker.com/get-docker/
.. _Docker-compose: https://docs.docker.com/compose/install/

Installation
^^^^^^^^^^^^^^^^^^^

- Download or clone this repository (or the docker-compose.yml and the .env.example)
- copy ``.env.example`` to ``.env`` and edit at least the ``DBUser`` and ``DBPassword`` values. If you want to use a different port than 5000/tcp, change ``APPPort``. Attention: DBPort does not change the port of the postgres instance that is deployed by default in the compose file!
- run ``docker-compose up -d``
- visit your NetEvent installation with your browser, login with ``Admin`` and ``Test123..`` and configure your instance

Notes
^^^^^^^^^^^^^^^^^^^

- This container is intended to be running behind any kind of reverse proxy and therfore it is started with http only. If you need help for this, take a look in the :doc:`/admin/reverseproxy`. 
- You can find the container image on `Docker Hub`_
- You can find the ``Dockerfile`` inside the root of our github repository and build it yourself

.. _Docker Hub: https://hub.docker.com/r/lan2play/netevent
