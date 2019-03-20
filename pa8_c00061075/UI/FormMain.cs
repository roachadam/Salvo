using Newtonsoft.Json;
using pa8_c00061075.Game;
using pa8_c00061075.Models;
using pa8_c00061075.Networking;
using System;
using System.Drawing;
using System.Net;
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
            
        }

        private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_server != null)
                return;
            _server = new Server();
            _server.StartServer();
            string ip = GetIPAddress();
            Clipboard.SetText("127.0.0.1");
            MessageBox.Show($"Your IP Address is: {ip}\n\nIt has been copied to your clipboard to send to your opponent!"); 
        }

        public void ConnectClient(string ip)
        {
            if (_client != null)
                return;

            _client = new Client(ip);
            _client.StartClient();
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
    }
}
