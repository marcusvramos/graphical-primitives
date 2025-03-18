using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap currentImage;

        public Form1()
        {
            InitializeComponent();
            this.Padding = new Padding(50, 0, 0, 0);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnCarregar_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(ofd.FileName);
                    currentImage = new Bitmap(originalImage);
                    picOriginal.Image = currentImage;
                    AtualizarMiniaturas(currentImage);
                    tbBrilho.Value = 0;
                    tbHue.Value = 0;
                }
            }
        }

        private void btnGrayLuminancia_Click(object sender, EventArgs e)
        {
            if (currentImage == null) return;
            Bitmap temp = new Bitmap(currentImage.Width, currentImage.Height);
            for (int y = 0; y < currentImage.Height; y++)
            {
                for (int x = 0; x < currentImage.Width; x++)
                {
                    Color c = currentImage.GetPixel(x, y);
                    int gray = (int)(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
                    gray = Math.Max(0, Math.Min(255, gray));
                    temp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            currentImage = temp;
            picOriginal.Image = currentImage;
            AtualizarMiniaturas(currentImage);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;
            currentImage = new Bitmap(originalImage);
            picOriginal.Image = currentImage;
            tbBrilho.Value = 0;
            tbHue.Value = 0;
            AtualizarMiniaturas(currentImage);
        }

        private void btnIntervaloHue_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;
            double minH = 0;
            double maxH = 360;
            double.TryParse(txtMinHue.Text, out minH);
            double.TryParse(txtMaxHue.Text, out maxH);
            Bitmap temp = new Bitmap(originalImage.Width, originalImage.Height);
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color c = originalImage.GetPixel(x, y);
                    double H, S, I;
                    RGBToHSI(c.R, c.G, c.B, out H, out S, out I);
                    if (H >= minH && H <= maxH)
                    {
                        temp.SetPixel(x, y, c);
                    }
                    else
                    {
                        temp.SetPixel(x, y, Color.Black);
                    }
                }
            }
            currentImage = temp;
            picOriginal.Image = currentImage;
            AtualizarMiniaturas(currentImage);
        }

        private void PicOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentImage == null) return;
            Point pt = TranslateZoomMousePosition(picOriginal, e.Location);
            if (pt.X < 0 || pt.X >= currentImage.Width || pt.Y < 0 || pt.Y >= currentImage.Height)
                return;
            Color c = currentImage.GetPixel(pt.X, pt.Y);
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int cC = 255 - r;
            int cM = 255 - g;
            int cY = 255 - b;
            double h, s, i;
            RGBToHSI(r, g, b, out h, out s, out i);
            string info = $"RGB({r}, {g}, {b}) | CMY({cC}, {cM}, {cY}) | HSI(H={h:F1}, S={s:F1}, I={i:F1})";
            lblInfo.Text = info;
        }

        private void TbBrilho_Scroll(object sender, EventArgs e)
        {
            if (originalImage == null) return;
            double brilho = tbBrilho.Value;
            Bitmap temp = new Bitmap(originalImage.Width, originalImage.Height);
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color c = originalImage.GetPixel(x, y);
                    double H, S, I;
                    RGBToHSI(c.R, c.G, c.B, out H, out S, out I);
                    I = Math.Max(0, Math.Min(255, I + brilho));
                    Color newColor = HSIToRGB(H, S, I);
                    temp.SetPixel(x, y, newColor);
                }
            }
            double hueShift = tbHue.Value;
            if (Math.Abs(hueShift) > 0.001)
            {
                temp = AplicarMudancaHue(temp, hueShift);
            }
            currentImage = temp;
            picOriginal.Image = currentImage;
            AtualizarMiniaturas(currentImage);
        }

        private void TbHue_Scroll(object sender, EventArgs e)
        {
            if (originalImage == null) return;
            double hueShift = tbHue.Value;
            int brilho = tbBrilho.Value;
            Bitmap temp = new Bitmap(originalImage.Width, originalImage.Height);
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color c = originalImage.GetPixel(x, y);
                    int r = c.R + brilho;
                    int g = c.G + brilho;
                    int b = c.B + brilho;
                    r = Math.Max(0, Math.Min(255, r));
                    g = Math.Max(0, Math.Min(255, g));
                    b = Math.Max(0, Math.Min(255, b));
                    temp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            temp = AplicarMudancaHue(temp, hueShift);
            currentImage = temp;
            picOriginal.Image = currentImage;
            AtualizarMiniaturas(currentImage);
        }

        private Point TranslateZoomMousePosition(PictureBox pic, Point mouse)
        {
            if (pic.Image == null) return new Point(-1, -1);
            float imageAspect = (float)pic.Image.Width / pic.Image.Height;
            float boxAspect = (float)pic.Width / pic.Height;
            int imgWidth, imgHeight, offsetX, offsetY;
            if (imageAspect > boxAspect)
            {
                imgWidth = pic.Width;
                imgHeight = (int)(pic.Width / imageAspect);
                offsetX = 0;
                offsetY = (pic.Height - imgHeight) / 2;
            }
            else
            {
                imgHeight = pic.Height;
                imgWidth = (int)(pic.Height * imageAspect);
                offsetX = (pic.Width - imgWidth) / 2;
                offsetY = 0;
            }
            int mx = mouse.X - offsetX;
            int my = mouse.Y - offsetY;
            if (mx < 0 || my < 0 || mx >= imgWidth || my >= imgHeight)
                return new Point(-1, -1);
            float rx = mx / (float)imgWidth;
            float ry = my / (float)imgHeight;
            int px = (int)(rx * pic.Image.Width);
            int py = (int)(ry * pic.Image.Height);
            return new Point(px, py);
        }

        private void RGBToHSI(int R, int G, int B, out double H, out double S, out double I)
        {
            double r = R / 255.0;
            double g = G / 255.0;
            double b = B / 255.0;
            double num = 0.5 * ((r - g) + (r - b));
            double den = Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b));
            double theta = 0;
            if (den != 0)
            {
                double acosValue = num / den;
                if (acosValue > 1.0) acosValue = 1.0;
                if (acosValue < -1.0) acosValue = -1.0;
                theta = Math.Acos(acosValue);
            }
            if (b > g) theta = 2.0 * Math.PI - theta;
            H = theta * 180.0 / Math.PI;
            double minVal = Math.Min(r, Math.Min(g, b));
            double sum = r + g + b;
            S = (sum == 0) ? 0 : 1 - (3 * minVal / sum);
            double intensity = sum / 3.0;
            S *= 100.0;
            I = intensity * 255.0;
        }

        private Color HSIToRGB(double H, double S, double I)
        {
            double h = H * Math.PI / 180.0;
            double s = S / 100.0;
            double i = I / 255.0;
            double r = 0, g = 0, b = 0;
            if (h < 0) h += 2.0 * Math.PI;
            double z = 1.0 - Math.Abs((h / (Math.PI / 3.0)) % 2.0 - 1.0);
            double c = (3 * i * s) / (1 + z);
            double x = c * z;
            double hDeg = H;
            double m = i * (1 - s);
            if (hDeg >= 0 && hDeg < 120)
            {
                r = c + m;
                g = x + m;
                b = m;
            }
            else if (hDeg >= 120 && hDeg < 240)
            {
                r = m;
                g = c + m;
                b = x + m;
            }
            else
            {
                r = x + m;
                g = m;
                b = c + m;
            }
            int R = (int)Math.Round(r * 255.0);
            int G = (int)Math.Round(g * 255.0);
            int B = (int)Math.Round(b * 255.0);
            R = Math.Max(0, Math.Min(255, R));
            G = Math.Max(0, Math.Min(255, G));
            B = Math.Max(0, Math.Min(255, B));
            return Color.FromArgb(R, G, B);
        }

        private void AtualizarMiniaturas(Bitmap source)
        {
            if (source == null) return;
            picGrayR.Image = CriarGrayChannelRGB(source, 'R');
            picGrayG.Image = CriarGrayChannelRGB(source, 'G');
            picGrayB.Image = CriarGrayChannelRGB(source, 'B');
            picGrayH.Image = CriarGrayChannelHSI(source, 'H');
            picGrayS.Image = CriarGrayChannelHSI(source, 'S');
            picGrayI.Image = CriarGrayChannelHSI(source, 'I');
        }

        private Bitmap CriarGrayChannelRGB(Bitmap src, char canal)
        {
            Bitmap gray = new Bitmap(src.Width, src.Height);
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);
                    int val = 0;
                    switch (canal)
                    {
                        case 'R': val = c.R; break;
                        case 'G': val = c.G; break;
                        case 'B': val = c.B; break;
                    }
                    gray.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            return gray;
        }

        private Bitmap CriarGrayChannelHSI(Bitmap src, char canal)
        {
            Bitmap gray = new Bitmap(src.Width, src.Height);
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);
                    double H, S, I;
                    RGBToHSI(c.R, c.G, c.B, out H, out S, out I);
                    double valor = 0;
                    switch (canal)
                    {
                        case 'H': valor = (H / 360.0) * 255.0; break;
                        case 'S': valor = (S / 100.0) * 255.0; break;
                        case 'I': valor = I; break;
                    }
                    int v = (int)Math.Round(valor);
                    v = Math.Max(0, Math.Min(255, v));
                    gray.SetPixel(x, y, Color.FromArgb(v, v, v));
                }
            }
            return gray;
        }

        private Bitmap AplicarMudancaHue(Bitmap src, double hueShift)
        {
            Bitmap result = new Bitmap(src.Width, src.Height);
            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);
                    double H, S, I;
                    RGBToHSI(c.R, c.G, c.B, out H, out S, out I);
                    H += hueShift;
                    if (H < 0) H += 360.0;
                    if (H >= 360) H -= 360.0;
                    Color newColor = HSIToRGB(H, S, I);
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;
        }
    }
}
