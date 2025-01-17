﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace proj
{
    
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiedNew,
        Deleted
    }

    public partial class accountView : Form
    {

        dataBase database = new dataBase();
        private string fName;

        int selectedRow;

        public accountView(string fName)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.fName = fName;
            label2.Text = $"Добро пожаловать, {fName}";
        }

        private void CreateColumns()
        {
            dataGridView.Columns.Add("id", "id");
            dataGridView.Columns.Add("fName", "Имя");
            dataGridView.Columns.Add("uLogin", "Логин");
            dataGridView.Columns.Add("phoneNumber", "Телефон");
            dataGridView.Columns.Add("companyName", "Компания");
            dataGridView.Columns.Add("IsNew", String.Empty);

            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("cName", "Название");
            dataGridView1.Columns.Add("country", "Страна");
            dataGridView1.Columns.Add("bossName", "Директор");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetGuid(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), RowState.ModifiedNew);
        }

        private void ReadSingleRow2(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetGuid(0), record.GetString(1), record.GetString(2), record.GetString(3), RowState.ModifiedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"SELECT id, fName, uLogin, phoneNumber, companyName FROM users";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }

        private void RefreshDataGrid2(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = $"SELECT id, cName, country, bossName FROM company";

            SqlCommand command = new SqlCommand(queryString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow2(dgw, reader);
            }
            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView);
            RefreshDataGrid2(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[selectedRow];

                textBox_fName.Text = row.Cells[1].Value.ToString();
                textBox_phone.Text = row.Cells[3].Value.ToString();
                textBox_login.Text = row.Cells[2].Value.ToString();
                textBox_company.Text = row.Cells[4].Value.ToString();
            }
        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox_companyName.Text = row.Cells[1].Value.ToString();
                textBox_country.Text = row.Cells[2].Value.ToString();
                textBox_bossName.Text = row.Cells[3].Value.ToString();
            }

        }


        private void SearchUser(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select id, fName, uLogin, phoneNumber, companyName from users where concat (id, fName, uLogin, phoneNumber, companyName) like '%" + textBox_searchUsers.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = com.ExecuteReader();
            
            while(reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }

            reader.Close();
        }


        private void SearchCompany(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select id, cName, country, bossName from company where concat (id, cName, country, bossName) like '%" + textBox_searchCompany.Text + "%'";

            SqlCommand com = new SqlCommand(searchString, database.getConnection());

            database.openConnection();

            SqlDataReader reader = com.ExecuteReader();

            while(reader.Read())
            {
                ReadSingleRow2(dgw, reader);
            }

            reader.Close();
        }

        private void deleteRowUsers()
        {
            int index = dataGridView.CurrentCell.RowIndex;

            dataGridView.Rows[index].Visible = false;

            if (dataGridView.Rows[index].Cells[2].Value == null || dataGridView.Rows[index].Cells[2].Value.ToString() == string.Empty)
            {
                dataGridView.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView.Rows[index].Cells[5].Value = RowState.Deleted;
        }

        private void UpdateUsers()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                {
                    continue;
                }

                if (rowState == RowState.Deleted)
                {
                    var login = dataGridView.Rows[index].Cells[2].Value?.ToString();
                    if (string.IsNullOrEmpty(login))
                    {
                        continue;
                    }

                    var deleteQuary = $"delete from users where uLogin = @login";

                    using (SqlCommand com = new SqlCommand(deleteQuary, database.getConnection()))
                    {
                        com.Parameters.AddWithValue("@login", login);
                        com.ExecuteNonQuery();
                    }
                }
            }

            database.closeConnection();
        }


        private void textBox_searchUsers_TextChanged(object sender, EventArgs e)
        {
            SearchUser(dataGridView);
        }

        private void textBox_searchCompany_TextChanged(object sender, EventArgs e)
        {
            SearchCompany(dataGridView1);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            deleteRowUsers();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            UpdateUsers();
        }
    }
}
