using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelDraw;
        private GroupBox groupBoxMenu;
        private RadioButton rbEqReta;
        private RadioButton rbDDA;
        private RadioButton rbPMedio;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            this.Text = "Desenhador de Retas";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.Gainsboro;
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.AutoScaleDimensions = new SizeF(9F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;

            this.panelDraw = new Panel();
            this.panelDraw.Dock = DockStyle.Fill;
            this.panelDraw.BackColor = Color.White;
            this.panelDraw.MouseDown += new MouseEventHandler(this.panelDraw_MouseDown);
            this.panelDraw.Paint += new PaintEventHandler(this.panelDraw_Paint);

            this.groupBoxMenu = new GroupBox();
            this.groupBoxMenu.Dock = DockStyle.Right;
            this.groupBoxMenu.Width = 300;
            this.groupBoxMenu.Text = "Algoritmos de Reta";
            this.groupBoxMenu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.groupBoxMenu.ForeColor = Color.DarkSlateGray;
            this.groupBoxMenu.BackColor = Color.WhiteSmoke;

            this.rbEqReta = new RadioButton();
            this.rbEqReta.Text = "Equacao da Reta";
            this.rbEqReta.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbEqReta.ForeColor = Color.DarkSlateBlue;
            this.rbEqReta.Location = new Point(20, 50);
            this.rbEqReta.AutoSize = true;
            this.rbEqReta.Checked = true;

            this.rbDDA = new RadioButton();
            this.rbDDA.Text = "DDA";
            this.rbDDA.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbDDA.ForeColor = Color.DarkSlateBlue;
            this.rbDDA.Location = new Point(20, 90);
            this.rbDDA.AutoSize = true;

            this.rbPMedio = new RadioButton();
            this.rbPMedio.Text = "Ponto Médio";
            this.rbPMedio.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbPMedio.ForeColor = Color.DarkSlateBlue;
            this.rbPMedio.Location = new Point(20, 130);
            this.rbPMedio.AutoSize = true;

            Button btnClear = new Button();
            btnClear.Text = "Limpar";
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(20, 170);
            btnClear.Size = new Size(100, 40);
            btnClear.Click += new EventHandler(this.btnClear_Click);

            this.groupBoxMenu.Controls.Add(btnClear);


            this.groupBoxMenu.Controls.Add(this.rbEqReta);
            this.groupBoxMenu.Controls.Add(this.rbDDA);
            this.groupBoxMenu.Controls.Add(this.rbPMedio);

            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.groupBoxMenu);

            this.MinimumSize = new Size(800, 600);
            this.ResumeLayout(false);
        }
    }
}
