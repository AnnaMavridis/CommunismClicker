using System.IO;

namespace CommunismClicker
{
    public partial class Form1 : Form
    {
        Image marxImage;
        int waehrung = 0;
        int level = 0;
        int upgrade = 0;
        double multiplikator = 0;
        bool finish = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);

            this.Text = "Communism Clicker";
            //Größe fest und relativ zum Bildschirm
            this.ClientSize = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.marxImage = Properties.Resources.marxImage;

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Brush brushRed = new SolidBrush(Color.Red);
            Brush brushBlack = new SolidBrush(Color.Black);

            int xMarx = (ClientSize.Width - marxImage.Width) / 2;
            int yMarx = (ClientSize.Height - marxImage.Height) / 2;
            g.DrawImage(marxImage, xMarx, yMarx);

            Rectangle upgradeButton = new Rectangle(730, 150, 55, 20);
            g.FillRectangle(brushRed, upgradeButton);
            g.DrawString("Upgrades", this.Font, brushBlack, 730, 130);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
