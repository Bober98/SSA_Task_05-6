using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {

        const int n = 3;
        const int sizeButton = 50;
        public int[,] map = new int[n * n, n * n];
        public Button[,] buttons = new Button[n * n, n * n];
        public Form1()
        {
            InitializeComponent();
            GenerateMap();
        }

        public void GenerateMap()
        {
            for(int i = 0; i < n * n; i++)
            {
                for(int j = 0; j < n * n; j++)
                {
                    map[i, j] = (i * n + i / n + j) % (n * n) + 1;
                    buttons[i, j] = new Button();
                }
            }
            
            Random r = new Random();
            for(int i = 0; i < 40; i++)
            {
                ShuffleMap(r.Next(0, 5));
            }
           
            CreateMap();
            HideCells();
        }

        public void HideCells()
        {
            int N = 40;
            Random r = new Random();
            while (N > 0)
            {
                for (int i = 0; i < n * n; i++)
                {
                    for (int j = 0; j < n * n; j++)
                    {
                        if (!string.IsNullOrEmpty(buttons[i, j].Text)){
                            int a = r.Next(0, 3);
                            buttons[i, j].Text = a == 0 ? "" : buttons[i, j].Text;
                            buttons[i, j].Enabled = a == 0 ? true : false;

                            if (a == 0)
                                N--;
                            if (N <= 0)
                                break;
                        }
                    }
                    if (N <= 0)
                        break;
                }
            }
        }

        public void ShuffleMap(int i)
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowsInBlock();
                    break;
                case 2:
                    SwapColumnsInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInColumn();
                    break;
                default:
                    MatrixTransposition();
                    break;
            }
        }

        public void SwapBlocksInColumn()
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
                block2 = r.Next(0, n);
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[i,j];
                    map[i,j] = map[i,k];
                    map[i,k] = temp;
                    k++;
                }
            }
        }

        public void SwapBlocksInRow()
        {
            Random r = new Random();
            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);
            while (block1 == block2)
                block2 = r.Next(0, n);
            block1 *= n;
            block2 *= n;
            for(int i = 0; i < n * n; i++)
            {
                var k = block2;
                for(int j = block1; j < block1 + n; j++)
                {
                    var temp = map[j, i];
                    map[j, i] = map[k, i];
                    map[k, i] = temp;
                    k++;
                }
            }
        }
        
        public void SwapRowsInBlock()
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row1 == row2)
                row2 = r.Next(0, n);
            var line2 = block * n + row2;
            for(int i = 0; i < n * n; i++)
            {
                var temp = map[line1, i];
                map[line1, i] = map[line2, i];
                map[line2, i] = temp;
            }
        }

        public void SwapColumnsInBlock()
        {
            Random r = new Random();
            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var line1 = block * n + row1;
            var row2 = r.Next(0, n);
            while (row1 == row2)
                row2 = r.Next(0, n);
            var line2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                var temp = map[i,line1];
                map[ i, line1] = map[i,line2];
                map[i,line2] = temp;
            }
        }

        public void MatrixTransposition()
        {
            int[,] tMap = new int[n * n, n * n];
            for(int i = 0; i < n * n; i++)
            {
                for(int j = 0; j < n * n; j++)
                {
                    tMap[i, j] = map[j, i];
                }
            }
            map = tMap;
        }

        public void CreateMap()
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    Button button = new Button();
                    buttons[i, j] = button;
                    button.Size = new Size(sizeButton, sizeButton);
                    button.Text = map[i, j].ToString();
                    button.Click += OnCellPressed;
                    button.Location = new Point(j * sizeButton, i * sizeButton);
                    this.Controls.Add(button);
                }
            }
        }

        public void OnCellPressed(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;
            if (string.IsNullOrEmpty(buttonText))
            {
                pressedButton.Text = "1";
            }
            else
            {
                int num = int.Parse(buttonText);
                num++;
                if (num == 10)
                    num = 1;
                pressedButton.Text = num.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < n * n; i++)
            {
                for(int j = 0; j < n * n; j++)
                {
                    var btnText = buttons[i, j].Text;
                    if(btnText != map[i, j].ToString())
                    {
                        MessageBox.Show("Неверно!");
                        return;
                    }
                }
            }
            MessageBox.Show("Верно!");
            for(int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateMap();

        }

        public bool SolveSudoku(int[,] sudoku)
        {
            int row, col;

            // Поиск пустой клетки
            if (!FindEmptyCell(sudoku, out row, out col))
            {
                return true; // Если нет пустых клеток, то судоку уже решено
            }

            // Попробовать разместить числа от 1 до 9 в пустую клетку
            for (int num = 1; num <= 9; num++)
            {
                // Проверяем, можно ли разместить число в клетке
                if (CanPlaceNumber(sudoku, row, col, num))
                {
                    // Если можно, размещаем число в клетке
                    sudoku[row, col] = num;

                    // Рекурсивно вызываем SolveSudoku для следующей клетки
                    if (SolveSudoku(sudoku))
                        return true; // Если решение найдено, возвращаем true

                    // Если решение не найдено, отменяем размещение и пытаемся с другим числом
                    sudoku[row, col] = 0;
                }
            }

            // Если ни одно число не подходит, возвращаем false
            return false;
        }

        private static bool FindEmptyCell(int[,] sudoku, out int row, out int col)
        {
            row = -1;
            col = -1;
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        return true;
                    }
                }
            }
            return false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public bool CanPlaceNumber(int[,] sudoku, int row, int col, int num)
        {
            // Отладочный вывод
            MessageBox.Show($"Пытаемся разместить число {num} в клетке [{row}, {col}]");

            // Проверяем, что число не встречается в строке и столбце
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[row, i] == num || sudoku[i, col] == num)
                    return false;
            }

            // Проверяем, что число не встречается в квадрате 3x3
            int startRow = row - row % 3;
            int startCol = col - col % 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (sudoku[i, j] == num)
                        return false;
                }
            }

            // Если число не встречается ни в строке, ни в столбце, ни в квадрате 3x3, можно его разместить
            return true;
        }
        public void ClearBoard()
        {
            // Очищаем текст и делаем доступными все кнопки на доске
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    buttons[i, j].Text = "";
                    buttons[i, j].Enabled = true;
                }
            }
        }
        public string PrintMap(int[,] sudoku)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    sb.Append(sudoku[i, j] + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SolveSudoku(map))
            {
                MessageBox.Show("Судоку успешно решено!\n\nРешение:\n" + PrintMap(map));
            }
            else
            {
                MessageBox.Show("Не удалось найти решение для судоку.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearBoard();
        }

        // Ваш существующий код здесь...
    }

}

