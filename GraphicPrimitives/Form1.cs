using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GraphicPrimitives
{
    public partial class Form1 : Form
    {
        private Point? startPoint = null;
        private Bitmap drawingBitmap;
        private List<List<Point>> polygons = new List<List<Point>>();
        private List<Point> polygonPoints = new List<Point>();
        private bool isDrawingPolygon = false;
        private Color selectedFillColor = Color.Red; // Cor padrão para preenchimento
        private List<Color?> polygonFillColors = new List<Color?>();

        public Form1()
        {
            InitializeComponent();
            AddFillMenu(); // Adiciona o menu de preenchimento
            AddTransformMenu(); // Adiciona o menu de transformações 2D
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            drawingBitmap = new Bitmap(
                panelDraw.ClientSize.Width,
                panelDraw.ClientSize.Height,
                PixelFormat.Format32bppArgb
            );
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White); // Inicializa o bitmap com fundo branco
            }
        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingBitmap, 0, 0);
        }

        private void algoritmosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBoxMenu.Text = "Algoritmos";
            rbEqReta.Visible = true;
            rbDDA.Visible = true;
            rbPMedio.Visible = true;
            rbEqCirc.Visible = true;
            rbPMCirc.Visible = true;
            rbCircPoligono.Visible = true;
            rbElipse.Visible = true;
            listBoxPolygons.Visible = false;
            labelPolygonPoints.Visible = false;
            isDrawingPolygon = false;
            groupBoxMenu.PerformLayout();
        }

        private void panelDraw_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawingPolygon)
            {
                if (e.Button == MouseButtons.Left)
                {
                    polygonPoints.Add(e.Location);
                    using (Graphics g = Graphics.FromImage(drawingBitmap))
                    {
                        g.FillEllipse(Brushes.Black, e.X - 2, e.Y - 2, 5, 5);
                        if (polygonPoints.Count > 1)
                        {
                            Point lastPoint = polygonPoints[polygonPoints.Count - 2];
                            g.DrawLine(Pens.Black, lastPoint, e.Location);
                        }
                    }
                    panelDraw.Invalidate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (polygonPoints.Count > 2)
                    {
                        using (Graphics g = Graphics.FromImage(drawingBitmap))
                        {
                            g.DrawLine(Pens.Black, polygonPoints[polygonPoints.Count - 1], polygonPoints[0]);
                        }
                        polygons.Add(new List<Point>(polygonPoints));
                        polygonFillColors.Add(null); // Sem preenchimento inicialmente
                        listBoxPolygons.Items.Add($"Polígono {polygons.Count}");
                        panelDraw.Invalidate();
                    }
                    polygonPoints.Clear();
                }
            }
            else
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
                        DrawLineEquationUnsafe(startPoint.Value, endPoint, Color.Red);
                    else if (rbDDA.Checked)
                        DrawLineDDAUnsafe(startPoint.Value, endPoint, Color.Green);
                    else if (rbPMedio.Checked)
                        DrawLineBresenhamUnsafe(startPoint.Value, endPoint, Color.Blue);
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
                    else if (rbElipse.Checked)
                    {
                        int a = Math.Abs(endPoint.X - startPoint.Value.X);
                        int b = Math.Abs(endPoint.Y - startPoint.Value.Y);
                        DrawEllipseMidpointUnsafe(startPoint.Value, a, b, Color.DarkRed);
                    }
                    panelDraw.Invalidate();
                    startPoint = null;
                }
            }
        }

        private void listBoxPolygons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex != -1)
            {
                var polygon = polygons[listBoxPolygons.SelectedIndex];
                labelPolygonPoints.Text = "Pontos do Polígono:\r\n";
                foreach (var point in polygon)
                {
                    labelPolygonPoints.Text += $"({point.X}, {point.Y})\r\n";
                }
            }
        }

        private void poligonosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isDrawingPolygon = true;
            polygonPoints.Clear();
            groupBoxMenu.Text = "Polígonos";
            rbEqReta.Visible = false;
            rbDDA.Visible = false;
            rbPMedio.Visible = false;
            rbEqCirc.Visible = false;
            rbPMCirc.Visible = false;
            rbCircPoligono.Visible = false;
            rbElipse.Visible = false;
            listBoxPolygons.Visible = true;
            labelPolygonPoints.Visible = true;
            groupBoxMenu.PerformLayout();
        }

        private int CalcularRaio(Point c, Point p)
        {
            int dx = p.X - c.X;
            int dy = p.Y - c.Y;
            return (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));
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

        #region Desenho de Primitivas
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
                if (p0.X > p1.X) { Point temp = p0; p0 = p1; p1 = temp; dx = p1.X - p0.X; dy = p1.Y - p0.Y; }
                double m = dx != 0 ? (double)dy / dx : 0;
                for (int x = p0.X; x <= p1.X; x++)
                {
                    int y = (int)Math.Round(p0.Y + m * (x - p0.X));
                    PutPixel(ptr, stride, x, y, color);
                }
            }
            else
            {
                if (p0.Y > p1.Y) { Point temp = p0; p0 = p1; p1 = temp; dx = p1.X - p0.X; dy = p1.Y - p0.Y; }
                double mInv = dy != 0 ? (double)dx / dy : 0;
                for (int y = p0.Y; y <= p1.Y; y++)
                {
                    int x = (int)Math.Round(p0.X + mInv * (y - p0.Y));
                    PutPixel(ptr, stride, x, y, color);
                }
            }
            drawingBitmap.UnlockBits(data);
        }

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

        private unsafe void DrawLineBresenhamUnsafe(Point p0, Point p1, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;

            int x0 = p0.X, y0 = p0.Y;
            int x1 = p1.X, y1 = p1.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap(ref x0, ref y0); Swap(ref x1, ref y1); }
            if (x0 > x1) { Swap(ref x0, ref x1); Swap(ref y0, ref y1); }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                if (steep) PutPixel(ptr, stride, y, x, color);
                else PutPixel(ptr, stride, x, y, color);
                error -= dy;
                if (error < 0) { y += ystep; error += dx; }
            }
            drawingBitmap.UnlockBits(data);
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a; a = b; b = temp;
        }

        private unsafe void DrawCircleEquationUnsafe(Point center, int radius, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
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
            int d = 1 - radius;
            PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            while (x < y)
            {
                x++;
                if (d < 0) d += 2 * x + 1;
                else { y--; d += 2 * (x - y) + 1; }
                PutCirclePoints(ptr, stride, cx, cy, x, y, color);
            }
            drawingBitmap.UnlockBits(data);
        }

        private void DrawCirclePolygonApprox(Point center, int radius, Color color)
        {
            int n = 60;
            double anguloPorSegmento = (2.0 * Math.PI) / n;
            Point[] vertices = new Point[n];
            for (int i = 0; i < n; i++)
            {
                double ang = i * anguloPorSegmento;
                int x = center.X + (int)Math.Round(radius * Math.Cos(ang));
                int y = center.Y + (int)Math.Round(radius * Math.Sin(ang));
                vertices[i] = new Point(x, y);
            }
            for (int i = 0; i < n; i++)
            {
                Point p0 = vertices[i];
                Point p1 = vertices[(i + 1) % n];
                DrawLineDDAUnsafe(p0, p1, color);
            }
        }

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

        private unsafe void DrawEllipseMidpointUnsafe(Point center, int a, int b, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, drawingBitmap.Width, drawingBitmap.Height);
            BitmapData data = drawingBitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* ptr = (byte*)data.Scan0;
            int stride = data.Stride;
            int cx = center.X;
            int cy = center.Y;
            double a2 = a * (double)a;
            double b2 = b * (double)b;
            double x = 0;
            double y = b;
            double d1 = b2 - (a2 * b) + (0.25 * a2);
            PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);
            while ((b2 * (x + 1)) < (a2 * (y - 0.5)))
            {
                if (d1 < 0) d1 += b2 * (2 * x + 3);
                else { d1 += b2 * (2 * x + 3) + a2 * (-2 * y + 2); y--; }
                x++;
                PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);
            }
            double d2 = b2 * ((x + 0.5) * (x + 0.5)) + a2 * ((y - 1) * (y - 1)) - a2 * b2;
            while (y > 0)
            {
                if (d2 < 0) { x++; d2 += b2 * (2 * x + 2) + a2 * (-2 * y + 3); }
                else d2 += a2 * (-2 * y + 3);
                y--;
                PutEllipsePoints(ptr, stride, cx, cy, (int)x, (int)y, color);
            }
            drawingBitmap.UnlockBits(data);
        }

        private unsafe void PutEllipsePoints(byte* ptr, int stride, int cx, int cy, int x, int y, Color color)
        {
            PutPixel(ptr, stride, cx + x, cy + y, color);
            PutPixel(ptr, stride, cx - x, cy + y, color);
            PutPixel(ptr, stride, cx + x, cy - y, color);
            PutPixel(ptr, stride, cx - x, cy - y, color);
        }
        #endregion

        #region Preenchimento de Polígonos
        private void FloodFill(Bitmap bmp, int x, int y, Color targetColor, Color replacementColor)
        {
            if (targetColor.ToArgb() == replacementColor.ToArgb()) return;
            Queue<Point> pixels = new Queue<Point>();
            pixels.Enqueue(new Point(x, y));
            while (pixels.Count > 0)
            {
                Point pt = pixels.Dequeue();
                if (pt.X < 0 || pt.X >= bmp.Width || pt.Y < 0 || pt.Y >= bmp.Height) continue;
                if (bmp.GetPixel(pt.X, pt.Y).ToArgb() != targetColor.ToArgb()) continue;
                bmp.SetPixel(pt.X, pt.Y, replacementColor);
                pixels.Enqueue(new Point(pt.X + 1, pt.Y));
                pixels.Enqueue(new Point(pt.X - 1, pt.Y));
                pixels.Enqueue(new Point(pt.X, pt.Y + 1));
                pixels.Enqueue(new Point(pt.X, pt.Y - 1));
            }
        }

        private void PreenchimentoFloodFill(Point seed, Color fillColor)
        {
            Color targetColor = drawingBitmap.GetPixel(seed.X, seed.Y);
            FloodFill(drawingBitmap, seed.X, seed.Y, targetColor, fillColor);
            panelDraw.Invalidate();
        }

        private void ScanlineFill(Bitmap bmp, List<Point> polygon, Color fillColor)
        {
            if (polygon == null || polygon.Count < 3) return;
            int ymin = polygon.Min(p => p.Y);
            int ymax = polygon.Max(p => p.Y);
            List<Edge> edges = new List<Edge>();
            for (int i = 0; i < polygon.Count; i++)
            {
                Point p1 = polygon[i];
                Point p2 = polygon[(i + 1) % polygon.Count];
                if (p1.Y == p2.Y) continue;
                Edge edge = new Edge();
                if (p1.Y < p2.Y)
                {
                    edge.yMin = p1.Y;
                    edge.yMax = p2.Y;
                    edge.x = p1.X;
                    edge.invSlope = (float)(p2.X - p1.X) / (p2.Y - p1.Y);
                }
                else
                {
                    edge.yMin = p2.Y;
                    edge.yMax = p1.Y;
                    edge.x = p2.X;
                    edge.invSlope = (float)(p1.X - p2.X) / (p1.Y - p2.Y);
                }
                edges.Add(edge);
            }
            List<Edge> AET = new List<Edge>();
            for (int y = ymin; y < ymax; y++)
            {
                foreach (Edge edge in edges)
                    if (edge.yMin == y)
                        AET.Add(edge);
                AET.RemoveAll(e => e.yMax == y);
                AET.Sort((a, b) => a.x.CompareTo(b.x));
                for (int i = 0; i < AET.Count; i += 2)
                {
                    if (i + 1 < AET.Count)
                    {
                        int xStart = (int)Math.Ceiling(AET[i].x);
                        int xEnd = (int)Math.Floor(AET[i + 1].x);
                        for (int x = xStart; x <= xEnd; x++)
                        {
                            if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                                bmp.SetPixel(x, y, fillColor);
                        }
                    }
                }
                for (int i = 0; i < AET.Count; i++)
                    AET[i].x += AET[i].invSlope;
            }
        }

        private class Edge
        {
            public int yMin;
            public int yMax;
            public float x;
            public float invSlope;
        }
        #endregion

        #region Transformações 2D
        private void AddTransformMenu()
        {
            ToolStripMenuItem transformMenuItem = new ToolStripMenuItem("Transformações 2D");

            ToolStripMenuItem scaleItem = new ToolStripMenuItem("Escala");
            scaleItem.Click += ScaleItem_Click;
            transformMenuItem.DropDownItems.Add(scaleItem);

            ToolStripMenuItem translateItem = new ToolStripMenuItem("Translação");
            translateItem.Click += TranslateItem_Click;
            transformMenuItem.DropDownItems.Add(translateItem);

            ToolStripMenuItem rotateItem = new ToolStripMenuItem("Rotação");
            rotateItem.Click += RotateItem_Click;
            transformMenuItem.DropDownItems.Add(rotateItem);

            ToolStripMenuItem shearItem = new ToolStripMenuItem("Cisalhamento");
            shearItem.Click += ShearItem_Click;
            transformMenuItem.DropDownItems.Add(shearItem);

            ToolStripMenuItem reflectItem = new ToolStripMenuItem("Reflexão");
            reflectItem.Click += ReflectItem_Click;
            transformMenuItem.DropDownItems.Add(reflectItem);

            this.menuStrip.Items.Add(transformMenuItem);
        }

        private void ScaleItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para aplicar a transformação.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputSx = Interaction.InputBox("Digite o fator de escala em X:", "Escala", "1");
            string inputSy = Interaction.InputBox("Digite o fator de escala em Y:", "Escala", "1");
            if (!double.TryParse(inputSx, out double sx) || !double.TryParse(inputSy, out double sy))
            {
                MessageBox.Show("Valores inválidos. Use números decimais.");
                return;
            }
            string inputOrigin = Interaction.InputBox("Aplicar em relação à origem (0) ou ao centro do polígono (1)?", "Escala", "0");
            if (!int.TryParse(inputOrigin, out int origin) || (origin != 0 && origin != 1))
            {
                MessageBox.Show("Escolha inválida. Use 0 ou 1.");
                return;
            }
            bool relativeToCenter = origin == 1;
            ApplyTransformation(polygon, GetScaleMatrix(sx, sy, relativeToCenter ? CalculateCentroid(polygon) : new Point(0, 0)));
        }

        private void TranslateItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para aplicar a transformação.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputDx = Interaction.InputBox("Digite o deslocamento em X:", "Translação", "0");
            string inputDy = Interaction.InputBox("Digite o deslocamento em Y:", "Translação", "0");
            if (!int.TryParse(inputDx, out int dx) || !int.TryParse(inputDy, out int dy))
            {
                MessageBox.Show("Valores inválidos. Use números inteiros.");
                return;
            }
            ApplyTransformation(polygon, GetTranslationMatrix(dx, dy));
        }

        private void RotateItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para aplicar a transformação.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputAngle = Interaction.InputBox("Digite o ângulo de rotação em graus:", "Rotação", "0");
            if (!double.TryParse(inputAngle, out double angle))
            {
                MessageBox.Show("Valor inválido. Use um número decimal.");
                return;
            }
            string inputOrigin = Interaction.InputBox("Aplicar em relação à origem (0) ou ao centro do polígono (1)?", "Rotação", "0");
            if (!int.TryParse(inputOrigin, out int origin) || (origin != 0 && origin != 1))
            {
                MessageBox.Show("Escolha inválida. Use 0 ou 1.");
                return;
            }
            bool relativeToCenter = origin == 1;
            ApplyTransformation(polygon, GetRotationMatrix(angle, relativeToCenter ? CalculateCentroid(polygon) : new Point(0, 0)));
        }

        private void ShearItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para aplicar a transformação.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputShx = Interaction.InputBox("Digite o fator de cisalhamento em X:", "Cisalhamento", "0");
            string inputShy = Interaction.InputBox("Digite o fator de cisalhamento em Y:", "Cisalhamento", "0");
            if (!double.TryParse(inputShx, out double shx) || !double.TryParse(inputShy, out double shy))
            {
                MessageBox.Show("Valores inválidos. Use números decimais.");
                return;
            }
            ApplyTransformation(polygon, GetShearMatrix(shx, shy));
        }

        private void ReflectItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para aplicar a transformação.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputAxis = Interaction.InputBox("Refletir em relação ao eixo X (0) ou Y (1)?", "Reflexão", "0");
            if (!int.TryParse(inputAxis, out int axis) || (axis != 0 && axis != 1))
            {
                MessageBox.Show("Escolha inválida. Use 0 para X ou 1 para Y.");
                return;
            }
            bool reflectX = axis == 0;
            // Usar o centro da tela como pivot
            //Point center = CalculateCentroid(polygon);
            Point center = new Point(panelDraw.Width / 2, panelDraw.Height / 2);
            ApplyTransformation(polygon, GetReflectionMatrix(reflectX, center));
        }

        private Matrix3x3 GetScaleMatrix(double sx, double sy, Point pivot)
        {
            Matrix3x3 translateToPivot = Matrix3x3.Translation(-pivot.X, -pivot.Y);
            Matrix3x3 scale = Matrix3x3.Scale(sx, sy);
            Matrix3x3 translateBack = Matrix3x3.Translation(pivot.X, pivot.Y);
            return translateBack * scale * translateToPivot;
        }

        private Matrix3x3 GetTranslationMatrix(int dx, int dy)
        {
            return Matrix3x3.Translation(dx, dy);
        }

        private Matrix3x3 GetRotationMatrix(double angleDegrees, Point pivot)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            Matrix3x3 translateToPivot = Matrix3x3.Translation(-pivot.X, -pivot.Y);
            Matrix3x3 rotate = Matrix3x3.Rotation(angleRadians);
            Matrix3x3 translateBack = Matrix3x3.Translation(pivot.X, pivot.Y);
            return translateBack * rotate * translateToPivot;
        }

        private Matrix3x3 GetShearMatrix(double shx, double shy)
        {
            return Matrix3x3.Shear(shx, shy);
        }

        private Matrix3x3 GetReflectionMatrix(bool reflectX, Point pivot)
        {
            Matrix3x3 translateToPivot = Matrix3x3.Translation(-pivot.X, -pivot.Y);
            Matrix3x3 reflection = reflectX ?
                new Matrix3x3(new double[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } }) :
                new Matrix3x3(new double[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });
            Matrix3x3 translateBack = Matrix3x3.Translation(pivot.X, pivot.Y);
            return translateBack * reflection * translateToPivot;
        }

        private Point CalculateCentroid(List<Point> polygon)
        {
            int sumX = 0, sumY = 0;
            foreach (var pt in polygon)
            {
                sumX += pt.X;
                sumY += pt.Y;
            }
            return new Point(sumX / polygon.Count, sumY / polygon.Count);
        }

        private void ApplyTransformation(List<Point> polygon, Matrix3x3 transformationMatrix)
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                double[] point = { polygon[i].X, polygon[i].Y, 1 };
                double[] transformed = transformationMatrix.Multiply(point);
                polygon[i] = new Point((int)Math.Round(transformed[0]), (int)Math.Round(transformed[1]));
            }
            RedrawPolygons();
        }

        private void RedrawPolygons()
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
                for (int i = 0; i < polygons.Count; i++)
                {
                    var poly = polygons[i];
                    if (poly.Count > 1)
                    {
                        for (int j = 0; j < poly.Count - 1; j++)
                        {
                            DrawLineDDAUnsafe(poly[j], poly[j + 1], Color.Black);
                        }
                        DrawLineDDAUnsafe(poly[poly.Count - 1], poly[0], Color.Black);
                        // Reaplicar preenchimento se o polígono estiver preenchido
                        if (polygonFillColors[i].HasValue)
                        {
                            ScanlineFill(drawingBitmap, poly, polygonFillColors[i].Value);
                        }
                    }
                }
            }
            panelDraw.Invalidate();
        }
        #endregion

        private class Matrix3x3
        {
            private double[,] matrix = new double[3, 3];

            public Matrix3x3(double[,] values)
            {
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        matrix[i, j] = values[i, j];
            }

            public static Matrix3x3 Translation(double tx, double ty)
            {
                return new Matrix3x3(new double[,] { { 1, 0, tx }, { 0, 1, ty }, { 0, 0, 1 } });
            }

            public static Matrix3x3 Scale(double sx, double sy)
            {
                return new Matrix3x3(new double[,] { { sx, 0, 0 }, { 0, sy, 0 }, { 0, 0, 1 } });
            }

            public static Matrix3x3 Rotation(double angleRadians)
            {
                double cos = Math.Cos(angleRadians);
                double sin = Math.Sin(angleRadians);
                return new Matrix3x3(new double[,] { { cos, -sin, 0 }, { sin, cos, 0 }, { 0, 0, 1 } });
            }

            public static Matrix3x3 Shear(double shx, double shy)
            {
                return new Matrix3x3(new double[,] { { 1, shx, 0 }, { shy, 1, 0 }, { 0, 0, 1 } });
            }

            public double[] Multiply(double[] vector)
            {
                double[] result = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    result[i] = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        result[i] += matrix[i, j] * vector[j];
                    }
                }
                return result;
            }

            public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
            {
                double[,] result = new double[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        result[i, j] = 0;
                        for (int k = 0; k < 3; k++)
                        {
                            result[i, j] += a.matrix[i, k] * b.matrix[k, j];
                        }
                    }
                }
                return new Matrix3x3(result);
            }
        }

        private void AddFillMenu()
        {
            ToolStripMenuItem fillPolygonItem = new ToolStripMenuItem("Preencher Polígono");
            fillPolygonItem.Click += FillPolygonItem_Click;
            this.menuStrip.Items.Add(fillPolygonItem);
        }

        private void FillPolygonItem_Click(object sender, EventArgs e)
        {
            if (listBoxPolygons.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um polígono na lista para preenchê-lo.");
                return;
            }
            var polygon = polygons[listBoxPolygons.SelectedIndex];
            string inputAlg = Interaction.InputBox("Escolha o algoritmo de preenchimento:\n1 - Flood Fill\n2 - Scanline Fill", "Algoritmo de Preenchimento", "1");
            if (!int.TryParse(inputAlg, out int alg) || (alg != 1 && alg != 2))
            {
                MessageBox.Show("Algoritmo inválido. Escolha 1 ou 2.");
                return;
            }
            Color fillColor;
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = selectedFillColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    fillColor = cd.Color;
                    selectedFillColor = fillColor;
                }
                else return;
            }
            if (alg == 1)
            {
                int sumX = 0, sumY = 0;
                foreach (var pt in polygon)
                {
                    sumX += pt.X;
                    sumY += pt.Y;
                }
                Point centroid = new Point(sumX / polygon.Count, sumY / polygon.Count);
                PreenchimentoFloodFill(centroid, fillColor);
                polygonFillColors[listBoxPolygons.SelectedIndex] = fillColor; // Armazena a cor
            }
            else if (alg == 2)
            {
                ScanlineFill(drawingBitmap, polygon, fillColor);
                polygonFillColors[listBoxPolygons.SelectedIndex] = fillColor; // Armazena a cor
                panelDraw.Invalidate();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
            }
            panelDraw.Invalidate();
            startPoint = null;
            polygons.Clear();
            listBoxPolygons.Items.Clear();
        }
    }
}