dotnet publish -o Builds/Win64/ -r win-x64 -c Release -f netcoreapp3.1 --no-build -p:PublishTrimmed=true -p:PublishSingleFile=true -p:PublishReadyToRun=true --force --self-contained true
dotnet publish -o Builds/Win32 -r win-x86 -c Release -f netcoreapp3.1 --no-build -p:PublishTrimmed=true -p:PublishSingleFile=true -p:PublishReadyToRun=true --force --self-contained true
pause