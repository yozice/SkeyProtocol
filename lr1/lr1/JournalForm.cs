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
    public partial class JournalForm : Form
    {
        public JournalForm()
        {
            InitializeComponent();
        }

        private void JournalForm_Load(object sender, EventArgs e)
        {
            using(StreamReader sr = new StreamReader(@"log.txt"))
            {
                richTextBox1.Text = sr.ReadToEnd();
            }
        }
    }
}
