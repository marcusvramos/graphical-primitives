using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Painel principal de desenho
        private Panel panelDraw;

        // GroupBox do menu lateral
        private GroupBox groupBoxMenu;

        // RadioButtons de Reta
        private RadioButton rbEqReta;
        private RadioButton rbDDA;
        private RadioButton rbPMedio;

        // RadioButtons de Circunferência
        private RadioButton rbEqCirc;
        private RadioButton rbPMCirc;

        // (NOVO) RadioButton de Polígono
        private RadioButton rbCircPoligono;

        /// <summary>
        /// Limpa os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Método necessário para suporte ao Designer – não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            // 
            // Configurações do Form
            // 
            this.Text = "Desenhador de Retas e Circunferências";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackColor = Color.Gainsboro;
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.AutoScaleDimensions = new SizeF(9F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;

            // 
            // Painel de desenho
            // 
            this.panelDraw = new Panel();
            this.panelDraw.Dock = DockStyle.Fill;
            this.panelDraw.BackColor = Color.White;
            this.panelDraw.MouseDown += new MouseEventHandler(this.panelDraw_MouseDown);
            this.panelDraw.Paint += new PaintEventHandler(this.panelDraw_Paint);

            // 
            // groupBoxMenu
            // 
            this.groupBoxMenu = new GroupBox();
            this.groupBoxMenu.Dock = DockStyle.Right;
            this.groupBoxMenu.Width = 300;
            this.groupBoxMenu.Text = "Algoritmos";
            this.groupBoxMenu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.groupBoxMenu.ForeColor = Color.DarkSlateGray;
            this.groupBoxMenu.BackColor = Color.WhiteSmoke;

            // 
            // Rádios de Reta
            // 
            this.rbEqReta = new RadioButton();
            this.rbEqReta.Text = "Equação da Reta";
            this.rbEqReta.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbEqReta.ForeColor = Color.DarkSlateBlue;
            this.rbEqReta.Location = new Point(20, 50);
            this.rbEqReta.AutoSize = true;
            this.rbEqReta.Checked = true;

            this.rbDDA = new RadioButton();
            this.rbDDA.Text = "DDA";
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

            // 
            // Rádios de Circunferência
            // 
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

            // 
            // (NOVO) RadioButton Polígono Regular
            // 
            this.rbCircPoligono = new RadioButton();
            this.rbCircPoligono.Text = "Circunf. (Polígono)";
            this.rbCircPoligono.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.rbCircPoligono.ForeColor = Color.DarkSlateBlue;
            this.rbCircPoligono.Location = new Point(20, 200);
            this.rbCircPoligono.AutoSize = true;

            // 
            // Botão Limpar
            // 
            Button btnClear = new Button();
            btnClear.Text = "Limpar";
            btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(20, 240);
            btnClear.Size = new Size(100, 40);
            btnClear.Click += new EventHandler(this.btnClear_Click);

            // 
            // Adiciona controles ao groupBox
            // 
            this.groupBoxMenu.Controls.Add(this.rbEqReta);
            this.groupBoxMenu.Controls.Add(this.rbDDA);
            this.groupBoxMenu.Controls.Add(this.rbPMedio);
            this.groupBoxMenu.Controls.Add(this.rbEqCirc);
            this.groupBoxMenu.Controls.Add(this.rbPMCirc);
            this.groupBoxMenu.Controls.Add(this.rbCircPoligono);
            this.groupBoxMenu.Controls.Add(btnClear);

            // 
            // Adiciona os controles ao Form
            // 
            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.groupBoxMenu);

            this.MinimumSize = new Size(800, 600);
            this.ResumeLayout(false);
        }
    }
}
