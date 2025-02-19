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
            if (startPoint == null)
            {
                startPoint = e.Location;
                using (Graphics g = Graphics.FromImage(drawingBitmap))
                {
                    g.FillEllipse(Brushes.Black, e.X - 2, e.Y - 2, 5, 5);
                }
                panelDraw.Invalidate(new Rectangle(e.X - 3, e.Y - 3, 7, 7));
            }
            else
            {
                Point endPoint = e.Location;
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

                Rectangle invalidateRect = GetInvalidationRect(startPoint.Value, endPoint);
                panelDraw.Invalidate(invalidateRect);

                startPoint = null;
            }
        }

        private Rectangle GetInvalidationRect(Point p1, Point p2)
        {
            int x = Math.Min(p1.X, p2.X) - 1;
            int y = Math.Min(p1.Y, p2.Y) - 1;
            int width = Math.Abs(p2.X - p1.X) + 3;
            int height = Math.Abs(p2.Y - p1.Y) + 3;
            return new Rectangle(x, y, width, height);
        }

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

        // 1. Metodo: Equacao Real da Reta
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

        // 2. Metodo: DDA
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

        // 3. Metodo: Ponto Medio (Bresenham)
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
            }

            panelDraw.Invalidate();

            startPoint = null;
        }


        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
