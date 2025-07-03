using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunismClicker
{
    public static class SpielstandManager
    {
        public static string AktuellerPfad { get; set; } //Public string der den Pfad zum aktuell geladenen Spielstand enthält

        public static string SpeicherOrdner = "spielstaende";

        public static string HolePfad(string name)
        {
            return Path.Combine(SpeicherOrdner, name + ".txt");
        }
    }
}