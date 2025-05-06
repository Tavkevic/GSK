using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2_Polygons
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen DrawPen = new Pen(Color.Black, 1);
        List<Point> VertexList = new List<Point>(); // Список точек для фигуры
        List<Point> VertexListB = new List<Point>();
        bool isDrawing = false;
        Pen dashedPen = new Pen(Color.Yellow, 2);

    public Form1()
        {
            InitializeComponent();
            g = pictureBox.CreateGraphics(); // Инициализация графики
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            this.pictureBox.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            clearButton.Click += new EventHandler(clearButton_Click);
            fillStyleComboBox.SelectedIndex = 0;
            colorPickButton.BackColor = Color.Black;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            g = pictureBox.CreateGraphics(); //инициализация графики 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void colorPickButton_click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем выбранный цвет
                Color selectedColor = colorDialog1.Color;

                // Устанавливаем цвет кнопки выбора цвета
                colorPickButton.BackColor = selectedColor;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Drawbefore(e);

            }




            else if (e.Button == MouseButtons.Right && isDrawing)
            {
                if (VertexList.Count == 1)
                {
                    Draw(e);
                }
                else if (VertexList.Count > 1)
                {

                    
                    // Добавляем третью точку
                    VertexList.Add(new Point(e.X, e.Y));

                    // Рисуем третью точку
                    g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 2, 2);

                    // Рисуем линию между второй и третьей точками
                    g.DrawLine(dashedPen, VertexList[1], VertexList[2]);

                    // Замыкаем фигуру, соединяя третью точку с первой
                    g.DrawLine(dashedPen, VertexList[2], VertexList[0]);

                    FillPolygonStandard();
                    
                    VertexList.Clear();
                    isDrawing = false;
                }
                else if (VertexList.Count > 2)
                {
                    // Замыкаем фигуру, соединяя последнюю точку с первой
                    g.DrawLine(DrawPen, VertexList[VertexList.Count - 1], VertexList[0]);

                    FillPolygonStandard();
                    

                    // Очищаем список вершин и завершаем процесс рисования
                    VertexList.Clear();
                    isDrawing = false;
                }
            }
        }

        private void Drawbefore(MouseEventArgs e)
        {
            VertexListB.Add(new Point(e.X, e.Y));

            dashedPen.DashPattern = new float[] { 10, 5 };
            
            g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 2, 2);

            if (VertexListB.Count == 2)
            {
                Point p1 = VertexListB[0];
                Point p2 = new Point(VertexListB[0].X, VertexListB[1].Y + (VertexListB[0].Y - VertexListB[1].Y)/2);
                Point p3 = new Point(VertexListB[0].X + (VertexListB[1].X - VertexListB[0].X) / 4, VertexListB[1].Y + (VertexListB[0].Y - VertexListB[1].Y) / 2);
                Point p4 = new Point(VertexListB[0].X + (VertexListB[1].X - VertexListB[0].X) / 2, VertexListB[1].Y);
                Point p5 = new Point(VertexListB[0].X + 3 * (VertexListB[1].X - VertexListB[0].X) / 4, VertexListB[1].Y + (VertexListB[0].Y - VertexListB[1].Y) / 2);
                Point p6 = new Point(VertexListB[1].X, VertexListB[1].Y + (VertexListB[0].Y - VertexListB[1].Y) / 2);
                Point p7 = new Point(VertexListB[1].X, VertexListB[0].Y);


                
                g.DrawLine(dashedPen, p1, p2);
                g.DrawLine(dashedPen, p2, p3);
                g.DrawLine(dashedPen, p3, p4);
                g.DrawLine(dashedPen, p4, p5);
                g.DrawLine(dashedPen, p5, p6);
                g.DrawLine(dashedPen, p6, p7);
                g.DrawLine(dashedPen, p7, p1);
            }

        }

        private void Draw(MouseEventArgs e)
        {
            // Добавляем новую вершину в список
            VertexList.Add(new Point(e.X, e.Y));

            // Рисуем вершину
            g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 2, 2);

            // Если вершин больше одной, рисуем линию между последней и предпоследней вершинами
            if (VertexList.Count == 2)
            {
                g.DrawLine(DrawPen, VertexList[VertexList.Count - 2], VertexList[VertexList.Count - 1]);
            }

            // Устанавливаем флаг, что идет процесс рисования
            isDrawing = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Очищаем PictureBox
            g.Clear(Color.White);

            // Очищаем список вершин
            VertexList.Clear();

            // Сбрасываем флаг рисования
            isDrawing = false;
        }

        // Стандартный алгоритм закрашивания
        private void FillPolygonStandard()
        {
            int Ymin = VertexList.Min(p => p.Y);
            int Ymax = VertexList.Max(p => p.Y);

            for (int Y = Ymin; Y <= Ymax; Y++)
            {
                List<int> Xb = new List<int>();

                for (int i = 0; i < VertexList.Count; i++)
                {
                    int k = (i < VertexList.Count - 1) ? i + 1 : 0;

                    int yi = VertexList[i].Y;
                    int yk = VertexList[k].Y;

                    if ((yi < Y && yk >= Y) || (yi >= Y && yk < Y))
                    {
                        double x = VertexList[i].X + (double)(Y - yi) / (yk - yi) * (VertexList[k].X - VertexList[i].X);
                        Xb.Add((int)Math.Round(x));
                    }
                }

                Xb.Sort();

                for (int j = 0; j < Xb.Count; j += 2)
                {
                    int xl = Xb[j];
                    int xr = Xb[j + 1];
                    g.DrawLine(new Pen(colorPickButton.BackColor), xl, Y, xr, Y);
                }
            }
        }
    }
}