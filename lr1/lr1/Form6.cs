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
    public partial class Form6 : Form
    {
        profile userF6;
        bool isFirstAuthF6;

        public Form6(profile user, bool isFirstAuth)
        {
            userF6 = user;
            isFirstAuthF6 = isFirstAuth;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChgPswdForm newForm = new ChgPswdForm(userF6, isFirstAuthF6);
            newForm.ShowDialog();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            if (isFirstAuthF6)
            {
                ChgPswdForm newForm = new ChgPswdForm(userF6, true);
                newForm.ShowDialog();
            }
            isFirstAuthF6 = false;
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"data.txt"))
            {
                for (int i = 0; i < dataBase.dBase.Count; i++)
                {
                    sw.WriteLine(dataBase.dBase[i].name + " " +
                        dataBase.dBase[i].password + " " +
                        dataBase.dBase[i].len_pswrd + " " +
                        dataBase.dBase[i].is_blocked + " " +
                        dataBase.dBase[i].is_restricted);
                }
            }
            this.Owner.Show();
        }
    }
}
