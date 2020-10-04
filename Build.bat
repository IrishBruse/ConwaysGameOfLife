dotnet publish -o "Builds/ConwaysGameOfLife Win-x64" -c Release -r win10-x64 --no-self-contained -p:PublishSingleFile=true
dotnet publish -o "Builds/ConwaysGameOfLife Win-x86" -c Release -r win10-x86 --no-self-contained -p:PublishSingleFile=true
dotnet publish -o "Builds/ConwaysGameOfLife Linux-x64" -c Release -r linux-x64 --no-self-contained -p:PublishSingleFile=true
dotnet publish -o "Builds/ConwaysGameOfLife MacOS-x64" -c Release -r osx.10.14-x64 --no-self-contained -p:PublishSingleFile=true