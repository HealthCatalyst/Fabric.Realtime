# Fabric.Realtime
Provides a real-time messaging service where the client can subscribe to a queue to receive HL7 messages

To run this:
1. Run RabbitMQ docker:
docker run -d --hostname fabric.realtime.rabbithost --name fabric.realtime.rabbitmq rabbitmq:3
documentation: https://hub.docker.com/_/rabbitmq/

2. Run Fabric.Realtime.Web project


# To send a message:
curl -u guest:guest -H "content-type:application/json" -X POST -d'{"properties":{"delivery_mode":2},"routing_key":"fabric.interfaceengine","payload":"HI","payload_encoding":"string"}' http://localhost:15672/api/exchanges/%2F/amq.default/publish

# To view existing messages:
curl -i -u guest:guest -H "content-type:application/json" -X POST http://localhost:15672/api/queues/%2F/fabric.interfaceengine/get -d'{"count":5,"requeue":true,"encoding":"auto","truncate":50000}'
