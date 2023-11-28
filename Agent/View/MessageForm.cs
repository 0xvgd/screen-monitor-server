using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agent.View
{
    public partial class MessageForm : Form
    {

        public delegate void SendMessageToUserDelegate(String content);
        public SendMessageToUserDelegate SendMessageToUser = null;
        public MessageForm()
        {
            InitializeComponent();
        }

        private void apply_btn_Click(object sender, EventArgs e)
        {
            if (content_textBox.Text == "")
            {
                MessageBox.Show("Enter message");
                return;
            }
            if (SendMessageToUser != null)
                SendMessageToUser(content_textBox.Text);
            this.Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
