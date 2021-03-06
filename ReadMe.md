
# Account Management

This project has been created to understand how the CAP library works. The CAP library is used with SQL Server and RabbitMQ.

## __RUN__

__Way 1__

The project could be executed via _docker-compose_. If you have an IDE which is capable of debugging _docker-compose file_, _docker-compose.yml_ which is located at the main directory would  be useful for you.

In case of choosing this way to run the project, you can reach swagger screen via _http://localhost:10000_.

Note: Because the sql server needs more time to be ready compared to AccountManagement-Api, it might take a while for you to reach the endpoints after `docker-compose up` command execution.

__Way 2__

If you want to execute the project without using docker, it is required that you set the connection strings inside the AccountManagement/appsettings.json file.

Changes to be made are:
1. DbConfig -> DbOptions -> ConnectionStr value should be changed with the Sql Server connection string that you have.
2. RabbitMQConfig -> RabbitMQOptions -> HostName, VirtualHost, Username, Password values should be changed with the RabbitMq platform information that you have.

In the first item if the connection string information is invalid, application will crash immediately. In order to check if the remaining settings are valid, you can use http://localhost:5000/healthchecks-ui endpoint.

