using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaDavidRares
{
    public partial class Form1 : Form
    {
        public static User user;
        public static string connString;

        public Form1()
        {
            InitializeComponent();
            connString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Angajati.accdb";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void mainMenu1_Load(object sender, EventArgs e)
        {
            mainMenu1.Connstring = connString;
            mainMenu1.SendToBack();
        }

        private void logIn1_Load(object sender, EventArgs e)
        {
            logIn1.Connstring = connString;
            logIn1.BringToFront();
        }
    }
}
