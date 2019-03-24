using Newtonsoft.Json;
using pa8_c00061075.Game;
using pa8_c00061075.Models;
using pa8_c00061075.Networking;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using pa8_c00061075.Structs;

namespace pa8_c00061075.UI
{
    public partial class FormMain : Form
    {
        private Salvo _salvo;

        private Server _server;
        private Client _client;

        private bool _shouldProcess = true;
        private bool _isExiting;

        #region  " Form "

        public FormMain()
        {
            _salvo = new Salvo();
            InitializeComponent();

            DrawMyShip();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //DEBUGGING ONLY
            //if (System.Diagnostics.Debugger.IsAttached)
            //    startServerToolStripMenuItem_Click(null, null);
            //else
            //    ConnectClient("127.0.0.1");


            PopulateLocations();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_isExiting)
                CheckExit();
        }

        private void PopulateLocations()
        {
            string[] col = new[] { "A", "B", "C", "D", "E" };
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    cbLocation.Items.Add(col[j] + (i + 1));
                }
            }

            cbLocation.SelectedIndex = 0;
        }

        #endregion

        #region " Menu Strip "

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_server != null)
                return;
            _server = new Server();
            _server.StartServer();
            _server.Attack += Attack;
            _server.AttackResult += AttackResult;
            _server.WinResult += WinResult;
            //string ip = GetIPAddress();
            Clipboard.SetText("127.0.0.1");
            //MessageBox.Show($"Your IP Address is: {ip}\n\nIt has been copied to your clipboard to send to your opponent!"); 


            // Wait for opponent to connect

            //Enabled = false;
        }


        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormClientConnect(this).ShowDialog();
        }

        #endregion

        #region " Draw Ship "

        private void MarkHit(Coordinates coords)
        {
            Invoke(new MethodInvoker(() =>
            {
                string name = GetTileName(coords);
                foreach (Control c in Controls)
                {
                    Button b = c as Button;
                    if (b == null)
                        continue;

                    if (b.Name == name)
                    {
                        b.BackColor = Color.Red;
                        b.Invoke(new MethodInvoker(() => b.Enabled = false));
                    }

                }
            }));

        }
        private void DrawMyShip()
        {
            Location myShipLoc = _salvo.Ships[0].Location;

            foreach (Coordinates coords in myShipLoc.Coordinates)
            {
                string name = GetTileName(coords);
                foreach (Control c in Controls)
                {
                    Button b = c as Button;
                    if (b == null)
                        continue;

                    if (b.Name == name)
                        b.BackColor = Color.Black;
                }
            }
        }

        private string GetTileName(Coordinates coords)
        {
            return "y" + coords.X + coords.Y;
        }

        #endregion


        // Called when opponent attacks you
        private void Attack(SalvoAttack attack)
        {
            if (!_shouldProcess)
                return;
            Coordinates attackCoords = new Coordinates(attack.X, attack.Y);
            bool wasHit = _salvo.IsHit(attackCoords);
            if (wasHit)
            {
                MarkHit(attackCoords);
                _salvo.MarkHitOnMyShip(attackCoords);
            }
               

            _server?.SendAttackResult(new SalvoData()
            {
                IsAttackResult = true,
                AttackResult = new SalvoAttackResult()
                {
                    WasHit = wasHit,
                }
            });
            _client?.SendAttackResult(new SalvoData()
            {
                IsAttackResult = true,
                AttackResult = new SalvoAttackResult()
                {
                    WasHit = wasHit,
                }
            });
            //bool wasFinalBlow = _salvo.ShipSunk();

            //if (wasFinalBlow)
            //{
            //    _server?.SendWinResult(new SalvoData()
            //    {
            //        IsWinResult = true,
            //        SalvoWinResult = new SalvoWinResult()
            //        {
            //            DidWin = false
            //        }
            //    });
            //    _client?.SendWinResult(new SalvoData()
            //    {
            //        IsWinResult = true,
            //        SalvoWinResult = new SalvoWinResult()
            //        {
            //            DidWin = false
            //        }
            //    });
            //    new FormGameOver(true).ShowDialog();
            //    Application.Exit();
            //}
            //else
            //{
            //    _server?.SendAttackResult(new SalvoData()
            //    {
            //        IsAttackResult = true,
            //        AttackResult = new SalvoAttackResult()
            //        {
            //            WasHit = wasHit,
            //        }
            //    });
            //    _client?.SendAttackResult(new SalvoData()
            //    {
            //        IsAttackResult = true,
            //        AttackResult = new SalvoAttackResult()
            //        {
            //            WasHit = wasHit,
            //        }
            //    });
            //}

            lOGrid.Invoke(new MethodInvoker(() => lOGrid.Text = GetGridLocation(attackCoords)));
            lOHit.Invoke(new MethodInvoker(() => lOHit.Text = (wasHit ? "Hit!" : "Miss!")));
            btnLaunch.Invoke(new MethodInvoker(() => btnLaunch.Enabled = true));

        }

        // Called after you attack
        private void AttackResult(SalvoAttackResult attackResult)
        {
            if (!_shouldProcess)
                return;
            if (attackResult.WasHit)
            {
                lHitMiss.Invoke(new MethodInvoker(() => lHitMiss.Text = "Hit!"));
                _salvo.MarkHitOnEnemyShip(_lastAttack);
            }
            else
            {
                lHitMiss.Invoke(new MethodInvoker(() => lHitMiss.Text = "Miss!"));
            }

            if (_salvo.DidSinkEnemy())
            {
                _server?.SendWinResult(new SalvoData()
                {
                    IsWinResult = true,
                    SalvoWinResult = new SalvoWinResult()
                    {
                        DidWin = false
                    }
                });
                _client?.SendWinResult(new SalvoData()
                {
                    IsWinResult = true,
                    SalvoWinResult = new SalvoWinResult()
                    {
                        DidWin = false
                    }
                });
                new FormGameOver(true).ShowDialog();
                Application.Exit();
            }

        }

        private void WinResult(SalvoWinResult winResult)
        {
            new FormGameOver(winResult.DidWin).ShowDialog();
            Application.Exit();
        }

        public void ConnectClient(string ip)
        {
            if (_client != null)
                return;
            _client = new Client(ip);
            _client.Attack += Attack;
            _client.AttackResult += AttackResult;
            _client.WinResult += WinResult;
            new Thread(() =>
            {
                _client.StartClient();
            }).Start();
        }




        #region  " Buttons "

        private void your_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            // If marking hit on ship location, should be red to denote a hit

            if (b.FlatAppearance.BorderColor == Color.Red)
            {
                b.FlatAppearance.BorderSize = 2;
                b.FlatAppearance.BorderColor = Color.Black;
            }
            else if (b.BackColor == Color.Black)
            {
                b.FlatAppearance.BorderSize = 2;
                b.FlatAppearance.BorderColor = Color.Red;
            }
            else if (b.BackColor == SystemColors.Highlight)
            {
                b.BackColor = Color.White;
            }
            else if (b.BackColor == Color.White)
            {
                b.BackColor = SystemColors.Highlight;
            }
        }

        private void opponent_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            // Press once for white mark, a second press for red hit

            if (b.BackColor == SystemColors.Highlight)
                b.BackColor = Color.White;

            else if (b.BackColor == Color.Red)
                b.BackColor = SystemColors.Highlight;

            else if (b.BackColor == Color.White)
                b.BackColor = Color.Red;
        }

        private Coordinates _lastAttack;
        private void btnLaunch_Click(object sender, EventArgs e)
        {
            Coordinates coords = GetCoordinates();
            var attack = new SalvoAttack()
            {
                X = coords.X,
                Y = coords.Y
            };

            _server?.LaunchAttack(new SalvoData()
            {
                Message = "thjis is from the client",
                IsAttack = true,
                Attack = attack
            });
            _client?.LaunchAttack(new SalvoData()
            {
                Message = "thjis is from the client",
                IsAttack = true,
                Attack = attack
            });
            _lastAttack = coords;
            lGrid.Text = cbLocation.Text;
            btnLaunch.Enabled = false;
        }

        #endregion

        #region  " Helper Methods "

        private string GetIPAddress()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string ip = JsonConvert.DeserializeObject<dynamic>(wc.DownloadString("http://ipinfo.io/json")).ip;
                    return ip;
                }
            }
            catch
            {
                return "ERROR";
            }
        }

        private Coordinates GetCoordinates()
        {
            string letter = cbLocation.Text.Substring(0, 1);
            string num = cbLocation.Text.Substring(1);

            int y = 0;
            switch (letter)
            {
                case "A":
                    y = 0;
                    break;
                case "B":
                    y = 1;
                    break;
                case "C":
                    y = 2;
                    break;
                case "D":
                    y = 3;
                    break;
                case "E":
                    y = 4;
                    break;
            }
            int x = int.Parse(num) - 1;


            return new Coordinates(x, y);
        }

        private string GetGridLocation(Coordinates coords)
        {
            string result = string.Empty;
            switch (coords.Y)
            {
                case 0:
                    result = "A";
                    break;
                case 1:
                    result = "B";
                    break;
                case 2:
                    result = "C";
                    break;
                case 3:
                    result = "D";
                    break;
                case 4:
                    result = "E";
                    break;
            }

            int x = coords.X + 1;
            return result + x;
        }


        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckExit();
        }

        private void CheckExit()
        {
            _isExiting = true;

            if(MessageBox.Show("Are you sure you want to exit? The current game state will be lost.", 
                   "Are you use?", MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            {
                _server?.StopServer();
                _client?.StopClient();

                Application.Exit();
            }

            _isExiting = false;
        }
    }
}
