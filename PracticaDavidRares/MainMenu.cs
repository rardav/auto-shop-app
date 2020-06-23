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
using System.IO;
using System.Threading;

namespace PracticaDavidRares
{

    public partial class MainMenu : UserControl
    {
        public static User user;
        private string connstring;
        private string[] selectedItm;
        public List<Vehicle> queue;


        public MainMenu()
        {
            InitializeComponent();
            selectedItm = new string[2];
            queue = new List<Vehicle>();
        }

        public User User
        {
            get
            {
                return user;
            }
        }

        public string Connstring { get => connstring; set => connstring = value; }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void buttonLogOff_Click(object sender, EventArgs e)
        {
            this.SendToBack();
        }


        private void populateListView()
        {
            listView1.Items.Clear();
            OleDbConnection conn = new OleDbConnection(connstring);
            try
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand("SELECT * FROM Angajati WHERE Nume <> 'ADMIN';");
                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Nume"].ToString() + " " + reader["Prenume"].ToString());
                    if (reader["Functia"].ToString() == "Director")
                        item.ImageIndex = 2;
                    else if (reader["Functia"].ToString() == "Mecanic")
                        item.ImageIndex = 0;
                    else if (reader["Functia"].ToString() == "Asistent")
                        item.ImageIndex = 1;
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                listView1.Sort();
            }
        }

