# -----------
# Build stage
# -----------

FROM debian:11 AS build
LABEL stage=builder
LABEL org.opencontainers.image.authors="Alexander@volzit.de"

#args
ARG NETEVENTNETVER=0.0.1
ENV NETEVENTNETVER=$NETEVENTNETVER

#install prereqs
WORKDIR /
RUN apt-get update -qqy && DEBIAN_FRONTEND=noninteractive apt-get install -y \
    wget bash apt-transport-https
RUN wget -q -O - https://deb.nodesource.com/setup_current.x | bash -
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update -qqy && apt-get install -y dotnet-sdk-7.0 python3 python3-pip nodejs
RUN pip install lastversion
RUN eval apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

#get wait-for
RUN wget -q $(lastversion https://github.com/eficode/wait-for --format assets)

#get NetEvent and publish it
COPY NetEvent /NetEvent
COPY .sonarlint /.sonarlint
WORKDIR /NetEvent
RUN dotnet publish ./Server/NetEvent.Server.csproj -c Release -p:PublishProfile=DefaultPublish -p:Version=$NETEVENTNETVER

# -----------
# Final stage
# -----------

FROM debian:11
LABEL org.opencontainers.image.authors="Alexander@volzit.de"

#dotnet config
ENV ASPNETCORE_URLS="http://+:5000"
ENV DBProvider="sqlite"
ENV DBName="/data/netevent.db"
ENV DBServer=""
ENV DBPort=""
ENV DBUser=""
ENV DBPassword=""
ENV TZ="Europe/Berlin"

#User creation
RUN groupadd -g 1010 -r NetEvent && useradd --create-home --no-log-init -u 1010 -r -g NetEvent NetEvent

#default datadir
RUN mkdir /data && chown -R NetEvent:NetEvent /data
VOLUME [ "/data" ]

#container scripts
COPY docker/start-container /usr/local/bin/start-container
COPY --from=build /wait-for /usr/local/bin/wait-for.sh
RUN chmod +x /usr/local/bin/start-container
RUN chmod +x /usr/local/bin/wait-for.sh

#install prereqs
RUN apt-get update -qqy && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y wget apt-transport-https && \
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update -qqy && \
    apt-get install -y aspnetcore-runtime-7.0 netcat curl && \
    eval apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

#copy NetEvent files
WORKDIR /NetEvent
COPY --from=build /NetEvent/Server/bin/Release/net7.0/publish /NetEvent
RUN chmod +x NetEvent.Server

#versioning
ARG BUILDNUMBER
ENV BUILDNUMBER=$BUILDNUMBER
ARG BUILDID
ENV BUILDID=$BUILDID
ARG SOURCE_COMMIT
ENV SOURCE_COMMIT=$SOURCE_COMMIT
ARG BUILDNODE
ENV BUILDNODE=$BUILDNODE

#Expose the port used
EXPOSE 5000/tcp

#User
USER NetEvent

# run
WORKDIR /NetEvent
CMD [ "/bin/sh", "-c", "start-container" ]
HEALTHCHECK --retries=3 --timeout=5s CMD curl --fail http://localhost:5000/healthz || exit