# Devlopment

## Migration

- get

```bash
dotnet ef migrations list --project GoVibe.Infrastructure --startup-project GoVibe.API
```

- Add

```bash
dotnet ef migrations add InitialCreate --project GoVibe.Infrastructure --startup-project GoVibe.API
```

- update database

```bash
dotnet ef database update --project GoVibe.Infrastructure --startup-project GoVibe.API

dotnet ef database update 0 --project GoVibe.Infrastructure --startup-project GoVibe.API
```

- remove migration

```bash
dotnet ef migrations remove --project GoVibe.Infrastructure --startup-project GoVibe.API
```

## Frontend

```bash
ng new my-workspace --no-create-application

cd my-workspace
ng generate application my-app
ng generate library my-lib

ng g library components --project-root=libs/components;

ng s dashboard
```

## Services

- command

```bash
# garage installed
# postgreSQL installed
# RabbitMQ installed
# Elastic installed

# PostgreSQL
sudo systemctl unmask postgresql
sudo systemctl start postgresql

# Elasticsearch
elasticsearch-9.3.1/bin/elasticsearch

# RabbitMQ 
rabbitmq_server-3.12.14/sbin/rabbitmq-server
./sbin/rabbitmqctl stop
./sbin/rabbitmqctl shutdown
# http://localhost:15672

# garage

```
