using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pa8_c00061075.UI
{
    public partial class FormGameOver : Form
    {
        public FormGameOver(bool won)
        {
            
            InitializeComponent();
            lMessage.Text = won ? "Winner!" : "Game Over!";
        }

        private void GameOver_Load(object sender, EventArgs e)
        {

        }
    }
}
