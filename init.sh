## First initial install of npm to ensure it runs npm or update npm modules
echo "Installing NPM modules."
npm install
npm install --prefix HoneyDo.Web/ClientApp

## Setup a postgres docker and migrate the tables to it - if this fails, run the following commands to remove the container and re-create it
## `docker ps` - copy container ID and replace [container ID] in `docker stop [411b2c4fd3e9]` then `docker rm [411b2c4fd3e9]`
echo "Creating docker postgres image and migrating the database."
docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
dotnet tool install --global dotnet-ef #will warn if alaready installed
dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext

## Install dotnet dev-certs for https and then trust them
echo "Installing dotnet tool 'dotnet-dev-certs'."
dotnet tool install --global dotnet-dev-certs #will warn if already installed
dotnet dev-certs https # linux
dotnet dev-certs https --trust # mac/windows

## Set up JwtKey with input or random key with dotnet-user-serets
echo "Checking for dotnet tool 'dotnet-user-secrets'."
if dotnet user-secrets --version; then
    echo "'dotnet-user-secrets' is installed."
else
    echo "Installing dotnet tool 'dotnet-user-secrets'..."
    dotnet tool install --global dotnet-user-secrets
fi

## Set up JwtKey with defined key or random key TODO: Ignore it
read -p "Input JwtKey or press (enter) for a random key:" key
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
    found=$(dotnet user-secrets list -p "./HoneyDo.Web" | grep "FirebaseJson")
    if [ -n "$found" ]; then
        unset found
        echo "Key saved successfully."
    elif [ -z "$found" ]; then
        unset found
        echo "Key was not saved successfully. Please check key and try again."
        exit 1
    fi

else
    echo "Skipping firebasejson setup..."
fi

## cleanup
unset key jwt found ans

## Complete
echo "Initialization complete. Please manually check for errors. When you are ready to run Honey-dos, run the command 'npm run watch'."
exit 0
