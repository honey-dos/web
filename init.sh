
dotnet restore
dotnet build
npm install --prefix HoneyDo.Web/ClientApp
docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext
