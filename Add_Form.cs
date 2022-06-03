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
    public partial class Add_Form : Form
    {
        SqlConnection connection;
        DataTable table;
        public Add_Form(SqlConnection form)
        {
            InitializeComponent();
            connection = form;
            connection.Open();
            ComboBox_pk_key.Items.Clear();
            string request = "select pk_number_of_prod,name from product";
            SqlCommand command = new SqlCommand(request,connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
            table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                ComboBox_pk_key.Items.Add(row[1]+"("+row[0]+")");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = connection;
            connection.Open();
            int fk_key=0;
            string selected = ComboBox_pk_key.SelectedItem.ToString();
            string[] subs = selected.Split('(');
            foreach(DataRow row in table.Rows)
            {
                if (row[1].ToString() == subs[0])
                    fk_key = Convert.ToInt32(row[0]);
            }
            string sqlExpression = "insertCost";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            
            command.CommandType = CommandType.StoredProcedure;

            var date = textBox_date1.Text;
            var name = textBox_name2.Text;
            var count = textBox_count1.Text;
            int price = 0;
            var fkKey = ComboBox_pk_key.Text.ToString();

            if (int.TryParse(textBox_price1.Text, out price))
            {
                SqlParameter dateParam = new SqlParameter
                {
                    ParameterName = "@date",
                    Value = date
                };
                command.Parameters.Add(dateParam);

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = count
                };
                command.Parameters.Add(countParam);

                SqlParameter priceParam = new SqlParameter
                {
                    ParameterName = "@price",
                    Value = price
                };
                command.Parameters.Add(priceParam);

                SqlParameter fkKeyParam = new SqlParameter
                {
                    ParameterName = "@pk_key",
                    Value = fk_key
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
