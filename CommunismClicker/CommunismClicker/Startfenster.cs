using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommunismClicker
{
    public partial class Startfenster : Form
    {
        
        private readonly string speicherOrdner = "spielstaende";
        public Startfenster()
        {
            InitializeComponent();
            Directory.CreateDirectory(speicherOrdner);
            LadeSpielstandListe();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void LadeSpielstandListe()
        {
            comboBoxSpielstände.Items.Clear(); // alte Liste leeren

            // Stelle sicher, dass der Ordner existiert
            Directory.CreateDirectory(speicherOrdner);

            // Spielstand-Dateien holen (alle .txt-Dateien im Ordner)
            string[] dateien = Directory.GetFiles(speicherOrdner, "*.txt");

            // Nur den Dateinamen ohne Pfad und ohne Erweiterung anzeigen
            foreach (string datei in dateien)
            {
                string name = Path.GetFileNameWithoutExtension(datei);
                comboBoxSpielstände.Items.Add(name);
            }

            // Optional: ersten Eintrag auswählen
            if (comboBoxSpielstände.Items.Count > 0)
                comboBoxSpielstände.SelectedIndex = 0;
        }

        public string HolePfad(string name)
        {
            return Path.Combine(speicherOrdner, name + ".txt");
        }
        private void Startfenster_Load(object sender, EventArgs e)
        {

        }

        private void spielStarten_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name des neuen Spielstands:", "Neuer Spielstand", "Spielstand_" + DateTime.Now.Ticks);
            if (string.IsNullOrWhiteSpace(name)) return;

            // Neue Werte setzen
            Spielstand.Instance.Index = 0;
            Spielstand.Instance.Titel = name;
            Spielstand.Instance.Waehrung = 0;
            Spielstand.Instance.Durchgespielt = false;
            Spielstand.Instance.Level = 1;
            Spielstand.Instance.Multiplikator = 1.0;
            Spielstand.Instance.Upgrades = new bool[7];
            
            Spielstand.Instance.Speichern(HolePfad(name));

            LadeSpielstandListe();
            comboBoxSpielstände.SelectedItem = name;
            MessageBox.Show("Neuer Spielstand erstellt!");
        }

        private void weiterSpielen_Click(object sender, EventArgs e)
        {
            if (comboBoxSpielstände.SelectedItem == null)
            {
                MessageBox.Show("Bitte einen Spielstand auswählen.");
                return;
            }

            string datei = HolePfad(comboBoxSpielstände.SelectedItem.ToString());
            Spielstand.Instance.Laden(datei);
            SpielstandManager.AktuellerPfad = datei;

            // Neues Fenster öffnen
            Form1 spielForm = new Form1(this);
            spielForm.Show();

            // Startfenster ausblenden oder schließen
            this.Hide();
        }

        private void spielVerlassen_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void comboBoxSpielstände_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
