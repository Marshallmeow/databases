﻿using System;
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
    public partial class add_form_otdel : Form
    {
        SqlConnection connection;
        DataTable table;
        public add_form_otdel(SqlConnection form)
        {
            InitializeComponent();
            connection = form;
            connection.Open();
            ComboBox_pk_key.Items.Clear();
            string request = "select id,type from sklad";
            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
            table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                ComboBox_pk_key.Items.Add(row[1] + "(" + row[0] + ")");
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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
            string sqlExpression = "insertOtdel";
            SqlCommand command = new SqlCommand(sqlExpression,connection);
            command.CommandType = CommandType.StoredProcedure;

            var name = textBox_name.Text;
            var phone = textBox_phone.Text;
            var addres = textBox_addres.Text;         

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = name
            };
            command.Parameters.Add(nameParam);

            SqlParameter phoneParam = new SqlParameter
            {
                ParameterName = "@phone",
                Value = phone
            };
            command.Parameters.Add(phoneParam);

            SqlParameter addresParam = new SqlParameter
            {
                ParameterName = "@address",
                Value = addres
            };
            command.Parameters.Add(addresParam);

            SqlParameter pkParam = new SqlParameter
            {
                ParameterName = "@pk_number_of_otdel",
                Value = fk_key
            };
            command.Parameters.Add(pkParam);

            var id = command.ExecuteNonQuery();

            MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            connection.Close();         
        }

    }
}
