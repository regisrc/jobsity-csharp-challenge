
# Jobsity-csharp-challenge

## Setup that the project was created

- Visual Studio 2022 Version 17.4.0
- .NET 7.0

## RabbitMQ

You will need to install rabbitMQ, to run rabbitMQ install erlang.
If you want to see the rabbitMQ management panel, you will need to run this command.

The default credentials are guest/guest and the url is http://localhost:15672.

```bash
rabbitmq-plugins enable rabbitmq_management
```
 - [Erlang download](https://www.erlang.org/downloads)
 - [RabbitMQ download](https://www.rabbitmq.com/download.html)

## SQLite
If you want to see the data from database, you can install DB Browser.

 - [DB Browser download](https://sqlitebrowser.org)
## ChatRoom

Before run your project for the first time, you need to go to

```
launchSettings.json
```

and change db_file_path enviroment variable to your computer path, 
after that go to the console, and run:

```
Update-Database
```

Now you can start the backend project :)
## Frontend
To run the frontend you need to install nodeJS and yarn

- [NodeJS download](https://nodejs.org/en/download/)
- [Yarn doc](https://classic.yarnpkg.com/lang/en/docs/install/#windows-stable)

And run this commands in your source frontend path

```bash
yarn
yarn start
```

yarn to install the dependencies and yarn start to run your project


## Considerations

You need to create the user and the chatroom in swagger, 
for the token and user id you can find it on the database or at 
session storage of your browser after you login.
