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
    public partial class ActSelForm : Form
    {
        private static profile userF2;
        public static bool userExists;
        public static bool isFirstAuthF2;

        public ActSelForm(string name)
        {
            try
            {
                using (StreamReader sr = new StreamReader(@"data.txt"))
                {
                    string line;
                    string[] lineSplitted = new string[3];
                    profile user;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        lineSplitted = line.Split('\t');
                        user.name = lineSplitted[0];
                        user.password = lineSplitted[1];
                        user.hashIter = Convert.ToInt32(lineSplitted[2]);
                        dataBase.dBase.Add(user);
                    }
                }
            }
            catch (FileNotFoundException e1)
            {
                using (StreamWriter sw = new StreamWriter(@"log.txt"))
                {
                    sw.WriteLine("Создание учетной записи администратора..");
                }
                profile user;
                user.name = "Admin";
                user.password = "";
                user.hashIter = 0;
                dataBase.dBase.Add(user);
            }
            userExists = false;
            foreach(profile user in dataBase.dBase)
            {
                if(user.name == name)
                {
                    userF2 = user;
                    userExists = true;
                    if (userF2.password == "")
                    {
                        using (StreamWriter sw = new StreamWriter(@"log.txt", true))
                        {
                            sw.WriteLine("Первый вход пользователя:" + " " + userF2.name);
                        }
                        isFirstAuthF2 = true;
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(@"log.txt", true))
                        {
                            sw.WriteLine("Авторизация пользователя:" + " " + userF2.name);
                        }
                        isFirstAuthF2 = false;
                    }
                    break;
                }
            }
            InitializeComponent();
        }

        private void ActSelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"data.txt"))
            {
                for (int i = 0; i < dataBase.dBase.Count; i++)
                {
                    sw.WriteLine(dataBase.dBase[i].name + "\t" +
                        dataBase.dBase[i].password + "\t" +
                        dataBase.dBase[i].hashIter);
                }
            }
            using(StreamWriter sw = new StreamWriter(@"log.txt", true))
            {
                sw.WriteLine("----------------------------");
            }
            dataBase.dBase.Clear();
            this.Owner.Show();
        }

        private void ActSelForm_Load(object sender, EventArgs e)
        {
            using(StreamWriter sw = new StreamWriter(@"log.txt", true))
            {
                sw.WriteLine("Вход в программу");
            }
            if (isFirstAuthF2)
            {
                ChgPswdForm newForm = new ChgPswdForm(userF2, true);
                newForm.ShowDialog();
                isFirstAuthF2 = false;
            }
            if (userF2.name != "Admin")
            {
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChgPswdForm newForm = new ChgPswdForm(userF2, false);
            newForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ViewUsersForm newForm = new ViewUsersForm();
                newForm.ShowDialog();
            }
            catch(NullReferenceException e1)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddUsersForm newForm = new AddUsersForm();
            newForm.ShowDialog();
        }

        public static bool checkHash(string password)
        {
            using (StreamWriter sw = new StreamWriter(@"log.txt", true))
            {
                sw.WriteLine("Аутентификация пользователя..");
            }
            string hashedPassword = "";
            using (SHA512 shaM = new SHA512Managed())
            {
                hashedPassword = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
            // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
            hashedPassword = hashedPassword.Replace(" ", "");
            hashedPassword = hashedPassword.Replace("\n", "");
            hashedPassword = hashedPassword.Replace("\t", "");
            hashedPassword = hashedPassword.Replace("\r", "");

            if (hashedPassword == userF2.password)
            {
                int ind = -1;
                foreach (profile userBuf in dataBase.dBase)
                {
                    ind++;
                    if (userBuf.name == userF2.name)
                    {
                        break;
                    }
                }
                // проверка номера итерации ключа
                // если 2, то генерируем новые одноразовые ключи
                if(userF2.hashIter == 2)
                {
                    string hPswd = password;
                    for (var i = 0; i < 499; i++)
                    {
                        using (SHA512 shaM = new SHA512Managed())
                        {
                            hPswd = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(hPswd)));
                        }
                    }
                    // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
                    hashedPassword = hashedPassword.Replace(" ", "");
                    hashedPassword = hashedPassword.Replace("\n", "");
                    hashedPassword = hashedPassword.Replace("\t", "");
                    hashedPassword = hashedPassword.Replace("\r", "");

                    userF2.password = hPswd;
                    userF2.hashIter = 500;
                    dataBase.dBase[ind] = userF2;
                    return true;
                }
                else
                {
                    hashedPassword = password;
                    // Удаляем из захешированной фразы лишние символы, которые мешают считыванию
                    hashedPassword = hashedPassword.Replace(" ", "");
                    hashedPassword = hashedPassword.Replace("\n", "");
                    hashedPassword = hashedPassword.Replace("\t", "");
                    hashedPassword = hashedPassword.Replace("\r", "");
                    userF2.password = hashedPassword;
                    userF2.hashIter = userF2.hashIter - 1;
                    dataBase.dBase[ind] = userF2;
                    return true;
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(@"log.txt", true))
                {
                    sw.WriteLine("Неверно введен пароль");
                }
                return false;
            }
        }

        public static int getHashIterNumber()
        {
            using (StreamWriter sw = new StreamWriter(@"log.txt", true))
            {
                sw.WriteLine("Получение номера ключа..");
            }
            return userF2.hashIter;
        }
    }
}
