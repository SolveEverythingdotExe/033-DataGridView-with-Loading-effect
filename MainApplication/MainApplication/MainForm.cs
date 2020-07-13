using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MainApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //lets create a method to load the data
        private void LoadData()
        {
            //FOR FELLOW DEVELOPERS =====> just kindly replace the connection string and your good to go
            string ConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;User ID=sa;Initial Catalog=PaymentSystem;Data Source=.\SQLSERVER2016";
            string CommandText = "SELECT Id, Payer, Payee, Amount FROM tblTransactions";

            dataGridView.LoadData(ConnectionString, CommandText, false);
        }

        //now lets call it on formload
        private void MainForm_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        //reload the data once refresh button is clicked
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        //lets test
    }
}
