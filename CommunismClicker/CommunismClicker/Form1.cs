using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CommunismClicker
{
    public partial class Form1 : Form
    {
        Image marxImage;

        private Button saveButton;

        private RectangleF marxBereich;
        private Rectangle bereichUpgradeButton;
        private int upgradeKosten = 20;
        private float upgradeFaktor = 1.5f;
        private float fortschrittProzent = 0f;

        public double Waehrung = Spielstand.Instance.Waehrung;
        public int Level = Spielstand.Instance.Level;
        public bool[] Upgrade = Spielstand.Instance.Upgrades;
        public double Multiplikator = Spielstand.Instance.Multiplikator;
        public bool Durchgespielt = Spielstand.Instance.Durchgespielt;

        private Label waehrungLabel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Spielstand.Instance.Laden();
            Waehrung = Spielstand.Instance.Waehrung;
            Level = Spielstand.Instance.Level;
            Multiplikator = Spielstand.Instance.Multiplikator;
            Durchgespielt = Spielstand.Instance.Durchgespielt;
            Upgrade = Spielstand.Instance.Upgrades;

            this.saveButton = new Button();
            this.saveButton.Text = "Spielstand speichern";
            this.saveButton.Font = new Font("Arial", 10, FontStyle.Regular);
            this.saveButton.Size = new Size(180, 40);
            this.saveButton.Location = new Point(20, 60);
            this.saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);


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
            this.waehrungLabel.Text = $"Währung: {Waehrung} ☭";

            this.Controls.Add(this.waehrungLabel);
            this.Resize += (s, ev) => this.Invalidate();

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Spielstand.Instance.Waehrung = Convert.ToInt32(Waehrung);
            Spielstand.Instance.Level = Level;
            Spielstand.Instance.Multiplikator = Multiplikator;
            Spielstand.Instance.Durchgespielt = Durchgespielt;
            Spielstand.Instance.Upgrades = Upgrade;

            Spielstand.Instance.Speichern();

            MessageBox.Show("Spielstand gespeichert!");
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

            fortschrittProzent = Math.Min(1f, (float)Waehrung / upgradeKosten);

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
                Waehrung += Convert.ToInt32(Multiplikator);
                waehrungLabel.Text = $"Währung: {Waehrung} ☭";
                Invalidate();
            }
            else if (bereichUpgradeButton.Contains(e.Location))
            {
                ShopFenster shop = new ShopFenster();
                shop.ShowDialog();
            } 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
