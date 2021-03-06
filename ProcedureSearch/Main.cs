﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProcedureSearch
{
    public partial class Main : Form
    {
        public string VAULT_PATH = ConfigurationManager.AppSettings["VAULT_PATH"];
        public string IID_DATABASE_PATH = ConfigurationManager.AppSettings["IID_DATABASE_PATH"];
        public string LOGFILE_PATH = ConfigurationManager.AppSettings["LOGFILE_PATH"];
        SearchingProgressForm SearchingForm = new SearchingProgressForm();
        BackgroundWorker TPBWorker;
        BackgroundWorker PSBWorker;

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
            // initialize backgroundworkers
            TPBWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true            
            };
            TPBWorker.DoWork += TPBWorker_DoWork;
            TPBWorker.RunWorkerCompleted += TPBWorker_RunWorkerCompleted;

            PSBWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            PSBWorker.DoWork += PSBWorker_DoWork;
            PSBWorker.RunWorkerCompleted += PSBWorker_RunWorkerCompleted;

            rt.TextChanged += (sender, e) => {
                if (rt.Visible)
                    rt.ScrollToCaret();
            };
            this.KeyPreview = true;

            this.Icon = Properties.Resources.icon;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //set default config values if paths are null or empty (config file didn't load)
            if (string.IsNullOrEmpty(VAULT_PATH))
            {
                VAULT_PATH = @"\\pandora\vault";
            }
            if (string.IsNullOrEmpty(IID_DATABASE_PATH))
            {
                IID_DATABASE_PATH = @"\\ares\shared\Operations\Test Engineering\Test Softwares\ProcedureSearch\ProductCodesMaster.mdb";
            }
            if (string.IsNullOrEmpty(LOGFILE_PATH))
            {
                LOGFILE_PATH = @"\\ares\shared\Operations\Test Engineering\Test Softwares\ProcedureSearch\log.txt";
            }

            SearchingForm.Hide();

            Logger.Log("Program loaded, ready to search.", rt, true);

            TPSerialEntryComboBox.Select();
        }

        /// <summary>
        /// Performs an action safely the appropriate thread.
        /// </summary>
        /// <param name="a">Action to perform.</param>
        private void ExecuteSecure(Action a)
        // Usage example: ExecuteSecure(() => this.someLabel.Text = "foo");
        {
            Invoke((MethodInvoker)delegate { a(); });
        }

        private void OpenFile(string FileName)
        {
            try
            {
                Logger.Log($"Opening document {FileName}", rt, true);
                Process.Start(FileName);
            }
            catch (Exception ex)
            {
                Logger.Log("Error: " + ex.Message, rt, Color.Red, true);
            }
        }

        private string GetProductFromIID(string IID)
        {
            var product = "";
            try
            {
                using (OleDbConnection conn = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={IID_DATABASE_PATH}"))
                {
                    conn.Open();
                    using (OleDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM ProductCode WHERE (ProductID = @iid)";
                        cmd.Parameters.AddWithValue("iid", IID);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read() && !reader.IsDBNull(1))
                        {
                            product = reader.GetString(1);
                        }
                    }
                    conn.Close(); 
                }
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured with IID database: " + ex.Message, rt, Color.Red, true));
                return product;
            }
            
            return product;
        }

        private List<string> GetProcedures(string input, bool IsSerial)
        {
            List<string> DocumentList = new List<string>();
            string ProductNumber = input;
            string dir;
            try
            {
                if (IsSerial)
                {
                    // Using the IID (first 3 chars) from serial number, find corresponding product number
                    ProductNumber = GetProductFromIID(input.Substring(0, 3));
                }

                if (ProductNumber == "")
                {
                    return DocumentList;
                }
                
                // get first 3 digits of product number to determine if final assy or sub assy
                var ProductPrefix = ProductNumber.Substring(0, 3);
                // remove first 4 characters of product number to get assembly number
                var AssemblyNumber = ProductNumber.Remove(0, 4);
                // find procedure based on first 3 digits
                switch (ProductPrefix)
                {
                    case "231":
                    {
                        dir = $@"{VAULT_PATH}\Released_Part_Information\234-xxxxx_Assy_Test_Proc\Standard_Products\234-{AssemblyNumber.Substring(0, 5)}";
                        break;
                    }

                    case "216":
                    {
                        dir = $@"{VAULT_PATH}\Released_Part_Information\225-xxxxx_Proc_Mfg_Test\225-{AssemblyNumber.Substring(0, 5)}";
                        break;
                    }

                    default:
                    {
                        return DocumentList;
                    }
                }

                if (!Directory.Exists(dir))
                {
                    return DocumentList;
                }
                
                var files = Directory.EnumerateFiles(dir, "*" + AssemblyNumber + "*", SearchOption.AllDirectories);
                // if no matches, check for -XX files
                if (!files.Any())
                    files = Directory.EnumerateFiles(dir, "*" + AssemblyNumber.Substring(0, 5) + "-XX" + "*", SearchOption.AllDirectories);
                //if no matches again, check in to-be-released test engineering folder
                if (!files.Any())
                {
                    dir = $@"\\ares\shared\Operations\Test Engineering\Documents ready for release";
                    files = Directory.EnumerateFiles(dir, "*" + AssemblyNumber + "*", SearchOption.AllDirectories);
                }

                foreach (var f in files)
                {
                    // check for duplicate files in list, and if no duplicates exist, add the file
                    // Also, check for archive, and do NOT add archived files to list.
                    if (!DocumentList.Contains(f) && !(f.ToLower().Contains("archive")))
                    {
                        DocumentList.Add(f);
                    }
                }

                return DocumentList;
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured: " + ex.Message, rt, Color.Red, true));
                return DocumentList;
            }
        }

        private List<string> GetProcessSheets(string input, bool IsSerial)
        {
            List<string> DocumentList = new List<string>();
            string ProductNumber = input;
            string dir;
            string ProductPrefix = "";
            try
            {
                if (IsSerial)
                {
                    // Using the IID (first 3 chars) from serial number, find corresponding product number 
                    ProductNumber = GetProductFromIID(input.Substring(0, 3));
                }
                
                if (ProductNumber == "")
                {
                    return DocumentList;
                }

                // get first 2 or 3 digits of product number to determine what type of part
                // 2 digits for G- and E- series, 3 digits for all others
                if (ProductNumber.StartsWith("G", StringComparison.CurrentCulture) || 
                    ProductNumber.StartsWith("E", StringComparison.CurrentCulture) ||
                    ProductNumber.StartsWith("V", StringComparison.CurrentCulture))
                {
                    ProductPrefix = ProductNumber.Substring(0, 1);
                }
                else
                {
                    ProductPrefix = ProductNumber.Substring(0, 3);
                }
                
                // remove first 4 characters of product number to get assembly number
                //var AssemblyNumber = ProductNumber.Remove(0, 4);
                switch (ProductPrefix)
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
                    case "516":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\516-xxxxx_Prototype_PCB\" + ProductNumber.Substring(0, 9));
                        break;
                    }
                    case "531":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\531-xxxxx_Shipping_Final_Assy\" + ProductNumber.Substring(0, 9));
                        break;
                    }
                    case "533":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\533-Prototype_Final_Assy\" + ProductNumber.Substring(0, 9));
                        break;
                    }
                    case "G":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\G_SERIES\");
                        break;
                    }
                    case "E":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\E_SERIES\PROCESS_SHEETS\");
                        break;
                    }
                    case "V":
                    {
                        dir = ($@"{VAULT_PATH}\Operations_Documents\PROCESS SHEETS\V_SERIES\PROCESS_SHEETS\");
                        break;
                    }

                    default:
                    {
                        return DocumentList;
                    }
                }
                MessageBox.Show(dir);
                // make sure dir exists
                if (!Directory.Exists(dir))
                {
                    return DocumentList;
                }
                
                // get all pdfs matching part number
                var files = Directory.EnumerateFiles(dir, "*" + ProductNumber + "*" + ".pdf", SearchOption.AllDirectories);
                // if E series, force add the catch-all pdf process sheet for all E-series products to the files variable
                if (dir.Contains("E_SERIES"))
                {
                    files = Directory.EnumerateFiles(dir, "*" + "DIGITAL E5 AND E6" + "*" + ".pdf", SearchOption.AllDirectories);
                }
                if (!files.Any())
                    files = Directory.EnumerateFiles(dir, ProductNumber.Substring(0, 9) + "-XX" + "*" + ".pdf", SearchOption.AllDirectories);
                if (!files.Any())
                    files = Directory.EnumerateFiles(dir, ProductNumber.Substring(0, 9) + "-ALL" + "*" + ".pdf", SearchOption.AllDirectories);
                
                if (!files.Any())
                    return DocumentList;

                var fileinfo = new List<FileInfo>();
                foreach (var f in files)
                {
                    if (!f.ToLower().Contains("archive"))
                    {
                        fileinfo.Add(new FileInfo(f));
                    }
                }

                // only attempt to add files to DocumentList if the fileinfo list is populated, to avoid null exception
                if (fileinfo.Any())
                {
                    DocumentList.Add(fileinfo.OrderByDescending(f => f.LastWriteTime).FirstOrDefault().ToString());
                }

                return DocumentList;
            }
            catch (Exception ex)
            {
                ExecuteSecure(() => Logger.Log("An error has occured: "+ ex.TargetSite + " - " + ex.Message, rt, Color.Red, true));
                return DocumentList;
            }            
        }

        private void TPOpenButton_Click(object sender, EventArgs e)
        {
            if (TPResultsListBox.Items.Count == 0)
            {
                Logger.Log("No procedure selected, enter a serial number or product and search.", rt, Color.Red, true);
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

            TPFilenameTextbox.Clear();
            TPDateTextbox.Clear();
            TPResultsListBox.Items.Clear();
            Regex rx = new Regex("[^a-zA-Z0-9-]");
            TPSerialEntryComboBox.Text = rx.Replace(TPSerialEntryComboBox.Text, "");
            var Serial = TPSerialEntryComboBox.Text;

            if (Serial.Length < 3 || (Serial.Contains('-') && Serial.Length < 9))
            {
                Logger.Log($"Invalid serial number or product entered.", rt, true);
                return;
            }

            if (!TPSerialEntryComboBox.Items.Contains(TPSerialEntryComboBox.Text))
            {
                TPSerialEntryComboBox.Items.Insert(0, TPSerialEntryComboBox.Text);
            }

            TPSerialEntryComboBox.SelectAll();
            tabControl.Enabled = false;
            SearchingForm.Show();
            TPBWorker.RunWorkerAsync(Serial);
        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!tabControl.Enabled) return;

            if (tabControl.SelectedTab.Text == "Test Procedures")
            {
                if (e.KeyChar == '[')
                {
                    e.KeyChar = (char)Keys.None;
                    e.Handled = true;
                }
                if (TPSerialEntryComboBox.Focused && e.KeyChar == (char)Keys.Enter)
                {
                    TPSearchButton.PerformClick();
                    e.Handled = true;
                    return;
                }
                if (!TPSerialEntryComboBox.Focused && e.KeyChar != (char)Keys.Enter)
                {
                    TPSerialEntryComboBox.Focus();
                    TPSerialEntryComboBox.Text = e.KeyChar.ToString();
                    TPSerialEntryComboBox.SelectionStart = TPSerialEntryComboBox.Text.Length;
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == ']')
                {
                    e.KeyChar = (char)Keys.None;
                    TPSearchButton.PerformClick();
                    return;
                }                
            }

            if (tabControl.SelectedTab.Text == "Process Sheets")
            {
                if (e.KeyChar == '[')
                {
                    e.KeyChar = (char)Keys.None;
                    e.Handled = true;
                }
                if (PSSerialEntryComboBox.Focused && e.KeyChar == (char)Keys.Enter)
                {
                    PSSearchButton.PerformClick();
                    e.Handled = true;
                    return;
                }
                if (!PSSerialEntryComboBox.Focused && e.KeyChar != (char)Keys.Enter)
                {
                    PSSerialEntryComboBox.Focus();
                    PSSerialEntryComboBox.Text = e.KeyChar.ToString();
                    PSSerialEntryComboBox.SelectionStart = PSSerialEntryComboBox.Text.Length;
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == ']')
                {
                    e.KeyChar = (char)Keys.None;
                    PSSearchButton.PerformClick();
                    return;
                }                
            }            
        }

        private void TPBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> procedures;
            var input = (string)e.Argument;
            if (input.Contains("-"))
            {
                procedures = GetProcedures(input, false);
            }
            else
            {
                procedures = GetProcedures(input, true);
            }
            var product = new Product(input, procedures);
            e.Result = product;
        }

        private void TPBWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tabControl.Enabled = true;
            
            SearchingForm.Hide();
            var product = (Product)e.Result;
            string ProductNumber = product.ProductNumber;
            List<string> ProceduresList = product.DocumentList;
            TPFilenameTextbox.Text = ProductNumber;
            TPSerialEntryComboBox.Focus();

            if (!ProceduresList.Any())
            {
                Logger.Log($"No procedures found for {ProductNumber}", rt, true);
                return;
            }

            foreach (var p in ProceduresList)
            {
                var f = new FileInfo(p);
                TPResultsListBox.Items.Add(f);
            }

            TPResultsListBox.SelectedItem = TPResultsListBox.Items[0];
            Logger.Log($"Found procedures for {ProductNumber}", rt, true);
        }  

        private void PSBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> procedures;            
            var input = (string)e.Argument;
            // if input contains "-", or starts with G or E, it is not a serial number.
            if (input.Contains("-") || 
                input.StartsWith("G", StringComparison.CurrentCulture) || 
                input.StartsWith("E", StringComparison.CurrentCulture) ||
                input.StartsWith("V", StringComparison.CurrentCulture))
            {
                procedures = GetProcessSheets(input, false);
            }
            else
            {
                procedures = GetProcessSheets(input, true);
            }
            var product = new Product(input, procedures);
            e.Result = product;
        }

        private void PSBWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tabControl.Enabled = true;

            SearchingForm.Hide();
            var product = (Product)e.Result;
            string ProductNumber = product.ProductNumber;
            List<string> ProceduresList = product.DocumentList;
            PSFilenameTextbox.Text = ProductNumber;
            PSSerialEntryComboBox.Focus();

            if (!ProceduresList.Any())
            {
                Logger.Log($"No process sheets found for {ProductNumber}", rt, true);
                return;
            }

            foreach (var p in ProceduresList)
            {
                var f = new FileInfo(p);
                PSResultsListBox.Items.Add(f);
            }

            PSResultsListBox.SelectedItem = PSResultsListBox.Items[0];                        
            Logger.Log($"Found process sheet for {ProductNumber}", rt, true);
        }      
        
        private void TPSerialEntryComboBox_TextChanged(object sender, EventArgs e)
        {
            var currPos = TPSerialEntryComboBox.SelectionStart;
            TPSerialEntryComboBox.Text = TPSerialEntryComboBox.Text.ToUpper();
            TPSerialEntryComboBox.SelectionStart = currPos;
        }

        private void PSSerialEntryComboBox_TextChanged(object sender, EventArgs e)
        {
            var currPos = PSSerialEntryComboBox.SelectionStart;
            PSSerialEntryComboBox.Text = PSSerialEntryComboBox.Text.ToUpper();
            PSSerialEntryComboBox.SelectionStart = currPos;
        }

        private void PSSearchButton_Click(object sender, EventArgs e)
        {
            if (PSBWorker.IsBusy) return;

            PSFilenameTextbox.Clear();
            PSDateTextbox.Clear();
            PSResultsListBox.Items.Clear();
            Regex rx = new Regex("[^a-zA-Z0-9-]");
            PSSerialEntryComboBox.Text = rx.Replace(PSSerialEntryComboBox.Text, "");
            var Serial = PSSerialEntryComboBox.Text;

            if (Serial.Length < 3 || (Serial.Contains('-') && Serial.Length < 9))
            {
                Logger.Log($"Invalid serial number or product entered.", rt, true);
                return;
            }

            if (!PSSerialEntryComboBox.Items.Contains(PSSerialEntryComboBox.Text))
            {
                PSSerialEntryComboBox.Items.Insert(0, PSSerialEntryComboBox.Text);
            }

            PSSerialEntryComboBox.SelectAll();
            tabControl.Enabled = false;
            SearchingForm.Show();
            PSBWorker.RunWorkerAsync(Serial);            
        }

        private void PSOpenButton_Click(object sender, EventArgs e)
        {
            if (PSResultsListBox.Items.Count == 0)
            {
                Logger.Log("No process sheet selected, enter a serial number and search.", rt, Color.Red, true);
                return;
            }

            var f = new FileInfo(PSResultsListBox.SelectedItem.ToString().ToLower());
            OpenFile(PSResultsListBox.SelectedItem.ToString());
        }

        private void TPResultsListBox_SelectedValueChanged(object sender, EventArgs e)
        {   
            var f = new FileInfo(TPResultsListBox.SelectedItem.ToString());
            TPDateTextbox.Text = f.LastWriteTime.ToShortDateString();            
            TPFilenameTextbox.Text = f.Name.Substring(0, f.Name.IndexOf(" ", StringComparison.CurrentCulture));
        }

        private void PSResultsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var f = new FileInfo(PSResultsListBox.SelectedItem.ToString());
            PSDateTextbox.Text = f.LastWriteTime.ToShortDateString();            
            PSFilenameTextbox.Text = f.Name.Substring(0, f.Name.IndexOf(" ", StringComparison.CurrentCulture));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.Log("Program exited.", rt, true);
        }
    }
}