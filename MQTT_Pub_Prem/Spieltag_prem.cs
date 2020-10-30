using System;

namespace MQTT_Result_Prem
{
    public class Spieltag_prem
    {
        string[] PremierLeague = new string[6] { "FC Chelsea", "Manchester Untd", "Manchester City", "FC Arsenal", "FC Liverpool", "Tottenham Hotspur" };

        Random Zufall = new Random();

        void ShuffleArray(string[] arr)
        {
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int randomIndex = Zufall.Next(0, i + 1);
                string temp = arr[i];
                arr[i] = arr[randomIndex];
                arr[randomIndex] = temp;
            }
        }

        public string createSpieltagPremierLeague()
        {
            ShuffleArray(PremierLeague);
            var s = "";
            s += PremierLeague[0] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + PremierLeague[1] + "\n";
            s += PremierLeague[2] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + PremierLeague[3] + "\n";
            s += PremierLeague[4] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + PremierLeague[5] + "\n";
            return s;
        }
    }
}