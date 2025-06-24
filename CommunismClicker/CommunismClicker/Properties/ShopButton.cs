using System.Drawing;

namespace CommunismClicker
{
    internal class ShopButton
    {
        public Rectangle Area { get; private set; }
        public string Text { get; private set; }
        public bool Geklickt { get; set; }

        public ShopButton(int x, int y, int width, int height, string text)
        {
            Area = new Rectangle(x, y, width, height);
            Text = text;
            Geklickt = false;
        }

        public void Draw(Graphics g)
        {
            // Hintergrundfarbe je nach Zustand
            Color fillColor = Geklickt ? Color.DarkRed : Color.Red;

            using (Brush brush = new SolidBrush(fillColor))
            {
                g.FillRectangle(brush, Area);
            }

            using (Brush textBrush = new SolidBrush(Color.White))
            using (StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            using (Font font = new Font("Arial", 12))
            {
                g.DrawString(Text, font, textBrush, Area, sf);
            }

            // Optional: Rahmen zeichnen
            using (Pen borderPen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(borderPen, Area);
            }
        }

        public bool Contains(Point p)
        {
            return Area.Contains(p);
        }
    }
}

