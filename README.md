# Honey-Dos

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

## Git Flow

- master -> PR only from dev - with version tag
- dev -> locked to prevent direct commits. PR only from feature/branches
- feature/[feature/issue#] - commit changes here

If commiting against an issue/PR, please add the `#` in front of commit message.

Example: Mike sees issue #3 on the kanban board/issues and wants to work on it. After assigning it to himself
he branches off the latest commit on `dev` and names it `feature/#3` or `feature/#3-update-readme` and then begins coding.
When Mike is finished, he commits his code with message `#3 Updated readme` and will open a PR to merge his branch into `dev`.
Once approved by Elanore, PR will be merged into `dev`. After all features and bug fixes are completed for milestone team meets
up does a release updating the version information and adding a tag.

## Coding Standards

- master branch
  - builds
  - runs
  - tests pass
- master tagged for releases
- code format
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
