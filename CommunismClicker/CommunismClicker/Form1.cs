using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CommunismClicker
{
    public partial class Form1 : Form
    {
        Image marxImage;
        private RectangleF marxBereich;

        private Rectangle bereichUpgradeButton;
        private int upgradeKosten = 20;
        private float upgradeFaktor = 1.5f;
        private float fortschrittProzent = 0f;

        int waehrung = 0;
        int level = 0;
        int upgrade = 0;
        double multiplikator = 1;
        bool finish = false;

        private Label waehrungLabel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.MouseClick += new MouseEventHandler(Form1_MouseClick);

            this.Text = "Communism Clicker";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.marxImage = Properties.Resources.marxImage;

            this.waehrungLabel = new Label();
            this.waehrungLabel.AutoSize = true;
            this.waehrungLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            this.waehrungLabel.Location = new Point(20, 20);
            this.waehrungLabel.Text = $"Währung: {waehrung} ☭";

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

            fortschrittProzent = Math.Min(1f, (float)waehrung / upgradeKosten);

            int balkenBreite = (int)(buttonBreite * fortschrittProzent);
            int balkenHoehe = 16;
            int balkenX = 2;
            int balkenY = 4;

            Rectangle progressBar = new Rectangle(balkenX, balkenY, balkenBreite, balkenHoehe);

            g.FillRectangle(Brushes.Black, new Rectangle(balkenX, balkenY, buttonBreite - 4, balkenHoehe));
            g.FillRectangle(Brushes.Yellow, progressBar);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (marxBereich.Contains(e.Location))
            {
                waehrung += Convert.ToInt32(multiplikator);
                waehrungLabel.Text = $"Währung: {waehrung} ☭";
                Invalidate();
            }
            else if (bereichUpgradeButton.Contains(e.Location))
            {
                //Öffnen Upgrade Fenster
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
