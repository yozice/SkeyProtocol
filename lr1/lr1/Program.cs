using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace lr1
{
    public struct profile
    {
        public string name;
        public string password;
        public int hashIter; 
    }

    static class dataBase
    {
        public static List<profile> dBase = new List<profile>();
    }

    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}

/*
            FileStream fstream = new FileStream(@"data.txt", FileMode.Append);
            byte[] arr = System.Text.Encoding.Default.GetBytes("cheburashka");
            fstream.Write(arr, 0, arr.Length);
            MessageBox.Show(
                System.Convert.ToString(arr.Length), 
                "Сообщение", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1, 
                MessageBoxOptions.DefaultDesktopOnly);

    */
