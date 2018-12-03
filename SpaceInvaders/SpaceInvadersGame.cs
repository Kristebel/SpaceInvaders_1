using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class SpaceInvadersGame : Form
    {
        private Shooter _shooter = new Shooter();
        private Bullet _bullet = new Bullet();
        private BlackInvader _blackInvader = new BlackInvader();
        private bool _shoot = false;
        private int _scoreCount = 0;
        private int _sideBounds = 20;
        private DateTime _lastCheck;
        private BindingList<BlackInvader> _blackInvaders;

        public SpaceInvadersGame()
        {
            InitializeComponent();
        }

        private void GetInvaders()
        {
            var invader = new BlackInvader();
            _blackInvaders = new BindingList<BlackInvader>();
            for (int i = 0; i + invader.Width < gamePanel.Size.Width - _sideBounds; i += invader.Width)
            {
                _blackInvaders.Add(new BlackInvader
                {
                    Location = new Point(i, 0)
                });
            }
        }

        private void MoveInvaders()
        {
            if (_blackInvaders.Last().Location.X + _blackInvader.Width < gamePanel.Bounds.Right)
            {
                foreach (var i in _blackInvaders)
                {
                    i.Location = new Point(i.Location.X + _blackInvader.Speed, i.Location.Y + _blackInvader.Move);
                }
            }
            if (_blackInvaders.First().Location.X - _blackInvader.Width > gamePanel.Bounds.Left)
            {
                foreach (var i in _blackInvaders)
                {
                    i.Location = new Point(i.Location.X - _blackInvader.Speed, i.Location.Y + _blackInvader.Move);

                }
            }
            _lastCheck = DateTime.Now;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_shoot && _bullet != null)
            {
                _bullet.Location = new Point(_bullet.Location.X, _bullet.Location.Y - _shooter.Speed);
            }

            if (_lastCheck.AddSeconds(6) <= DateTime.Now && _blackInvaders?.Count > 0)
            {
                MoveInvaders();
            }

            foreach (Control i in gamePanel.Controls)
            {
                foreach (Control b in gamePanel.Controls)
                {
                    if (i is BlackInvader)
                    {
                        if (b is Bullet)
                        {
                            if (i.Bounds.IntersectsWith(b.Bounds))
                            {
                                _blackInvaders.Remove(i as BlackInvader);
                                gamePanel.Controls.Remove(i);
                                gamePanel.Controls.Remove(b);
                                _scoreCount++;
                                _shoot = false;
                                return;
                            }
                            else if (b.Location.Y < 0)
                            {
                                gamePanel.Controls.Remove(b);
                                _shoot = false;
                                return;
                            }
                        }
                        if (i.Bounds.IntersectsWith(this._shooter.Bounds) || i.Bounds.Bottom >= this.gamePanel.Bounds.Bottom)
                        {
                            timer.Stop();
                            MessageBox.Show("GAME OVER!!!");
                            btnStart.Enabled = true;
                            return;
                        }
                    }
                }
            }
            scoreCount.Text = _scoreCount.ToString();
        }

        private void SpaceInvaders_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && gamePanel.Bounds.Right - _shooter.Width > _shooter.Location.X)
            {
                _shooter.Location = new Point(_shooter.Location.X + _shooter.Speed, _shooter.Location.Y);
            }
            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && (_shooter.Location.X - _shooter.Speed) > gamePanel.Location.X)
            {
                _shooter.Location = new Point(_shooter.Location.X - _shooter.Speed, _shooter.Location.Y);
            }
            if (e.KeyCode == Keys.Space && !_shoot)
            {
                _bullet = new Bullet();
                _bullet.Location = new Point(_shooter.Location.X, _shooter.Location.Y);
                gamePanel.Controls.Add(_bullet);
                _shoot = true;
            }
        }

        private void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GetInvaders();  
        }

        private void bgw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            foreach (var invader in _blackInvaders)
                gamePanel.Controls.Add(invader);

            _shooter.Location = new Point(gamePanel.Bounds.Width / 2, gamePanel.Bounds.Bottom - _shooter.Width);
            gamePanel.Controls.Add(_shooter);
            _shooter.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            gamePanel.Controls.Clear();
            gamePanel.Refresh();
            _shoot = false;
            btnStart.Enabled = false;
            _scoreCount = 0;
            bgw.RunWorkerAsync();
            timer.Start();
            _lastCheck = DateTime.Now;
        }
    }
}
