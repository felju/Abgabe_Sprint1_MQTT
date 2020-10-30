using System;

namespace MQTT_Result_Buli
{
    public class Spieltag_buli
    {
        string[] Bundesliga = new string[6] { "Bayern München", "Dortmund", "Bor. M'Gladbach", "Hertha BSC", "Leverkusen", "VfB Stuttgart" };
        string[] ZweiteBundesliga = new string[6] { "TSV Phoenix Lomersheim", "Karlsruher SC", "Stuttgarter Kickers", "HSG Rüppur-Bulach", "SSV Ettlingen", "HSV" };

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

        public string createSpieltagBundesliga()
        {
            ShuffleArray(Bundesliga);
            var s = "";
            s += Bundesliga[0] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + Bundesliga[1] + "\n";
            s += Bundesliga[2] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + Bundesliga[3] + "\n";
            s += Bundesliga[4] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + Bundesliga[5] + "\n";
            return s;
        }

        public string createSpieltagZweiteBundesliga()
        {
            ShuffleArray(ZweiteBundesliga);
            var s = "";
            s += ZweiteBundesliga[0] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + ZweiteBundesliga[1] + "\n";
            s += ZweiteBundesliga[2] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + ZweiteBundesliga[3] + "\n";
            s += ZweiteBundesliga[4] + "    " + Zufall.Next(0, 10) + ":" + Zufall.Next(0, 10) + "    " + ZweiteBundesliga[5] + "\n";
            return s;
        }
    }
}
