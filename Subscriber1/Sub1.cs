using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTest
{
    class Sub1
    {
        static void Main(string[] args)
        {
            //Instanz der Klasse MqttClient wird erzeugt (Package: uPLibrary.Networking.M2Mqtt, MQTT für C#)
            //Parameter: Hostname/IP für Zugriff auf MQTT-Broker als String übergeben
            MqttClient client = new MqttClient("desktop-dpcek01");

            /*
             * Der Client registriert sich für Event, um veröffentlichte Nachrichten zu erhalten
             * Die erhaltene Nachricht wird dem EventHandler MqttMsgPublishReceived angefügt
             */
            client.MqttMsgPublishReceived += Client_recievedMessage;

            string clientId = "client_Sub1_"; // sinnhafte Benamung, um über Webinterface auf Client schließen zu können

            /*
             *  über Connect wird sich mit dem Broker verbunden
             *  weitere Parameter werden der Methode übergeben:
             *  - clientId: mit dieser Id verbindet sich unser Client mit dem Broker
             *  - username + passwort: Credentials, die zum Herstellen einer Verbindung berechtigen (entsprechender User wurde separat in RabbitMQ angelegt)
             *  - cleanSession: haben wir auf 'false' gesetzt, um eine persistente Sitzung aufzubauen - anderenfalls würde die Abwesenheits-Queue des Clients geleert werden
             *                  und er würde keine verpassten Nachrichten nach einem neuen Verbindungsaufbau erhalten
             *  - keepAlivePeriod: Zeitangebe in Millisekunden, in der maximal keine Koomunikation zwischen Cleint und Broker stattfindet
             *                      --> da bei uns der default Wert in allen Testdurchläufen funktioniert hat, haben wir diesen nicht individuell angepasst
             *  
             *  Außerdem wird der Rückgabewert noch in ein Byte geschrieben. Dies half beim Troubleshooting zu Beginn. Inzwischen ist die Ausgabe auskommentiert.
             *  Ist 'status' ungleich 0, konnte keine Verbindung aufgebaut werden
             */
            byte status = client.Connect(clientId, "mqtt-test", "mqtt-test", false, default);
            //Console.WriteLine(status);

            Console.Write("ZUM ABBRUCH EINE BELIEGE TASTE DRÜCKEN\n\n");
            Console.WriteLine("subscribed: /buli1, /buli2\n");

            /*
             * Der Subscribe-Methode werden als Parameter die Arrays der Topics und der QoS-Levels mitgegeben
             * Zu beachten ist, dass für jedes weitere Topic ein weiterer Eintrag im Array des QoS-Levels ergänzt werden muss
             * 
             * Informationen über die QoS-Levels haben wir unter anderem hier bezogen: https://mntolia.com/mqtt-qos-levels-explained/
             * In unserem Fall QoS1, um zu gewährleisten, dass unsere Nachrichten zuverlässig zugestellt/empfangen werden.
             */
            client.Subscribe(new String[] { "/buli1", "/buli2" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            // durch die folgenden fünf Zeilen kann des Programm durch die Eingabe einer beliebigen Taste unterbrochen werden
            var stop = Console.ReadKey();
            if (stop!=null)
            {
                client.Disconnect();
            }
        }

        /*
         * Funktion zum Anfügen der Nachrichten an den EventHandler
         * Neben der eigentlichen Nachricht geben wir der Übersicht geschuldet noch das Topic mit aus
         */
        static void Client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic.ToString();
            var message = System.Text.Encoding.Default.GetString(e.Message);
            Console.WriteLine("Message received for topic '" + topic + "': \n--------------------\n" + message + "--------------------\n");
        }

    }
}