﻿using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Subscriber2
{
    class Sub2
    {
        static void Main(string[] args)
        {
            // Instanz für Client wird erstellt
            MqttClient client = new MqttClient("desktop-dpcek01");

            // Für Event registrieren, um veröffentlichte Nachrichten zu erhalten
            client.MqttMsgPublishReceived += Client_recievedMessage;

            string clientId = "client_Sub2_";
            byte status = client.Connect(clientId, "mqtt-test", "mqtt-test", false, default);

            //Console.WriteLine(status); // ungleich 0, falls Verbindung nicht aufgebaut werden konnte
            Console.Write("ZUM ABBRUCH EINE BELIEGE TASTE DRÜCKEN\n\n");
            Console.WriteLine("subscribed: /buli2, /prem\n");

            client.Subscribe(new String[] { "/buli2", "/prem" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            var stop = Console.ReadKey();
            if (stop != null)
            {
                client.Disconnect();
            }
        }

        static void Client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic.ToString();
            var message = System.Text.Encoding.Default.GetString(e.Message);
            Console.WriteLine("Message received for topic '" + topic + "': \n--------------------\n" + message + "--------------------\n");
        }
    }
}
