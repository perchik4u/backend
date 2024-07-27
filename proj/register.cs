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



namespace proj
{


    public partial class register : Form
    {
        dataBase database = new dataBase();

        public register()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            button1.Click += new System.EventHandler(this.button1_Click);
        }

        private void register_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var login = textBox1.Text;
            var password = textBox2.Text;
            var name = textBox3.Text;
            var number = textBox4.Text;
            var country = textBox5.Text;
            var company = textBox6.Text;
            bool isBoss = checkBox1.Checked;

            using (SqlCommand command = new SqlCommand("RegisterUserFIX", database.getConnection()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password); 
                command.Parameters.AddWithValue("@IsBoss", isBoss);
                command.Parameters.AddWithValue("@CompanyName", company);
                command.Parameters.AddWithValue("@PhoneNumber", number);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Country", country);

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Аккаунт успешно создан", "Успешная регистрация");
                    login frm_login = new login();
                    this.Hide();
                    frm_login.ShowDialog();
                    this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Аккаунт не создан: " + ex.Message, "Ошибка");
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }


        private void showPassword_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            showPassword.Visible = false;
            hidePassword.Visible = true;
        }

        private void hidePassword_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            showPassword.Visible = true;
            hidePassword.Visible = false;
        }
    }
}
