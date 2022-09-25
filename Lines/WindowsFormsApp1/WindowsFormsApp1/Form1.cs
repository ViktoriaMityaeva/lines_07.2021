using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 9;
            dataGridView1.ColumnCount = 9;
            dataGridView1.RowHeadersVisible = false;                                  //Убираем заголовки строк
            dataGridView1.ColumnHeadersVisible = false;                               //Убираем заголовки столбцов

            foreach (DataGridViewColumn column in dataGridView1.Columns)              //Задаём ширину столбца
                column.Width = 55;
            foreach (DataGridViewRow row in dataGridView1.Rows)                       //Задаём высоту строки
                row.Height = 55;

            Step(mas);
            ColorOfCell(mas);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)       //Убираем выделение ячейки(синим цветом)
        {
            this.dataGridView1.ClearSelection();
        }

        int[,] mas = new int[9, 9];                                                   //Инициализируем массив
        private void ColorOfCell(int[,] mas)                                          //Метод для окраски ячеек
        {
            for (int a = 0; a < 9; a++)
                for (int b = 0; b < 9; b++)
                {
                    if (mas[a, b] == 0)
                        dataGridView1.Rows[a].Cells[b].Style.BackColor = Color.White; //Если ячейка пустая, то она белая
                    if (mas[a, b] == 1)
                        dataGridView1.Rows[a].Cells[b].Style.BackColor = Color.Yellow;
                    if (mas[a, b] == 2)
                        dataGridView1.Rows[a].Cells[b].Style.BackColor = Color.Red;
                    if (mas[a, b] == 3)
                        dataGridView1.Rows[a].Cells[b].Style.BackColor = Color.Pink;
                }
        }

        private void Step(int[,] mas)                                                 //Метод для хода компьютера
        {
            int count = 0;                                                            //Переменная для счёта
            while (count < 3)
            {
                Random rnd = new Random();                                            //Создание объекта для генерации чисел
                int value1 = rnd.Next(0, 9);                                          //Получаем случайное число
                int value2 = rnd.Next(0, 9);
                if (mas[value1, value2] == 0)
                {
                    int color = rnd.Next(1, 4);
                    count++;
                    if (color == 1) mas[value1, value2] = 1;
                    if (color == 2) mas[value1, value2] = 2;
                    if (color == 3) mas[value1, value2] = 3;
                }
            }

        }


        private int EmptyСell(int[,] mas)                                             //Метод для определения кол-ва пустых ячеек
        {
            int count = 0;                                                            //Счётчик пустых ячеек
            for (int a = 0; a < 9; a++)
                for (int b = 0; b < 9; b++)
                    if (mas[a, b] == 0) count++;
            return (count);                                                           //Возвращаем число пустых ячеек
        }

        int CountClick = 0;
        int cell = 0;
        int pointx1 = 0, pointy1 = 0;
        int pointx2 = 0, pointy2 = 0;
        int now = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)  //Матод для перестановки ячейки
        {
            if (EmptyСell(mas) < 4)                                                   //Игра завершается с выводом окна
            {
                MessageBox.Show("Игра закончилась со счётом: " + CountOfScore.ToString(), "Итоги");
                Environment.Exit(0);
            }
            int value1 = dataGridView1.CurrentCell.RowIndex;
            int value2 = dataGridView1.CurrentCell.ColumnIndex;
            if (mas[value1, value2] != 0)
            {
                now = CountClick;
                pointx1 = value1;                                                     //Координаты начальной точки
                pointy1 = value2;
            }

            if (mas[value1, value2] == 0 & CountClick == now)
            {
                pointx2 = value1;                                                     //Координаты конечной точки 
                pointy2 = value2;
                CountClick--;
                if (CanMove(pointx2, pointy2))                                        //Если точка достажима, то переприсваиваем значения 
                {
                    cell = mas[pointx1, pointy1];
                    mas[pointx2, pointy2] = cell;
                    mas[pointx1, pointy1] = 0;
                    /*
                    if (!Clearing(mas, false))
                    {
                        Step(mas);
                        Clearing(mas, false);
                    }
                    */
                    Step(mas);
                    Clearing(mas, false);
                }
            }
            ColorOfCell(mas);
        }

        private bool[,] taken;                                                        //Массив в котором отмечены ходы
        private bool CanMove(int pointx2, int pointy2)                                //Метод проверки достижимости точки 
        {
            taken = new bool[9, 9];
            Walk(pointx1, pointy1, true);
            return taken[pointx2, pointy2];
        }

        private void Walk(int pointx1, int pointy1, bool launch = false)              //Метод передвижения по полю до конечной точки
        {
            if (!launch)                                                              //Проверяем все точки, кроме начальной
            {
                if (pointx1 < 0 | pointx1 > 8 | pointy1 < 0 | pointy1 > 8) return;    //Точка находится в границах массива
                if (mas[pointx1, pointy1] > 0) return;                                //Ячейка пустая
                if (taken[pointx1, pointy1]) return;                                  //Ячейка уже проверена
            }
            taken[pointx1, pointy1] = true;
            Walk(pointx1 - 1, pointy1);                                               //Движение вверх
            Walk(pointx1, pointy1 + 1);                                               //Движение вправо
            Walk(pointx1 + 1, pointy1);                                               //Движение вниз
            Walk(pointx1, pointy1 - 1);                                               //Движение влево
        }

        int CountOfScore = 0;
        int c = 0;                                                                    //Переменная для определения порядка удаления
        int deleted = 0;
        int count1 = 0, count2 = 0, count3 = 0, count4 = 0;
        int po_x1 = 0, po_x2 = 0, po_x3 = 0, po_x4 = 0;
        int po_y1 = 0, po_y2 = 0, po_y3 = 0, po_y4 = 0;
        private int DeleteItems1(int[,] mas)
        {
            count1 = 0;
            for (int a = 0; a < 9; a++)                                               //Горизонтальные линии
            {
                for (int b = 2; b < 7; b++)
                {
                    int value = mas[a, b];
                    if (value == mas[a, b - 2] & value == mas[a, b - 1] & value == mas[a, b + 1] & value == mas[a, b + 2] & mas[a, b] != 0)
                    {
                        count1++;
                        po_x1 = b;
                        po_y1 = a;
                    }
                }
                if (count1 > 0) deleted = 1;
            }
            return (deleted);
        }

        private int DeleteItems2(int[,] mas)
        {
            count2 = 0;
            for (int b = 0; b < 9; b++)                                               //Вертикальные линии
            {
                for (int a = 2; a < 7; a++)
                {
                    int value = mas[a, b];
                    if (value == mas[a - 2, b] & value == mas[a - 1, b] & value == mas[a + 1, b] & value == mas[a + 2, b] & mas[a, b] != 0)
                    {
                        count2++;
                        po_x2 = a;
                        po_y2 = b;
                    }
                }
                if (count2 > 0) deleted = 2;
            }
            return (deleted);
        }

        private int DeleteItems3(int[,] mas)
        {
            count3 = 0;
            for (int a = 2; a < 7; a++)                                               //Главная Диагональ
            {
                for (int b = 2; b < 7; b++)
                {
                    int value = mas[a, b];
                    if (value == mas[a - 2, b - 2] & value == mas[a - 1, b - 1] & value == mas[a + 1, b + 1] & value == mas[a + 2, b + 2] & mas[a, b] != 0)
                    {
                        count3++;
                        po_x3 = b;
                        po_y3 = a;
                    }
                }
                if (count3 > 0) deleted = 3;
            }
            return (deleted);
        }

        private int DeleteItems4(int[,] mas)
        {
            count4 = 0;
            for (int a = 2; a < 7; a++)                                               //Побочная Диагональ
            {
                for (int b = 2; b < 7; b++)
                {
                    int value = mas[a, b];
                    if (value == mas[a - 2, b + 2] & value == mas[a - 1, b + 1] & value == mas[a + 1, b - 1] & value == mas[a + 2, b - 2] & mas[a, b] != 0)
                    {
                        count4++;
                        po_x4 = b;
                        po_y4 = a;
                    }
                }

                if (count4 > 0) deleted = 4;
            }
            if (deleted == 4) po_x4 -= 2; 
            //Если элементов больше 4, то координату по ширине сдвигаем влево (с координатой по высоте работаем в методе с удалением)
            return (deleted);
        }

        private bool Clearing(int[,] mas, bool clear)                                 //Метод для нахождения элементов одного цвета в линиях боллее 5 шт
        {
            c = 0;
            if (DeleteItems1(mas) == 1 & DeleteItems3(mas) == 3 & DeleteItems4(mas) == 4)
            {
                c = 1; Delete(po_x1, po_y1, count1, mas);                             //Пересечение горизонтальной и диагоналей 
                c = 3; Delete(po_x3, po_y3, count3, mas);
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 2 & DeleteItems3(mas) == 3 & DeleteItems4(mas) == 4)
            {
                c = 2; Delete(po_x2, po_y2, count2, mas);                             //Пересечение вертикальной и диагоналей
                c = 3; Delete(po_x3, po_y3, count3, mas);
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 1 & DeleteItems2(mas) == 2)
            {
                c = 1; Delete(po_x1, po_y1, count1, mas);                             //Пересечение горизонтальной и вертикальной
                c = 2; Delete(po_x2, po_y2, count2, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 1 & DeleteItems3(mas) == 3)
            {
                c = 1; Delete(po_x1, po_y1, count1, mas);                             //Пересечение горизонтальной и главной диагонали
                c = 3; Delete(po_x3, po_y3, count3, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 1 & DeleteItems4(mas) == 4)
            {
                c = 1; Delete(po_x1, po_y1, count1, mas);                             //Пересечение горизонтальной и побочной диагонали
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }

            if (DeleteItems2(mas) == 2 & DeleteItems3(mas) == 3)
            {
                c = 2; Delete(po_x2, po_y2, count2, mas);                             //Пересечение вертикальной и главной диагонали
                c = 3; Delete(po_x3, po_y3, count3, mas);
                clear = true;
            }

            if (DeleteItems2(mas) == 2 & DeleteItems4(mas) == 4)
            {
                c = 2; Delete(po_x2, po_y2, count2, mas);                             //Пересечение вертикальной и побочной диагонали
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }

            if (DeleteItems3(mas) == 3 & DeleteItems4(mas) == 4)
            {
                c = 3; Delete(po_x3, po_y3, count3, mas);                             // Пересечение диагоналей
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 1)
            {
                c = 1; Delete(po_x1, po_y1, count1, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 2)
            {
                c = 2; Delete(po_x2, po_y2, count2, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 3)
            {
                c = 3; Delete(po_x3, po_y3, count3, mas);
                clear = true;
            }

            if (DeleteItems1(mas) == 4)
            {
                c = 4; Delete(po_x4, po_y4, count4, mas);
                clear = true;
            }
            return (clear);
        }

        private void Delete(int pointx, int pointy, int count, int[,] mas)            //Метод для удаления 
        {
            count += 4;
            if (count > 4)
            {
                if(c < 4) pointx += 2;
                if (c > 2) pointy += 2;
                int da = 0;
                int da2 = 0;
                for (int i = 0; i < count; i++)
                {
                    if (c < 4) da = pointx - i;
                    if (c == 4) da = pointx + i;
                    da2 = pointy - i;
                    if (c == 1) mas[pointy, da] = 0;                                  //Удаление горизонтальных элементов
                    else
                    if (c == 2) mas[da, pointy] = 0;                                  //Удаление вертикальных элементов
                    else
                    if (c == 3) mas[da2, da] = 0;                                     //Удаление элементов по главной диагонали
                    else
                    if (c == 4) mas[da2, da] = 0;                                     //Удаление элементов по побочной диагонали
                }
                textBox1.Text = Score(count).ToString();                              //Вывод текущего счёта
            }
        }

        private int Score(int count)                                                  //Метод для подсчёта баллов 
        {
            if (count == 5) CountOfScore += 10;
            if (count == 6) CountOfScore += 12;
            if (count == 7) CountOfScore += 18;
            if (count == 8) CountOfScore += 28;
            if (count == 9) CountOfScore += 42;
            return (CountOfScore);
        }
    }
}
