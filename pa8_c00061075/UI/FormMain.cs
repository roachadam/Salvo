using Newtonsoft.Json;
using pa8_c00061075.Game;
using pa8_c00061075.Models;
using pa8_c00061075.Networking;
using System;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace pa8_c00061075.UI
{
    public partial class FormMain : Form
    {
        private Salvo _salvo;

        private Server _server;
        private Client _client;

        public FormMain()
        {
            _salvo = new Salvo();
            InitializeComponent();

            DrawMyShip();
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


        private void FormMain_Load(object sender, EventArgs e)
        {
            //DEBUGGING ONLY
            if (System.Diagnostics.Debugger.IsAttached)
                startServerToolStripMenuItem_Click(null, null);
            else
                ConnectClient("127.0.0.1");
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_server != null)
                return;
            _server = new Server();
            _server.StartServer();
            //string ip = GetIPAddress();
            Clipboard.SetText("127.0.0.1");
            //MessageBox.Show($"Your IP Address is: {ip}\n\nIt has been copied to your clipboard to send to your opponent!"); 


            // Wait for opponent to connect

            //Enabled = false;
        }

        public void ConnectClient(string ip)
        {
            if (_client != null)
                return;
            new Thread(() =>
            {
                _client = new Client(ip);
                _client.StartClient();
            }).Start();

        }

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

        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormClientConnect(this).ShowDialog();
        }

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
    }
}
