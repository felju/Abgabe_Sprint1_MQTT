using System;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MQTT_Result_Prem;
using System.Threading;

namespace MQTT_Pub_Prem
{
    // Producer/Publisher für Nachrichten bezüglich der Premier-League
    class Pub_Prem
    {
        public static void Main(string[] args)
        {
            //erzeugt Instanz eines Spieltags
            Spieltag_prem st = new Spieltag_prem();

            //zufällige Spieltagbegegnungen der 1. und 2. Liga mit Ergebnissen werden erzeugt und den Variablen für Nachrichten zugewiesen
            var nachricht_prem = st.createSpieltagPremierLeague();

            //Instanz der Klasse MqttClient wird erzeugt (Package: uPLibrary.Networking.M2Mqtt, MQTT für C#)
            //Parameter: Hostname/IP für Zugriff auf MQTT-Broker als String übergeben
            MqttClient client = new MqttClient("desktop-dpcek01");

            string clientId = "client_Pub_Prem_"; // sinnhafte Benamung, um über Webinterface auf Client schließen zu können

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

            // Client registriert sich beim Event, um über erfolgreiche/nicht erfolgreiche Übertragung von Nachrichten informiert zu werden, beim Aufruf von Publish().
            client.MqttMsgPublished += client_MqttMsgPublished;

            /* 
             * Publish() verschickt die Nachrichten
             * Parameter, die mit übergeben werden:
             *  - Topic, unter welchen published wird als String
             *  - eigentliche Nachricht, codiert als Array von Bytes
             *  - Quality of Services Level als Byte, in unserem Fall QoS1, um zu gewährleisten, dass unsere Nachrichten zuverlässig zugestellt/empfangen werden.
             *    Informationen über die QoS-Levels haben wir unter anderem hier bezogen: https://mntolia.com/mqtt-qos-levels-explained/
             *  - Angabe von retained als Boolean, wenn true, dann wird die letzte Nachricht, die erfolgreich übermittelt wurde gestored und immer wieder an CLient weitergegeben.
             *    In unserem Fall false, da wir aus Gründen der Übersichtlichkeit darauf verzichten und es als unnötig ansehen, eine bereits erfolgreiche Nachricht immer wieder zuzustellen.
             */
            ushort msgId_prem = client.Publish("/prem", // Topic das abboniert wird
                           Encoding.UTF8.GetBytes(nachricht_prem), // erzeugtes Spielergebnis der 1. Bundesliga
                           MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, // QoS-Level 1
                           false // 'retained' wird auf false gesetzt
                           );

            /* 
             *  Mit Disconnect() kann die Verbindung von Client zum Broker wieder abgebaut werden.
             *  Durch das Betätigen einer beliebigen Taste ermöglichen wir es unseren Client die Verbindung direkt ordentlich abzubauen.
             */
            var stop = Console.ReadKey();
            if (stop != null)
            {
                client.Disconnect();
            }
        }

        /* 
         * Funktion zum Anfügen der Information an den EventHandler.
         * Client wird beim Publishen der Nachricht darüber informiert, ob Vorgang erfolgreich/nicht erfolgreich war.
         */
        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            if (e.IsPublished)
            {
                Console.WriteLine("Übertragung für Topic abgeschlossen");
            }
            else
            {
                Console.WriteLine("Übertragung fehlgeschlagen");
                Debug.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
            }
        }
    }
}
