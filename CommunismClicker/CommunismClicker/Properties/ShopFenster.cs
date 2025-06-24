using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CommunismClicker
{
    public partial class ShopFenster : Form
    {
       

        private List<ShopButton> rectangles = new List<ShopButton>();

        public ShopFenster()
        {
            this.Text = "blub blub";
            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.Paint += ShopFenster_Paint;
            this.MouseClick += ShopFenster_MouseClick;

            // Einzelne Upgrades hinzufügen
            AddRectangle(50, 50, 150, 80, "1.Upgrade");
            AddRectangle(250, 50, 150, 80, "2.Upgrade");
            AddRectangle(150, 180, 200, 100, "3.Upgrade");

           
        }

        private void ShopFenster_Load(object sender, EventArgs e)
        {
           
        }

        private void AddRectangle(int x, int y, int width, int height, string text)
        {
            rectangles.Add(new ShopButton(x, y, width, height, text));
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
            foreach (var rect in rectangles)
            {
                if (!rect.Geklickt && rect.Contains(e.Location))
                {
                    rect.Geklickt = true;
                    MessageBox.Show($"Rechteck '{rect.Text}' wurde geklickt!");
                    Invalidate(); // Neu zeichnen
                    break; // Nur eines pro Klick
                }
            }
        }
    }

}


