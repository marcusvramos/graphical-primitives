using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel panelRight;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.PictureBox picGrayR;
        private System.Windows.Forms.PictureBox picGrayG;
        private System.Windows.Forms.PictureBox picGrayB;
        private System.Windows.Forms.PictureBox picGrayH;
        private System.Windows.Forms.PictureBox picGrayS;
        private System.Windows.Forms.PictureBox picGrayI;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TrackBar tbBrilho;
        private System.Windows.Forms.TrackBar tbHue;
        private System.Windows.Forms.Button btnCarregar;
        private System.Windows.Forms.Label lblBrilho;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.Label lblTonsRGB;
        private System.Windows.Forms.Label lblTonsHSI;
        private System.Windows.Forms.FlowLayoutPanel panelRGB;
        private System.Windows.Forms.FlowLayoutPanel panelHSI;
        private System.Windows.Forms.Button btnGrayLuminancia;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblMinHue;
        private System.Windows.Forms.TextBox txtMinHue;
        private System.Windows.Forms.Label lblMaxHue;
        private System.Windows.Forms.TextBox txtMaxHue;
        private System.Windows.Forms.Button btnIntervaloHue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Forms Designer

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1950, 1120);
            this.Name = "Form1";
            this.Text = "Processamento de Imagens - Computação Gráfica";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            this.panelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.AutoScroll = true;
            this.panelRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelRight.WrapContents = false;
            this.panelRight.Padding = new System.Windows.Forms.Padding(16);
            this.panelRight.Width = 600;
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(62, 62, 66);

            this.picOriginal = new System.Windows.Forms.PictureBox();
            this.picOriginal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
            this.picOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picOriginal.Location = new System.Drawing.Point(0, 0);
            this.picOriginal.Margin = new System.Windows.Forms.Padding(5);
            this.picOriginal.Name = "picOriginal";
            this.picOriginal.Size = new System.Drawing.Size(1350, 900);
            this.picOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOriginal.TabStop = false;
            this.picOriginal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicOriginal_MouseMove);

            this.btnCarregar = new System.Windows.Forms.Button();
            this.btnCarregar.AutoSize = true;
            this.btnCarregar.Text = "Carregar Imagem";
            this.btnCarregar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.btnCarregar.Padding = new System.Windows.Forms.Padding(8);
            this.btnCarregar.BackColor = System.Drawing.Color.FromArgb(28, 151, 234);
            this.btnCarregar.ForeColor = System.Drawing.Color.White;
            this.btnCarregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCarregar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCarregar.Click += new System.EventHandler(this.btnCarregar_Click_1);

            this.lblInfo = new System.Windows.Forms.Label();
            this.lblInfo.AutoSize = true;
            this.lblInfo.Text = "Passe o mouse na imagem...";
            this.lblInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.lblBrilho = new System.Windows.Forms.Label();
            this.lblBrilho.AutoSize = true;
            this.lblBrilho.Text = "Brilho";
            this.lblBrilho.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblBrilho.ForeColor = System.Drawing.Color.White;
            this.lblBrilho.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.tbBrilho = new System.Windows.Forms.TrackBar();
            this.tbBrilho.Minimum = -100;
            this.tbBrilho.Maximum = 100;
            this.tbBrilho.Value = 0;
            this.tbBrilho.TickFrequency = 10;
            this.tbBrilho.Width = 500;
            this.tbBrilho.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.tbBrilho.Scroll += new System.EventHandler(this.TbBrilho_Scroll);

            this.lblHue = new System.Windows.Forms.Label();
            this.lblHue.AutoSize = true;
            this.lblHue.Text = "Matiz (Hue)";
            this.lblHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblHue.ForeColor = System.Drawing.Color.White;
            this.lblHue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.tbHue = new System.Windows.Forms.TrackBar();
            this.tbHue.Minimum = -180;
            this.tbHue.Maximum = 180;
            this.tbHue.Value = 0;
            this.tbHue.TickFrequency = 30;
            this.tbHue.Width = 500;
            this.tbHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.tbHue.Scroll += new System.EventHandler(this.TbHue_Scroll);

            this.lblTonsRGB = new System.Windows.Forms.Label();
            this.lblTonsRGB.Text = "Tons de Cinza (RGB)";
            this.lblTonsRGB.AutoSize = true;
            this.lblTonsRGB.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTonsRGB.ForeColor = System.Drawing.Color.White;
            this.lblTonsRGB.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.panelRGB = new System.Windows.Forms.FlowLayoutPanel();
            this.panelRGB.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelRGB.AutoSize = true;
            this.panelRGB.WrapContents = false;
            this.panelRGB.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);

            this.picGrayR = new System.Windows.Forms.PictureBox();
            this.picGrayR.Size = new System.Drawing.Size(150, 150);
            this.picGrayR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayR.Margin = new System.Windows.Forms.Padding(8);

            this.picGrayG = new System.Windows.Forms.PictureBox();
            this.picGrayG.Size = new System.Drawing.Size(150, 150);
            this.picGrayG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayG.Margin = new System.Windows.Forms.Padding(8);

            this.picGrayB = new System.Windows.Forms.PictureBox();
            this.picGrayB.Size = new System.Drawing.Size(150, 150);
            this.picGrayB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayB.Margin = new System.Windows.Forms.Padding(8);

            this.panelRGB.Controls.Add(this.picGrayR);
            this.panelRGB.Controls.Add(this.picGrayG);
            this.panelRGB.Controls.Add(this.picGrayB);

            this.lblTonsHSI = new System.Windows.Forms.Label();
            this.lblTonsHSI.Text = "Tons de Cinza (HSI)";
            this.lblTonsHSI.AutoSize = true;
            this.lblTonsHSI.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTonsHSI.ForeColor = System.Drawing.Color.White;
            this.lblTonsHSI.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.panelHSI = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHSI.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelHSI.AutoSize = true;
            this.panelHSI.WrapContents = false;
            this.panelHSI.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);

            this.picGrayH = new System.Windows.Forms.PictureBox();
            this.picGrayH.Size = new System.Drawing.Size(150, 150);
            this.picGrayH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayH.Margin = new System.Windows.Forms.Padding(8);

            this.picGrayS = new System.Windows.Forms.PictureBox();
            this.picGrayS.Size = new System.Drawing.Size(150, 150);
            this.picGrayS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayS.Margin = new System.Windows.Forms.Padding(8);

            this.picGrayI = new System.Windows.Forms.PictureBox();
            this.picGrayI.Size = new System.Drawing.Size(150, 150);
            this.picGrayI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrayI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGrayI.Margin = new System.Windows.Forms.Padding(8);

            this.panelHSI.Controls.Add(this.picGrayH);
            this.panelHSI.Controls.Add(this.picGrayS);
            this.panelHSI.Controls.Add(this.picGrayI);

            this.btnGrayLuminancia = new System.Windows.Forms.Button();
            this.btnGrayLuminancia.AutoSize = true;
            this.btnGrayLuminancia.Text = "Converter Luminância";
            this.btnGrayLuminancia.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.btnGrayLuminancia.Padding = new System.Windows.Forms.Padding(8);
            this.btnGrayLuminancia.BackColor = System.Drawing.Color.FromArgb(28, 151, 234);
            this.btnGrayLuminancia.ForeColor = System.Drawing.Color.White;
            this.btnGrayLuminancia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrayLuminancia.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnGrayLuminancia.Click += new System.EventHandler(this.btnGrayLuminancia_Click);

            this.btnReset = new System.Windows.Forms.Button();
            this.btnReset.AutoSize = true;
            this.btnReset.Text = "Resetar Imagem";
            this.btnReset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.btnReset.Padding = new System.Windows.Forms.Padding(8);
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(180, 60, 60);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            this.lblMinHue = new System.Windows.Forms.Label();
            this.lblMinHue.AutoSize = true;
            this.lblMinHue.Text = "Min Hue";
            this.lblMinHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblMinHue.ForeColor = System.Drawing.Color.White;
            this.lblMinHue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.txtMinHue = new System.Windows.Forms.TextBox();
            this.txtMinHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.txtMinHue.Text = "0";
            this.txtMinHue.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.lblMaxHue = new System.Windows.Forms.Label();
            this.lblMaxHue.AutoSize = true;
            this.lblMaxHue.Text = "Max Hue";
            this.lblMaxHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblMaxHue.ForeColor = System.Drawing.Color.White;
            this.lblMaxHue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);

            this.txtMaxHue = new System.Windows.Forms.TextBox();
            this.txtMaxHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.txtMaxHue.Text = "360";
            this.txtMaxHue.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.btnIntervaloHue = new System.Windows.Forms.Button();
            this.btnIntervaloHue.AutoSize = true;
            this.btnIntervaloHue.Text = "Aplicar Intervalo Hue";
            this.btnIntervaloHue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.btnIntervaloHue.Padding = new System.Windows.Forms.Padding(8);
            this.btnIntervaloHue.BackColor = System.Drawing.Color.FromArgb(28, 151, 234);
            this.btnIntervaloHue.ForeColor = System.Drawing.Color.White;
            this.btnIntervaloHue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIntervaloHue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnIntervaloHue.Click += new System.EventHandler(this.btnIntervaloHue_Click);

            this.panelRight.Controls.Add(this.btnCarregar);
            this.panelRight.Controls.Add(this.lblInfo);
            this.panelRight.Controls.Add(this.lblBrilho);
            this.panelRight.Controls.Add(this.tbBrilho);
            this.panelRight.Controls.Add(this.lblHue);
            this.panelRight.Controls.Add(this.tbHue);
            this.panelRight.Controls.Add(this.lblTonsRGB);
            this.panelRight.Controls.Add(this.panelRGB);
            this.panelRight.Controls.Add(this.lblTonsHSI);
            this.panelRight.Controls.Add(this.panelHSI);
            this.panelRight.Controls.Add(this.btnGrayLuminancia);
            this.panelRight.Controls.Add(this.btnReset);
            this.panelRight.Controls.Add(this.lblMinHue);
            this.panelRight.Controls.Add(this.txtMinHue);
            this.panelRight.Controls.Add(this.lblMaxHue);
            this.panelRight.Controls.Add(this.txtMaxHue);
            this.panelRight.Controls.Add(this.btnIntervaloHue);

            this.Controls.Add(this.picOriginal);
            this.Controls.Add(this.panelRight);
        }

        #endregion
    }
}
