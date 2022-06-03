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
    public partial class add_form_product : Form
    {
        SqlConnection connection;
        DataTable table;
        public add_form_product(SqlConnection form)
        {
            InitializeComponent();
            connection = form;
            connection.Open();
            ComboBox_pk_key.Items.Clear();
            string request = "select pk_number_of_post,product from product";
            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
            table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                ComboBox_pk_key.Items.Add(row[1] + "(" + row[0] + ")");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            SqlConnection con = connection;
            connection.Open();
            int fk_key = 0;
            string selected = ComboBox_pk_key.SelectedItem.ToString();
            string[] subs = selected.Split('(');
            foreach (DataRow row in table.Rows)
            {
                if (row[1].ToString() == subs[0])
                    fk_key = Convert.ToInt32(row[0]);
            }
            string sqlExpression = "insertProduct";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = CommandType.StoredProcedure;

            var name = textBox_name.Text;
            var date = textBox_date.Text;
            var storage = textBox_storage.Text;
            var conditions = textBox_conditions.Text;
            int count = 0;
            var unit = textBox_unit.Text;
            var fkKey = ComboBox_pk_key.Text;

            if (int.TryParse(textBox_count.Text, out count))
            {

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@date",
                    Value = date
                };
                command.Parameters.Add(dateParam);

                SqlParameter storageParam = new SqlParameter
                {
                    ParameterName = "@storage",
                    Value = storage
                };
                command.Parameters.Add(storageParam);

                SqlParameter conditionsParam = new SqlParameter
                {
                    ParameterName = "@conditions",
                    Value = conditions
                };
                command.Parameters.Add(conditionsParam);

                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = count
                };
                command.Parameters.Add(countParam);

                SqlParameter unitParam = new SqlParameter
                {
                    ParameterName = "@unit",
                    Value = unit
                };
                command.Parameters.Add(unitParam);

                SqlParameter fkKeyParam = new SqlParameter
                {
                    ParameterName = "@pk_number_of_prod",
                    Value = fkKey
                };
                command.Parameters.Add(fkKeyParam);

                var id = command.ExecuteNonQuery();

                MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Запись должна иметь цифровой формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            connection.Close(); 
                  
        }
    }
}
