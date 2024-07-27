using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace proj
{
    public partial class login : Form
    {

        dataBase database = new dataBase();
        

        public login()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            button2.Click += new System.EventHandler(this.button2_Click);
        }

        private void login_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var login = textBox12.Text;
            var password = textBox11.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = "SELECT uLogin, uPassword, fName FROM users WHERE uLogin = @login and uPassword = @password";

            try
            {
                using (SqlConnection connection = database.getConnection())
                {
                    if (connection == null)
                    {
                        MessageBox.Show("Подключение не было создано", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (connection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Подключение не открыто", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (SqlCommand command = new SqlCommand(querystring, connection))
                    {
                        command.Parameters.Add("@login", SqlDbType.NVarChar).Value = login;
                        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;

                        adapter.SelectCommand = command;
                        adapter.Fill(table);

                        if (table.Rows.Count == 1)
                        {
                            string name = table.Rows[0]["fName"].ToString();
                            MessageBox.Show("Вы успешно вошли", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            accountView frm1 = new accountView(name);
                            this.Hide();
                            frm1.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            MessageBox.Show("Такого аккаунта не существует", "Аккаунт не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            register reg = new register();
            this.Hide();
            reg.ShowDialog();
            this.Show();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            textBox11.UseSystemPasswordChar = true;
        }

        private void showPassword_Click(object sender, EventArgs e)
        {
            textBox11.UseSystemPasswordChar = false;
            showPassword.Visible = false;
            hidePassword.Visible = true;
        }

        private void hidePassword_Click(object sender, EventArgs e)
        {
            textBox11.UseSystemPasswordChar = true;
            showPassword.Visible = true;
            hidePassword.Visible = false;
        }
    }
}
