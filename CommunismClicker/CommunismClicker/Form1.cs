using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CommunismClicker
{
    public partial class Form1 : Form
    {
        Image marxImage;
        Image stern;

        private RectangleF marxBereich;
        private Rectangle bereichUpgradeButton;
        private Rectangle zurueckButton;
        private int upgradeKosten = 20;
        private float upgradeFaktor = 1.5f;
        private float fortschrittProzent = 0f;
        private string pfad;

        Spielstand spielstand = new Spielstand();

        public double Waehrung;
        public int Level;
        public bool[] Upgrade;
        public double Multiplikator;
        public bool Durchgespielt;

        private Label waehrungLabel;

        private Startfenster startFenster;
        public Form1(Startfenster start, string pPfad)
        {
            InitializeComponent();
            this.KeyPreview = true; // Wichtig!
            this.KeyDown += Form1_KeyDown;
            startFenster = start;
            pfad = pPfad;
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

            this.marxImage = Properties.Resources.marxImage;
            this.stern = Properties.Resources.RoterStern;

            this.waehrungLabel = new Label();
            this.waehrungLabel.AutoSize = true;
            this.waehrungLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            this.waehrungLabel.Location = new Point(20, 20);
            this.waehrungLabel.Text = $"Währung: {Convert.ToInt32(Waehrung)} ☭";

            this.Controls.Add(this.waehrungLabel);
            this.Resize += (s, ev) => this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

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

            zurueckButton = new Rectangle(buttonX, 10, buttonBreite/2, buttonHoehe/2);

            g.FillRectangle(Brushes.Red, zurueckButton);
            g.DrawRectangle(Pens.Black, zurueckButton);

            string zurueckText = "Zurück";
            g.DrawString(zurueckText, this.Font, Brushes.White,
                buttonX + (buttonBreite/2 - textSize.Width) / 2,
                10 + (buttonHoehe/2 - textSize.Height) / 2);

            fortschrittProzent = Math.Min(1f, (float)Waehrung / upgradeKosten);

            int balkenBreite = (int)(buttonBreite * fortschrittProzent);
            int balkenHoehe = 16;
            int balkenX = 2;
            int balkenY = 4;

            Rectangle progressBar = new Rectangle(balkenX, balkenY, balkenBreite, balkenHoehe);

            g.FillRectangle(Brushes.Black, new Rectangle(balkenX, balkenY, buttonBreite - 4, balkenHoehe));
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
                Waehrung += Convert.ToInt32(Multiplikator);
                waehrungLabel.Text = $"Währung: {Waehrung} ☭";
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ZurueckZumMenu();
            }
        }

        private void ZurueckZumMenu()
        {
            DialogResult result = MessageBox.Show("Möchtest du speichern?", "Zurück zum Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                spielstand.Waehrung = Convert.ToInt32(Waehrung);
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
    }
}
