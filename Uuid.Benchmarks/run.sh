#!/usr/bin/env bash

BASE_DIR="$(cd "$(dirname "$0")" && pwd)"
(cd $BASE_DIR && sudo dotnet publish -c Release)
PUBLISH_PATH=$(realpath "${BASE_DIR}/bin/Release/netcoreapp3.0/publish/")
(cd $PUBLISH_PATH && sudo dotnet Uuid.Benchmarks.dll)
