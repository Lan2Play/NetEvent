# Admins quickstart guide!
## Running NetEvent

### Prerequirements

- [Docker](https://docs.docker.com/get-docker/)
- [Docker-compose](https://docs.docker.com/compose/install/)

### Installation

- Download or clone this repository (or the docker-compose.yml and the .env.example)
- if you want to use a different port than 5000/tcp, change the first 5000 the docker-compose.yml
- copy `.env.example` to `.env` and edit at least the `DBUser` and `DBPassword` values. Attention: DBPort does not change the port of the postgres instance that is configured by default in the compose file!
- run `docker-compose up -d`
- visit your NetEvent installation with your browser, login with Admin and Test123.. and configure your instance
