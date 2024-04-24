#!/bin/bash

docker run --name postgres-db -e POSTGRES_PASSWORD=3sp3rand0 -p 5432:5432 postgres