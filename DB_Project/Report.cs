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
    public partial class Report : Form
    {
        MySqlConnection connection;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        MySqlDataReader dr;
        DataSet ds;


        public Report()
        {
            InitializeComponent();
        }

      

        private void Report_Load(object sender, EventArgs e)
        {

            connection = new MySqlConnection("server=localhost;database=clinic;uid=root;pwd=root");
            try
            {
                connection.Open();

                String q = "select name from diseases;";
                cmd = new MySqlCommand(q, connection);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                    for (int i = 0; i < row.ItemArray.Length; i++)
                        comboBox1.Items.Add(row.ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            connection.Open();
            cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select * from patient where f_name='" + textBox1.Text + "' and l_name='" + textBox2.Text + "';";
            /*cmd.CommandText = "select p.*,d.name disease,m.name medicine from patient as p,diseases as d,medicines as m,"
                + "patient_diseases as pd, patient_medicines as pm,diseases_medicines as dm where p.ssn=pd.p_id and p.ssn=pm.p_id"
                + " and pd.d_id=d.d_id and pm.m_id=m.m_id and d.d_id=dm.d_id and m.m_id=dm.m_id and dm.d_id=pd.d_id"
                + " and dm.m_id=pm.m_id and p.date=pd.date and p.date=pm.date and p.f_name='" + textBox1.Text + "';";*/
            dr = cmd.ExecuteReader();
            dt=new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource=dt;
            connection.Close();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            connection.Open();
            cmd.CommandText = "select d.name disease,m.name medicine from diseases d,medicines m,diseases_medicines dm"
                + " where d.d_id=dm.d_id and m.m_id=dm.m_id and d.name='" + comboBox1.Text+ "';";
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            connection.Open();
            cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select p.*,d.name disease,m.name medicine from patient as p,diseases as d,medicines as m,"
                + "patient_diseases as pd, patient_medicines as pm,diseases_medicines as dm where p.ssn=pd.p_id and p.ssn=pm.p_id"
                + " and pd.d_id=d.d_id and pm.m_id=m.m_id and d.d_id=dm.d_id and m.m_id=dm.m_id and dm.d_id=pd.d_id"
                + " and dm.m_id=pm.m_id and p.date=pd.date and p.date=pm.date and p.f_name='" + textBox3.Text
                + "' and p.l_name='" + textBox4.Text + "';";
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
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
            //connection.Open();
            //string q = "select * from patient;";
            //ds = new DataSet();
            //cmd = new MySqlCommand(q, connection);
            //da = new MySqlDataAdapter(cmd);

            //da.Fill(ds);
            //CrystalReport3 re = new CrystalReport3();
            //re.SetDataSource(ds);
            this.Hide();
            Crystal x = new Crystal();
            x.Show();
            //connection.Close();
        }
    }
}
