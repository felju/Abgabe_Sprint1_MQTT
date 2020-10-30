using System;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MQTT_Result_Prem;
using System.Threading;

namespace MQTT_Pub_Prem
{
    class Pub_Prem
    {
        public static void Main(string[] args)
        {
            Spieltag_prem st = new Spieltag_prem();
            var nachricht_prem = st.createSpieltagPremierLeague();

            MqttClient client = new MqttClient("desktop-dpcek01");

            string clientId = "client_Pub_Prem_";
            byte status = client.Connect(clientId, "mqtt-test", "mqtt-test", false, default);
            Console.Write("ZUM ABBRUCH EINE BELIEGE TASTE DRÜCKEN\n\n");

            client.MqttMsgPublished += client_MqttMsgPublished;

            ushort msgId_prem = client.Publish("/prem", // Topic das abboniert wird
                           Encoding.UTF8.GetBytes(nachricht_prem), // erzeugtes Spielergebnis der 1. Bundesliga
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
