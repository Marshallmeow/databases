using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace database
{
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }
    public partial class Form1 : Form
    {       

        int selectedRow;
        string _login=null;
        string _password=null;
        string con=null;
        SqlConnection connection;
       
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);

        }

        public void connection_func(string login, string password)
        {
             _login = login;
             _password = password;
             con = $"Server = MSI;User ID= {_login};Password={_password};Initial Catalog = yakovlev; Integrated Security = false";
            bool validUser = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                }
            }
            catch (SqlException)
            {
                validUser = false;
            }

            if (validUser)
            {
                MessageBox.Show("Login successful!");

                RefreshDataGrid(dataGridView1);
            }

            else
                MessageBox.Show("Login failed!");
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView8.Visible = true;
            DataView(dataGridView8);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CreateColumns();
            CreateColumnsHaving();
            CreateColumnsView();
            CreateColumnsCorel();

            //if (dataGridView2.Visible == true)
            //{
            //    HavingDataGrid(dataGridView2);
            //}
            //else if (dataGridView8.Visible == true)
            //{
            //    DataView(dataGridView8);
            //}
            //else if (dataGridView9.Visible == true)
            //{
            //    DataCorel(dataGridView9);
            //}
            //else
            //{
            //    RefreshDataGrid(dataGridView1);
            //}
            CreateColumns_worker();

            CreateColumns_sklad();

            CreateColumns_supp();

            CreateColumns_otdel();

            CreateColumns_prod();

        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("date", "Дата продажи");
            dataGridView1.Columns.Add("name", "Наименование продукта");
            dataGridView1.Columns.Add("count", "Количество");
            dataGridView1.Columns.Add("price", "Стоимость");
            dataGridView1.Columns.Add("pk_key", "fk_key");
            dataGridView1.Columns.Add("IsNew",String.Empty);
        }
 
        private void ReadSingleRow(DataGridView dgw,IDataRecord record) 
        {
            dgw.Rows.Add(record.GetInt32(0),record.GetDateTime(1), record.GetString(2), record.GetInt32(3), 
                record.GetInt32(4), record.GetInt32(5),RowState.ModifiedNew);
        }

        public void RefreshDataGrid(DataGridView dgw)
        {           
                dgw.Rows.Clear();
                string queryString = $"select * from cost";
                connection = new SqlConnection(con);
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ReadSingleRow(dgw, reader);
                }
                reader.Close();
            //DataTable table1 = new DataTable();
            //SqlDataAdapter adapter1 = new SqlDataAdapter(queryString,connection);
            //adapter1.Fill(table1);
            //int fk = Convert.ToInt32(table1.Rows[0][5]);
            //DataTable table;
            //DataGridViewComboBoxCell ComboBoxCell = new DataGridViewComboBoxCell();
            ////ComboBoxCell.Items.AddRange(new string[] { "aaa", "bbb", "ccc" });
            //string request = "select id,name from product";
            //SqlCommand command1 = new SqlCommand(request, connection);
            //command.ExecuteNonQuery();
            //SqlDataAdapter adapter = new SqlDataAdapter(request, connection);
            //table = new DataTable();
            //adapter.Fill(table);
            
            //foreach (DataRow row in table.Rows)
            //{
            //    ComboBoxCell.Items.Add(row[1] + "(" + row[0] + ")");
            //}
            //dataGridView1.Rows[0].Cells[5] = ComboBoxCell;
            ////this.dataGridView1[5, 0] = ComboBoxCell;
            ////this.dataGridView1.Columns[5] = ComboBoxCell;
            ////for (int i=0;i<table1.Columns.Count;i++)
            ////{
            ////    this.dataGridView1[5, i] = ComboBoxCell;
            ////}
            ////this.dataGridView1[5, 0].Value = fk.ToString();

            connection.Close();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_date.Text = row.Cells[1].Value.ToString();
                textBox_name.Text = row.Cells[2].Value.ToString();
                textBox_count.Text = row.Cells[3].Value.ToString();
                textBox_price.Text = row.Cells[4].Value.ToString();
                textBox_pk_id.Text = row.Cells[5].Value.ToString();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            RefreshDataGrid(dataGridView1);
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Add_Form addfrm = new Add_Form(connection);
            addfrm.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"select * from cost where concat (id, date,name,count,price,pk_key) like '%" + textBox_search.Text + "%'";
            SqlCommand com = new SqlCommand(searchString, connection);
            connection.Open();
            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
            connection.Close();           
        }

        private void deletedRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[6].Value = RowState.Deleted;
        }

        private new void Update()
        {

 
        connection.Open();
        for (int index = 0; index < dataGridView1.Rows.Count; index++)
        {
            var rowState = (RowState)dataGridView1.Rows[index].Cells[6].Value;

            if (rowState == RowState.Existed)
                continue;

            if (rowState == RowState.Deleted)
            {
                string sqlExpression = "deleteCost";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };
                command.Parameters.Add(idParam);

                command.ExecuteNonQuery();
            }
        }
        connection.Close();
            
        }

        private void change()
        {

            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = textBox_id.Text;
            var date = textBox_date.Text;
            var name = textBox_name.Text;
            var count = textBox_count.Text;
            int price;
            var fkKey = textBox_pk_id.Text;

            if (dataGridView1.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox_price.Text, out price))
                {
                    dataGridView1.Rows[selectedRow].SetValues(id,date, name, count, price, fkKey);
                    dataGridView1.Rows[selectedRow].Cells[6].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат!");
                }
            }
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deletedRow();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            update_cost update = new update_cost(connection);
            update.Show();
        }



        private void btnTrans_Click(object sender, EventArgs e)
        {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                var addQuery = $"insert into cost (date,name,count,price,pk_key) values ('20.02.2020'," +
                $" 'Ряба', 100,30,2)";
                var addQuery1 = $"insert into cost (date,name,count,price,pk_key) values ('25.06.2021'," +
                    $" 'Махеев', 50,60,2)";
                try
                {

                    command = new SqlCommand(addQuery, connection);
                    command.ExecuteNonQuery();

                    command = new SqlCommand(addQuery1, connection);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Запись создана!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Запись должна иметь цифровой формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    transaction.Rollback();
                }
                    
        }


        //2d HAVING--------------------------------------------------------------------------
        private void CreateColumnsHaving()
        {
            dataGridView2.Columns.Add("name", "Наименование продукта");
            dataGridView2.Columns.Add("price", "Стоимость");
            dataGridView2.Columns.Add("amount", "Кол-во");
        }

        private void ReadSingleRowHaving(DataGridView dgwhaving, IDataRecord record1)
        {
            dgwhaving.Rows.Add(record1.GetString(0), record1.GetInt32(1),
                record1.GetInt32(2));
        }

        private void HavingDataGrid(DataGridView dgwhaving)
        {
            dgwhaving.Rows.Clear();

            {
                string queryString = $"select name,price,COUNT(*) AS amount from cost GROUP BY name,price HAVING COUNT(*) > 1";
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRowHaving(dgwhaving, reader);
                }
                reader.Close();

                connection.Close();
            }
        }

        private void btnHaving_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;   
            HavingDataGrid(dataGridView2);

        }
        //-------------------------------------------------------------------------------------

        //2e ALL-------------------------------------------------------------------------------
        private void DataGridAll(DataGridView dgw)
        {
            dgw.Rows.Clear();

            {
                string queryString = $"select * from cost where price > ALL(select price from cost where name='Моцарелла')";
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRow(dgw, reader);
                }
                reader.Close();

                connection.Close();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            DataGridAll(dataGridView1);
        }

        //--------------------------------------------------VIEW--------------------------------------------------
        private void CreateColumnsView()
        {

            dataGridView8.Columns.Add("name", "наименование отдела");
            dataGridView8.Columns.Add("address", "адрес");
            dataGridView8.Columns.Add("fio", "фио");
            dataGridView8.Columns.Add("spec", "должность");
        }
        private void ReadSingleRowView(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetString(3));
        }
        private void DataView(DataGridView dgw)
        {
            dgw.Rows.Clear();

            {
                string queryString = $"select * from otdelworker";
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRowView(dgw, reader);
                }
                reader.Close();

                connection.Close();
            }
        }

        private void CreateColumnsCorel()
        {

            dataGridView9.Columns.Add("name", "Наименование поставщика");
            dataGridView9.Columns.Add("product", "Продукта");
        }
        private void ReadSingleRowCorel(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetString(0), record.GetString(1));
        }
        private void DataCorel(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string queryString = $"select name,product from supplier  where '1' in (select pk_number_of_prod from product where pk_number_of_post=pk_number_of_prod)";
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRowCorel(dgw, reader);
                }
                reader.Close();

                connection.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            dataGridView9.Visible = true;
            DataCorel(dataGridView9);
        }

        //--------------------------------------------------------------------------------

        //-----------------------------------------WORKER---------------------------------
        private void CreateColumns_worker()
        {
            dataGridView3.Columns.Add("id", "ID");
            dataGridView3.Columns.Add("FIO", "ФИО");
            dataGridView3.Columns.Add("pasport", "паспортные данные");
            dataGridView3.Columns.Add("spec", "должность");
            dataGridView3.Columns.Add("age", "возраст");
            dataGridView3.Columns.Add("male", "Пол");
            dataGridView3.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow_worker(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3),
                record.GetString(4), record.GetString(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid_worker(DataGridView dgw)
        {
            
            dgw.Rows.Clear();
            string queryString = $"select * from worker";
            connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow_worker(dgw, reader);
            }
            reader.Close();

            connection.Close();
            
        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedRow];

                textBox_id_worker.Text = row.Cells[0].Value.ToString();
                textBox_FIO.Text = row.Cells[1].Value.ToString();
                textBox_pasport.Text = row.Cells[2].Value.ToString();
                textBox_spec.Text = row.Cells[3].Value.ToString();
                textBox_Age.Text = row.Cells[4].Value.ToString();
                textBox_male.Text = row.Cells[5].Value.ToString();
            }
        }

        private void btnUpdate_worker_Click_1(object sender, EventArgs e)
        {
            RefreshDataGrid_worker(dataGridView3);
        }

        private void Search_worker(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string searchString = $"select * from worker where concat (id, FIO, pasport, spec, age, male) like '%" + textBox_search_worker.Text + "%'";
                SqlCommand com = new SqlCommand(searchString, connection);
                connection.Open();
                SqlDataReader read = com.ExecuteReader();

                while (read.Read())
                {
                    ReadSingleRow_worker(dgw, read);
                }
                read.Close();
            }
        }

        private void deletedRow_worker()
        {
            int index = dataGridView3.CurrentCell.RowIndex;
            dataGridView3.Rows[index].Visible = false;

            if (dataGridView3.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView3.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
            dataGridView3.Rows[index].Cells[6].Value = RowState.Deleted;
        }

        private new void Update_worker()
        {

            {
                connection.Open();
                for (int index = 0; index < dataGridView3.Rows.Count; index++)
                {
                    var rowState = (RowState)dataGridView3.Rows[index].Cells[6].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {
                        string sqlExpression = "deleteWorker";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        var id = Convert.ToInt32(dataGridView3.Rows[index].Cells[0].Value);

                        SqlParameter idParam = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParam);
                        command.ExecuteNonQuery();
                    }

                    if (rowState == RowState.Modified)
                    {
                        var id = dataGridView3.Rows[index].Cells[0].Value.ToString();
                        var fio = dataGridView3.Rows[index].Cells[1].Value.ToString();
                        var pasport = dataGridView3.Rows[index].Cells[2].Value.ToString();
                        var spec = dataGridView3.Rows[index].Cells[3].Value.ToString();
                        var age = dataGridView3.Rows[index].Cells[4].Value.ToString();
                        var male = dataGridView3.Rows[index].Cells[5].Value.ToString();

                        string sqlExpression = "updateWorker";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter idParamtr = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParamtr);

                        SqlParameter fioParam = new SqlParameter
                        {
                            ParameterName = "@FIO",
                            Value = fio
                        };
                        command.Parameters.Add(fioParam);

                        SqlParameter pasportParam = new SqlParameter
                        {
                            ParameterName = "pasport",
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

                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        private void change_worker()
        {
            var selectedRowIndex = dataGridView3.CurrentCell.RowIndex;

            var id = textBox_id_worker.Text;
            var fio = textBox_FIO.Text;
            var pasport = textBox_pasport.Text;
            var spec = textBox_spec.Text;
            var age = textBox_Age.Text;
            var male = textBox_male.Text;

            if (dataGridView3.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {            
                dataGridView3.Rows[selectedRow].SetValues(id, fio, pasport, spec, age, male);
                dataGridView3.Rows[selectedRow].Cells[6].Value = RowState.Modified;            
            }
        }

        private void btnNew_worker_Click_1(object sender, EventArgs e)
        {
            add_form_worker addfrm = new add_form_worker(connection);
            addfrm.Show();
        }

        private void btnDel_worker_Click(object sender, EventArgs e)
        {
            deletedRow_worker();
        }

        private void btnChange_worker_Click(object sender, EventArgs e)
        {
            change_worker();
        }

        private void btnSave_worker_Click(object sender, EventArgs e)
        {
            Update_worker(); 
        }

        private void textBox_search_worker_TextChanged_1(object sender, EventArgs e)
        {
            Search_worker(dataGridView3);
        }

        //------------------------------------------------SKLAD--------------------------------------------------------------------------------
        private void CreateColumns_sklad()
        {
            dataGridView4.Columns.Add("id", "ID");
            dataGridView4.Columns.Add("count_cells", "Кол-во ячеек");
            dataGridView4.Columns.Add("type", "Типы секций");
            dataGridView4.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow_sklad(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1),
                record.GetString(2), RowState.ModifiedNew);
        }
        private void RefreshDataGrid_sklad(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string queryString = $"select * from sklad";
                connection = new SqlConnection(con);
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ReadSingleRow_sklad(dgw, reader);
                }
                reader.Close();

                connection.Close();
            }
        }
        private void dataGridView4_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView4.Rows[selectedRow];

                textBox_id_sklad.Text = row.Cells[0].Value.ToString();
                textBox_counter.Text = row.Cells[1].Value.ToString();
                textBox_type.Text = row.Cells[4].Value.ToString();
            }
        }

        private void Search_sklad(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string searchString = $"select * from sklad where concat (id, count_cells, type) like '%" + textBox_search_sklad.Text + "%'";
                SqlCommand com = new SqlCommand(searchString, connection);
                connection.Open();

                SqlDataReader read = com.ExecuteReader();

                while (read.Read())
                {
                    ReadSingleRow_sklad(dgw, read);
                }
                read.Close();
            }
        }

        private void deletedRow_sklad()
        {
            int index = dataGridView4.CurrentCell.RowIndex;
            dataGridView4.Rows[index].Visible = false;

            if (dataGridView4.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView4.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView4.Rows[index].Cells[5].Value = RowState.Deleted;

        }
        private new void Update_sklad()
        {

            {
                connection.Open();
                for (int index = 0; index < dataGridView4.Rows.Count; index++)
                {
                    var rowState = (RowState)dataGridView4.Rows[index].Cells[5].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {
                        string sqlExpression = "deleteSklad";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        var id = Convert.ToInt32(dataGridView4.Rows[index].Cells[0].Value);

                        SqlParameter idParam = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParam);
                        command.ExecuteNonQuery();
                    }

                    if (rowState == RowState.Modified)
                    {
                        var id = dataGridView4.Rows[index].Cells[0].Value.ToString();
                        var count = dataGridView4.Rows[index].Cells[1].Value.ToString();
                        var type = dataGridView4.Rows[index].Cells[2].Value.ToString();

                        string sqlExpression = "updateSklad";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter idParamtr = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParamtr);

                        SqlParameter countParam = new SqlParameter
                        {
                            ParameterName = "@count_cells",
                            Value = count
                        };
                        command.Parameters.Add(countParam);

                        SqlParameter typeParam = new SqlParameter
                        {
                            ParameterName = "@type",
                            Value = type
                        };
                        command.Parameters.Add(typeParam);

                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        private void change_sklad()
        {
            var selectedRowIndex = dataGridView4.CurrentCell.RowIndex;

            var id = textBox_id_sklad.Text;
            var count = textBox_counter.Text;
            var type = textBox_type.Text;          

            if (dataGridView4.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView4.Rows[selectedRow].SetValues(id, count, type);
                dataGridView4.Rows[selectedRow].Cells[3].Value = RowState.Modified;
            }
        }

        private void btnNew_sklad_Click(object sender, EventArgs e)
        {
            add_form_sklad addfrm = new add_form_sklad(connection);
            addfrm.Show();
        }

        private void btnDel_sklad_Click(object sender, EventArgs e)
        {
            deletedRow_sklad();
        }

        private void btnChange_sklad_Click(object sender, EventArgs e)
        {
            change_sklad();
        }

        private void btnSave_sklad_Click(object sender, EventArgs e)
        {
            Update_sklad();
        }

        private void btnUpdate_sklad_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_sklad(dataGridView4);
        }

        private void textBox_search_sklad_TextChanged(object sender, EventArgs e)
        {
            Search_sklad(dataGridView4);
        }

        //------------------------SUPPLIER--------------------------------------------------------

        private void CreateColumns_supp()
        {
            dataGridView5.Columns.Add("id", "ID");
            dataGridView5.Columns.Add("name", "Название фирмы");
            dataGridView5.Columns.Add("phone", "Телефонный номер");
            dataGridView5.Columns.Add("addres", "Адрес");
            dataGridView5.Columns.Add("product", "Продукт");
            dataGridView5.Columns.Add("pk_number_of_post", "Номер продукта");
            dataGridView5.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow_supp(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3),
                record.GetString(4), record.GetInt32(5), RowState.ModifiedNew);
        }

        private void RefreshDataGrid_supp(DataGridView dgw)
        {      
            dgw.Rows.Clear();
            string queryString = $"select * from supplier";
            connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(queryString, connection);
            //if (connection.State == System.Data.ConnectionState.Closed)
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow_supp(dgw, reader);
            }
            reader.Close();

            connection.Close();         
        }
        private void Search_supp(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string searchString = $"select * from supplier where concat (id, name, phone, addres, product, pk_number_of_post) like '%" + textBox_search_supp.Text + "%'";
                SqlCommand com = new SqlCommand(searchString, connection);
                connection.Open();

                SqlDataReader read = com.ExecuteReader();

                while (read.Read())
                {
                    ReadSingleRow_supp(dgw, read);
                }
                read.Close();
            }
        }

        private void deletedRow_supp()
        {
            int index = dataGridView5.CurrentCell.RowIndex;
            dataGridView5.Rows[index].Visible = false;

            if (dataGridView5.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView5.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
            dataGridView5.Rows[index].Cells[6].Value = RowState.Deleted;
        }
        private void dataGridView5_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView5.Rows[selectedRow];

                textBox_id_supplier.Text = row.Cells[0].Value.ToString();
                textBox_names.Text = row.Cells[1].Value.ToString();
                textBox_phone.Text = row.Cells[2].Value.ToString();
                textBox_addres.Text = row.Cells[3].Value.ToString();
                textBox_prod.Text = row.Cells[4].Value.ToString();
                textBox_pk.Text = row.Cells[5].Value.ToString();
            }
        }

        private new void Update_supp()
        {

            {
                connection.Open();

                for (int index = 0; index < dataGridView5.Rows.Count; index++)
                {
                    var rowState = (RowState)dataGridView5.Rows[index].Cells[6].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {
                        string sqlExpression = "deleteSupplier";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        var id = Convert.ToInt32(dataGridView5.Rows[index].Cells[0].Value);

                        SqlParameter idParam = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParam);
                        command.ExecuteNonQuery();
                    }

                    if (rowState == RowState.Modified)
                    {
                        var id = dataGridView5.Rows[index].Cells[0].Value.ToString();
                        var names = dataGridView5.Rows[index].Cells[1].Value.ToString();
                        var phone = dataGridView5.Rows[index].Cells[2].Value.ToString();
                        var addres = dataGridView5.Rows[index].Cells[3].Value.ToString();
                        var product = dataGridView5.Rows[index].Cells[4].Value.ToString();
                        var pk = dataGridView5.Rows[index].Cells[5].Value.ToString();

                        string sqlExpression = "updateSupplier";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter idParamtr = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParamtr);

                        SqlParameter namesParam = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = names
                        };
                        command.Parameters.Add(namesParam);

                        SqlParameter phoneParam = new SqlParameter
                        {
                            ParameterName = "@phone",
                            Value = phone
                        };
                        command.Parameters.Add(phoneParam);

                        SqlParameter addresParam = new SqlParameter
                        {
                            ParameterName = "@addres",
                            Value = addres
                        };
                        command.Parameters.Add(addresParam);

                        SqlParameter productParam = new SqlParameter
                        {
                            ParameterName = "@product",
                            Value = product
                        };
                        command.Parameters.Add(productParam);

                        SqlParameter pkParam = new SqlParameter
                        {
                            ParameterName = "@pk_number_of_post",
                            Value = pk
                        };
                        command.Parameters.Add(pkParam);

                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        private void change_supp()
        {

            var selectedRowIndex = dataGridView5.CurrentCell.RowIndex;

            var id = textBox_id_supplier.Text;
            var names = textBox_names.Text;
            var phone = textBox_phone.Text;
            var addres = textBox_addres.Text;
            var product = textBox_prod.Text;
            var pk = textBox_pk.Text;

            if (dataGridView5.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView5.Rows[selectedRow].SetValues(id, names, phone, addres, product, pk);
                dataGridView5.Rows[selectedRow].Cells[6].Value = RowState.Modified;
            }
        }

        private void btnNew_supp_Click(object sender, EventArgs e)
        {
            add_form_supp addfrm = new add_form_supp(connection);
            addfrm.Show();
        }

        private void btnDel_supp_Click(object sender, EventArgs e)
        {
            deletedRow_supp();
        }

        private void btnChange_supp_Click(object sender, EventArgs e)
        {
            change_supp();
        }

        private void btnSave_supp_Click(object sender, EventArgs e)
        {
            Update_supp();
        }

        private void btnUpdate_supp_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_supp(dataGridView5);
        }

        private void textBox_search_supp_TextChanged(object sender, EventArgs e)
        {
            Search_supp(dataGridView5);
        }

        //-----------------------OTDEL-------------------------------------------------------------
        private void CreateColumns_otdel()
        {
            dataGridView6.Columns.Add("id", "ID");
            dataGridView6.Columns.Add("name", "Название отдела");
            dataGridView6.Columns.Add("phone", "Телефонный номер");
            dataGridView6.Columns.Add("address", "Адрес");
            dataGridView6.Columns.Add("pk_number_of_otdel", "Номер отдела");
            dataGridView6.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow_otdel(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3),
                record.GetInt32(4), RowState.ModifiedNew);
        }
        private void RefreshDataGrid_otdel(DataGridView dgw)
        {

            
            dgw.Rows.Clear();
            string queryString = $"select * from otdel";
            connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_otdel(dgw, reader);
            }
            reader.Close();
            connection.Close();
            
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView6.Rows[selectedRow];

                textBox_id_otdel.Text = row.Cells[0].Value.ToString();
                textBox_namee.Text = row.Cells[1].Value.ToString();
                textBox_phonee.Text = row.Cells[2].Value.ToString();
                textBox_adress.Text = row.Cells[3].Value.ToString();
                textBox_pk_otdel.Text = row.Cells[4].Value.ToString();
            }
        }

        private void Search_otdel(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string searchString = $"select * from otdel where concat (id, name, phone, address, pk_number_of_otdel) like '%" + textBox_search_otdel.Text + "%'";
                SqlCommand com = new SqlCommand(searchString, connection);
                connection.Open();

                SqlDataReader read = com.ExecuteReader();

                while (read.Read())
                {
                    ReadSingleRow_otdel(dgw, read);
                }
                read.Close();
            }
        }

        private void deletedRow_otdel()
        {
            int index = dataGridView6.CurrentCell.RowIndex;
            dataGridView6.Rows[index].Visible = false;
            if (dataGridView6.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView6.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView6.Rows[index].Cells[5].Value = RowState.Deleted;
        }
        private new void Update_otdel()
        {

            {
                connection.Open();

                for (int index = 0; index < dataGridView6.Rows.Count; index++)
                {
                    var rowState = (RowState)dataGridView6.Rows[index].Cells[5].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {
                        string sqlExpression = "deleteOtdel";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        var id = Convert.ToInt32(dataGridView6.Rows[index].Cells[0].Value);

                        SqlParameter idParam = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParam);
                        command.ExecuteNonQuery();
                    }

                    if (rowState == RowState.Modified)
                    {
                        var id = dataGridView6.Rows[index].Cells[0].Value.ToString();
                        var name = dataGridView6.Rows[index].Cells[1].Value.ToString();
                        var phone = dataGridView6.Rows[index].Cells[2].Value.ToString();
                        var address = dataGridView6.Rows[index].Cells[3].Value.ToString();
                        var pk = dataGridView6.Rows[index].Cells[4].Value.ToString();

                        string sqlExpression = "updateOtdel";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter idParamtr = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParamtr);

                        SqlParameter nameParam = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = name
                        };
                        command.Parameters.Add(nameParam);

                        SqlParameter phoneParam = new SqlParameter
                        {
                            ParameterName = "phone",
                            Value = phone
                        };
                        command.Parameters.Add(phoneParam);

                        SqlParameter addressParam = new SqlParameter
                        {
                            ParameterName = "@address",
                            Value = address
                        };
                        command.Parameters.Add(addressParam);

                        SqlParameter pkParam = new SqlParameter
                        {
                            ParameterName = "@pk_number_of_otdel",
                            Value = pk
                        };
                        command.Parameters.Add(pkParam);

                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        private void change_otdel()
        {
            var selectedRowIndex = dataGridView6.CurrentCell.RowIndex;

            var id = textBox_id_otdel.Text;
            var name = textBox_namee.Text;
            var phone = textBox_phonee.Text;
            var address = textBox_adress.Text;
            var pk = textBox_pk_otdel.Text;

            if (dataGridView6.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView6.Rows[selectedRow].SetValues(id, name, phone, address, pk);
                dataGridView6.Rows[selectedRow].Cells[5].Value = RowState.Modified;
            }
        }

        private void btnNew_otdel_Click(object sender, EventArgs e)
        {
            add_form_otdel addfrm = new add_form_otdel(connection);
            addfrm.Show();
        }

        private void btnDel_otdel_Click(object sender, EventArgs e)
        {
            deletedRow_otdel();
        }

        private void btnChange_otdel_Click(object sender, EventArgs e)
        {
            change_otdel();
        }

        private void btnSave_otdel_Click(object sender, EventArgs e)
        {
            Update_otdel();
        }

        private void btnUpdate_otdel_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_otdel(dataGridView6);
        }
        private void textBox_search_otdel_TextChanged(object sender, EventArgs e)
        {
            Search_otdel(dataGridView6);
        }

        //----------------------------PRODUCT------------------------------------------------

        private void CreateColumns_prod()
        {
            dataGridView7.Columns.Add("id", "ID");
            dataGridView7.Columns.Add("name", "Наименование продукта");
            dataGridView7.Columns.Add("date", "Дата производства");
            dataGridView7.Columns.Add("storage", "Срок годности");
            dataGridView7.Columns.Add("conditions", "Условия хранения");
            dataGridView7.Columns.Add("count", "Количество");
            dataGridView7.Columns.Add("unit", "Единица измерения");
            dataGridView7.Columns.Add("pk_number_of_prod", "Номер продукта");
            dataGridView7.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow_prod(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDateTime(2), record.GetString(3), record.GetString(4),
                record.GetInt16(5), record.GetString(6),record.GetInt32(7), RowState.ModifiedNew);
        }

        private void RefreshDataGrid_prod(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from product";
            connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow_prod(dgw, reader);
            }
            reader.Close();
            connection.Close();
            
        }
        private void Search_prod(DataGridView dgw)
        {

            {
                dgw.Rows.Clear();
                string searchString = $"select * from product where concat (id, name, date, storage, conditions,count,unit, pk_number_of_prod) like '%" + textBox_search_prod.Text + "%'";
                SqlCommand com = new SqlCommand(searchString, connection);
                //connection.Open();

                SqlDataReader read = com.ExecuteReader();

                while (read.Read())
                {
                    ReadSingleRow_prod(dgw, read);
                }
                read.Close();
            }
        }

        private void deletedRow_prod()
        {
            int index = dataGridView7.CurrentCell.RowIndex;
            dataGridView7.Rows[index].Visible = false;

            if (dataGridView7.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView7.Rows[index].Cells[8].Value = RowState.Deleted;
                return;
            }
            dataGridView7.Rows[index].Cells[8].Value = RowState.Deleted;
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView7.Rows[selectedRow];

                textBox_id_prod.Text = row.Cells[0].Value.ToString();
                textBox_name_prod.Text = row.Cells[1].Value.ToString();
                textBox_date_prod.Text = row.Cells[2].Value.ToString();
                textBox_storage_prod.Text = row.Cells[3].Value.ToString();
                textBox_cond_prod.Text = row.Cells[4].Value.ToString();
                textBox_count_prod.Text = row.Cells[5].Value.ToString();
                textBox_unit_prod.Text = row.Cells[6].Value.ToString();
                textBox_pk_prod.Text = row.Cells[7].Value.ToString();
            }
        }

        private new void Update_prod()
        {

            {
                connection.Open();

                for (int index = 0; index < dataGridView7.Rows.Count; index++)
                {
                    var rowState = (RowState)dataGridView7.Rows[index].Cells[8].Value;

                    if (rowState == RowState.Existed)
                        continue;

                    if (rowState == RowState.Deleted)
                    {
                        string sqlExpression = "deleteProduct";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        var id = Convert.ToInt32(dataGridView7.Rows[index].Cells[0].Value);

                        SqlParameter idParam = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParam);
                        command.ExecuteNonQuery();
                    }

                    if (rowState == RowState.Modified)
                    {
                        var id = dataGridView7.Rows[index].Cells[0].Value.ToString();
                        var names = dataGridView7.Rows[index].Cells[1].Value.ToString();
                        var date = dataGridView7.Rows[index].Cells[2].Value.ToString();
                        var storage = dataGridView7.Rows[index].Cells[3].Value.ToString();
                        var cond = dataGridView7.Rows[index].Cells[4].Value.ToString();
                        var count = dataGridView7.Rows[index].Cells[5].Value.ToString();
                        var unit = dataGridView7.Rows[index].Cells[6].Value.ToString();
                        var pk = dataGridView7.Rows[index].Cells[7].Value.ToString();

                        string sqlExpression = "updateProduct";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter idParamtr = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(idParamtr);

                        SqlParameter namesParam = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = names
                        };
                        command.Parameters.Add(namesParam);

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

                        SqlParameter condParam = new SqlParameter
                        {
                            ParameterName = "@conditions",
                            Value = cond
                        };
                        command.Parameters.Add(condParam);

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

                        SqlParameter pkParam = new SqlParameter
                        {
                            ParameterName = "@pk_number_of_prod",
                            Value = pk
                        };
                        command.Parameters.Add(pkParam);

                        command.ExecuteNonQuery();
                    }
                }
                
                
                ;
            }
        }

        private void change_prod()
        {

            var selectedRowIndex = dataGridView7.CurrentCell.RowIndex;

            var id = textBox_id_prod.Text;
            var names = textBox_name_prod.Text;
            var date = textBox_date_prod.Text;
            var storage = textBox_storage_prod.Text;
            var cond = textBox_cond_prod.Text;
            var count = textBox_count_prod.Text;
            var unit = textBox_unit_prod.Text;
            var pk = textBox_pk_prod.Text;

            if (dataGridView7.Rows[selectedRow].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView7.Rows[selectedRow].SetValues(id, names, date, storage, cond, count, unit, pk);
                dataGridView7.Rows[selectedRow].Cells[8].Value = RowState.Modified;
            }
        }
        private void btnNew_prod_Click(object sender, EventArgs e)
        {
            add_form_product addfrm = new add_form_product(connection);
            addfrm.Show();
        }

        private void btnDel_prod_Click(object sender, EventArgs e)
        {
            deletedRow_prod();
        }

        private void btnChange_prod_Click(object sender, EventArgs e)
        {
            change_prod();
        }

        private void btnSave_prod_Click(object sender, EventArgs e)
        {
            Update_prod();
        }

        private void btnUpdate_prod_Click(object sender, EventArgs e)
        {
            RefreshDataGrid_prod(dataGridView7);
        }

        private void textBox_search_prod_TextChanged(object sender, EventArgs e)
        {
            Search_prod(dataGridView7);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_search.Text = " ";
        }

        private void btnClear_worker_Click(object sender, EventArgs e)
        {
            textBox_search_worker.Text = " ";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox_search_sklad.Text = " ";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox_search_supp.Text = " ";
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            textBox_search_otdel.Text = " ";
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            textBox_search_prod.Text = " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log_in log_In = new log_in(this);
            log_In.Show();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            object value = dataGridView1.Rows[0].Cells[5].Value;
            if ((dataGridView1.Rows[0].Cells[5].Value) !=null)
            {
                //(dataGridView1.Columns[5]).Items.Add(value);
                e.ThrowException = false;
            }
        }
    }
}
