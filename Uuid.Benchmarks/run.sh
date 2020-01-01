#!/usr/bin/env bash

BASE_DIR="$(cd "$(dirname "$0")" && pwd)"
(cd $BASE_DIR && sudo dotnet publish -c Release)
PUBLISH_PATH="${BASE_DIR}/bin/Release/netcoreapp3.1/publish/"
echo $PUBLISH_PATH
echo "-------------"
cd $PUBLISH_PATH && sudo dotnet Uuid.Benchmarks.dll
