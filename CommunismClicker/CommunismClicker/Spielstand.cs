using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunismClicker
{
    internal class Spielstand
    {
        public int Index = 0;
        public string Titel = "";
        public int Waehrung = 0;
        public bool Durchgespielt = false;
        public int Level = 0;
        public double Multiplikator = 1.0;
        public bool[] Upgrades = new bool[7];
        
        public void Speichern(string pfad)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(pfad))
                {
                    string upgradesString = string.Join("&&", Array.ConvertAll(Upgrades, u => u.ToString()));
                    string zeile = $"{Index}&&{Titel}&&{Waehrung}&&{Durchgespielt}&&{Level}&&{Multiplikator}&&{upgradesString}";
                    writer.WriteLine(zeile);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern: " + ex.Message);
            }
        }

        public void Laden(string pfad)
        {
            if (!File.Exists(pfad)) return;

            string[] teile = File.ReadAllText(pfad).Split(new[] { "&&" }, StringSplitOptions.None);
            if (teile.Length < 13) return;

            Index = int.Parse(teile[0]);
            Titel = teile[1];
            Waehrung = int.Parse(teile[2]);
            Durchgespielt = bool.Parse(teile[3]);
            Level = int.Parse(teile[4]);
            Multiplikator = double.Parse(teile[5]);

            for (int i = 0; i < 7; i++)
                Upgrades[i] = bool.Parse(teile[6 + i]);
        }
    }
}
