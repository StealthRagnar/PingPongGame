using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPongGame
{
    public partial class Form1 : Form
    {
        private int originalWidth;
        private int originalHeight;
        int ballXspeed = 4;
        int ballYspeed = 4;
        int speed = 2;
        Random rand = new Random();
        bool goDown, goUp;
        int computer_speed_change = 50;
        int PlayerScore = 0;
        int ComputerScore = 0;
        int PlayerSpeed = 8;
        private const int DefaultPlayerSpeed = 8;
        int[] i = { 10, 11, 16, 18};
        int[] j = { 10, 11, 16, 18};
        private int difficultyLevel = 1;

        private void Form1_Resize(object sender, EventArgs e)
        {
            float widthScale = (float)this.ClientSize.Width / originalWidth;
            float heightScale = (float)this.ClientSize.Height / originalHeight;

            // Adjust paddle sizes
            Player.Width = (int)(20 * widthScale);
            Player.Height = (int)(100 * heightScale);

            Computer.Width = (int)(20 * widthScale);
            Computer.Height = (int)(100 * heightScale);

            // Adjust ball size
            ball.Width = (int)(20 * widthScale);
            ball.Height = (int)(20 * heightScale);

            // Adjust positions proportionally
            Player.Left = 20; // Fixed position on the left
            Player.Top = (int)(Player.Top * heightScale); // Scale its position

            Computer.Left = this.ClientSize.Width - Computer.Width - 20; // Align to the right edge
            Computer.Top = (int)(Computer.Top * heightScale); // Scale its position

            ball.Left = (int)(ball.Left * widthScale);
            ball.Top = (int)(ball.Top * heightScale);
        }

        public Form1()
        {
            InitializeComponent();
            originalWidth = this.ClientSize.Width;
            originalHeight = this.ClientSize.Height;
            this.Resize += new EventHandler(Form1_Resize);
        }

        private void Player_Click(object sender, EventArgs e)
        {

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            ball.Top -= ballYspeed;
            ball.Left -= ballXspeed;

            this.Text = "Player Score: " + PlayerScore + " -- Computer Score: " +
                ComputerScore;
            if(ball.Top < 0 || ball.Bottom > this.ClientSize.Height)
            {
                ballYspeed = -ballYspeed;
            }
            if(ball.Left < -2) // computer score
            {
                ball.Left = 300;
                ballXspeed = -ballXspeed;
                ComputerScore++;
            }

            if (ball.Right > this.ClientSize.Width + 2)
            {
                ball.Left = 300;
                ballXspeed = -ballXspeed;
                PlayerScore++;
            }
            if(Computer.Top <= 1)
            {
                Computer.Top = 0;
            }
            else if (Computer.Bottom >= this.ClientSize.Height)
            {
                Computer.Top = this.ClientSize.Height - Computer.Height;
            } 

            if(ball.Top < Computer.Top + (Computer.Height/2)&& ball.Left > 300)
            {
                Computer.Top -= speed;
            }
            if (ball.Top > Computer.Top + (Computer.Height / 2) && ball.Left > 300)
            {
                Computer.Top += speed;
            }

            computer_speed_change -= 1;
            if(computer_speed_change < 0)
            {
                speed = i[rand.Next(i.Length)];
                computer_speed_change = 50;
            }

            if(goDown && Player.Top + Player.Height < this.ClientSize.Height)
            {
                Player.Top += PlayerSpeed;
            }

            if(goUp && Player.Top > 0)
            {
                Player.Top -= PlayerSpeed;
            }

            CheckCollision(ball, Player, Player.Right + 5);
            CheckCollision(ball, Computer, Computer.Left - 35);

            if(ComputerScore > 3)
            {
                GameOver("Sorry, You lost the Game. Better Luck Next Time");
            }
            else if(PlayerScore > 3)
            {
                GameOver("Congratulations! You did it");
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
                goDown = true;
            }
            if(e.KeyCode == Keys.W)
            {
                goUp = true;
            }
               
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.W)
            {
                goUp = false;
            }
        }

        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offset)
        {
            if (PicOne.Bounds.IntersectsWith(PicTwo.Bounds))
            {
                PicOne.Left = offset;

                int x = j[rand.Next(j.Length)];
                int y = j[rand.Next(j.Length)];

                if(ballXspeed < 0)
                {
                    ballXspeed = x;
                }
                else
                {
                    ballXspeed = -x;
                }

                if (ballYspeed < 0)
                {
                    ballYspeed = -y;
                }
                else
                {
                    ballYspeed = y;
                }


            }
        }

        private void Computer_Click(object sender, EventArgs e)
        {

        }

        private void GameOver(string message)
        {
            GameTimer.Stop();
            MessageBox.Show(message, "Udyan Says: ");

            // Reset game scores
            ComputerScore = 0;
            PlayerScore = 0;

            // Reset ball speed
            ballXspeed = ballYspeed = 4;

            if (message.Contains("Congratulations")) // Player wins
            {
                // Increase difficulty
                difficultyLevel++;
                speed += 1; // Increase computer speed
                computer_speed_change = Math.Max(10, computer_speed_change - 5); // Reduce reaction time, minimum 10
                PlayerSpeed = DefaultPlayerSpeed; // Reset player speed on win
            }
            else // Player loses
            {
                difficultyLevel = 1; // Reset difficulty level
                speed = 2;           // Reset computer speed
                computer_speed_change = 50; // Reset reaction time

                // Increase player speed to make it easier in the next round
                PlayerSpeed = Math.Min(20, PlayerSpeed + 2); // Cap player speed at 20
            }

            // Restart the game
            GameTimer.Start();
        }
    }
}
