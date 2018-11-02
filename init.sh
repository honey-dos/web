# Restore and build the dotnet app
dotnet restore
dotnet build

# Install NPM and start the server, it will run side-by-side with the dotnet API server for speed
npm install --prefix HoneyDo.Web/ClientApp
npm start --prefix HoneyDo.Web/ClientApp

# Setup a postgres docker and migrate the tables to it - if this fails, run the following commands to remove the container and re-create it
# `docker ps` - copy container ID and replace [container ID] in `docker stop [411b2c4fd3e9]` then `docker rm [411b2c4fd3e9]`
docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext

# Install dotnet dev-certs for https and then trust them
dotnet tool install --global dotnet-dev-certs
dotnet dev-certs https

# Lastly run the API server
dotnet run --project HoneyDo.Web/HoneyDo.Web.csproj 