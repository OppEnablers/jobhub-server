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

docker run -d \
  --name $CONTAINER_NAME \
  -p $HOST_PORT:$CONTAINER_PORT \
  $IMAGE_NAME
