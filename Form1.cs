using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        int directionX;
        int directionY;

        string tempMove; // переменная для запоминания предыдущей клавиши движения

        Point fruitLocation;

        int score = 0;

        const int unitSize = 40;

        List<PictureBox> snake = new List<PictureBox>();

        PictureBox fruit;
        PictureBox unit;

        Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            // Подписка на ивенты
            this.KeyDown += GetInput;
            timer.Tick += Update;

            // Генерация карты
            MapGenerate();

            // Создание змейки
            unit = new PictureBox();
            unit.Size = new Size(unitSize, unitSize);
            unit.Location = new Point(40, 40);
            unit.BackColor = Color.Lime;

            Controls.Add(unit);

            snake.Add(unit);

            // Задание направления движения змейки
            directionX = 1;
            directionY = 0;

            // Генерация фруктов
            FruitGenerate();
        }

        private void StopGameTrigger()
        {
            if (snake[0].Location.X > 760 || snake[0].Location.X < 0 || snake[0].Location.Y > 760 || snake[0].Location.Y < 0)
            {
                GameOver();
            }

            if (score > 0)
            {
                for (int i = snake.Count - 1; i >= 1; i--)
                {
                    if (snake[0].Location == snake[i].Location) GameOver();
                }
            }
        }

        private void MapGenerate()
        {
            for (int i = 0; i <= 800; i += 40)
            {
                // создание вехрних стен
                PictureBox topSideWall = new PictureBox();

                topSideWall.BackColor = Color.Black;
                topSideWall.Size = new Size(800, 1);
                topSideWall.Location = new Point(topSideWall.Location.X, i);

                Controls.Add(topSideWall);

                // создание боковых стен
                PictureBox leftSideWall = new PictureBox();

                leftSideWall.BackColor = Color.Black;
                leftSideWall.Size = new Size(1, 800);
                leftSideWall.Location = new Point(i, leftSideWall.Location.Y);

                Controls.Add(leftSideWall);
            }
        }

        private void FruitGenerate()
        {
            fruit = new PictureBox();

            fruit.Size = new Size(unitSize, unitSize);
            fruit.BackColor = Color.Gold;

            int fruitX = 0;
            int fruitY = 0;

            bool isSnake = true;

            while (isSnake)
            {
                fruitX = random.Next(1, 20);
                fruitY = random.Next(1, 20);

                foreach (var unit in snake)
                {
                    if (unit.Location == new Point(fruitX * unitSize, fruitY * unitSize)) break;

                    isSnake = false;
                    break;
                }
            }

            fruit.Location = new Point(fruitX * unitSize, fruitY * unitSize);
            fruitLocation = fruit.Location;

            Controls.Add(fruit);
        }

        private void EatFruit()
        {
            if (snake[0].Location == fruitLocation)
            {
                score++;
                scoreLB.Text = $"Score : {score}";

                Controls.Remove(fruit);

                FruitGenerate();

                unit = new PictureBox();
                unit.Size = new Size(unitSize, unitSize);
                unit.Location = new Point(snake.Last().Location.X - directionX * unitSize, snake.Last().Location.Y - directionY * unitSize);
                unit.BackColor = Color.Lime;

                snake.Add(unit);

                Controls.Add(unit);

            }
        }

        private void Update(object myObject, EventArgs e)
        {
            SnakeMovement();

            EatFruit();

            StopGameTrigger();
        }

        private void SnakeMovement()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }

            snake[0].Location = new Point(snake[0].Location.X + directionX * unitSize, snake[0].Location.Y + directionY * unitSize);
        }

        private void GetInput(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                case "D":
                    if (tempMove != "A")
                    {
                        directionX = 1;
                        directionY = 0;

                        tempMove = "D";
                    }
                    break;
                case "Left":
                case "A":
                    if (tempMove != "D")
                    {
                        directionX = -1;
                        directionY = 0;

                        tempMove = "A";
                    }
                    break;
                case "Up":
                case "W":
                    if (tempMove != "S")
                    {
                        directionX = 0;
                        directionY = -1;

                        tempMove = "W";
                    }
                    break;
                case "Down":
                case "S":
                    if (tempMove != "W")
                    {
                        directionX = 0;
                        directionY = 1;

                        tempMove = "S";
                    }
                    break;
            }
        }

        private void GameOver()
        {
            timer.Stop();

            var result = MessageBox.Show($"YOU LOOOSE\nYOUR SCORE : {score}\nDO YOU WANNA TRY AGAIN???", "GAME OVER", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
