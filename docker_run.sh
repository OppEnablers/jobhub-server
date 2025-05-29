#!/bin/bash

IMAGE_NAME="oppenablers_softeng2server"
CONTAINER_NAME="JobHub_Server"
HOST_PORT=5131
CONTAINER_PORT=8080

if [ "$(docker ps -aq -f name=$CONTAINER_NAME)" ]; then
    echo "Stopping and removing existing container: $CONTAINER_NAME"
    docker stop $CONTAINER_NAME
    docker rm $CONTAINER_NAME
fi

docker build -f JobHubServer/Dockerfile -t $IMAGE_NAME .
docker run -d \
  --name $CONTAINER_NAME \
  -p $HOST_PORT:$CONTAINER_PORT \
  -e GOOGLE_APPLICATION_CREDENTIALS=/app/keys/FireBaseServiceKey.json \
  -v ./FireBaseServiceKey.json:/app/keys/FireBaseServiceKey.json \
  $IMAGE_NAME
