# OurNotes Backend Part

## Run project locally
First of all you need to install SQL Server on our machine. I presume that there won't be any problems with Windows but with M1 Mac there can be some troubles.<br>
To run this on your macbook (Apple M1) follow the next steps:
- Install docker
- Run command in the terminal: `docker pull mcr.microsoft.com/azure-sql-edge`
- Then run the next command in terminal with substituted `<StrongPassword>` value: `sudo docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=<StrongPassword>' -p 1433:1433 --name sqledge -d mcr.microsoft.com/azure-sql-edge`
- Run database update command to apply a migration to a DB with specified DbContext file (we have a couple of them here): `dotnet ef database update --context "ApplicationIdentityDbContext"`
- Run the project

### Link to run Swagger UI: <span>`https://localhost:<port>/swagger/index.html`<span>

#### This document will be updated according to the upcoming changes...