using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProcedureSearch
{
    public partial class Main : Form
    { 
        public string VAULT_PATH = ConfigurationManager.AppSettings["VAULT_PATH"];
        public string IID_DATABASE_PATH = ConfigurationManager.AppSettings["IID_DATABASE_PATH"];
        SearchingProgressForm SearchingForm = new SearchingProgressForm();

        class Product
        {
            public string ProductNumber { get; set; }
            public List<string> ProcedureList { get; set; }

            public Product(string productNumber, List<string> procedureList)
            {
                ProductNumber = productNumber;
                ProcedureList = procedureList;
            }
        }
        
        public Main()
        {
            InitializeComponent();
            InitializeEventHandlers();            
        }

        private void InitializeEventHandlers()
        {
            rt.TextChanged += (sender, e) => {
                if (rt.Visible)
                    rt.ScrollToCaret();
            };
            this.KeyPreview = true;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SearchingForm.Hide();

            Logger.Log("Program loaded, ready to search.", rt);

            TPSerialEntryComboBox.Select();
        }

        /// <summary>
        /// Performs an action safely the appropriate thread.
        /// </summary>
        /// <param name="a">Action to perform.</param>
        private void ExecuteSecure(Action a)
        // Usage example: ExecuteSecure(() => this.someLabel.Text = "foo");
        {
            Invoke((MethodInvoker)delegate
            {
                a();
            });
        }

        private void OpenFile(string FileName)
        {
            try
            {
                Logger.Log($"Opening test procedure {FileName}", rt);
                var fi = new FileInfo(FileName);
                Process.Start(fi.FullName);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), rt);
            }
        }

        private string GetProductFromIID(string IID)
        {
            var RetVal = "";
            string Provider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            var ConnString = (Provider + IID_DATABASE_PATH);
            var DbConnection = new OleDbConnection { ConnectionString = ConnString };
            try
            {
                DbConnection.Open();
                var SqlStr = $@"SELECT * FROM ProductCode WHERE (ProductID = '{IID}')";
                var DbCmd = new OleDbCommand(SqlStr, DbConnection);
                var DataReader = DbCmd.ExecuteReader();
                while (DataReader.Read())
                {
                    if (DataReader.GetValue(1).ToString() != "")
                    {
                        RetVal = DataReader.GetValue(1).ToString();
                    }                    
                }
                DbConnection.Close();
                return RetVal;
                              
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured in iid: " + ex.Message, rt, Color.Red));
                return RetVal;
            }
        }

        private Product FindProcedures(string SerialNumber)
        {
            List<string> ProcedureList = new List<string>();
            try
            {
                // get IID from serial numbers
                var IID = SerialNumber.Substring(0, 3);
                // Using the IID from serial number, find corresponding product number
                var ProductNumber = GetProductFromIID(IID);
                if (ProductNumber == "")
                {
                    return new Product(SerialNumber, ProcedureList);
                }
                // get first 3 digits of product number to determine if final assy or sub assy
                var ProdFirst3 = ProductNumber.Substring(0, 3);
                // remove first 4 characters of product number to get assembly number
                var AssemblyNumber = ProductNumber.Remove(0, 4);
                // find procedure based on first 3 digits
                switch (ProdFirst3)
                {
                    case "231":
                    if (!Directory.Exists($@"{VAULT_PATH}\Released_Part_Information\234-xxxxx_Assy_Test_Proc\Standard_Products\234-{AssemblyNumber.Substring(0, 5)}"))
                    {
                        return new Product(ProductNumber, ProcedureList);
                    }
                    foreach (string file in Directory.EnumerateFiles($@"{VAULT_PATH}\Released_Part_Information\234-xxxxx_Assy_Test_Proc\Standard_Products\234-{AssemblyNumber.Substring(0, 5)}", 
                                                                    ("*" + AssemblyNumber + "*"), System.IO.SearchOption.AllDirectories))
                    {
                        // check for duplicate files in list, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (!ProcedureList.Contains(file) && (file.ToLower().Contains("archive") == false))
                        {
                            ProcedureList.Add(file);
                        }
                    }

                    if (!ProcedureList.Any())
                    {                     
                        foreach (string file in Directory.EnumerateFiles($@"\\ares\shared\Operations\Test Engineering\Documents ready for release",
                                                                               ("*" + AssemblyNumber + "*"), System.IO.SearchOption.TopDirectoryOnly))
                        {
                            // check for duplicate files in listbox, and if no duplicates exist, add the file
                            if (!ProcedureList.Contains(file))
                            {
                                ProcedureList.Add(file);
                            }
                        }
                    }
                    return new Product(ProductNumber, ProcedureList);

                    case "216":
                    foreach (string file in Directory.EnumerateFiles($@"{VAULT_PATH}\Released_Part_Information\225-xxxxx_Proc_Mfg_Test\",
                                                                    ("*" + (AssemblyNumber + "*")), System.IO.SearchOption.AllDirectories))
                    {
                        // check for duplicate files in list, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (!ProcedureList.Contains(file) && (file.ToLower().Contains("archive") == false))
                        {
                            ProcedureList.Add(file);
                        }
                    }

                    if (!ProcedureList.Any())
                    {
                        foreach (string file in Directory.EnumerateFiles($@"\\ares\shared\Operations\Test Engineering\Documents ready for release",
                                                                               ("*" + (AssemblyNumber + "*")), System.IO.SearchOption.TopDirectoryOnly))
                        {
                            // check for duplicate files in listbox, and if no duplicates exist, add the file
                            if (!ProcedureList.Contains(file))
                            {
                                ProcedureList.Add(file);
                            }
                        }
                    }
                    return new Product(ProductNumber, ProcedureList);
                    default:
                    return new Product(ProductNumber, ProcedureList);
                }
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured in findproc: " + ex.Message, rt, Color.Red));
                return new Product(null, ProcedureList);
            }
        }

        private void TPOpenButton_Click(object sender, EventArgs e)
        {
            if (TPResultsListBox.Items.Count == 0)
            {
                Logger.Log("No procedure selected, enter a serial number and search.", rt, Color.Red);
                return;
            }

            var f = new FileInfo(TPResultsListBox.SelectedItem.ToString().ToLower());

            if (f.ToString().Contains("ready for release"))
            {
                MessageBox.Show("Note:  This procedure has not yet been released to the vault.", "Procedure Search", MessageBoxButtons.OK);
            }

            if (TPArchivedLabel.Visible)
            {
                if (MessageBox.Show("This procedure is archived, and may not be the newest revision.\n\nAre you sure you want to open this procedure?",
                                "Procedure Search", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }

            OpenFile(TPResultsListBox.SelectedItem.ToString());
        }

        private void TPSearchButton_Click(object sender, EventArgs e)
        {
            if (bWorker.IsBusy) return;

            TPResultsListBox.Items.Clear();
            Regex rx = new Regex("[^a-zA-Z0-9]");
            var Serial = TPSerialEntryComboBox.Text;
            Serial = rx.Replace(TPSerialEntryComboBox.Text, "");
            
            if (Serial.Length < 3)
            {
                Logger.Log("Invalid serial number entered.", rt);
                return;
            }

            if (!TPSerialEntryComboBox.Items.Contains(TPSerialEntryComboBox.Text))
            {
                TPSerialEntryComboBox.Items.Add(TPSerialEntryComboBox.Text);
            }

            TPSerialEntryComboBox.SelectAll();
            this.Enabled = false;
            SearchingForm.Show();
            bWorker.RunWorkerAsync(TPSerialEntryComboBox.Text);
        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.Enabled == false) return;

            if (tabControl.SelectedTab.Text == "Test Procedures")
            {
                if (TPResultsListBox.Focused || e.KeyChar == (char)Keys.Enter)
                {
                    TPSearchButton.PerformClick();
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == '[')
                {
                    TPSerialEntryComboBox.SelectAll();
                    e.KeyChar = (char)Keys.None;
                }
                if (e.KeyChar == ']')
                {
                    e.KeyChar = (char)Keys.None;
                    TPSearchButton.PerformClick();
                    return;
                }
                if (!TPSerialEntryComboBox.Focused)
                {
                    TPSerialEntryComboBox.Focus();
                    TPSerialEntryComboBox.Text = e.KeyChar.ToString();
                    TPSerialEntryComboBox.SelectionStart = TPSerialEntryComboBox.Text.Length;
                    e.Handled = true;
                }
            }

            if (tabControl.SelectedTab.Name == "Process Sheets")
            {

            }
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var product = FindProcedures((string)e.Argument);
            e.Result = product;
        }

        private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            
            SearchingForm.Hide();
            var product = (Product)e.Result;
            string ProductNumber = product.ProductNumber;
            List<string> ProceduresList = product.ProcedureList;
            TPFilenameTextbox.Text = ProductNumber;

            if (!ProceduresList.Any())
            {
                Logger.Log($"No procedures found for {ProductNumber}", rt);
                return;
            }

            foreach (var p in ProceduresList)
            {
                var f = new FileInfo(p);
                TPResultsListBox.Items.Add(f);
            }
        }

        private void TPSerialEntryComboBox_TextChanged(object sender, EventArgs e)
        {
            TPSerialEntryComboBox.Text = TPSerialEntryComboBox.Text.ToUpper();
            TPSerialEntryComboBox.SelectionStart = TPSerialEntryComboBox.Text.Length;
        }        
    }
}
