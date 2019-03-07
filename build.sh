#!/bin/zsh

echo "off"

dotnet build
dotnet test ./Mc.Tests/Mc.Tests.csproj
