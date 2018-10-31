# Honey Do

## Installation
1. clone repo
2. set env variables in `.bash_profile`, `.zshrc` or equivalent
    ```
    export ASPNETCORE_ENVIRONMENT=Development
    export ASPNETCORE_HTTPS_PORT=5000
    export NODE_ENV=development
    ```
3. make init script executable `chmod +x init.sh`
4. run init script `./init.sh`

## Coding Standards
- master branch
    - builds
    - runs
    - tests pass
- branches w/ tags created for releases
- code
    - Javascript
        - prettier, no errors
        - eslint, no errors
    - C#
        - vscode formatting
- PRs
    - must be reviewed
- Database schema changes are made through migrations

## Infrastructure Notes

To add a migration `dotnet ef migrations add {migration name} -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c {HoneyDoContext}`

To update database with migrations `dotnet ef database update -p ./HoneyDo.Infrastructure/ -s ./HoneyDo.Web/ -c {HoneyDoContext}`

## JWT Notes
**Not yet Needed -**
`JwtKey` must be added to the dotnet secrets store for development use `dotnet user-secrets` in the `HoneyDo.Web` directory.

## Database
To run local database

- First time: `docker run --name honeydo-db -e POSTGRES_DB=honeydo -e POSTGRES_USER=honeydo-user -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres`
- Following: `docker start honeydo-db`
