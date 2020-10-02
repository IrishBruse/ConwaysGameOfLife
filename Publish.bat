dotnet publish -o Builds/ -r win-x64 -c Release -f netcoreapp3.1 --no-build -p:PublishTrimmed=true -p:PublishSingleFile=true -p:PublishReadyToRun=true --force --self-contained true
cd Builds
del ConwaysGameOfLife.pdb
ren ConwaysGameOfLife.exe ConwaysGameOfLife_x64.exe
cd ..

dotnet publish -o Builds/ -r win-x86 -c Release -f netcoreapp3.1 --no-build -p:PublishTrimmed=true -p:PublishSingleFile=true -p:PublishReadyToRun=true --force --self-contained true
cd Builds
del ConwaysGameOfLife.pdb
ren ConwaysGameOfLife.exe ConwaysGameOfLife_x86.exe
cd ..
