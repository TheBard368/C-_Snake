using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeCSharp
{
    public partial class Form : System.Windows.Forms.Form
    {
        int cols = 50, rows = 25;
        int score = 0;
        int dx = 0, dy = 0;
        int front = 0, back = 0;

        Piece[] snake = new Piece[1250];
        List<int> aval = new List<int>();
        bool[,] visit;

        Random rand = new Random();
        Timer time = new Timer();
        public Form()
        {
            InitializeComponent();
            intial();
            launchTimer();
        }

        private void intial()
        {
            visit = new bool[rows, cols];
            Piece head = new Piece((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            lFood.Location = new Point((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);

            for(int i=0; i<rows; i++)
            {
                for(int j=0; j<cols; j++)
                {
                    visit[i, j] = false;
                    aval.Add(i * cols + j);
                }
            }

            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            aval.Remove((head.Location.Y / 20) * cols + (head.Location.X / 20));
            Controls.Add(head);
            snake[front] = head;


        }

        private void launchTimer()
        {
            time.Interval = 50;
            time.Tick += move;
            time.Start();
        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X;
            int y = snake[front].Location.Y;

            if(dx == 0 && dy == 0) return;
            if(collisionWall(x + dx, y + dy))
            {
                time.Stop();
                MessageBox.Show("Game Over");
                return;
            }

            if (collisionFood(x + dx, y + dy))
            {
                score++;
                lScore.Text = "Points : " + score.ToString();

                if (collision((y + dy) / 20, (x + dx) / 20))
                {
                    time.Stop();
                    MessageBox.Show("Game Over");
                    return;
                }

                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + 1250) % 1250;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                randFood();
            }
            else
            {
                if (collision((y + dy) / 20, (x + dx) / 20))
                {
                    time.Stop();
                    MessageBox.Show("Game Over");
                    return;
                }

                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + 1250) % 1250;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + 1250) % 1250;
                visit[(y + dy) / 20, (x + dx) / 20] = true;

            }
        }

        private void randFood()
        {
            aval.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if(!visit[i,j])aval.Add(i * cols + j);
                }
            }
            int idx = rand.Next(aval.Count) % aval.Count;
            lFood.Left = (aval[idx] * 20) % Width;
            lFood.Top = (aval[idx] * 20) / Width * 20;

        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            dx = dy = 0;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    dx = 20;
                    break;
                case Keys.Left:
                    dx = -20;
                    break;
                case Keys.Up:
                    dy = -20;
                    break;
                case Keys.Down:
                    dy = 20;
                    break;
            }
        }

        private bool collision(int x, int y)
        {
            return visit[x, y];
           
        }

        private bool collisionFood(int x, int y)
        {
            return x == lFood.Location.X && y == lFood.Location.Y;
        }

        private bool collisionWall(int x, int y)
        {
            return (x < 0 || y < 0 || x > 980 || y > 480);
        }
    }
}
