#!/bin/zsh

echo "--------------------------------"
echo "         XUnit Test"
echo "         Test Compiler"
echo " "
echo " "
echo "--------------------------------"

dotnet build
dotnet test ./Mc.Tests/Mc.Tests.csproj
