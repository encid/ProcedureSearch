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

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            var SearchingProgressForm = new SearchingProgressForm();
            SearchingProgressForm.Hide();

            Logger.Log("Program loaded, ready to search.", rt);

            TPSerialEntryComboBox.Select();
        }

        private void OpenFile(string FileName)
        {
            try
            {
                Logger.Log($"Opening test procedure {FileName}", rt);
                var fi = (FileInfo)TPResultsListBox.SelectedItem;
                Process.Start(fi.FullName);
            }
            catch
            {

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
                Logger.Log("An error occured while accessing the IID database: " + ex, rt, Color.Red);
                return RetVal;
            }
        }

        private List<string> findProcedures(string SerialNumber)
        {
            List<string> ProcedureList = new List<string>();
            bool gotFile = false;
            Regex rx = new Regex("[^a-zA-Z0-9]");
            try
            {
                // clear textboxes
                //lstProc.Items.Clear();
                //txtProc.Clear();
                //txtProd.Clear();
                //txtDate.Clear();
                // trim non-alphanumeric characters
                var Serial = rx.Replace(SerialNumber, "");
                // On Error Resume Next
                // check length of barcode to determine validity
                //if ((serial == ""))
                //{
                //    writeLog("Please scan or enter a serial number");
                //    cboSerial.Focus();
                //    cboSerial.SelectAll();
                //    return;
                //}

                //if (((serial.Length == 1)
                //            || (serial.Length == 2)))
                //{
                //    writeLog(("Invalid serial number: " + serialNumber));
                //    cboSerial.Focus();
                //    cboSerial.SelectAll();
                //    return;
                //}

                // get IID from serial numbers
                var IID = Serial.Substring(0, 3);
                // Using the IID from serial number, find corresponding product number
                var ProductNumber = GetProductFromIID(IID);
                // get first 3 digits of product number to determine if final assy or sub assy
                var ProdFirst3 = ProductNumber.Substring(0, 3);
                // remove first 4 characters of product number to get assembly number
                var AssemblyNumber = ProductNumber.Remove(0, 4);
                // find procedure based on first 3 digits
                switch (ProdFirst3)
                {
                    case "231":
                    foreach (string currentFile in Directory.EnumerateFiles(@"\pandora\vault\Released_Part_Information\234-xxxxx_Assy_Test_Proc\Standard_Products\", ("*"
                                    + (AssemblyNumber + "*")), System.IO.SearchOption.AllDirectories))
                    {
                        FileInfo f = new FileInfo(currentFile);
                        // check for duplicate files in listbox, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (((lstProc.FindStringExact(Path.GetFileName(f.FullName)) == -1)
                                    && (f.FullName.ToLower().Contains("archive") == false)))
                        {
                            lstProc.Items.Add(f);
                        }

                        if ((lstProc.Items.Count > 0))
                        {
                            gotFile = true;
                        }

                    }

                    if ((gotFile == true))
                    {
                        writeLog(("Found procedure for ["+ (IID + ("] product number " + prod))));
                        txtProd.Text = prod;
                        cboSerial.SelectAll();
                        lstProc.SelectedIndex = 0;
                        lstProc.Focus();
                    }
                    else if ((gotFile == false))
                    {
                        foreach (string currentFile in Directory.EnumerateFiles("\\\\ares\\shared\\Operations\\Test Engineering\\Documents ready for release", ("*"
                                        + (AssemblyNumber + "*")), SearchOption.TopDirectoryOnly))
                        {
                            FileInfo f = new FileInfo(currentFile);
                            // check for duplicate files in listbox, and if no duplicates exist, add the file
                            if ((lstProc.FindStringExact(Path.GetFileName(f.FullName)) == -1))
                            {
                                lstProc.Items.Add(f);
                            }

                            gotFile = true;
                        }

                        if ((gotFile == true))
                        {
                            WriteLog(string.Format("Found non-released procedure for [{0}] product number {1}", IID, prod));
                            txtProd.Text = prod;
                            cboSerial.SelectAll();
                            lstProc.SelectedIndex = 0;
                            lstProc.Focus();
                        }
                        else if ((gotFile == false))
                        {
                            writeLog(("Procedure not found for ["
                                            + (IID + ("] product number " + prod))));
                            cboSerial.Focus();
                            cboSerial.SelectAll();
                        }

                    }

                    break;
                    case "216":
                    foreach (string currentFile in Directory.EnumerateFiles("\\\\pandora\\vault\\Released_Part_Information\\225-xxxxx_Proc_Mfg_Test", ("*"
                                    + (AssemblyNumber + "*")), SearchOption.AllDirectories))
                    {
                        FileInfo f = new FileInfo(currentFile);
                        // check for duplicate files in listbox, and if no duplicates exist, add the file
                        // Also, check for archive, and do NOT add archived files to list.
                        if (((lstProc.FindStringExact(Path.GetFileName(f.FullName)) == -1)
                                    && (f.FullName.ToLower().Contains("archive") == false)))
                        {
                            lstProc.Items.Add(f);
                        }

                        if ((lstProc.Items.Count > 0))
                        {
                            gotFile = true;
                        }

                    }

                    if ((gotFile == true))
                    {
                        writeLog(("Found procedure for ["
                                        + (IID + ("] product number " + prod))));
                        txtProd.Text = prod;
                        cboSerial.SelectAll();
                        lstProc.SelectedIndex = 0;
                        lstProc.Focus();
                    }
                    else if ((gotFile == false))
                    {
                        foreach (string currentFile in Directory.EnumerateFiles("\\\\ares\\shared\\Operations\\Test Engineering\\Documents ready for release", ("*"
                                        + (AssemblyNumber + "*")), SearchOption.TopDirectoryOnly))
                        {
                            FileInfo f = new FileInfo(currentFile);
                            // check for duplicate files in listbox, and if no duplicates, add the file
                            if ((lstProc.FindStringExact(Path.GetFileName(f.FullName)) == -1))
                            {
                                lstProc.Items.Add(f);
                            }

                            gotFile = true;
                        }

                        if ((gotFile == true))
                        {
                            writeLog(("Found non-released procedure for ["
                                            + (IID + ("] product number " + prod))));
                            txtProd.Text = prod;
                            cboSerial.SelectAll();
                            lstProc.SelectedIndex = 0;
                            lstProc.Focus();
                        }
                        else if ((gotFile == false))
                        {
                            writeLog(("Procedure not found for ["
                                            + (IID + ("] product number " + prod))));
                            cboSerial.Focus();
                            cboSerial.SelectAll();
                        }
                        System.Data.OleDb.OleDbEnumerator.
                    }
                    break;
                    default:
                    writeLog(("Procedure not found for ["
                                    + (IID + ("] product number " + prod))));
                    cboSerial.Focus();
                    cboSerial.SelectAll();
                    return;
                    Errorcatch:
                    writeLog("An error has occured");
                    cboSerial.Focus();
                    cboSerial.SelectAll();
                    break;
                }
            }
            catch (Exception ex)
            {
                return;
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

        private void TPResultsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && TPResultsListBox.Items.Count > 0)
            {
                TPOpenButton_Click(sender, e);
            }
        }

        private void TPSearchButton_Click(object sender, EventArgs e)
        {
            string Provider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";            
            var ConnString = (Provider + @"S:\Operations\Test Engineering\FASTSeek\ProductCodesMaster.mdb");
            var DbConnection = new OleDbConnection { ConnectionString = ConnString };
            DbConnection.Open();
            var SqlStr = $@"SELECT * FROM ProductCode WHERE (ProductID = '{TPSerialEntryComboBox.Text}')";
            var DbCmd = new OleDbCommand(SqlStr, DbConnection);
            var DataReader = DbCmd.ExecuteReader();
            while (DataReader.Read())
            {
                if (DataReader.GetValue(1).ToString() != "")
                    {
                    // if procedure exists, assign product number to string prod
                    TPResultsListBox.Items.Add(DataReader.GetValue(1).ToString());
                }
            }
        }
    }
}
