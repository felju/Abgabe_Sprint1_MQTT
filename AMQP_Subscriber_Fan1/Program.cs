using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Fan1
{
    public static void Main(string[] args)
    {
        /*
         *  'ConnectionFactory' und die beiden folgenden usings werden benötigt, um die Verbindung zum Server aufzubauen.
         *  
         *  Gegenüber den MQTT-Clients connecten wir uns diese mal nicht über den Hostname, greifen lokal auf den Node zu.
         *  Credentials müssen nicht angegeben werden, da in diesem Fall der default Login 'guest:guest' genutzt wird (in der RabbitMQ-Config könnte dies angepasst werden)
         *  
         *  Der Channel ist der eigentliche Kommunikationskanal. Innerhalb des Channels werden dann noch weitere Spezifikationen vorgenommen.
         *  Bei uns zählen dazu Queues anlegen, konfigurieren und an den Exchange binden.
         *  
         *  Da die Implementierung mit AMQP nicht Teil der Aufgabe war, gehen wir hier auf die genauen Konfigurationen nicht ein. 
         *  Erwähnenswert ist an dieser Stelle, dass unser AMQP-Client den selben Exchange nutzt wie die MQTT-Clients.
         */
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var queueName = channel.QueueDeclare().QueueName;
            
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [binding_key...]",
                                        Environment.GetCommandLineArgs()[0]);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }

            foreach (var bindingKey in args)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "amq_new.topic",
                                  routingKey: bindingKey);
            }

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine("\n----------\nMessage received:\n" + message + "----------");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            Console.ReadLine();
        }
    }
}