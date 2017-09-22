# Fabric.Realtime
Provides a real-time messaging service where the client can subscribe to a queue to receive HL7 messages

To run this:
1. Run RabbitMQ docker:
docker run -d --hostname fabric.realtime.rabbithost --name fabric.realtime.rabbitmq rabbitmq:3
documentation: https://hub.docker.com/_/rabbitmq/

2. Run Fabric.Realtime.Web project

