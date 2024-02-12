## IBAY API - a student project about an asp.net simple api

## how to run ?

Theres a docker compose file, that runs both the api and the database, and are managed automatically by the asp api.

PLEASE ensure to have the .env file before launching the docker compose.

# this file should be containing 3 vars, such as : db_user : {
JWT_SECRET= secret of all the jwt tokens that will run on your api.
DB_USER : user of the database.
DB_PASSWORD : password of the database user.
}

if the docker file doesn't work, commented code is available in the `ibay/env.cs` to run. uncomment it and use your own database if you're using it.

# external softwares used 

Postgre SQL : this api is meant to work with postgresql, by any means you want, if you wanna use any other database, feel free to modify the code.

you only have to modify the db context to change the database.

# Filling Database

You can use the api itself to fill the database, however, please mind that your user must be seller to create products.

# Documentation

a openapi/swagger doc is provided to specify what are the endpoints of the api or the responses you can wait of it. you can access it by {your_address}/swagger

# Issues ?

this is a student project, it is not meant to be perfect or being flawless, but we probably won't resolve any issues you will open if you do so , so please no need to open issues.

However, if you have any questions about how this program works or about the code itself, don't wonder to message me and ask your question. thanks :)
