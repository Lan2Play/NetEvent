# -----------
# Build stage
# -----------

FROM debian:11 AS build
LABEL stage=builder
LABEL org.opencontainers.image.authors="Alexander@volzit.de"

#install prereqs
WORKDIR /
RUN apt-get update -qqy && DEBIAN_FRONTEND=noninteractive apt-get install -y \
    wget bash apt-transport-https
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update -qqy && apt-get install -y dotnet-sdk-6.0
RUN eval apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

#get NetEvent and publish it
COPY NetEvent /NetEvent
WORKDIR /NetEvent
RUN dotnet publish ./Server/NetEvent.Server.csproj -c Release -p:PublishProfile=DefaultPublish

# -----------
# Final stage
# -----------

FROM debian:11 
LABEL org.opencontainers.image.authors="Alexander@volzit.de" 

#dotnet config
ENV ASPNETCORE_URLS="http://+:5000"
ENV DATADIR="/data"

#User creation
RUN groupadd -g 1010 -r NetEvent && useradd --no-log-init -u 1010 -r -g NetEvent NetEvent

#default datadir
RUN mkdir /data && chown -R NetEvent:NetEvent /data
VOLUME [ "/data" ]

#install prereqs
RUN apt-get update -qqy && DEBIAN_FRONTEND=noninteractive apt-get install -y \
    wget apt-transport-https
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update -qqy && apt-get install -y aspnetcore-runtime-6.0 libleptonica-dev libtesseract-dev libc6-dev
RUN eval apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

#copy NetEvent files
WORKDIR /NetEvent
COPY --from=build /NetEvent/Server/bin/Release/net6.0/publish /NetEvent
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

#fix libraries
WORKDIR /NetEvent/x64
RUN ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 liblept.so.5
RUN ln -s /usr/lib/x86_64-linux-gnu/libleptonica.so libleptonica-1.80.0.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so libtesseract41.so

#Expose the port used
EXPOSE 5000/tcp

#User 
USER NetEvent

# run
WORKDIR /NetEvent
CMD [ "/bin/sh", "-c", "/NetEvent/NetEvent.Server" ]