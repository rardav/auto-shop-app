using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PracticaDavidRares
{
    public partial class LogIn : UserControl
    {
        public static User user;
        private string connstring;

        public LogIn()
        {
            InitializeComponent();
        }

        public User User
        {
            get
            {
                return user;
            }
        }

        public string Connstring { get => connstring; set => connstring = value; }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            panelLogIn.SendToBack();
            panelLogInInitial.BringToFront();
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxPassword.Clear();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            panelLogIn.BringToFront();
            panelLogInInitial.SendToBack();
        }

        private void buttonLogIn2_Click(object sender, EventArgs e)
        {
            if (textBoxFirstName.Text == "")
            {
                errorProvider1.SetError(textBoxFirstName, "Can't leave this field empty!");
            }
            else
            {
                if (textBoxLastName.Text == "")
                {
                    errorProvider1.SetError(textBoxLastName, "Can't leave this field empty!");
                }
                else
                {
                    if (textBoxPassword.Text == "")
                    {
                        errorProvider1.SetError(textBoxPassword, "Can't leave this field empty!");
                    }
                    else
                    {
                        int ok = 0;
                        OleDbConnection conn = new OleDbConnection(connstring);
                        try
                        {
                            conn.Open();
                            OleDbCommand comm = new OleDbCommand("SELECT * FROM Angajati;");
                            comm.Connection = conn;
                            OleDbDataReader reader = comm.ExecuteReader();
                            while(reader.Read())
                            {
                                if(reader["Nume"].ToString()==textBoxLastName.Text&& reader["Prenume"].ToString() == textBoxFirstName.Text&& reader["Parola"].ToString() == textBoxPassword.Text)
                                {
                                    ok = 1;
                                    string lName = reader["Nume"].ToString();
                                    string fName = reader["Prenume"].ToString();
                                    
                                }
                            }
                            reader.Close();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                        if(ok==1)
                        {
                            Form1.user = user;
                            panelLogIn.SendToBack();
                            panelLogInInitial.BringToFront();
                            this.SendToBack();
                            textBoxFirstName.Clear();
                            textBoxLastName.Clear();
                            textBoxPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No employee matches your input. Try again!");
                            textBoxFirstName.Clear();
                            textBoxLastName.Clear();
                            textBoxPassword.Clear();
                        }
                    }
                }
            }
        }
    }
}
