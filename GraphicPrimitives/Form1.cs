using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GraphicPrimitives
{
    public partial class Form1 : Form
    {
        private Point? startPoint = null;
        private Bitmap drawingBitmap;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            drawingBitmap = new Bitmap(panelDraw.ClientSize.Width, panelDraw.ClientSize.Height, PixelFormat.Format32bppArgb);
        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingBitmap, 0, 0);
        }

        private void panelDraw_MouseDown(object sender, MouseEventArgs e)
        {
            // Se não há ponto inicial registrado, usamos este clique como primeiro ponto
            if (startPoint == null)
            {
                startPoint = e.Location;
                // Desenha um marcador (opcional) para indicar o primeiro clique
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    g.FillEllipse(Brushes.Black, e.X - 2, e.Y - 2, 5, 5);
                }
                panelDraw.Invalidate(new Rectangle(e.X - 3, e.Y - 3, 7, 7));
            }
            else
            {
                // Já existe ponto inicial; este é o segundo clique
                Point endPoint = e.Location;

                // Desenhar de acordo com o RadioButton selecionado
                if (rbEqReta.Checked)
                {
                    // Reta - Equação da Reta
                    DrawLineEquationUnsafe(startPoint.Value, endPoint, Color.Red);
                }
                else if (rbDDA.Checked)
                {
                    // Reta - DDA
                    DrawLineDDAUnsafe(startPoint.Value, endPoint, Color.Green);
                }
                else if (rbPMedio.Checked)
                {
                    // Reta - Ponto Médio (Bresenham)
                    DrawLineBresenhamUnsafe(startPoint.Value, endPoint, Color.Blue);
                }
                else if (rbEqCirc.Checked)
                {
                    // Circunferência - Equação
                    int r = CalcularRaio(startPoint.Value, endPoint);
                    DrawCircleEquationUnsafe(startPoint.Value, r, Color.Orange);
                }
                else if (rbPMCirc.Checked)
                {
                    // Circunferência - Ponto Médio
                    int r = CalcularRaio(startPoint.Value, endPoint);
                    DrawCircleMidpointUnsafe(startPoint.Value, r, Color.Purple);
                }

                panelDraw.Invalidate(); // invalida tudo, garante que o círculo aparece
                startPoint = null;
            }
        }

        /// <summary>
        /// Função auxiliar para calcular o retângulo mínimo que contém a linha ou círculo.
        /// </summary>
        private Rectangle GetInvalidationRect(Point p1, Point p2)
        {
            int x = Math.Min(p1.X, p2.X) - 1;
            int y = Math.Min(p1.Y, p2.Y) - 1;
            int width = Math.Abs(p2.X - p1.X) + 3;
            int height = Math.Abs(p2.Y - p1.Y) + 3;
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Calcula o raio de um círculo dados o centro e um ponto no perímetro.
        /// </summary>
        private int CalcularRaio(Point c, Point p)
        {
            int dx = p.X - c.X;
            int dy = p.Y - c.Y;
            return (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
        }

        /// <summary>
        /// Desenha um pixel na posição (x, y).
        /// </summary>
        private unsafe void PutPixel(byte* ptr, int stride, int x, int y, Color color)
        {
            // Verifica se está dentro dos limites do Bitmap
            if (x < 0 || x >= drawingBitmap.Width || y < 0 || y >= drawingBitmap.Height)
                return;

            int index = y * stride + x * 4;
            ptr[index + 0] = color.B;   // Blue
            ptr[index + 1] = color.G;   // Green
            ptr[index + 2] = color.R;   // Red
            ptr[index + 3] = color.A;   // Alpha
        }

        // =================================
        // Métodos de Desenho de Retas
        // =================================

        /// <summary>
        /// 1. Método: Equação Real da Reta
        /// </summary>
        private unsafe void DrawLineEquationUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int dx = p1.X - p0.X;
            int dy = p1.Y - p0.Y;

            if (Math.Abs(dx) >= Math.Abs(dy))
            {
                // varredura em x
                if (p0.X > p1.X)
                {
                    // Se x0 > x1, inverte
                    Point temp = p0; p0 = p1; p1 = temp;
                    dx = p1.X - p0.X;
                    dy = p1.Y - p0.Y;
                }
                double m = dx != 0 ? (double)dy / dx : 0;
                for (int x = p0.X; x <= p1.X; x++)
                {
                    int y = (int)Math.Round(p0.Y + m * (x - p0.X));
                    PutPixel(ptr, stride, x, y, color);
                }
            }
            else
            {
                // varredura em y
                if (p0.Y > p1.Y)
                {
                    Point temp = p0; p0 = p1; p1 = temp;
                    dx = p1.X - p0.X;
                    dy = p1.Y - p0.Y;
                }
                double mInv = dy != 0 ? (double)dx / dy : 0;
                for (int y = p0.Y; y <= p1.Y; y++)
                {
                    int x = (int)Math.Round(p0.X + mInv * (y - p0.Y));
                    PutPixel(ptr, stride, x, y, color);
                }
            }

            drawingBitmap.UnlockBits(data);
        }

        /// <summary>
        /// 2. Método: DDA
        /// </summary>
        private unsafe void DrawLineDDAUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int dx = p1.X - p0.X;
            int dy = p1.Y - p0.Y;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            double Xinc = steps != 0 ? (double)dx / steps : 0;
            double Yinc = steps != 0 ? (double)dy / steps : 0;

            double x = p0.X;
            double y = p0.Y;
            for (int i = 0; i <= steps; i++)
            {
                PutPixel(ptr, stride, (int)Math.Round(x), (int)Math.Round(y), color);
                x += Xinc;
                y += Yinc;
            }

            drawingBitmap.UnlockBits(data);
        }

        /// <summary>
        /// 3. Método: Ponto Médio (Bresenham)
        /// </summary>
        private unsafe void DrawLineBresenhamUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int x0 = p0.X, y0 = p0.Y;
            int x1 = p1.X, y1 = p1.Y;

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                    PutPixel(ptr, stride, y, x, color);
                else
                    PutPixel(ptr, stride, x, y, color);

                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }

            drawingBitmap.UnlockBits(data);
        }

        // =================================
        // Métodos de Desenho de Circunferência
        // =================================

        /// <summary>
        /// 1. Método: Equação da Circunferência
        /// (Desenha 1/8 e espelha nos 8 octantes)
        /// </summary>
        private unsafe void DrawCircleEquationUnsafe(Point center, int radius, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int cx = center.X;
            int cy = center.Y;

            // varre x de 0 até r, calculando y = sqrt(r^2 - x^2)
            for (int x = 0; x <= radius; x++)
            {
                double temp = (double)(radius * radius) - (x * (double)x);
                if (temp < 0) break; // apenas por segurança numérica
                int y = (int)Math.Round(Math.Sqrt(temp));

                // coloca os 8 pontos de simetria
                PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            }

            drawingBitmap.UnlockBits(data);
        }

        /// <summary>
        /// 2. Método: Ponto Médio (Bresenham) para Circunferência
        /// </summary>
        private unsafe void DrawCircleMidpointUnsafe(Point center, int radius, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int cx = center.X;
            int cy = center.Y;

            int x = 0;
            int y = radius;
            int d = 1 - radius; // valor inicial

            // Desenha os pontos iniciais (x=0, y=r)
            PutCirclePoints(ptr, stride, cx, cy, x, y, color);

            while (x < y)
            {
                x++;
                if (d < 0)
                {
                    // escolhe E
                    d += 2 * x + 1;
                }
                else
                {
                    // escolhe SE
                    y--;
                    d += 2 * (x - y) + 1;
                }
                PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            }

            drawingBitmap.UnlockBits(data);
        }

        /// <summary>
        /// Função que coloca na tela os 8 pontos de simetria de (x,y).
        /// </summary>
        private unsafe void PutCirclePoints(byte* ptr, int stride, int cx, int cy, int x, int y, Color color)
        {
            // (cx + x, cy + y)
            PutPixel(ptr, stride, cx + x, cy + y, color);
            // (cx + x, cy - y)
            PutPixel(ptr, stride, cx + x, cy - y, color);
            // (cx - x, cy + y)
            PutPixel(ptr, stride, cx - x, cy + y, color);
            // (cx - x, cy - y)
            PutPixel(ptr, stride, cx - x, cy - y, color);

            // (cx + y, cy + x)
            PutPixel(ptr, stride, cx + y, cy + x, color);
            // (cx + y, cy - x)
            PutPixel(ptr, stride, cx + y, cy - x, color);
            // (cx - y, cy + x)
            PutPixel(ptr, stride, cx - y, cy + x, color);
            // (cx - y, cy - x)
            PutPixel(ptr, stride, cx - y, cy - x, color);
        }

        /// <summary>
        /// Troca de valores inteiros (auxiliar usada no Bresenham de reta).
        /// </summary>
        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Limpa o painel.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
            }
            panelDraw.Invalidate();
            startPoint = null;
        }
    }
}
