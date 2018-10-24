using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
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
        BackgroundWorker TPBWorker = new BackgroundWorker();
        BackgroundWorker PSBWorker = new BackgroundWorker();

        class Product
        {
            public string ProductNumber { get; set; }
            public List<string> DocumentList { get; set; }

            public Product(string productNumber, List<string> documentList)
            {
                ProductNumber = productNumber;
                DocumentList = documentList;
            }
        }
        
        public Main()
        {
            InitializeComponent();
            InitializeEventHandlers();            
        }

        private void InitializeEventHandlers()
        {
            TPBWorker.WorkerReportsProgress = true;
            TPBWorker.WorkerSupportsCancellation = true;
            TPBWorker.DoWork += TPBWorker_DoWork;
            TPBWorker.RunWorkerCompleted += TPBWorker_RunWorkerCompleted;

            PSBWorker.WorkerReportsProgress = true;
            PSBWorker.WorkerSupportsCancellation = true;
            PSBWorker.DoWork += PSBWorker_DoWork;
            PSBWorker.RunWorkerCompleted += PSBWorker_RunWorkerCompleted;

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

        private string GetIIDFromProduct(string product)
        {
            var RetVal = "";
            string Provider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            var ConnString = (Provider + IID_DATABASE_PATH);
            var DbConnection = new OleDbConnection { ConnectionString = ConnString };
            try
            {
                DbConnection.Open();
                var SqlStr = $@"SELECT * FROM ProductCode WHERE (Product = '{product}')";
                var DbCmd = new OleDbCommand(SqlStr, DbConnection);
                var DataReader = DbCmd.ExecuteReader();
                while (DataReader.Read())
                {
                    if (DataReader.GetValue(0).ToString() != "")
                    {
                        RetVal = DataReader.GetValue(0).ToString();
                    }
                }
                DbConnection.Close();
                return RetVal;

            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured in IID: " + ex.Message, rt, Color.Red));
                return RetVal;
            }
        }

        private Product FindProcedures(string SerialNumber)
        {
            List<string> DocumentList = new List<string>();
            try
            {
                // get IID from serial numbers
                var IID = SerialNumber.Substring(0, 3);
                // Using the IID from serial number, find corresponding product number
                var ProductNumber = GetProductFromIID(IID);
                if (ProductNumber == "")
                {
                    return new Product(SerialNumber, DocumentList);
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
                        return new Product(ProductNumber, DocumentList);
                    }
                    foreach (string file in Directory.EnumerateFiles($@"{VAULT_PATH}\Released_Part_Information\234-xxxxx_Assy_Test_Proc\Standard_Products\234-{AssemblyNumber.Substring(0, 5)}", 
                                                                    ("*" + AssemblyNumber + "*"), System.IO.SearchOption.AllDirectories))
                    {
                        // check for duplicate files in list, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (!DocumentList.Contains(file) && (file.ToLower().Contains("archive") == false))
                        {
                            DocumentList.Add(file);
                        }
                    }

                    if (!DocumentList.Any())
                    {                     
                        foreach (string file in Directory.EnumerateFiles($@"\\ares\shared\Operations\Test Engineering\Documents ready for release",
                                                                        ("*" + AssemblyNumber + "*"), System.IO.SearchOption.TopDirectoryOnly))
                        {
                            // check for duplicate files in listbox, and if no duplicates exist, add the file
                            if (!DocumentList.Contains(file))
                            {
                                DocumentList.Add(file);
                            }
                        }
                    }
                    return new Product(ProductNumber, DocumentList);

                    case "216":
                    foreach (string file in Directory.EnumerateFiles($@"{VAULT_PATH}\Released_Part_Information\225-xxxxx_Proc_Mfg_Test\",
                                                                    ("*" + (AssemblyNumber + "*")), System.IO.SearchOption.AllDirectories))
                    {
                        // check for duplicate files in list, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (!DocumentList.Contains(file) && (file.ToLower().Contains("archive") == false))
                        {
                            DocumentList.Add(file);
                        }
                    }

                    if (!DocumentList.Any())
                    {
                        foreach (string file in Directory.EnumerateFiles($@"\\ares\shared\Operations\Test Engineering\Documents ready for release",
                                                                               ("*" + (AssemblyNumber + "*")), System.IO.SearchOption.TopDirectoryOnly))
                        {
                            // check for duplicate files in listbox, and if no duplicates exist, add the file
                            if (!DocumentList.Contains(file))
                            {
                                DocumentList.Add(file);
                            }
                        }
                    }
                    return new Product(ProductNumber, DocumentList);
                    default:
                    return new Product(ProductNumber, DocumentList);
                }
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured: " + ex.Message, rt, Color.Red));
                return new Product(null, DocumentList);
            }
        }

        private Product FindProcessSheets(string SerialNumber)
        {
            List<string> DocumentList = new List<string>();
            string ProductNumber;
            string dir;
            try
            {
                // get IID from serial numbers
                var IID = SerialNumber.Substring(0, 3);
                // Using the IID from serial number, find corresponding product number
                ProductNumber = GetProductFromIID(IID);
                if (ProductNumber == "")
                {
                    return new Product(SerialNumber, DocumentList);
                }
                // get first 3 digits of product number to determine what type of part
                var ProdFirst3 = ProductNumber.Substring(0, 3);
                // remove first 4 characters of product number to get assembly number
                var AssemblyNumber = ProductNumber.Remove(0, 4);
                switch (ProdFirst3)
                {
                    case "213":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\213-xxxxx_Assy_Mech\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "216":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\216-xxxxx_PCB_Assy_Part_List\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "221":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\221-xxxxx_Internal_Harness\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "222":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\222-xxxxx_Extnl_Harness_Cable\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "230":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\230-xxxxx_Modified_Altered_Items\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "231":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\231-xxxxx_Shipping_Final_Assy\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    case "233":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\233-xxxxx_Final_Comp_Assy\" + ProductNumber.Substring(0, 9));
                        break;
                    }

                    default:
                    {
                        return new Product(ProductNumber, DocumentList);
                    }
                }
                // get all pdfs matching part number
                var files = dir.EnumerateFiles("*" + ProductNumber + "*" + ".pdf", SearchOption.AllDirectories);
                if (files.Any() == false)
                    files = dir.EnumerateFiles(ProductNumber.Substring(0, 9) + "-XX" + "*" + ".pdf", SearchOption.AllDirectories);
                if (files.Any() == false)
                    files = dir.EnumerateFiles(ProductNumber.Substring(0, 9) + "-ALL" + "*" + ".pdf", SearchOption.AllDirectories);
                MessageBox.Show(files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault().ToString());
                DocumentList.Add(files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault().ToString());

                return new Product(ProductNumber, DocumentList);
            }
            catch
            {
                return new Product(null, DocumentList);
            }            
        }

        private void TPOpenButton_Click(object sender, EventArgs e)
        {
            if (TPResultsListBox.Items.Count == 0 || TPResultsListBox.SelectedItem == null)
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
            if (TPBWorker.IsBusy) return;

            TPResultsListBox.Items.Clear();
            Regex rx = new Regex("[^a-zA-Z0-9]");
            var Serial = TPSerialEntryComboBox.Text;
            Serial = rx.Replace(TPSerialEntryComboBox.Text, "");
            
            if (Serial.Length < 3)
            {
                Logger.Log($"Invalid serial number entered.", rt);
                return;
            }

            if (!TPSerialEntryComboBox.Items.Contains(TPSerialEntryComboBox.Text))
            {
                TPSerialEntryComboBox.Items.Add(TPSerialEntryComboBox.Text);
            }

            TPSerialEntryComboBox.SelectAll();
            this.Enabled = false;
            SearchingForm.Show();
            TPBWorker.RunWorkerAsync(TPSerialEntryComboBox.Text);
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
                    return;
                }
            }

            if (tabControl.SelectedTab.Text == "Process Sheets")
            {
                if (PSResultsListBox.Focused || e.KeyChar == (char)Keys.Enter)
                {
                    PSSearchButton.PerformClick();
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == '[')
                {
                    PSSerialEntryComboBox.SelectAll();
                    e.KeyChar = (char)Keys.None;
                }
                if (e.KeyChar == ']')
                {
                    e.KeyChar = (char)Keys.None;
                    PSSearchButton.PerformClick();
                    return;
                }
                if (!PSSerialEntryComboBox.Focused)
                {
                    PSSerialEntryComboBox.Focus();
                    PSSerialEntryComboBox.Text = e.KeyChar.ToString();
                    PSSerialEntryComboBox.SelectionStart = PSSerialEntryComboBox.Text.Length;
                    e.Handled = true;
                    return;
                }
            }

            if (tabControl.SelectedTab.Text == "Product Code Lookup")
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    LookupSearchButton.PerformClick();
                    e.Handled = true;
                    return;
                }
                if (!LookupComboBox.Focused)
                {
                    LookupComboBox.Focus();
                    LookupComboBox.Text = e.KeyChar.ToString();
                    LookupComboBox.SelectionStart = LookupComboBox.Text.Length;
                    e.Handled = true;
                    return;
                }
            }
        }

        private void TPBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var product = FindProcedures((string)e.Argument);
            e.Result = product;
        }

        private void TPBWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            
            SearchingForm.Hide();
            var product = (Product)e.Result;
            string ProductNumber = product.ProductNumber;
            List<string> ProceduresList = product.DocumentList;
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

        private void PSBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var product = FindProcessSheets((string)e.Argument);
            e.Result = product;
        }

        private void PSBWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;

            SearchingForm.Hide();
            var product = (Product)e.Result;
            string ProductNumber = product.ProductNumber;
            List<string> ProceduresList = product.DocumentList;
            PSFileNameTextbox.Text = ProductNumber;

            if (!ProceduresList.Any())
            {
                Logger.Log($"No procedures found for {ProductNumber}", rt);
                return;
            }

            foreach (var p in ProceduresList)
            {
                var f = new FileInfo(p);
                PSResultsListBox.Items.Add(f);
            }
        }

        private void TPSerialEntryComboBox_TextChanged(object sender, EventArgs e)
        {
            TPSerialEntryComboBox.Text = TPSerialEntryComboBox.Text.ToUpper();
            TPSerialEntryComboBox.SelectionStart = TPSerialEntryComboBox.Text.Length;
        }

        private void PSSerialEntryComboBox_TextChanged(object sender, EventArgs e)
        {
            PSSerialEntryComboBox.Text = PSSerialEntryComboBox.Text.ToUpper();
            PSSerialEntryComboBox.SelectionStart = PSSerialEntryComboBox.Text.Length;
        }

        private void LookupSearchButton_Click(object sender, EventArgs e)
        {
            if (LookupComboBox.Text == "") return;
            if (!LookupComboBox.Items.Contains(LookupComboBox.Text)) LookupComboBox.Items.Add(LookupComboBox.Text);

            this.Enabled = false;
            LookupCodeTextbox.Clear();

            var IID = GetIIDFromProduct(LookupComboBox.Text);
            if (IID == "")
            {
                Logger.Log($"Product code not found for assembly: {LookupComboBox.Text}", rt);
                this.Enabled = true;
                return;
            }
            Logger.Log($"Product code for assembly {LookupComboBox.Text} is: {IID}", rt);
            LookupCodeTextbox.Text = IID;
            this.Enabled = true;
        }

        private void SearchTPFromLookupButton_Click(object sender, EventArgs e)
        {
            if (LookupCodeTextbox.Text == "") return;
            tabControl.SelectTab(0);
            TPSerialEntryComboBox.Text = LookupCodeTextbox.Text;
            TPSearchButton.PerformClick();
        }

        private void SearchPSFromLookupButton_Click(object sender, EventArgs e)
        {
            if (LookupCodeTextbox.Text == "") return;
            tabControl.SelectTab(1);
            PSSerialEntryComboBox.Text = LookupCodeTextbox.Text;
            PSSearchButton.PerformClick();
        }

        private void PSSearchButton_Click(object sender, EventArgs e)
        {
            if (PSBWorker.IsBusy) return;

            PSResultsListBox.Items.Clear();
            Regex rx = new Regex("[^a-zA-Z0-9]");
            var Serial = PSSerialEntryComboBox.Text;
            Serial = rx.Replace(PSSerialEntryComboBox.Text, "");

            if (Serial.Length < 3)
            {
                Logger.Log($"Invalid serial number entered.", rt);
                return;
            }

            if (!PSSerialEntryComboBox.Items.Contains(PSSerialEntryComboBox.Text))
            {
                TPSerialEntryComboBox.Items.Add(PSSerialEntryComboBox.Text);
            }

            PSSerialEntryComboBox.SelectAll();
            this.Enabled = false;
            SearchingForm.Show();
            PSBWorker.RunWorkerAsync(PSSerialEntryComboBox.Text);            
        }

        private void PSOpenButton_Click(object sender, EventArgs e)
        {
            if (PSResultsListBox.Items.Count == 0 || PSResultsListBox.SelectedItem == null)
            {
                Logger.Log("No process sheet selected, enter a serial number and search.", rt, Color.Red);
                return;
            }

            var f = new FileInfo(PSResultsListBox.SelectedItem.ToString().ToLower());
            MessageBox.Show(PSResultsListBox.SelectedItem.ToString());
            OpenFile(PSResultsListBox.SelectedItem.ToString());
        }
    }
}
