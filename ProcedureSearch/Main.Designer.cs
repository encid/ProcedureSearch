﻿using System.Windows.Forms;
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
            this.TPArchivedLabel = new System.Windows.Forms.Label();
            this.TPListGroupBox = new System.Windows.Forms.GroupBox();
            this.TPResultsListBox = new System.Windows.Forms.ListBox();
            this.TPSearchGroupBox = new System.Windows.Forms.GroupBox();
            this.TPSearchButton = new System.Windows.Forms.Button();
            this.TPSerialEntryComboBox = new System.Windows.Forms.ComboBox();
            this.TPOpenButton = new System.Windows.Forms.Button();
            this.TPFilenameLabel = new System.Windows.Forms.Label();
            this.TPDateLabel = new System.Windows.Forms.Label();
            this.TPDateTextbox = new System.Windows.Forms.TextBox();
            this.TPFilenameTextbox = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PSOpenButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PSDateTextbox = new System.Windows.Forms.TextBox();
            this.PSFileNameTextbox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PSResultsListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PSSearchButton = new System.Windows.Forms.Button();
            this.PSSerialEntryComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage3.SuspendLayout();
            this.TPListGroupBox.SuspendLayout();
            this.TPSearchGroupBox.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rt
            // 
            this.rt.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rt.Location = new System.Drawing.Point(13, 415);
            this.rt.Name = "rt";
            this.rt.Size = new System.Drawing.Size(536, 138);
            this.rt.TabIndex = 6;
            this.rt.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TPArchivedLabel);
            this.tabPage3.Controls.Add(this.TPListGroupBox);
            this.tabPage3.Controls.Add(this.TPSearchGroupBox);
            this.tabPage3.Controls.Add(this.TPOpenButton);
            this.tabPage3.Controls.Add(this.TPFilenameLabel);
            this.tabPage3.Controls.Add(this.TPDateLabel);
            this.tabPage3.Controls.Add(this.TPDateTextbox);
            this.tabPage3.Controls.Add(this.TPFilenameTextbox);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(529, 289);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Test Procedures";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TPArchivedLabel
            // 
            this.TPArchivedLabel.AutoSize = true;
            this.TPArchivedLabel.Location = new System.Drawing.Point(371, 228);
            this.TPArchivedLabel.Name = "TPArchivedLabel";
            this.TPArchivedLabel.Size = new System.Drawing.Size(61, 16);
            this.TPArchivedLabel.TabIndex = 6;
            this.TPArchivedLabel.Text = "Archived!";
            this.TPArchivedLabel.Visible = false;
            // 
            // TPListGroupBox
            // 
            this.TPListGroupBox.Controls.Add(this.TPResultsListBox);
            this.TPListGroupBox.Location = new System.Drawing.Point(13, 76);
            this.TPListGroupBox.Name = "TPListGroupBox";
            this.TPListGroupBox.Size = new System.Drawing.Size(502, 143);
            this.TPListGroupBox.TabIndex = 1;
            this.TPListGroupBox.TabStop = false;
            this.TPListGroupBox.Text = "Test Procedure";
            // 
            // TPResultsListBox
            // 
            this.TPResultsListBox.DisplayMember = "Name";
            this.TPResultsListBox.FormattingEnabled = true;
            this.TPResultsListBox.ItemHeight = 16;
            this.TPResultsListBox.Location = new System.Drawing.Point(15, 23);
            this.TPResultsListBox.Name = "TPResultsListBox";
            this.TPResultsListBox.Size = new System.Drawing.Size(470, 100);
            this.TPResultsListBox.TabIndex = 0;
            this.TPResultsListBox.ValueMember = "FullName";
            this.TPResultsListBox.SelectedValueChanged += new System.EventHandler(this.TPResultsListBox_SelectedValueChanged);
            this.TPResultsListBox.DoubleClick += new System.EventHandler(this.TPOpenButton_Click);
            // 
            // TPSearchGroupBox
            // 
            this.TPSearchGroupBox.Controls.Add(this.TPSearchButton);
            this.TPSearchGroupBox.Controls.Add(this.TPSerialEntryComboBox);
            this.TPSearchGroupBox.Location = new System.Drawing.Point(13, 12);
            this.TPSearchGroupBox.Name = "TPSearchGroupBox";
            this.TPSearchGroupBox.Size = new System.Drawing.Size(502, 58);
            this.TPSearchGroupBox.TabIndex = 1;
            this.TPSearchGroupBox.TabStop = false;
            this.TPSearchGroupBox.Text = "Scan Serial Number or Enter Part Number";
            // 
            // TPSearchButton
            // 
            this.TPSearchButton.Location = new System.Drawing.Point(347, 19);
            this.TPSearchButton.Name = "TPSearchButton";
            this.TPSearchButton.Size = new System.Drawing.Size(84, 27);
            this.TPSearchButton.TabIndex = 1;
            this.TPSearchButton.Text = "Search";
            this.TPSearchButton.UseVisualStyleBackColor = true;
            this.TPSearchButton.Click += new System.EventHandler(this.TPSearchButton_Click);
            // 
            // TPSerialEntryComboBox
            // 
            this.TPSerialEntryComboBox.FormattingEnabled = true;
            this.TPSerialEntryComboBox.Location = new System.Drawing.Point(15, 24);
            this.TPSerialEntryComboBox.Name = "TPSerialEntryComboBox";
            this.TPSerialEntryComboBox.Size = new System.Drawing.Size(279, 24);
            this.TPSerialEntryComboBox.TabIndex = 0;
            this.TPSerialEntryComboBox.TextChanged += new System.EventHandler(this.TPSerialEntryComboBox_TextChanged);
            // 
            // TPOpenButton
            // 
            this.TPOpenButton.Location = new System.Drawing.Point(360, 253);
            this.TPOpenButton.Name = "TPOpenButton";
            this.TPOpenButton.Size = new System.Drawing.Size(84, 27);
            this.TPOpenButton.TabIndex = 5;
            this.TPOpenButton.Text = "Open";
            this.TPOpenButton.UseVisualStyleBackColor = true;
            this.TPOpenButton.Click += new System.EventHandler(this.TPOpenButton_Click);
            // 
            // TPFilenameLabel
            // 
            this.TPFilenameLabel.AutoSize = true;
            this.TPFilenameLabel.Location = new System.Drawing.Point(66, 228);
            this.TPFilenameLabel.Name = "TPFilenameLabel";
            this.TPFilenameLabel.Size = new System.Drawing.Size(56, 16);
            this.TPFilenameLabel.TabIndex = 1;
            this.TPFilenameLabel.Text = "Product:";
            // 
            // TPDateLabel
            // 
            this.TPDateLabel.AutoSize = true;
            this.TPDateLabel.Location = new System.Drawing.Point(66, 258);
            this.TPDateLabel.Name = "TPDateLabel";
            this.TPDateLabel.Size = new System.Drawing.Size(39, 16);
            this.TPDateLabel.TabIndex = 2;
            this.TPDateLabel.Text = "Date:";
            // 
            // TPDateTextbox
            // 
            this.TPDateTextbox.Location = new System.Drawing.Point(124, 255);
            this.TPDateTextbox.Name = "TPDateTextbox";
            this.TPDateTextbox.ReadOnly = true;
            this.TPDateTextbox.Size = new System.Drawing.Size(154, 23);
            this.TPDateTextbox.TabIndex = 4;
            // 
            // TPFilenameTextbox
            // 
            this.TPFilenameTextbox.Location = new System.Drawing.Point(124, 225);
            this.TPFilenameTextbox.Name = "TPFilenameTextbox";
            this.TPFilenameTextbox.ReadOnly = true;
            this.TPFilenameTextbox.Size = new System.Drawing.Size(154, 23);
            this.TPFilenameTextbox.TabIndex = 3;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(12, 91);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(537, 318);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.PSOpenButton);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.PSDateTextbox);
            this.tabPage1.Controls.Add(this.PSFileNameTextbox);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(529, 289);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Process Sheets";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(371, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Archived!";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Product:";
            // 
            // PSOpenButton
            // 
            this.PSOpenButton.Location = new System.Drawing.Point(360, 253);
            this.PSOpenButton.Name = "PSOpenButton";
            this.PSOpenButton.Size = new System.Drawing.Size(84, 27);
            this.PSOpenButton.TabIndex = 11;
            this.PSOpenButton.Text = "Open";
            this.PSOpenButton.UseVisualStyleBackColor = true;
            this.PSOpenButton.Click += new System.EventHandler(this.PSOpenButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Date:";
            // 
            // PSDateTextbox
            // 
            this.PSDateTextbox.Location = new System.Drawing.Point(124, 255);
            this.PSDateTextbox.Name = "PSDateTextbox";
            this.PSDateTextbox.ReadOnly = true;
            this.PSDateTextbox.Size = new System.Drawing.Size(154, 23);
            this.PSDateTextbox.TabIndex = 10;
            // 
            // PSFileNameTextbox
            // 
            this.PSFileNameTextbox.Location = new System.Drawing.Point(124, 225);
            this.PSFileNameTextbox.Name = "PSFileNameTextbox";
            this.PSFileNameTextbox.ReadOnly = true;
            this.PSFileNameTextbox.Size = new System.Drawing.Size(154, 23);
            this.PSFileNameTextbox.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PSResultsListBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 143);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Sheet";
            // 
            // PSResultsListBox
            // 
            this.PSResultsListBox.DisplayMember = "Name";
            this.PSResultsListBox.FormattingEnabled = true;
            this.PSResultsListBox.ItemHeight = 16;
            this.PSResultsListBox.Location = new System.Drawing.Point(15, 24);
            this.PSResultsListBox.Name = "PSResultsListBox";
            this.PSResultsListBox.Size = new System.Drawing.Size(470, 100);
            this.PSResultsListBox.TabIndex = 0;
            this.PSResultsListBox.ValueMember = "FullName";
            this.PSResultsListBox.SelectedValueChanged += new System.EventHandler(this.PSResultsListBox_SelectedValueChanged);
            this.PSResultsListBox.DoubleClick += new System.EventHandler(this.PSOpenButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PSSearchButton);
            this.groupBox2.Controls.Add(this.PSSerialEntryComboBox);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 58);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scan Serial Number or Enter Part Number";
            // 
            // PSSearchButton
            // 
            this.PSSearchButton.Location = new System.Drawing.Point(347, 19);
            this.PSSearchButton.Name = "PSSearchButton";
            this.PSSearchButton.Size = new System.Drawing.Size(84, 27);
            this.PSSearchButton.TabIndex = 1;
            this.PSSearchButton.Text = "Search";
            this.PSSearchButton.UseVisualStyleBackColor = true;
            this.PSSearchButton.Click += new System.EventHandler(this.PSSearchButton_Click);
            // 
            // PSSerialEntryComboBox
            // 
            this.PSSerialEntryComboBox.FormattingEnabled = true;
            this.PSSerialEntryComboBox.Location = new System.Drawing.Point(15, 24);
            this.PSSerialEntryComboBox.Name = "PSSerialEntryComboBox";
            this.PSSerialEntryComboBox.Size = new System.Drawing.Size(279, 24);
            this.PSSerialEntryComboBox.TabIndex = 0;
            this.PSSerialEntryComboBox.TextChanged += new System.EventHandler(this.PSSerialEntryComboBox_TextChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(556, 568);
            this.Controls.Add(this.rt);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Procedure Search";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Main_KeyPress);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.TPListGroupBox.ResumeLayout(false);
            this.TPSearchGroupBox.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
        private GroupBox groupBox1;
        private ListBox PSResultsListBox;
        private GroupBox groupBox2;
        private Button PSSearchButton;
        private ComboBox PSSerialEntryComboBox;
        private Label label1;
        private Button PSOpenButton;
        private Label label2;
        private Label label3;
        private TextBox PSDateTextbox;
        private TextBox PSFileNameTextbox;
    }
}


