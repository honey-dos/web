## First initial install of npm to ensure it runs npm or update npm modules
echo "Installing NPM modules."
npm install
npm install --prefix HoneyDo.Web/ClientApp

## Setup a postgres docker and migrate the tables to it - if this fails, run the following commands to remove the container and re-create it
## troubleshooting: `docker ps` - copy container ID and replace [container ID] in `docker stop [411b2c4fd3e9]` then `docker rm [411b2c4fd3e9]`
echo "Checking for docker container."
if docker container ls -a | grep 'honeydo-db'; then
    echo "Docker has a container ready. Checking if container is running."
    if docker ps | grep 'honeydo-db'; then
        echo "Docker container is already running. Skipping..."
    else
        echo "Starting container..."
        docker start honeydo-db
    fi
else 
    echo "Creating docker postgres image and migrating the database."
    docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
fi

## Install dotnet ef for db migration
echo "Checking for dotnet tool 'ef'."
if dotnet ef --version; then
    echo "'ef' is installed."
else
    echo "Installing dotnet tool 'dotnet-ef'."
    dotnet tool install --global dotnet-ef #will warn if alaready installed
fi

## Database migration TODO: Set flag to only do database migration
echo "Migrating the database on docker"
dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext

## Install dotnet dev-certs for https
echo "Checking for dotnet tool 'dev-certs'."
if dotnet dev-certs --version; then
    echo "'dev-certs' is installed."
else
    echo "Installing dotnet tool 'dotnet-dev-certs'."
    dotnet tool install --global dotnet-dev-certs #will warn if already installed
fi

## Setting dotnet to trust ssl certificates for development
echo "Trusting development ssl certificates..."
if [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
    dotnet dev-certs https # linux
else
    dotnet dev-certs https --trust # mac/windows
fi

## Set up JwtKey with input or random key with dotnet-user-serets
echo "Checking for dotnet tool 'user-secrets'."
if dotnet user-secrets --version; then
    echo "'user-secrets' is installed."
else
    echo "Installing dotnet tool 'dotnet-user-secrets'..."
    dotnet tool install --global dotnet-user-secrets
fi

## Set up JwtKey with defined key at least 16 chars or random key TODO: Ignore it
read -p "Input JwtKey (16 char minimum) or press [enter] for a random key:" key
if [ -z "$key" ]; then
    echo "Generating Random Key..."
    key=$(openssl rand -base64 64 | tr -d '\n')
    dotnet user-secrets set JwtKey ${key} -p "./HoneyDo.Web"
elif [ -n "$key" ]; then
    echo "Using key given for JwtKey."
    dotnet user-secrets set JwtKey ${key} -p "./HoneyDo.Web"
fi

## Check if JwtKey saved successfully
echo "Checking if JwtKey was saved successfully..."
jwt=$(dotnet user-secrets list -p "./HoneyDo.Web" | grep "$key")
if [ -n "$jwt" ]; then
    echo "Key saved successfully."
elif [ -z "$jwt" ]; then
    echo "Key was not saved successfully. Please check key and try again."
    exit 1
fi
unset key jwt

## Set up FirebaseJson with input or TODO: ignore it
read -p "Would you like to setup the firebase json for login auth?: " ans
if [[ "$ans" =~ ^[yY]e?s?$ ]]; then
    echo "Checking if 'jq' is installed..."
    if jq --version; then
        echo "'jq' is installed."
    else
        echo "Please install 'jq' to continue."
        echo "Arch: 'sudo pacman -Sy jq'"
        echo "Mac: 'brew install jq'"
        # TODO: Prompt for install?
        exit 1
    fi
    echo "Checking for firebase.json in root..."
    if [ -f "./firebase.json" ]; then
        echo "Found firebase.json, setting user-secret..."
        cat ./firebase.json | jq 'tostring' | sed -e 's/"{/{ "FirebaseJson": "{/' | sed -e 's/}"/}"}/' | jq | dotnet user-secrets set -p "./HoneyDo.Web"
    else
        echo "Could not find firebase.json, please make sure it's in root: './web'."
        exit 1
    fi

    ## Check if FirebaseJson key saved successfully
    echo "Checking if FirebaseJson was saved successfully..."
    if dotnet user-secrets list -p "./HoneyDo.Web" | grep "FirebaseJson"; then
        echo "Key saved successfully."
    else
        echo "Key was not saved successfully. Please check key and try again."
        exit 1
    fi

else
    echo "Skipping firebasejson setup..."
fi

## cleanup
unset key jwt ans

## Complete
echo "Initialization complete. Please manually check for errors. When you are ready to run Honey-dos, run the command 'npm run watch'."
exit 0
