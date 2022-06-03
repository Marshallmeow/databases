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
    
    public partial class add_form_worker : Form
    {
        SqlConnection connection;
        public add_form_worker(SqlConnection form)
        {
            InitializeComponent();
            connection = form;
            connection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {

                string sqlExpression = "insertWorker";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;


                var fio = textBox_fio.Text;
                var pasport = textBox_pasport.Text;
                var spec = textBox_spec.Text;
                var age = textBox_age.Text;
                var male = textBox_male.Text;


                SqlParameter fioParam = new SqlParameter
                {
                    ParameterName = "@FIO",
                    Value = fio
                };
                command.Parameters.Add(fioParam);

                SqlParameter pasportParam = new SqlParameter
                {
                    ParameterName = "@pasport",
                    Value = pasport
                };
                command.Parameters.Add(pasportParam);

                SqlParameter specParam = new SqlParameter
                {
                    ParameterName = "@spec",
                    Value = spec
                };
                command.Parameters.Add(specParam);

                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@age",
                    Value = age
                };
                command.Parameters.Add(ageParam);

                SqlParameter maleParam = new SqlParameter
                {
                    ParameterName = "@male",
                    Value = male
                };
                command.Parameters.Add(maleParam);

                var id = command.ExecuteNonQuery();

                MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            
        } 
    }
    
}