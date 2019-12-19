using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace lr1
{
    public partial class LoginForm : Form
    {
        int cntFails;
        string prevName;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cntFails = 0;
            prevName = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            profile user;
            user.name = textBox1.Text;
            user.password = textBox2.Text;
            user.hashIter = 0;
            string hashedPassword = "";
            ActSelForm newForm = new ActSelForm(user.name);
            if (ActSelForm.userExists)
            {
                if (ActSelForm.isFirstAuthF2)
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    this.Hide();
                    newForm.Owner = this;
                    newForm.Show();
                }
                else
                {
                    user.hashIter = ActSelForm.getHashIterNumber();
                    hashedPassword = user.password;
                    for (var i = 0; i < user.hashIter - 1; i++) 
                    {
                        using (SHA512 shaM = new SHA512Managed())
                        {
                            hashedPassword = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(hashedPassword)));
                        }
                    }
                    if (ActSelForm.checkHash(hashedPassword))
                    {
                        textBox1.Text = "";
                        textBox2.Text = "";
                        this.Hide();
                        newForm.Owner = this;
                        newForm.Show();
                    }
                    else
                    {
                        label3.Text = "Неверный пароль";
                        cntFails++;
                    }
                }
            }
            else
            { 
                label3.Text = "Данного пользователя не существует";
            }
            if (cntFails == 2)
            {
                Close();
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 newForm = new AboutBox1();
            newForm.ShowDialog();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}