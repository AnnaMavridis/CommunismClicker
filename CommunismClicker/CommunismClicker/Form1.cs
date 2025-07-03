using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CommunismClicker
{
    public partial class Form1 : Form
    {
        private Image marxImage;
        private Image stern;
        private Image[] hintergruende = new Image[9];

        // Rechtecke für Klickbereiche
        private RectangleF marxBereich;
        private Rectangle bereichUpgradeButton;
        private Rectangle zurueckButton;

        private int[] levelKosten;
        private float fortschrittProzent = 0f;
        private string pfad;
        private string[] levelText;

        Spielstand spielstand = new Spielstand();

        public double Waehrung;
        public int Level;
        public bool[] Upgrade;
        public double Multiplikator;
        public bool Durchgespielt;

        // UI-Labels
        private Label waehrungLabel;
        private Label levelLabel;

        private float bildSkalierung = 1.0f;
        private System.Windows.Forms.Timer animationsTimer;
        private int animationsSchritte = 0;
        private const int maxSchritte = 5;
        private const float skalierungProSchritt = 0.02f;

        private Startfenster startFenster;
        public Form1(Startfenster start, string pPfad)
        {
            InitializeComponent();
            this.KeyPreview = true; // Wichtig!
            this.KeyDown += Form1_KeyDown;
            startFenster = start;
            pfad = pPfad;
            animationsTimer = new System.Windows.Forms.Timer();
            animationsTimer.Interval = 30; // alle 30ms
            animationsTimer.Tick += AnimationsTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            spielstand.Laden(pfad);

            Waehrung = spielstand.Waehrung;
            Level = spielstand.Level;
            Upgrade = spielstand.Upgrades;
            Multiplikator = spielstand.Multiplikator;
            Durchgespielt = spielstand.Durchgespielt;

            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);

            this.Text = "Communism Clicker";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;

            BilderSetzen();
            LevelTextSetzen();

            this.waehrungLabel = new Label();
            this.waehrungLabel.AutoSize = true;
            this.waehrungLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            this.waehrungLabel.Location = new Point(20, 20);
            this.waehrungLabel.Text = $"Währung: {Convert.ToInt32(Spielstand.AktuellerSpielstand.Waehrung)} ☭";

            this.levelLabel = new Label();
            this.levelLabel.AutoSize = true;
            this.levelLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            this.levelLabel.Location = new Point(ClientSize.Width/2, 30);
            this.levelLabel.Text = levelText[Level];

            this.Controls.Add(this.waehrungLabel);
            this.Controls.Add(this.levelLabel);
            this.Resize += (s, ev) => this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            e.Graphics.DrawImage(hintergruende[Level], new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height));

            Brush brushRed = new SolidBrush(Color.Red);
            Brush brushBlack = new SolidBrush(Color.Black);

            float zielBreite = ClientSize.Width * 0.3f;
            float zielHoehe = ClientSize.Height * 0.3f;

            float bildSeitenverhaeltnis = (float)marxImage.Width / marxImage.Height;
            float fensterSeitenverhaeltnis = zielBreite / zielHoehe;

            float zeichnungsBreite, zeichnungsHoehe;

            if (fensterSeitenverhaeltnis > bildSeitenverhaeltnis)
            {
                zeichnungsHoehe = zielHoehe;
                zeichnungsBreite = zeichnungsHoehe * bildSeitenverhaeltnis;
            }
            else
            {
                zeichnungsBreite = zielBreite;
                zeichnungsHoehe = zeichnungsBreite / bildSeitenverhaeltnis;
            }

            float xMarx = (ClientSize.Width - zeichnungsBreite) / 2f;
            float yMarx = (ClientSize.Height - zeichnungsHoehe) / 2f;

            marxBereich = new RectangleF(xMarx, yMarx, zeichnungsBreite, zeichnungsHoehe);
            g.DrawImage(marxImage, marxBereich);

            float skaliertBreite = zeichnungsBreite * bildSkalierung; // Animation beim Klicken
            float skaliertHoehe = zeichnungsHoehe * bildSkalierung;
            float skaliertX = (ClientSize.Width - skaliertBreite) / 2f;
            float skaliertY = (ClientSize.Height - skaliertHoehe) / 2f;

            marxBereich = new RectangleF(skaliertX, skaliertY, skaliertBreite, skaliertHoehe);
            g.DrawImage(marxImage, marxBereich);


            int buttonBreite = (int)(ClientSize.Width * 0.15f);
            int buttonHoehe = (int)(ClientSize.Height * 0.08f);
            int abstandVomRand = 30;

            int buttonX = ClientSize.Width - buttonBreite - abstandVomRand;
            int buttonY = ClientSize.Height - buttonHoehe - abstandVomRand;

            bereichUpgradeButton = new Rectangle(buttonX, buttonY, buttonBreite, buttonHoehe);

            g.FillRectangle(Brushes.DarkRed, bereichUpgradeButton);
            g.DrawRectangle(Pens.Black, bereichUpgradeButton);

            string upgradeText = "Upgrades";
            SizeF textSize = g.MeasureString(upgradeText, this.Font);
            g.DrawString(upgradeText, this.Font, Brushes.White,
                buttonX + (buttonBreite - textSize.Width) / 2,
                buttonY + (buttonHoehe - textSize.Height) / 2 - 10);

            zurueckButton = new Rectangle(buttonX, 22, buttonBreite/2, buttonHoehe/2);

            g.FillRectangle(brushRed, zurueckButton);
            g.DrawRectangle(Pens.Black, zurueckButton);

            string zurueckText = "Zurück";
            g.DrawString(zurueckText, this.Font, Brushes.White,
                buttonX + (buttonBreite/2 - textSize.Width) / 2,
                22 + (buttonHoehe/2 - textSize.Height) / 2);

            fortschrittProzent = Math.Min(1f, (float)Spielstand.AktuellerSpielstand.Waehrung / levelKosten[Level+1]);

            int balkenBreite = (int)(buttonBreite * fortschrittProzent);
            int balkenHoehe = 16;
            int balkenX = ClientSize.Width/2 - buttonBreite/2;
            int balkenY = ClientSize.Height - 40;

            Rectangle progressBar = new Rectangle(balkenX, balkenY, balkenBreite, balkenHoehe);

            g.FillRectangle(Brushes.Black, new Rectangle(balkenX, balkenY, buttonBreite - 2, balkenHoehe));
            g.FillRectangle(Brushes.Yellow, progressBar);

            if (Durchgespielt)
            {
                int sternMaß = ClientSize.Width / 32;
                int sternRand = sternMaß / 3;

                g.DrawImage(stern, sternRand, ClientSize.Height-(sternRand*4), sternMaß, sternMaß);
            }
        }
        
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (marxBereich.Contains(e.Location))
            {
                animationsSchritte = 0;
                animationsTimer.Start();
                Spielstand.AktuellerSpielstand.Waehrung += Convert.ToInt32(Multiplikator);
                waehrungLabel.Text = $"Währung: {Spielstand.AktuellerSpielstand.Waehrung} ☭";
                if (Spielstand.AktuellerSpielstand.Waehrung >= 200000)
                {
                    Spielstand.AktuellerSpielstand.Waehrung = 0;
                    Level = 0;
                    Durchgespielt = true;
                    Multiplikator = 1;
                    Upgrade = new bool[7];
                }
                else if (Spielstand.AktuellerSpielstand.Waehrung >= levelKosten[Level+1])
                {
                    Level++;
                    levelLabel.Text = levelText[Level];
                    if (Level == 9) 
                    {
                        MessageBox.Show("Ein erdlicher Schmetterling fliegt in einen kolibriartigen Alien des Planeten QWEZUP-3500. Das Universum implodiert und reißt alles in sich in Stücke. 200 Milliarden Jahre später, in einer anderen Dimension, hat ein Atze / eine Atzin eine gute Idee.");
                    }
                }
                Invalidate();
            }
            else if (bereichUpgradeButton.Contains(e.Location))
            {
                ShopFenster shop = new ShopFenster();
                shop.ShowDialog();
            }
            else if (zurueckButton.Contains(e.Location))
            {
                ZurueckZumMenu();
            }
        }

        //Escape als Tastendruck ruft die Zurück-Methode auf
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ZurueckZumMenu();
            }
        }

        //Array für alle Hintergrundbilder wird gesetzt. Bilder werden aus den VS Ressources genommen
        private void BilderSetzen()
        {
            marxImage = Properties.Resources.marxImage;
            stern = Properties.Resources.RoterStern;
            hintergruende[0] = Properties.Resources.Parkbank;
            hintergruende[1] = Properties.Resources.Viertel;
            hintergruende[2] = Properties.Resources.Stadt;
            hintergruende[3] = Properties.Resources.Land;
            hintergruende[4] = Properties.Resources.Kontinent;
            hintergruende[5] = Properties.Resources.Planet;
            hintergruende[6] = Properties.Resources.Galaxie;
            hintergruende[7] = Properties.Resources.Universum;
            hintergruende[8] = Properties.Resources.Explosion;
        }

        //Level Nachrrichten und Erreichpunkte werden gesetzt
        private void LevelTextSetzen()
        {
            levelText = new string[9];
            levelText[0] = "Du hast eine gute Idee!";
            levelText[1] = "Dein Viertel hat eine gute Idee!";
            levelText[2] = "Deine Stadt hat eine gute Idee!";
            levelText[3] = "Dein Land hat eine gute Idee!";
            levelText[4] = "Dein Kontinent hat eine gute Idee!";
            levelText[5] = "Dein Planet hat eine gute Idee!";
            levelText[6] = "Deine Galaxie hat eine gute Idee!";
            levelText[7] = "Dein Universum hat eine gute Idee!";
            levelText[8] = "Selbst in der Super Nova hast Du eine gute Idee!";

            levelKosten = new int[10];
            levelKosten[0] = 0;
            levelKosten[1] = 50;
            levelKosten[2] = 150;
            levelKosten[3] = 500;
            levelKosten[4] = 1500;
            levelKosten[5] = 5000;
            levelKosten[6] = 15000;
            levelKosten[7] = 50000;
            levelKosten[8] = 150000;
            levelKosten[9] = 200000;
        }

        private void ZurueckZumMenu()
        {
            DialogResult result = MessageBox.Show("Möchtest du speichern?", "Zurück zum Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                spielstand.Waehrung = Convert.ToInt32(Spielstand.AktuellerSpielstand.Waehrung);
                spielstand.Level = Level;
                spielstand.Multiplikator = Multiplikator;
                spielstand.Durchgespielt = Durchgespielt;
                spielstand.Upgrades = Upgrade;

                if (SpielstandManager.AktuellerPfad != null)
                {
                    spielstand.Speichern(SpielstandManager.AktuellerPfad);
                    MessageBox.Show("Spielstand gespeichert!");
                }
                else
                {
                    MessageBox.Show("Fehler: Kein Speicherpfad bekannt!");
                }
            }
            startFenster.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void AnimationsTimer_Tick(object sender, EventArgs e)
        {
            if (animationsSchritte < maxSchritte)
            {
                bildSkalierung += skalierungProSchritt;
            }
            else if (animationsSchritte < maxSchritte * 2)
            {
                bildSkalierung -= skalierungProSchritt;
            }
            else
            {
                bildSkalierung = 1.0f;
                animationsTimer.Stop();
            }

            animationsSchritte++;
            Invalidate(); // Neuzeichnen auslösen
        }
    }
}
