using System.Windows.Forms;
namespace ProcedureSearch
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rt = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TPListGroupBox = new System.Windows.Forms.GroupBox();
            this.TPArchivedLabel = new System.Windows.Forms.Label();
            this.TPOpenButton = new System.Windows.Forms.Button();
            this.TPDateTextbox = new System.Windows.Forms.TextBox();
            this.TPFilenameTextbox = new System.Windows.Forms.TextBox();
            this.TPDateLabel = new System.Windows.Forms.Label();
            this.TPFilenameLabel = new System.Windows.Forms.Label();
            this.TPResultsListBox = new System.Windows.Forms.ListBox();
            this.TPSearchGroupBox = new System.Windows.Forms.GroupBox();
            this.TPSearchButton = new System.Windows.Forms.Button();
            this.TPSerialEntryComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.bWorker = new System.ComponentModel.BackgroundWorker();
            this.tabPage3.SuspendLayout();
            this.TPListGroupBox.SuspendLayout();
            this.TPSearchGroupBox.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // rt
            // 
            this.rt.Location = new System.Drawing.Point(13, 415);
            this.rt.Name = "rt";
            this.rt.Size = new System.Drawing.Size(536, 138);
            this.rt.TabIndex = 6;
            this.rt.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TPListGroupBox);
            this.tabPage3.Controls.Add(this.TPSearchGroupBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(529, 296);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Test Procedures";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TPListGroupBox
            // 
            this.TPListGroupBox.Controls.Add(this.TPArchivedLabel);
            this.TPListGroupBox.Controls.Add(this.TPOpenButton);
            this.TPListGroupBox.Controls.Add(this.TPDateTextbox);
            this.TPListGroupBox.Controls.Add(this.TPFilenameTextbox);
            this.TPListGroupBox.Controls.Add(this.TPDateLabel);
            this.TPListGroupBox.Controls.Add(this.TPFilenameLabel);
            this.TPListGroupBox.Controls.Add(this.TPResultsListBox);
            this.TPListGroupBox.Location = new System.Drawing.Point(16, 83);
            this.TPListGroupBox.Name = "TPListGroupBox";
            this.TPListGroupBox.Size = new System.Drawing.Size(502, 203);
            this.TPListGroupBox.TabIndex = 1;
            this.TPListGroupBox.TabStop = false;
            this.TPListGroupBox.Text = "Test Procedure";
            // 
            // TPArchivedLabel
            // 
            this.TPArchivedLabel.AutoSize = true;
            this.TPArchivedLabel.Location = new System.Drawing.Point(358, 144);
            this.TPArchivedLabel.Name = "TPArchivedLabel";
            this.TPArchivedLabel.Size = new System.Drawing.Size(52, 13);
            this.TPArchivedLabel.TabIndex = 6;
            this.TPArchivedLabel.Text = "Archived!";
            this.TPArchivedLabel.Visible = false;
            // 
            // TPOpenButton
            // 
            this.TPOpenButton.Location = new System.Drawing.Point(347, 169);
            this.TPOpenButton.Name = "TPOpenButton";
            this.TPOpenButton.Size = new System.Drawing.Size(75, 23);
            this.TPOpenButton.TabIndex = 5;
            this.TPOpenButton.Text = "button2";
            this.TPOpenButton.UseVisualStyleBackColor = true;
            this.TPOpenButton.Click += new System.EventHandler(this.TPOpenButton_Click);
            // 
            // TPDateTextbox
            // 
            this.TPDateTextbox.Location = new System.Drawing.Point(111, 171);
            this.TPDateTextbox.Name = "TPDateTextbox";
            this.TPDateTextbox.Size = new System.Drawing.Size(154, 20);
            this.TPDateTextbox.TabIndex = 4;
            // 
            // TPFilenameTextbox
            // 
            this.TPFilenameTextbox.Location = new System.Drawing.Point(111, 137);
            this.TPFilenameTextbox.Name = "TPFilenameTextbox";
            this.TPFilenameTextbox.Size = new System.Drawing.Size(154, 20);
            this.TPFilenameTextbox.TabIndex = 3;
            // 
            // TPDateLabel
            // 
            this.TPDateLabel.AutoSize = true;
            this.TPDateLabel.Location = new System.Drawing.Point(53, 174);
            this.TPDateLabel.Name = "TPDateLabel";
            this.TPDateLabel.Size = new System.Drawing.Size(33, 13);
            this.TPDateLabel.TabIndex = 2;
            this.TPDateLabel.Text = "Date:";
            // 
            // TPFilenameLabel
            // 
            this.TPFilenameLabel.AutoSize = true;
            this.TPFilenameLabel.Location = new System.Drawing.Point(53, 140);
            this.TPFilenameLabel.Name = "TPFilenameLabel";
            this.TPFilenameLabel.Size = new System.Drawing.Size(47, 13);
            this.TPFilenameLabel.TabIndex = 1;
            this.TPFilenameLabel.Text = "Product:";
            // 
            // TPResultsListBox
            // 
            this.TPResultsListBox.DisplayMember = "Name";
            this.TPResultsListBox.FormattingEnabled = true;
            this.TPResultsListBox.Location = new System.Drawing.Point(15, 19);
            this.TPResultsListBox.Name = "TPResultsListBox";
            this.TPResultsListBox.Size = new System.Drawing.Size(470, 108);
            this.TPResultsListBox.TabIndex = 0;
            this.TPResultsListBox.ValueMember = "FullName";
            this.TPResultsListBox.DoubleClick += new System.EventHandler(this.TPOpenButton_Click);
            // 
            // TPSearchGroupBox
            // 
            this.TPSearchGroupBox.Controls.Add(this.TPSearchButton);
            this.TPSearchGroupBox.Controls.Add(this.TPSerialEntryComboBox);
            this.TPSearchGroupBox.Location = new System.Drawing.Point(16, 19);
            this.TPSearchGroupBox.Name = "TPSearchGroupBox";
            this.TPSearchGroupBox.Size = new System.Drawing.Size(502, 58);
            this.TPSearchGroupBox.TabIndex = 1;
            this.TPSearchGroupBox.TabStop = false;
            this.TPSearchGroupBox.Text = "Serial Number, Product Code, or Assembly";
            // 
            // TPSearchButton
            // 
            this.TPSearchButton.Location = new System.Drawing.Point(347, 19);
            this.TPSearchButton.Name = "TPSearchButton";
            this.TPSearchButton.Size = new System.Drawing.Size(75, 23);
            this.TPSearchButton.TabIndex = 1;
            this.TPSearchButton.Text = "button1";
            this.TPSearchButton.UseVisualStyleBackColor = true;
            this.TPSearchButton.Click += new System.EventHandler(this.TPSearchButton_Click);
            // 
            // TPSerialEntryComboBox
            // 
            this.TPSerialEntryComboBox.FormattingEnabled = true;
            this.TPSerialEntryComboBox.Location = new System.Drawing.Point(15, 19);
            this.TPSerialEntryComboBox.Name = "TPSerialEntryComboBox";
            this.TPSerialEntryComboBox.Size = new System.Drawing.Size(279, 21);
            this.TPSerialEntryComboBox.TabIndex = 0;
            this.TPSerialEntryComboBox.TextChanged += new System.EventHandler(this.TPSerialEntryComboBox_TextChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Location = new System.Drawing.Point(12, 91);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(537, 322);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(529, 296);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Process Sheets";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // bWorker
            // 
            this.bWorker.WorkerReportsProgress = true;
            this.bWorker.WorkerSupportsCancellation = true;
            this.bWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorker_DoWork);
            this.bWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bWorker_RunWorkerCompleted);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 568);
            this.Controls.Add(this.rt);
            this.Controls.Add(this.tabControl);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Procedure Search";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Main_KeyPress);
            this.tabPage3.ResumeLayout(false);
            this.TPListGroupBox.ResumeLayout(false);
            this.TPListGroupBox.PerformLayout();
            this.TPSearchGroupBox.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RichTextBox rt;
        private TabPage tabPage3;
        private GroupBox TPListGroupBox;
        private Button TPOpenButton;
        private TextBox TPDateTextbox;
        private TextBox TPFilenameTextbox;
        private Label TPDateLabel;
        private Label TPFilenameLabel;
        private ListBox TPResultsListBox;
        private GroupBox TPSearchGroupBox;
        private Button TPSearchButton;
        private ComboBox TPSerialEntryComboBox;
        private TabControl tabControl;
        private TabPage tabPage1;
        private Label TPArchivedLabel;
        private System.ComponentModel.BackgroundWorker bWorker;
    }
}