        private void populateListView2()
        {
            listViewEmployees.Items.Clear();
            OleDbConnection conn = new OleDbConnection(connstring);
            try
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand("SELECT * FROM Angajati WHERE Nume <> 'ADMIN';");
                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Nume"].ToString() + " " + reader["Prenume"].ToString());
                    item.SubItems.Add(reader["Car1"].ToString());
                    item.SubItems.Add(reader["Car2"].ToString());
                    item.SubItems.Add(reader["Car3"].ToString());
                    item.SubItems.Add(reader["Bus"].ToString());
                    item.SubItems.Add(reader["Truck"].ToString());
                    listViewEmployees.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                listViewEmployees.Sort();
            }
        }

        private void populateQueue()
        {

            listViewVehicles.Items.Clear();
            foreach(Vehicle v in queue)
            {
                ListViewItem item = new ListViewItem(v.Id.ToString() + System.Environment.NewLine+ v.VehicleTitle.ToString());
                if (v.Type == "Car")
                    item.ImageIndex = 0;
                else if (v.Type == "Bus")
                    item.ImageIndex = 1;
                else if (v.Type == "Truck")
                    item.ImageIndex = 2;
                listViewVehicles.Items.Add(item);
            }
        }

        private void buttonStaff_Click(object sender, EventArgs e)
        {
            panelStaff.Visible = true;
            panelStaff.Enabled = true;

            populateListView();
        }

        private void buttonBackStaff_Click(object sender, EventArgs e)
        {
            buttonDelete.Enabled = false;
            buttonEdit.Enabled = false;
            panelStaff.Visible = false;
            panelStaff.Enabled = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems!=null && listView1.SelectedItems.Count>0)
            {
                buttonDelete.Enabled = true;
                buttonEdit.Enabled = true;
            }
            else
            {
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
            }
            listView1.Focus();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            selectedItm = listView1.FocusedItem.SubItems[0].Text.Split(null);

            panelEdit.Enabled = true;
            panelEdit.Visible = true;
            buttonSaveEdit.Enabled = true;
            buttonSaveEdit.Visible = true;
            buttonBackEdit.Enabled = true;
            buttonBackEdit.Visible = true;

            OleDbConnection conn = new OleDbConnection(connstring);
            try
            {
                conn.Open();
                OleDbCommand comm = new OleDbCommand("SELECT * FROM Angajati WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';");
                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        textBoxLastName.Text = reader["Nume"].ToString();
                        textBoxFirstName.Text = reader["Prenume"].ToString();
                        dateTimePickerBirth.Value = Convert.ToDateTime(reader["Data_nasterii"].ToString());
                        dateTimePickerEmploy.Value = Convert.ToDateTime(reader["Data_angajarii"].ToString());
                        if (reader["Functia"].ToString() == "Director")
                            comboBoxPosition.Text = "Manager";
                        else if (reader["Functia"].ToString() == "Mecanic")
                            comboBoxPosition.Text = "Mechanic";
                        else if (reader["Functia"].ToString() == "Asistent")
                            comboBoxPosition.Text = "Assistant";
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
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            selectedItm = listView1.FocusedItem.SubItems[0].Text.Split(null);

            if(MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OleDbConnection conn = new OleDbConnection(connstring);
                try
                {
                    conn.Open();
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = conn;

                    comm.CommandText = "DELETE FROM Angajati WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    comm.ExecuteNonQuery();
                    System.Threading.Thread.Sleep(1000);
                    populateListView();


                    panelEdit.Enabled = false;
                    panelEdit.Visible = false;
                    buttonSaveEdit.Enabled = false;
                    buttonSaveEdit.Visible = false;
                    buttonBackEdit.Enabled = false;
                    buttonBackEdit.Visible = false;

                    buttonDelete.Enabled = false;
                    buttonEdit.Enabled = false;



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {

            }
           
        }

        private void buttonSaveEdit_Click(object sender, EventArgs e)
        {
            selectedItm = listView1.FocusedItem.SubItems[0].Text.Split(null);
            string format = "dd/MM/yyyy";
            string position="";
            if (comboBoxPosition.Text == "Manager")
                position = "Director";
            else
                if (comboBoxPosition.Text == "Mechanic")
                position = "Mecanic";
            else
                if (comboBoxPosition.Text == "Assistant")
                position = "Asistent";

            if(textBoxFirstName.Text=="")
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
                    OleDbConnection conn = new OleDbConnection(connstring);
                    try
                    {
                        conn.Open();
                        OleDbCommand comm = new OleDbCommand();
                        comm.Connection = conn;

                        comm.CommandText = "UPDATE Angajati SET Nume='" + textBoxLastName.Text + "', Prenume='" + textBoxFirstName.Text + "', Data_nasterii='" + dateTimePickerBirth.Value.ToString(format) + "', Data_angajarii='" + dateTimePickerEmploy.Value.ToString(format) + "', Functia='" + position + "' WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                        comm.ExecuteNonQuery();

                        MessageBox.Show("The information was updated successfully.");

                        populateListView();

                        panelEdit.Enabled = false;
                        panelEdit.Visible = false;
                        buttonSaveEdit.Enabled = false;
                        buttonSaveEdit.Visible = false;
                        buttonBackEdit.Enabled = false;
                        buttonBackEdit.Visible = false;

                        buttonDelete.Enabled = false;
                        buttonEdit.Enabled = false;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void buttonBackEdit_Click(object sender, EventArgs e)
        {
            panelEdit.Enabled = false;
            panelEdit.Visible = false;
            buttonSaveEdit.Enabled = false;
            buttonSaveEdit.Visible = false;
            buttonBackEdit.Enabled = false;
            buttonBackEdit.Visible = false;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            panelInsert.Enabled = true;
            panelInsert.Enabled = true;
            panelInsert.Show();
            buttonAdd.Enabled = true;
            buttonAdd.Visible = true;
            buttonBackInsert.Enabled = true;
            buttonBackInsert.Visible = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(textBoxFirstNameI.Text=="")
            {
                errorProvider1.SetError(textBoxFirstNameI, "Can't leave this field empty!");
            }
            else
            {
                if (textBoxLastNameI.Text == "")
                {
                    errorProvider1.SetError(textBoxLastNameI, "Can't leave this field empty!");
                }
                else
                {
                    if (comboBoxPositionI.Text == "")
                    {
                        errorProvider1.SetError(comboBoxPositionI, "Can't leave this field empty!");
                    }
                    else
                    {
                        if (textBoxPassword.Text == "")
                        {
                            errorProvider1.SetError(textBoxPassword, "Can't leave this field empty!");
                        }
                        else
                        {
                            string format = "dd/MM/yyyy";
                            string position = "";
                            if (comboBoxPositionI.Text == "Manager")
                                position = "Director";
                            else
                                if (comboBoxPositionI.Text == "Mechanic")
                                position = "Mecanic";
                            else
                                if (comboBoxPositionI.Text == "Assistant")
                                position = "Asistent";

                            OleDbConnection conn = new OleDbConnection(connstring);
                            try
                            {
                                conn.Open();
                                OleDbCommand comm = new OleDbCommand();
                                comm.Connection = conn;

                                comm.CommandText = "INSERT INTO Angajati (Nume, Prenume, Data_nasterii, Data_angajarii, Functia, Parola) VALUES ('" + textBoxLastNameI.Text + "', '" + textBoxFirstNameI.Text + "', '" + dateTimePickerBirthI.Value.ToString(format) + "', '" + dateTimePickerEmployI.Value.ToString(format) + "', '" + position + "', '" + textBoxPassword.Text + "');";
                                comm.ExecuteNonQuery();

                                MessageBox.Show("The employee was inserted successfully.");

                                populateListView();

                                textBoxFirstNameI.Clear();
                                textBoxFirstNameI.Clear();
                                dateTimePickerBirthI.Value = DateTime.Today;
                                dateTimePickerEmployI.Value = DateTime.Today;
                                comboBoxPositionI.SelectedIndex = -1;
                                textBoxPassword.Clear();

                                panelInsert.Enabled = false;
                                panelInsert.Visible = false;
                                panelInsert.Hide();
                                buttonAdd.Enabled = false;
                                buttonAdd.Visible = false;
                                buttonBackInsert.Enabled = false;
                                buttonBackInsert.Visible = false;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
            }
        }

        private void buttonBackInsert_Click(object sender, EventArgs e)
        {
            panelInsert.Enabled = false;
            panelInsert.Visible = false;
            panelInsert.Hide();
            buttonAdd.Enabled = false;
            buttonAdd.Visible = false;
            buttonBackInsert.Enabled = false;
            buttonBackInsert.Visible = false;
        }

        private void buttonService_Click(object sender, EventArgs e)
        {
            panelService.Visible = true;
            panelService.Enabled = true;
            panelService.Show();

            try
            {
                using (BinaryReader br = new BinaryReader(File.Open("Queue.dat", FileMode.Open)))
                {
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string title = br.ReadString();
                        string type = br.ReadString();

                        Vehicle v = new Vehicle(id, title, type);
                        queue.Add(v);
                    }
                    br.Close();
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(" File \"Queue.dat\" not found!");
            }

            populateListView2();
            populateQueue();
        }

        private void buttonBackService_Click(object sender, EventArgs e)
        {
            panelService.Visible = false;
            panelService.Enabled = false;
            panelService.Hide();


            using (BinaryWriter bw = new BinaryWriter(File.Open("Queue.dat", FileMode.Create)))
            {
                foreach (Vehicle v in queue)
                {
                    bw.Write(v.Id);
                    bw.Write(v.VehicleTitle);
                    bw.Write(v.Type);
                }
            }

            buttonRemove.Enabled = false;
            buttonAssign.Enabled = false;
            buttonFinish.Enabled = false;

            queue.Clear();
        }

        private void buttonAddCar_Click(object sender, EventArgs e)
        {
            panelAddCar.Visible = true;
            panelAddCar.Enabled = true;
            panelAddCar.Show();
            buttonAddAdd.Visible = true;
            buttonAddAdd.Enabled = true;
            buttonAddBack.Visible = true;
            buttonAddBack.Enabled = true;
            buttonAssign.Visible = false;
        }

        private void buttonAddBack_Click(object sender, EventArgs e)
        {
            panelAddCar.Visible = false;
            panelAddCar.Enabled = false;
            panelAddCar.Hide();
            buttonAddAdd.Visible = false;
            buttonAddAdd.Enabled = false;
            buttonAddBack.Visible = false;
            buttonAddBack.Enabled = false;
            buttonAssign.Visible = true;
        }

        private void buttonAddAdd_Click(object sender, EventArgs e)
        {
            if(textBoxVehicleId.Text=="")
            {
                errorProvider1.SetError(textBoxVehicleId, "Can't leave this field empty!");
            }
            else
            {
                if (textBoxVehicleTitle.Text == "")
                {
                    errorProvider1.SetError(textBoxVehicleTitle, "Can't leave this field empty!");
                }
                else
                {
                    if (comboBoxVehicleType.Text == "")
                    {
                        errorProvider1.SetError(comboBoxVehicleType, "Can't leave this field empty!");
                    }
                    else
                    {
                        int id = Convert.ToInt32(textBoxVehicleId.Text);
                        string title = textBoxVehicleTitle.Text;
                        string type = comboBoxVehicleType.Text;
                        Vehicle vehicle = new Vehicle(id, title, type);
                        queue.Add(vehicle);
                        populateQueue();

                        textBoxVehicleId.Clear();
                        textBoxVehicleTitle.Clear();
                        comboBoxVehicleType.SelectedIndex = -1;

                        panelAddCar.Visible = false;
                        panelAddCar.Enabled = false;
                        panelAddCar.Hide();
                        buttonAddAdd.Visible = false;
                        buttonAddAdd.Enabled = false;
                        buttonAddBack.Visible = false;
                        buttonAddBack.Enabled = false;
                        buttonAssign.Visible = true;
                        buttonRemove.Enabled = false;
                        buttonAssign.Enabled = false;
                    }
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int count = 0;
            int positionOfNewLine = listViewVehicles.FocusedItem.SubItems[0].Text.IndexOf("\r\n");
            string partBefore = "";
            if (positionOfNewLine >= 0)
            {
                partBefore = listViewVehicles.FocusedItem.SubItems[0].Text.Substring(0, positionOfNewLine);
            }
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (Vehicle v in queue)
                {
                    if (Convert.ToInt32(partBefore) == v.Id)
                        break;
                    count++;
                }
                queue.Remove(queue[count]);
                System.Threading.Thread.Sleep(1000);
                populateQueue();
                buttonRemove.Enabled = false;
                buttonAssign.Enabled = false;
            }
            else
            {

            }
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            comboBoxFinish.Items.Clear();
            if(listViewEmployees.FocusedItem.SubItems[1].Text!="0")
                comboBoxFinish.Items.Add("Car 1");
            if(listViewEmployees.FocusedItem.SubItems[2].Text != "0")
                comboBoxFinish.Items.Add("Car 2");
            if(listViewEmployees.FocusedItem.SubItems[3].Text != "0")
                comboBoxFinish.Items.Add("Car 3");
            if(listViewEmployees.FocusedItem.SubItems[4].Text != "0")
                comboBoxFinish.Items.Add("Bus");
            if(listViewEmployees.FocusedItem.SubItems[5].Text != "0")
                comboBoxFinish.Items.Add("Truck");

            buttonAssign.Visible = false;
            buttonFinish.Enabled = false;
            buttonFinishFinish.Enabled = true;
            buttonFinishFinish.Visible = true;
            buttonFinishBack.Enabled = true;
            buttonFinishBack.Visible = true;
            panelFinish.Visible = true;
            panelFinish.Enabled = true;
            panelFinish.Show();
        }

        private void listViewVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewVehicles.SelectedItems != null && listViewVehicles.SelectedItems.Count > 0)
            {
                buttonRemove.Enabled = true;
                buttonAssign.Enabled = true;
            }
            else
            {
                buttonRemove.Enabled = false;
                buttonAssign.Enabled = false;
            }
            listViewVehicles.Focus();
        }

        private void listViewEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEmployees.SelectedItems != null && listViewEmployees.SelectedItems.Count > 0 && ((listViewEmployees.FocusedItem.SubItems[1].Text != "0" || listViewEmployees.FocusedItem.SubItems[2].Text != "0" || listViewEmployees.FocusedItem.SubItems[3].Text != "0" || listViewEmployees.FocusedItem.SubItems[4].Text != "0" || listViewEmployees.FocusedItem.SubItems[5].Text != "0")))
            {
                buttonFinish.Enabled = true;
            }
            else
            {
                buttonFinish.Enabled = false;
            }
            listViewEmployees.Focus();
        }

        private void buttonAssign_Click(object sender, EventArgs e)
        {
            comboBoxAssignEmployee.Items.Clear();
            int count = 0;
            int positionOfNewLine = listViewVehicles.FocusedItem.SubItems[0].Text.IndexOf("\r\n");
            string partBefore = "";
            if (positionOfNewLine >= 0)
            {
                partBefore = listViewVehicles.FocusedItem.SubItems[0].Text.Substring(0, positionOfNewLine);
            }
            foreach (Vehicle v in queue)
            {
                if (Convert.ToInt32(partBefore) == v.Id)
                    break;
                count++;
            }
            if (queue[count].Type == "Car")
            {
                foreach(ListViewItem item in listViewEmployees.Items)
                {
                    if(item.SubItems[1].Text=="0"||item.SubItems[2].Text=="0"||item.SubItems[3].Text=="0")
                    {
                        comboBoxAssignEmployee.Items.Add(item.SubItems[0].Text);
                    }
                }
            }
            else if (queue[count].Type == "Bus")
            {
                foreach (ListViewItem item in listViewEmployees.Items)
                {
                    if (item.SubItems[4].Text == "0")
                    {
                        comboBoxAssignEmployee.Items.Add(item.SubItems[0].Text);
                    }
                }
            }
            else if (queue[count].Type == "Truck")
            {
                foreach (ListViewItem item in listViewEmployees.Items)
                {
                    if (item.SubItems[5].Text == "0")
                    {
                        comboBoxAssignEmployee.Items.Add(item.SubItems[0].Text);
                    }
                }
            }

            buttonAssign.Enabled = false;
            buttonAssign.Visible = false;
            buttonAssignAssign.Enabled = true;
            buttonAssignAssign.Visible = true;
            buttonAssignBack.Enabled = true;
            buttonAssignBack.Visible = true;
            panelAssign.Visible = true;
            panelAssign.Enabled = true;
            panelAssign.Show();
        }

        private void buttonAssignBack_Click(object sender, EventArgs e)
        {
            buttonAssign.Enabled = true;
            buttonAssign.Visible = true;
            buttonAssignAssign.Enabled = false;
            buttonAssignAssign.Visible = false;
            buttonAssignBack.Enabled = false;
            buttonAssignBack.Visible = false;
            panelAssign.Visible = false;
            panelAssign.Enabled = false;
            panelAssign.Hide();
        }

        private void buttonAssignAssign_Click(object sender, EventArgs e)
        {
            selectedItm = comboBoxAssignEmployee.Text.Split(null);

            int count = 0;
            int positionOfNewLine = listViewVehicles.FocusedItem.SubItems[0].Text.IndexOf("\r\n");
            string partBefore = "";
            if (positionOfNewLine >= 0)
            {
                partBefore = listViewVehicles.FocusedItem.SubItems[0].Text.Substring(0, positionOfNewLine);
            }
            foreach (Vehicle v in queue)
            {
                if (Convert.ToInt32(partBefore) == v.Id)
                    break;
                count++;
            }

            if(comboBoxAssignEmployee.Text=="")
            {
                errorProvider1.SetError(comboBoxAssignEmployee, "Can't leave this field empty!");
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(connstring);
                conn.Open();
                try
                {
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = conn;

                    int count2 = 0;
                    foreach (ListViewItem item in listViewEmployees.Items)
                    {
                        if (comboBoxAssignEmployee.Text == item.SubItems[0].Text)
                            break;
                        count2++;
                    }

                    if (queue[count].Type == "Car")
                    {
                        if (listViewEmployees.Items[count2].SubItems[1].Text == "0")
                        {
                            comm.CommandText = "UPDATE Angajati SET Car1=? WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                            comm.Parameters.Add("Car1", OleDbType.Integer).Value = queue[count].Id;
                        }
                        else if (listViewEmployees.Items[count2].SubItems[2].Text == "0")
                        {
                            comm.CommandText = "UPDATE Angajati SET Car2=? WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                            comm.Parameters.Add("Car2", OleDbType.Integer).Value = queue[count].Id;
                        }
                        else if (listViewEmployees.Items[count2].SubItems[3].Text == "0")
                        {
                            comm.CommandText = "UPDATE Angajati SET Car3=? WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                            comm.Parameters.Add("Car3", OleDbType.Integer).Value = queue[count].Id;
                        }
                    }
                    else if (queue[count].Type == "Bus")
                    {
                        comm.CommandText = "UPDATE Angajati SET Bus=? WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                        comm.Parameters.Add("Bus", OleDbType.Integer).Value = queue[count].Id;
                    }
                    else if (queue[count].Type == "Truck")
                    {
                        comm.CommandText = "UPDATE Angajati SET Truck=? WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                        comm.Parameters.Add("Truck", OleDbType.Integer).Value = queue[count].Id;
                    }
                    comm.ExecuteNonQuery();

                    queue.Remove(queue[count]);

                    populateQueue();

                    MessageBox.Show("The vehicle was assigned successfully.");
                    populateListView2();
                    buttonAssign.Enabled = true;
                    buttonAssign.Visible = true;
                    buttonAssignAssign.Enabled = false;
                    buttonAssignAssign.Visible = false;
                    buttonAssignBack.Enabled = false;
                    buttonAssignBack.Visible = false;
                    panelAssign.Visible = false;
                    panelAssign.Enabled = false;
                    panelAssign.Hide();
                    buttonAssign.Enabled = false;
                    buttonRemove.Enabled = false;
                    populateListView2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void buttonFinishFinish_Click(object sender, EventArgs e)
        {
            selectedItm = listViewEmployees.FocusedItem.SubItems[0].Text.Split(null);

            if(comboBoxFinish.Text=="")
            {
                errorProvider1.SetError(comboBoxFinish, "Can't leave this field empty!");
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(connstring);
                conn.Open();
                try
                {
                    OleDbCommand comm = new OleDbCommand();
                    comm.Connection = conn;

                    if (comboBoxFinish.Text == "Car 1")
                    {
                        comm.CommandText = "UPDATE Angajati SET Car1=0 WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    }
                    else if (comboBoxFinish.Text == "Car 2")
                    {
                        comm.CommandText = "UPDATE Angajati SET Car2=0 WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    }
                    else if (comboBoxFinish.Text == "Car 3")
                    {
                        comm.CommandText = "UPDATE Angajati SET Car3=0 WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    }
                    else if (comboBoxFinish.Text == "Bus")
                    {
                        comm.CommandText = "UPDATE Angajati SET Bus=0 WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    }
                    else if (comboBoxFinish.Text == "Truck")
                    {
                        comm.CommandText = "UPDATE Angajati SET Truck=0 WHERE Nume='" + selectedItm[0] + "' AND Prenume='" + selectedItm[1] + "';";
                    }
                    comm.ExecuteNonQuery();

                    populateQueue();

                    MessageBox.Show("The repair was finished successfully.");
                    populateListView2();
                    buttonAssign.Visible = true;
                    buttonFinishFinish.Enabled = false;
                    buttonFinishFinish.Visible = false;
                    buttonFinishBack.Enabled = false;
                    buttonFinishBack.Visible = false;
                    panelFinish.Visible = false;
                    panelFinish.Enabled = false;
                    panelFinish.Hide();
                    populateListView2();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void buttonFinishBack_Click(object sender, EventArgs e)
        {
            buttonAssign.Visible = true;
            buttonFinishFinish.Enabled = false;
            buttonFinishFinish.Visible = false;
            buttonFinishBack.Enabled = false;
            buttonFinishBack.Visible = false;
            panelFinish.Visible = false;
            panelFinish.Enabled = false;
            panelFinish.Hide();
        }
    }
}
