using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DB_Project
{
    public partial class Add_Patient : Form
    {
        MySqlConnection connection;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        MySqlDataReader dr;
        string Date;
        
        public Add_Patient()
        {
            InitializeComponent();
            
        }

        private void Add_Patient_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection("server=localhost;database=clinic;uid=root;pwd=root");
            try
            {
                connection.Open();
                String q = "select name from diseases;";
                cmd = new MySqlCommand(q,connection);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                    for (int i = 0; i < row.ItemArray.Length; i++)
                        checkedListBox2.Items.Add(row.ItemArray[0].ToString());

                /*foreach(DataRow row in dt.Rows)
                    for (int i = 0; i < row.ItemArray.Length; i++)
                        comboBox2.Items.Add(row.ItemArray[0].ToString());*/

                String qq = "select name from medicines;";
                cmd = new MySqlCommand(qq, connection);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                    for (int i = 0; i < row.ItemArray.Length; i++)
                        checkedListBox1.Items.Add(row.ItemArray[0].ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            connection.Open();
            cmd = new MySqlCommand();
            cmd.Connection=connection;
            Date = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            cmd.CommandText = "insert ignore into patient values (" + textBox1.Text + ", '" + textBox2.Text + "'" + ", '"
                + textBox3.Text + "'," + textBox7.Text + ",'" + comboBox1.Text + "'" + ", '"
                + textBox4.Text + "'" + ", '" + textBox5.Text + "', '" + Date + "', '" + textBox6.Text + "');";
            cmd.ExecuteNonQuery();

            foreach (string diseases in checkedListBox2.CheckedItems)
            {
                cmd.CommandText = "insert ignore into patient_diseases (p_id,d_id,date) select p.ssn,d.d_id,'" + Date + "' from patient as p,"
                    + "diseases as d,patient_diseases as pd where p.date<>pd.date and p.ssn=" + textBox1.Text + " and d.name='" + diseases + "';";
                
                cmd.ExecuteNonQuery();
            }
            
            /*cmd.CommandText = "insert into patient_diseases (p_id,d_id) select p.ssn,d.d_id from patient as p,diseases as d where p.ssn=" 
                + textBox1.Text + " and d.name='" + comboBox2.Text + "';";
            cmd.ExecuteNonQuery();*/

            foreach (string medicines in checkedListBox1.CheckedItems)
            {
                cmd.CommandText = "insert ignore into patient_medicines (p_id,m_id,date) select p.ssn,m.m_id,'" + Date + "' from patient as p,"
                    + "medicines as m,patient_medicines as pm where p.date<>pm.date and p.ssn=" + textBox1.Text + " and m.name='" + medicines + "';";
                
                cmd.ExecuteNonQuery();
            }


            foreach (string diseases in checkedListBox2.CheckedItems)
                foreach (string medicines in checkedListBox1.CheckedItems)
                {
                    cmd.CommandText = "insert  ignore into diseases_medicines (d_id,m_id) select d.d_id,m.m_id from diseases as d,"
                        + "medicines as m where d.name='" + diseases + "' and m.name='" + medicines + "';";
                    cmd.ExecuteNonQuery();
                }

            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); 
            textBox5.Clear(); textBox6.Clear(); textBox7.Clear();
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            connection.Open();
            cmd.CommandText = "select * from patient where f_name='" + textBox8.Text + "' and l_name='" + textBox9.Text + "';";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr.GetValue(0).ToString();
                textBox2.Text = dr.GetValue(1).ToString();
                textBox3.Text = dr.GetValue(2).ToString();
                textBox7.Text = dr.GetValue(3).ToString();
                textBox4.Text = dr.GetValue(5).ToString();
                textBox5.Text = dr.GetValue(6).ToString();
            }
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 x = new Form1();
            x.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connection.Open();
            cmd.CommandText = "insert ignore into diseases (name) values ('" + textBox10.Text + "');";
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            connection.Open();
            cmd.CommandText = "insert ignore into medicines (name,price) values ('" + textBox11.Text + "',"+textBox12.Text+");";
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
