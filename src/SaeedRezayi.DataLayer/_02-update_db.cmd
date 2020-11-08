REM dotnet tool install --global dotnet-ef --version 3.1.3
REM dotnet tool update --global dotnet-ef --version 3.1.3
REM dotnet build
dotnet ef --startup-project ../SaeedRezayi.Api/ database update
pause