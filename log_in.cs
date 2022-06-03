using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.Sql;

namespace database
{

    public partial class log_in : Form
    {

        Form1 mainform;
        public log_in(Form1 window)
        {
            mainform = window;
            InitializeComponent();
            mainform.connection_func("admin","admin");
            StartPosition = FormStartPosition.CenterScreen;
            this.textBox_password.KeyPress += new KeyPressEventHandler(keypressed);
        }

        private void keypressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                mainform.connection_func(textBox_login.Text, textBox_password.Text);
                this.Close();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '•';
            pictureBox3.Visible = false;
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;
        }



        private void btnEnter_Click(object sender, EventArgs e)
        {

            mainform.connection_func(textBox_login.Text, textBox_password.Text);
            this.Close();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox_login.Text = "";
            textBox_password.Text = "";
        }
    }
}
