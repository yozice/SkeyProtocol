using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace lr1
{
    public partial class ChgPswdForm : Form
    { 
        profile userF3;
        bool isFirstAuthF3;
        bool pswChanged;
        int ind;

        public ChgPswdForm(profile user, bool isFirstAuth)
        {
            ind = -1;
            foreach (profile userBuf in dataBase.dBase)
            {
                ind++;
                if (userBuf.name == user.name)
                {
                    userF3 = userBuf;
                    break;
                }
            }
            isFirstAuthF3 = isFirstAuth;
            pswChanged = false;
            InitializeComponent();
        }

        private void ChgPswdForm_Load(object sender, EventArgs e)
        {
            if (isFirstAuthF3) 
            {
                textBox3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool fLet = false;
            bool fSym = false;
            bool fPun = false;
            string hashedPassword;
            if (isFirstAuthF3)
            {
                if (textBox1.Text.Length >= 8)
                {
                    if (textBox1.Text == textBox2.Text)
                    {
                        hashedPassword = textBox1.Text;
                        for (var i = 0; i < 500; i++)
                        {
                            using (SHA512 shaM = new SHA512Managed())
                            {
                                hashedPassword = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(hashedPassword)));
                            }
                        }
                        // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
                        hashedPassword = hashedPassword.Replace(" ", "");
                        hashedPassword = hashedPassword.Replace("\n", "");
                        hashedPassword = hashedPassword.Replace("\t", "");
                        hashedPassword = hashedPassword.Replace("\r", "");

                        userF3.password = hashedPassword;
                        userF3.hashIter = 500;
                        pswChanged = true;
                    }
                    else
                    {
                        label5.Text = "Пароли не совпадают";
                    }
                }
                else
                {
                    label5.Text = "Длина пароля меньше 8 символов";
                }
            }
            else
            {
                hashedPassword = textBox3.Text;
                for (var i = 0; i < userF3.hashIter; i++)
                {
                    using (SHA512 shaM = new SHA512Managed())
                    {
                        hashedPassword = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(hashedPassword)));
                    }
                }
                // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
                hashedPassword = hashedPassword.Replace(" ", "");
                hashedPassword = hashedPassword.Replace("\n", "");
                hashedPassword = hashedPassword.Replace("\t", "");
                hashedPassword = hashedPassword.Replace("\r", "");

                if (hashedPassword==userF3.password)
                {
                    if (textBox1.Text.Length >= 8)
                    {
                        if (textBox1.Text == textBox2.Text)
                        {
                            hashedPassword = textBox1.Text;
                            for(var i =0;i<500;i++)
                            {
                                using (SHA512 shaM = new SHA512Managed())
                                {
                                    hashedPassword = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(hashedPassword)));
                                }
                            }
                            // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
                            hashedPassword = hashedPassword.Replace(" ", "");
                            hashedPassword = hashedPassword.Replace("\n", "");
                            hashedPassword = hashedPassword.Replace("\t", "");
                            hashedPassword = hashedPassword.Replace("\r", "");

                            userF3.password = hashedPassword;
                            userF3.hashIter = 500;
                            pswChanged = true;
                        }
                        else
                        {
                            label5.Text = "Пароли не совпадают";
                        }
                    }
                    else
                    {
                        label5.Text = "Длина пароля меньше 8 символов";
                    }
                }
                else
                {
                    label4.Text = "Неверно введен старый пароль";
                }
            }

            dataBase.dBase[ind] = userF3;
            if(pswChanged)
            {
                using (StreamWriter sw = new StreamWriter(@"log.txt", true))
                {
                    sw.WriteLine("Пароль изменен");
                }
                Close();
            }
        }

        private void ChgPswdForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isFirstAuthF3)
            {
                Application.Exit();
            }
            else
            {
                Close();
            }
        }
    }
}
