using System;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MQTT_Result_Buli;
using System.Threading;

namespace MQTT_Pub_Buli
{
    class Pub_Buli
    {
        static void Main(string[] args)
        {
            Spieltag_buli st = new Spieltag_buli();
            var nachricht_buli1 = st.createSpieltagBundesliga();
            var nachricht_buli2 = st.createSpieltagZweiteBundesliga();

            MqttClient client = new MqttClient("desktop-dpcek01");

            string clientId = "client_Pub_Buli_";
            byte status = client.Connect(clientId, "mqtt-test", "mqtt-test", false, default);
            Console.Write("ZUM ABBRUCH EINE BELIEGE TASTE DRÜCKEN\n\n");

            client.MqttMsgPublished += client_MqttMsgPublished;

            ushort msgId_buli1 = client.Publish("/buli1", // Topic das abboniert wird
                           Encoding.UTF8.GetBytes(nachricht_buli1), // erzeugtes Spielergebnis der 1. Bundesliga
                           MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, // QoS-Level 1
                           false // 'retained' wird auf false gesetzt
                           );

            ushort msgId_buli2 = client.Publish("/buli2", // Topic das abboniert wird
                           Encoding.UTF8.GetBytes(nachricht_buli2), // erzeugtes Spielergebnis der 2. Bundesliga
                           MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, // QoS-Level 1
                           false // 'retained' wird auf false gesetzt
                           );

            var stop = Console.ReadKey();
            if (stop != null)
            {
                client.Disconnect();
            }
        }
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
