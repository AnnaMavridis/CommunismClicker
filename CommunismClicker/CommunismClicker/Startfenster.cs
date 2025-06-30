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
        private Spielstand aktuellerSpielstand = new Spielstand();

        public string HolePfad(string name)
        {
            return Path.Combine(speicherOrdner, name + ".txt");
        }
        public Startfenster()
        {
            InitializeComponent();
            Directory.CreateDirectory(speicherOrdner);
            LadeSpielstandListe();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Startfenster_Load(object sender, EventArgs e)
        {
            string pfad = Path.Combine(Application.StartupPath, "Resources", "Flag_ANTIFA.png");
            pictureBox1.Image = Image.FromFile(pfad);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }


        /// <summary>
        /// Die Methode LadeSpielstandListe lädt alle Spielstände die im Ordner spielstaende gespeichert sind.
        /// Die Spielstände werden Alphabetisch sortiert und es wird automatisch der letzte Spielstand der gespielt wurde ausgewählt.
        /// </summary>
        private void LadeSpielstandListe()
        {
            comboBoxSpielstände.Items.Clear();
            Directory.CreateDirectory(speicherOrdner);
            var dateien = Directory.GetFiles(speicherOrdner, "*.txt")
                                   .Select(f => Path.GetFileNameWithoutExtension(f))
                                   .OrderBy(name => name) 
                                   .ToList();

            foreach (var name in dateien)
                comboBoxSpielstände.Items.Add(name);

           
            if (File.Exists("letzterSpielstand.txt")) // Schaut was der letzte gespielte spielstand ist
            {
                string letzter = File.ReadAllText("letzterSpielstand.txt");
                if (comboBoxSpielstände.Items.Contains(letzter))
                {
                    comboBoxSpielstände.SelectedItem = letzter;
                }
                else if (comboBoxSpielstände.Items.Count > 0)
                {
                    comboBoxSpielstände.SelectedIndex = 0; 
                }
            }
            else if (comboBoxSpielstände.Items.Count > 0)
            {
                comboBoxSpielstände.SelectedIndex = 0;
            }
        }

        

        private void spielStarten_Click(object sender, EventArgs e) // Diese Methode Erstellt einen neuen Spielstand
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Name des neuen Spielstands:", "Neuer Spielstand", "Spielstand_" + DateTime.Now.Ticks);
            if (string.IsNullOrWhiteSpace(name)) return;

            // Neue Werte setzen
            aktuellerSpielstand.Index = 0;
            aktuellerSpielstand.Titel = name;
            aktuellerSpielstand.Waehrung = 0;
            aktuellerSpielstand.Durchgespielt = false;
            aktuellerSpielstand.Level = 1;
            aktuellerSpielstand.Multiplikator = 1.0;
            aktuellerSpielstand.Upgrades = new bool[7];

            aktuellerSpielstand.Speichern(HolePfad(name));

            LadeSpielstandListe();
            comboBoxSpielstände.SelectedItem = name;
            MessageBox.Show("Neuer Spielstand erstellt!");
        }

        private void weiterSpielen_Click(object sender, EventArgs e) // Diese Methode lädt den ausgewählten Spielstand
        {
            if (comboBoxSpielstände.SelectedItem == null)
            {
                MessageBox.Show("Bitte einen Spielstand auswählen.");
                return;
            }

            string name = comboBoxSpielstände.SelectedItem.ToString();

            // letzten Spielstand speichern
            File.WriteAllText("letzterSpielstand.txt", name);

            Spielstand.AktuellerSpielstand = aktuellerSpielstand;


            string datei = HolePfad(comboBoxSpielstände.SelectedItem.ToString());
            aktuellerSpielstand.Laden(datei);
            SpielstandManager.AktuellerPfad = datei;

            // Neues Fenster öffnen
            Form1 spielForm = new Form1(this, datei);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
