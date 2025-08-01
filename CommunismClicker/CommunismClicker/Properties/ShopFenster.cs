﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CommunismClicker
{
    public partial class ShopFenster : Form
    {
        private List<ShopButton> rectangles = new List<ShopButton>();
        private string[] nachrichten = new string[7];
        private Spielstand spielstand;

        public ShopFenster()
        {
            this.spielstand = spielstand;

            this.Text = "Shop";
            this.DoubleBuffered = true;

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.Paint += ShopFenster_Paint;
            this.MouseClick += ShopFenster_MouseClick;

            // Einzelne Upgrades hinzufügen __Muss noch besser skaliert werden
            int startX = 100;
            int currentY = 50;
            int spacing = 20;
            AddRectangle(startX, currentY, 300, 80, "Das Manifest", 25);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "Organisier Dich!", 100);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "Das Kapital", 250);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "Argumentverstärker", 500);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "Kommunismus ist wenn..", 1000);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "Kein Salut mehr", 1250);
            currentY += 80 + spacing;
            AddRectangle(startX, currentY, 300, 80, "S.E.K.", 2000);

            for (int i = 0; i < rectangles.Count && i < Spielstand.AktuellerSpielstand.Upgrades.Length; i++)
            {
                rectangles[i].Geklickt = Spielstand.AktuellerSpielstand.Upgrades[i];
            }

            nachrichten = Nachrichten();
        }

        private void ShopFenster_Load(object sender, EventArgs e)
        {

        }

        //Methode um Rechtecke(Buttons) zu erzeugen
        private void AddRectangle(int x, int y, int width, int height, string text, int preis)
        {
            rectangles.Add(new ShopButton(x, y, width, height, text, preis));
        }

        private void ShopFenster_Paint(object sender, PaintEventArgs e)
        {
            foreach (var rect in rectangles)
            {
                rect.Draw(e.Graphics);
            }
        }

        private void ShopFenster_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var (rect, index) in rectangles.Select((rect, index) => (rect, index)))
            {
                //falls auf Rechteck geklickt
                if (!rect.Geklickt && rect.Contains(e.Location))
                {
                    //falls genug Währung
                    if (Spielstand.AktuellerSpielstand.Waehrung >= rect.Preis)
                    {
                        //abziehen von Währung
                        Spielstand.AktuellerSpielstand.Waehrung -= rect.Preis;
                        //Nachrichten anzeigen
                        MessageBox.Show(nachrichten[index]);
                        //Variable auf true um zu speichern ob Upgrade gekauft wurde
                        rect.Geklickt = true;

                        //Multiplikatoren erhöhen
                        switch (index)
                        {
                            case 0: Spielstand.AktuellerSpielstand.Multiplikator += 2.0; break;
                            case 1: Spielstand.AktuellerSpielstand.Multiplikator += 5.0; break;
                            case 2: Spielstand.AktuellerSpielstand.Multiplikator += 10.0; break;
                            case 3: Spielstand.AktuellerSpielstand.Multiplikator += 15.0; break;
                            case 4: Spielstand.AktuellerSpielstand.Multiplikator += 25.0; break;
                            case 5: Spielstand.AktuellerSpielstand.Multiplikator += 50.0; break;
                            case 6: Spielstand.AktuellerSpielstand.Multiplikator += 100.0; break;
                        }

                        //Speichern 
                        Spielstand.AktuellerSpielstand.Upgrades[index] = true;
                        Spielstand.AktuellerSpielstand.Speichern(SpielstandManager.AktuellerPfad);
                        Invalidate();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Du bist noch nicht bereit!");
                    }
                }
            }
        }

        private string[] Nachrichten()
        {
            string[] nachrichten = new string[7];

            nachrichten[0] = "Du hast das Kommunistische Manifest gelesen und gelernt, dass die Geschichte von Klassenkämpfen geprägt ist und das Proletariat sich erheben muss, um eine gerechte, klassenlose Gesellschaft zu schaffen. Dabei hast du verstanden, dass nur durch die Überwindung der kapitalistischen Ausbeutung wahre Freiheit und Gleichheit für alle möglich werden.";
            nachrichten[1] = "Du hast dich organisiert. Jetzt kämpfst du gemeinsam mit deinen Genoss*innen gegen Ausbeutung und Unterdrückung, um eine solidarische und gerechte Gesellschaft aufzubauen. Zusammen seid ihr stärker und ihr gebt einander Kraft.";
            nachrichten[2] = "Du hast das Kapital gelesen und gelernt, dass der Kapitalismus durch die Aneignung des Mehrwerts der Arbeiter ihre Ausbeutung systematisch vertieft. Du hast erkannt, dass dieses System auf Ungerechtigkeit und Krisen gebaut ist und nur durch den revolutionären Sturz der kapitalistischen Herrschaft eine befreite, klassenlose Gesellschaft entstehen kann.";
            nachrichten[3] = "Du hast einen Argumentverstärker🧱 bekommen. Strength +1000";
            nachrichten[4] = "Du hast iPhones gebannt, denn wie wir alle wissen: Kommunismus ist, wenn kein iPhone. Hat Karl Marx ja selbst so geschrieben.";
            nachrichten[5] = "Du hast Elon Musk festgenommen und enteignet. Sein heart goes out to nirgendwo mehr.";
            nachrichten[6] = "Du bist Teil des Schnitzel-Entwendungs-Kommandos (SEK) und hast erfolgreich Alice Weidels Schnitzel konfisziert.";

            return nachrichten;
        }
    }

}