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

namespace database
{
    public partial class sign_up : Form
    {

        database dataBase = new database();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (checkuser())

            {
                return;
            }

            var login = textBox_login2.Text;
            var password = textBox_password2.Text;

            string querystring = $"insert into register(login_user, password_user,is_admin) " +
                $"values('{login}','{password}',0)";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан", "Успех!");
                log_in frm_login = new log_in();
                this.Hide();
                frm_login.ShowDialog();

            }
            else 
            {
                MessageBox.Show("Аккаунт не создан!");
            
            }
            dataBase.closeConnection();          
        }
        
        private Boolean checkuser()
        {
            var loginUser = textBox_login2.Text;
            var passUser = textBox_password2.Text;


            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user,login_user, password_user, is_admin from register where " +
                $"login_user = '{loginUser}' and password_user = '{passUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует!", "Уже существует", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            { 
                return false; 
            }            
        }
        //-----------------------------------------------------------------------------------

        private void sign_up_Load(object sender, EventArgs e)
        {
            textBox_password2.PasswordChar = '•';
            pictureBox3.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox_password2.UseSystemPasswordChar = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox_password2.UseSystemPasswordChar = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = false;
        }

        private void sign_up_FormClosed(object sender, FormClosedEventArgs e)
        {
            log_in form_sign = new log_in();
            form_sign.Show();
            this.Hide();
        }
    }
}
