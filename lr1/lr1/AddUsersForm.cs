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

namespace lr1
{
    public partial class AddUsersForm : Form
    {
        public AddUsersForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            profile user;
            bool userExists = false;
            user.name = textBox1.Text;
            user.password = "";
            user.hashIter = 0;
            foreach(profile userBuf in dataBase.dBase)
            {
                if (userBuf.name == user.name)
                {
                    userExists = true;
                }
            }
            if(!userExists)
            {
                dataBase.dBase.Add(user);
                using (StreamWriter sw = new StreamWriter(@"log.txt", true))
                {
                    sw.WriteLine("Пользователь " + textBox1.Text + " добавлен");
                }
                label2.Text = "Пользователь " + textBox1.Text + " добавлен";
            }
            else
            {
                label2.Text = "Пользователь " + textBox1.Text + " уже существует";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddUsersForm_Load(object sender, EventArgs e)
        {

        }
    }
}
