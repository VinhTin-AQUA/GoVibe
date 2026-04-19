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

