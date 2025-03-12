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
            drawingBitmap = new Bitmap(
                panelDraw.ClientSize.Width, 
                panelDraw.ClientSize.Height, 
                PixelFormat.Format32bppArgb
            );
        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingBitmap, 0, 0);
        }

        private void panelDraw_MouseDown(object sender, MouseEventArgs e)
        {
            if (startPoint == null)
            {
                // Primeiro clique
                startPoint = e.Location;

                // Marca visualmente (opcional)
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    g.FillEllipse(Brushes.Black, e.X - 2, e.Y - 2, 5, 5);
                }
                panelDraw.Invalidate(new Rectangle(e.X - 3, e.Y - 3, 7, 7));
            }
            else
            {
                // Segundo clique
                Point endPoint = e.Location;

                // =====================================
                // RETAS
                // =====================================
                if (rbEqReta.Checked)
                {
                    DrawLineEquationUnsafe(startPoint.Value, endPoint, Color.Red);
                }
                else if (rbDDA.Checked)
                {
                    DrawLineDDAUnsafe(startPoint.Value, endPoint, Color.Green);
                }
                else if (rbPMedio.Checked)
                {
                    DrawLineBresenhamUnsafe(startPoint.Value, endPoint, Color.Blue);
                }

                // =====================================
                // CIRCUNFERÊNCIA
                // =====================================
                else if (rbEqCirc.Checked)
                {
                    int r = CalcularRaio(startPoint.Value, endPoint);
                    DrawCircleEquationUnsafe(startPoint.Value, r, Color.Orange);
                }
                else if (rbPMCirc.Checked)
                {
                    int r = CalcularRaio(startPoint.Value, endPoint);
                    DrawCircleMidpointUnsafe(startPoint.Value, r, Color.Purple);
                }
                else if (rbCircPoligono.Checked)
                {
                    int r = CalcularRaio(startPoint.Value, endPoint);
                    DrawCirclePolygonApprox(startPoint.Value, r, Color.Brown);
                }

                // =====================================
                // ELIPSE
                // =====================================
                else if (rbElipse.Checked)
                {
                    // Calcula semi-eixos a e b
                    // a = diferença em X, b = diferença em Y
                    int a = Math.Abs(endPoint.X - startPoint.Value.X);
                    int b = Math.Abs(endPoint.Y - startPoint.Value.Y);

                    DrawEllipseMidpointUnsafe(startPoint.Value, a, b, Color.DarkRed);
                }

                // Invalida tudo para forçar o repaint
                panelDraw.Invalidate();

                // Reseta o ponto inicial
                startPoint = null;
            }
        }

        /// <summary>
        /// Calcula o raio entre o ponto (centro) e outro (perímetro).
        /// </summary>
        private int CalcularRaio(Point c, Point p)
        {
            int dx = p.X - c.X;
            int dy = p.Y - c.Y;
            return (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
        }

        /// <summary>
        /// Desenha um pixel (x,y) no bitmap.
        /// </summary>
        private unsafe void PutPixel(byte* ptr, int stride, int x, int y, Color color)
        {
            if (x < 0 || x >= drawingBitmap.Width || y < 0 || y >= drawingBitmap.Height)
                return;

            int index = y * stride + x * 4;
            ptr[index + 0] = color.B;
            ptr[index + 1] = color.G;
            ptr[index + 2] = color.R;
            ptr[index + 3] = color.A;
        }

        // =================================
        // DESENHO DE RETAS
        // =================================

        // 1. Equação da Reta
        private unsafe void DrawLineEquationUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int dx = p1.X - p0.X;
            int dy = p1.Y - p0.Y;

            if (Math.Abs(dx) >= Math.Abs(dy))
            {
                if (p0.X > p1.X)
                {
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

        // 2. DDA
        private unsafe void DrawLineDDAUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
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

        // 3. Ponto Médio (Bresenham)
        private unsafe void DrawLineBresenhamUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
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

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        // =================================
        // DESENHO DE CIRCUNFERÊNCIA
        // =================================

        // 1. Equação Explícita (com raiz + simetria)
        private unsafe void DrawCircleEquationUnsafe(Point center, int radius, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int cx = center.X;
            int cy = center.Y;

            for (int x = 0; x <= radius; x++)
            {
                double temp = (radius * (double)radius) - (x * (double)x);
                if (temp < 0) break;
                int y = (int)Math.Round(Math.Sqrt(temp));

                PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            }

            drawingBitmap.UnlockBits(data);
        }

        // 2. Ponto Médio (Midpoint) da Circunferência
        private unsafe void DrawCircleMidpointUnsafe(Point center, int radius, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int cx = center.X;
            int cy = center.Y;

            int x = 0;
            int y = radius;
            int d = 1 - radius;

            PutCirclePoints(ptr, stride, cx, cy, x, y, color);

            while (x < y)
            {
                x++;
                if (d < 0)
                {
                    d += 2 * x + 1;
                }
                else
                {
                    y--;
                    d += 2 * (x - y) + 1;
                }
                PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            }

            drawingBitmap.UnlockBits(data);
        }

        // 3. Aproximação por Polígono Regular
        private void DrawCirclePolygonApprox(Point center, int radius, Color color)
        {
            // Decide quantos lados usar
            int n = 60; // pode ajustar para 30, 90 etc.

            // Calcula cada vértice do polígono
            double anguloPorSegmento = (2.0 * Math.PI) / n;
            Point[] vertices = new Point[n];

            for (int i = 0; i < n; i++)
            {
                double ang = i * anguloPorSegmento;
                int x = center.X + (int)Math.Round(radius * Math.Cos(ang));
                int y = center.Y + (int)Math.Round(radius * Math.Sin(ang));
                vertices[i] = new Point(x, y);
            }

            // Desenha linhas entre vértices consecutivos
            for (int i = 0; i < n; i++)
            {
                Point p0 = vertices[i];
                Point p1 = vertices[(i + 1) % n];
                // Usa DDA para cada lado, por exemplo
                DrawLineDDAUnsafe(p0, p1, color);
            }
        }

        /// <summary>
        /// Desenha os 8 pontos de simetria (x,y) de uma circunferência centrada em (cx,cy).
        /// </summary>
        private unsafe void PutCirclePoints(byte* ptr, int stride, int cx, int cy, int x, int y, Color color)
        {
            PutPixel(ptr, stride, cx + x, cy + y, color);
            PutPixel(ptr, stride, cx - x, cy + y, color);
            PutPixel(ptr, stride, cx + x, cy - y, color);
            PutPixel(ptr, stride, cx - x, cy - y, color);

            PutPixel(ptr, stride, cx + y, cy + x, color);
            PutPixel(ptr, stride, cx - y, cy + x, color);
            PutPixel(ptr, stride, cx + y, cy - x, color);
            PutPixel(ptr, stride, cx - y, cy - x, color);
        }

        // =================================
        // DESENHO DE ELIPSE (PONTO MÉDIO)
        // =================================

        private unsafe void DrawEllipseMidpointUnsafe(Point center, int a, int b, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(
                rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb
            );
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int cx = center.X;
            int cy = center.Y;

            // Equações do midpoint da elipse
            double a2 = a * (double)a;
            double b2 = b * (double)b;

            // 1ª Região
            double x = 0;
            double y = b;

            // d1 inicial
            double d1 = b2 - (a2 * b) + (0.25 * a2);
            PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);

            // Enquanto slope < -1 => (2 b^2 x < 2 a^2 y)
            while ((b2 * (x + 1)) < (a2 * (y - 0.5)))
            {
                if (d1 < 0)
                {
                    // Escolhe E
                    d1 += b2 * (2 * x + 3);
                }
                else
                {
                    // Escolhe SE
                    d1 += b2 * (2 * x + 3) + a2 * (-2 * y + 2);
                    y--;
                }
                x++;
                PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);
            }

            // 2ª Região
            double d2 = b2 * ((x + 0.5) * (x + 0.5))
                      + a2 * ((y - 1) * (y - 1))
                      - a2 * b2;

            while (y > 0)
            {
                if (d2 < 0)
                {
                    // escolhe E (x++, y--)
                    x++;
                    d2 += b2 * (2 * x + 2) + a2 * (-2 * y + 3);
                }
                else
                {
                    // escolhe S (y--)
                    d2 += a2 * (-2 * y + 3);
                }
                y--;
                PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);
            }

            drawingBitmap.UnlockBits(data);
        }

        /// <summary>
        /// Desenha os 4 pontos de simetria de uma elipse (x,y) centrada em (cx,cy).
        /// </summary>
        private unsafe void PutEllipsePoints(byte* ptr, int stride, int cx, int cy, int x, int y, Color color)
        {
            PutPixel(ptr, stride, cx + x, cy + y, color);
            PutPixel(ptr, stride, cx - x, cy + y, color);
            PutPixel(ptr, stride, cx + x, cy - y, color);
            PutPixel(ptr, stride, cx - x, cy - y, color);
        }

        // =================================
        // LIMPAR
        // =================================

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
