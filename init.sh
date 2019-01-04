# First initial install of npm to ensure it runs npm
echo "Installing NPM modules."
#npm install
#npm install --prefix HoneyDo.Web/ClientApp

# Setup a postgres docker and migrate the tables to it - if this fails, run the following commands to remove the container and re-create it
# `docker ps` - copy container ID and replace [container ID] in `docker stop [411b2c4fd3e9]` then `docker rm [411b2c4fd3e9]`
echo "Creating docker postgres image and migrating the database."
#docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres
#dotnet tool install --global dotnet-ef #will warn if alaready installed
#dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c HoneyDoContext

# Install dotnet dev-certs for https and then trust them
echo "Installing dotnet tool 'dotnet-dev-certs'."
#dotnet tool install --global dotnet-dev-certs #will warn if already installed
#dotnet dev-certs https # linux
#dotnet dev-certs https --trust # mac/windows

# Set up JwtKey with input or random key with dotnet-user-serets
echo "Installing dotnet tool 'dotnet-user-secrets'."
#dotnet tool install --global dotnet-user-secrets

cd HoneyDo.Web
read -p "Input JwtKey or press (enter) for a random key:" key
if [ -z "$key" ]; then
    echo "Generating Random Key..."
    key=$(openssl rand -base64 64 | tr -d '\n')
    dotnet user-secrets set JwtKey ${key}
elif [ -n "$key" ]; then
    echo "Using key given for JwtKey."
    dotnet user-secrets set JwtKey ${key}
fi

# Check if key saved successfully
# error=$(echo $(dotnet user-secrets list) | grep $key)
# if [ -z "$key" ]; then
#     echo "Key saved successfully."
# elif [ -n "$key" ]; then
#     echo "Key was not saved successfully. Please check key and try again."
# fi

# cd ..

# Doesn't hurt to check again
# read -p "Do you want to see that the key saved successfully?: (y/n - default)" input
# if [ "$input" = "Y" ] || [ "$input" = "y" ]; then
#     cd Honeydo.Web
#     echo $(dotnet user-secrets list)
# fi

# Set up FirebaseJson with input or TODO: ignore it
# read -p "Would you like to setup the firebase json for login auth?: " ans
# if [ "$ans" =~ "/^[yY][es]*$/" ]; then
#     echo "Checking for firebase.json in root..."
#     if [ -f "firebase.json" ]; then
#         echo "Found firebase.json, setting user-secret..."
#         local filter filename='./firebase.json'

#         # including bash variable name in reduction
#         filter='to_entries | map("[\(.key | @sh)]=\(.value | @sh) ") | "associativeArray=(" + add + ")"'

#         # using --argfile && --null-input
#         local -A "$(jq --raw-output --null-input --argfile file "$filename" \ "\$filename | ${filter}")"

#         # map definition && reduce replacement
#         filter='[to_entries[]|"["+(.key|@sh)+"]="+(.value|@sh)]|"("+join(" ")+")"'

#         # input redirection && --join-output
#         local -A associativeArray=$(jq --join-output "${filter}" < "${filename}")
#         #firebasejson=
#         #dotnet user-secrets set JwtKey ${firebasejson}
#     else
#         echo "Could not find firebase.json, please make sure it's in root './web'."
#     fi
# fi

# Complete
echo "Initialization complete. Please manually check for errors. When you are ready to run Honey-dos, run the command `npm run watch`."
