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
        private RadioButton rbEqCirc;
        private RadioButton rbPMCirc;
        private RadioButton rbCircPoligono;
        private RadioButton rbElipse;
        private MenuStrip menuStrip;
        private ToolStripMenuItem algoritmosToolStripMenuItem;
        private ToolStripMenuItem poligonosToolStripMenuItem;
        private ListBox listBoxPolygons;
        private TextBox labelPolygonPoints;

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

            this.Text = "Desenhador de Retas, Circunferências e Elipses";
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
            this.groupBoxMenu.Text = "Algoritmos";
            this.groupBoxMenu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.groupBoxMenu.ForeColor = Color.DarkSlateGray;
            this.groupBoxMenu.BackColor = Color.WhiteSmoke;

            this.rbEqReta = new RadioButton();
            this.rbEqReta.Text = "Equação da Reta";
            this.rbEqReta.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbEqReta.ForeColor = Color.DarkSlateBlue;
            this.rbEqReta.Location = new Point(20, 50);
            this.rbEqReta.AutoSize = true;
            this.rbEqReta.Checked = true;

            this.rbDDA = new RadioButton();
            this.rbDDA.Text = "Reta (DDA)";
            this.rbDDA.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbDDA.ForeColor = Color.DarkSlateBlue;
            this.rbDDA.Location = new Point(20, 80);
            this.rbDDA.AutoSize = true;

            this.rbPMedio = new RadioButton();
            this.rbPMedio.Text = "Reta (Ponto Médio)";
            this.rbPMedio.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbPMedio.ForeColor = Color.DarkSlateBlue;
            this.rbPMedio.Location = new Point(20, 110);
            this.rbPMedio.AutoSize = true;

            this.rbEqCirc = new RadioButton();
            this.rbEqCirc.Text = "Circunf. (Equação)";
            this.rbEqCirc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbEqCirc.ForeColor = Color.DarkSlateBlue;
            this.rbEqCirc.Location = new Point(20, 140);
            this.rbEqCirc.AutoSize = true;

            this.rbPMCirc = new RadioButton();
            this.rbPMCirc.Text = "Circunf. (Pto Médio)";
            this.rbPMCirc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbPMCirc.ForeColor = Color.DarkSlateBlue;
            this.rbPMCirc.Location = new Point(20, 170);
            this.rbPMCirc.AutoSize = true;

            this.rbCircPoligono = new RadioButton();
            this.rbCircPoligono.Text = "Circunf. (Polígono)";
            this.rbCircPoligono.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbCircPoligono.ForeColor = Color.DarkSlateBlue;
            this.rbCircPoligono.Location = new Point(20, 200);
            this.rbCircPoligono.AutoSize = true;

            this.rbElipse = new RadioButton();
            this.rbElipse.Text = "Elipse (Pto Médio)";
            this.rbElipse.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbElipse.ForeColor = Color.DarkSlateBlue;
            this.rbElipse.Location = new Point(20, 230);
            this.rbElipse.AutoSize = true;

            Button btnClear = new Button();
            btnClear.Text = "Limpar";
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(20, 270);
            btnClear.Size = new Size(100, 40);
            btnClear.Click += new EventHandler(this.btnClear_Click);

            this.menuStrip = new MenuStrip();
            this.menuStrip.Dock = DockStyle.Top;
            this.menuStrip.BackColor = Color.WhiteSmoke;
            this.menuStrip.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

            this.algoritmosToolStripMenuItem = new ToolStripMenuItem();
            this.algoritmosToolStripMenuItem.Text = "Algoritmos";
            this.algoritmosToolStripMenuItem.Click += new EventHandler(this.algoritmosToolStripMenuItem_Click);

            this.poligonosToolStripMenuItem = new ToolStripMenuItem();
            this.poligonosToolStripMenuItem.Text = "Polígonos";
            this.poligonosToolStripMenuItem.Click += new EventHandler(this.poligonosToolStripMenuItem_Click);

            this.listBoxPolygons = new ListBox();
            this.listBoxPolygons.Location = new Point(20, 50);
            this.listBoxPolygons.Size = new Size(260, 100);
            this.listBoxPolygons.SelectedIndexChanged += new EventHandler(this.listBoxPolygons_SelectedIndexChanged);
            this.listBoxPolygons.Visible = false;

            this.labelPolygonPoints = new TextBox();
            this.labelPolygonPoints.Location = new Point(20, 150);
            this.labelPolygonPoints.Size = new Size(260, 100);
            this.labelPolygonPoints.Multiline = true;
            this.labelPolygonPoints.ReadOnly = true;
            this.labelPolygonPoints.ScrollBars = ScrollBars.Vertical;
            this.labelPolygonPoints.Font = new Font("Consolas", 10);
            this.labelPolygonPoints.WordWrap = false;
            this.labelPolygonPoints.Text = "Pontos do Polígono:";
            this.labelPolygonPoints.Visible = false;

            this.menuStrip.Items.Add(this.algoritmosToolStripMenuItem);
            this.menuStrip.Items.Add(this.poligonosToolStripMenuItem);

            this.groupBoxMenu.Controls.Add(this.rbEqReta);
            this.groupBoxMenu.Controls.Add(this.rbDDA);
            this.groupBoxMenu.Controls.Add(this.rbPMedio);
            this.groupBoxMenu.Controls.Add(this.rbEqCirc);
            this.groupBoxMenu.Controls.Add(this.rbPMCirc);
            this.groupBoxMenu.Controls.Add(this.rbCircPoligono);
            this.groupBoxMenu.Controls.Add(this.rbElipse);
            this.groupBoxMenu.Controls.Add(btnClear);
            this.groupBoxMenu.Controls.Add(this.listBoxPolygons);
            this.groupBoxMenu.Controls.Add(this.labelPolygonPoints);

            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.groupBoxMenu);
            this.Controls.Add(this.menuStrip);

            this.MinimumSize = new Size(800, 600);
            this.ResumeLayout(false);
        }
    }
}