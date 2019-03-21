using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pa8_c00061075.UI
{
    public partial class FormClientConnect : Form
    {
        private FormMain _formMain;
        public FormClientConnect(FormMain formMain)
        {
            _formMain = formMain;
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbIp.Text)) return;
            
            new Thread(() =>
            {
                _formMain.ConnectClient(tbIp.Text);
            }).Start();
            //Thread.Sleep(500);
            //Hide();
        }
    }
}
