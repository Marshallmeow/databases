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
    public partial class add_form_sklad : Form
    {
        SqlConnection connection;
        public add_form_sklad(SqlConnection form)
        {
            InitializeComponent();
            connection = form;
            connection.Open();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
                
                string sqlExpression = "insertSklad";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

                var counter = textBox_counter.Text;
                var free = textBox_free.Text;
                var occ = textBox_occ.Text;
                var type = textBox_type.Text;

                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count_cells",
                    Value = counter
                };
                command.Parameters.Add(countParam);

                SqlParameter freeParam = new SqlParameter
                {
                    ParameterName = "free_cells",
                    Value = free
                };
                command.Parameters.Add(freeParam);

                SqlParameter occParam = new SqlParameter
                {
                    ParameterName = "@occ_cells",
                    Value = occ
                };
                command.Parameters.Add(occParam);

                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@type",
                    Value = type
                };
                command.Parameters.Add(typeParam);

                var id = command.ExecuteNonQuery();

                MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            
        }
    }
}
