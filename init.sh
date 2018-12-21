# First initial install of npm to ensure it runs npm
npm install
npm install --prefix HoneyDo.Web/ClientApp

# Setup a postgres docker and migrate the tables to it - if this fails, run the following commands to remove the container and re-create it
# `docker ps` - copy container ID and replace [container ID] in `docker stop [411b2c4fd3e9]` then `docker rm [411b2c4fd3e9]`
docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
dotnet tool install --global dotnet-ef #will warn if alaready installed
dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext

# Install dotnet dev-certs for https and then trust them
dotnet tool install --global dotnet-dev-certs #will warn if already installed
dotnet dev-certs https # linux
dotnet dev-certs https --trust # mac/windows

# Set up JwtKey with input or random key with dotnet-user-serets
read -p "Input JwtKey or press (enter) for a random key:" key
if [ -z "$key" ]; then
    echo "Generating Random Key"
    key=$(openssl rand -base64 64)
fi
cd HoneyDo.Web
dotnet tool install --global dotnet-user-secrets
dotnet user-secrets set JwtKey ${key}