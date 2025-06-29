namespace CommunismClicker
{
    partial class Startfenster
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            spielStarten = new Button();
            weiterSpielen = new Button();
            spielVerlassen = new Button();
            comboBoxSpielstände = new ComboBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // spielStarten
            // 
            spielStarten.Location = new Point(48, 69);
            spielStarten.Name = "spielStarten";
            spielStarten.Size = new Size(153, 62);
            spielStarten.TabIndex = 0;
            spielStarten.Text = "Neuen Spielstand erstellen";
            spielStarten.UseVisualStyleBackColor = true;
            spielStarten.Click += spielStarten_Click;
            // 
            // weiterSpielen
            // 
            weiterSpielen.Location = new Point(48, 189);
            weiterSpielen.Name = "weiterSpielen";
            weiterSpielen.Size = new Size(153, 62);
            weiterSpielen.TabIndex = 1;
            weiterSpielen.Text = "weiter Spielen";
            weiterSpielen.UseVisualStyleBackColor = true;
            weiterSpielen.Click += weiterSpielen_Click;
            // 
            // spielVerlassen
            // 
            spielVerlassen.Location = new Point(602, 332);
            spielVerlassen.Name = "spielVerlassen";
            spielVerlassen.Size = new Size(155, 78);
            spielVerlassen.TabIndex = 2;
            spielVerlassen.Text = "Spiel Verlassen";
            spielVerlassen.UseVisualStyleBackColor = true;
            spielVerlassen.Click += spielVerlassen_Click;
            // 
            // comboBoxSpielstände
            // 
            comboBoxSpielstände.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSpielstände.FormattingEnabled = true;
            comboBoxSpielstände.Location = new Point(48, 316);
            comboBoxSpielstände.Name = "comboBoxSpielstände";
            comboBoxSpielstände.Size = new Size(153, 28);
            comboBoxSpielstände.TabIndex = 3;
            comboBoxSpielstände.SelectedIndexChanged += comboBoxSpielstände_SelectedIndexChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(0, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(798, 447);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Startfenster
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(comboBoxSpielstände);
            Controls.Add(spielVerlassen);
            Controls.Add(weiterSpielen);
            Controls.Add(spielStarten);
            Controls.Add(pictureBox1);
            Name = "Startfenster";
            Text = "Startfenster";
            Load += Startfenster_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button spielStarten;
        private Button weiterSpielen;
        private Button spielVerlassen;
        private ComboBox comboBoxSpielstände;
        private PictureBox pictureBox1;
    }
}