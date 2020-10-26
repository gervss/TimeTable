using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace TimeTable2018
{
    public partial class Form1 : Form
    {
        SqlCommand cmd;

        //eto connection sa database
        SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\DB_Database.mdf;Integrated Security = True");

        public Form1()
        {
            InitializeComponent();
        }

        private void loaddata()
        {
            //eto nag list sa data gridview para makita nga reserve
            this.tbl_timeTableAdapter.Fill(this.dS_Front.tbl_time);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tinawag ko load data ginawa ko variable para madali lng sya refresh pag add ako
            loaddata();

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            //need to pag gagawa ka insert into na query para ipasok sa database need mo muna open connection
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            //ginawa ko string value date filter na combobox
             string strTime_Start = datepick1.selectedValue.ToString();
             string strTime_End = datepick2.selectedValue.ToString();

            //saka ko sya convert date time para string basahin datetime na table duon sa database kase ndi varchar gamit ko
             DateTime dateTime_Start = Convert.ToDateTime(strTime_Start);
             DateTime dateTime_End = Convert.ToDateTime(strTime_End);


            //eto query para select time pa greater than alam mo nayun haha
            cmd = new SqlCommand("SELECT * FROM tbl_time WHERE [Time_From] >= @Time_From AND [Time_To] <= @Time_To AND [Time_Date] LIKE @Time_Date", con);

            //eto parameter parra alamin value sa design na seselect nya
            cmd.Parameters.AddWithValue("@Time_From", datepick1.selectedValue);
            cmd.Parameters.AddWithValue("@Time_To", datepick2.selectedValue);
            cmd.Parameters.AddWithValue("@Time_Date", datepick3.selectedValue);

            //excute na nya para alamin select nya
            SqlDataReader dr = cmd.ExecuteReader();

            if(dr.HasRows)
            {

                MessageBox.Show("Its Already Occupied");
                dr.Close();
                //dr.close every mag output ka data dpat close after nun
            }
          //Not updated.
            else
            {

                dr.Close();

                //eto alam mo narin to 
                if (dateTime_Start > dateTime_End)
                {
                    MessageBox.Show("Bawal ibalik ang nakaraan oras");
                }
                else
                {
                    dr.Close();

                    //alam mo nato equal equal para sa same value
                    if (dateTime_Start == dateTime_End)
                    {
                        MessageBox.Show("Bawal parehas na oras");
                    }
                    else
                    {
                        //eto para malan nya text kung wala laman alam mo nayan hahah
                        if(txt_name.Text == "" || txt_event.Text == "")
                        {
                            MessageBox.Show("Nakalimutan mo Lagyan Textbox");
                        }
                        else
                        {
                            //same lng din to ng java gumagamit lng ako parameter para sure na makuha value
                            cmd = new SqlCommand("Insert into tbl_time values (@Name, @Time_Event, @Time_From, @Time_To, @Time_Date)", con);
                            cmd.Parameters.AddWithValue("@Name", txt_name.Text);
                            cmd.Parameters.AddWithValue("@Time_Event", txt_event.Text);
                            cmd.Parameters.AddWithValue("@Time_From", datepick1.selectedValue);
                            cmd.Parameters.AddWithValue("@Time_To", datepick2.selectedValue);
                            cmd.Parameters.AddWithValue("@Time_Date", datepick3.selectedValue);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Successfully Added");

                            //eto ulit ginawa ko variable sa taas para refresh data list
                            loaddata();

                            txt_name.Text = "";
                            txt_event.Text = "";
                        }
                    }
                }
            }
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
