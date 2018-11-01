using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace ProcedureSearch
{
    public static class Logger
    {        
        /// <summary>
        /// Write a message to the status log textbox, with default black foreground text color.
        /// </summary>
        /// <param name="message">Message to write to the status log.</param>
        /// <param name="box">RichTextBox control to use for writing</param>
        /// <param name="writeToFile">Option to write to logfile</param>
        public static void Log(string message, RichTextBox box, bool writeToFile)
        {
            string LOGFILE_PATH = ConfigurationManager.AppSettings["LOGFILE_PATH"];
            var time = $"{DateTime.Now:hh:mm:sstt}";
            var datetime = time + " " + DateTime.Now.ToShortTimeString();
            var str = $"{time} > {message}";
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(str, Color.Black);
            }
            else
                box.AppendText(Environment.NewLine + str, Color.Black);

            if (writeToFile)
            {
                try
                {
                    using (var writer = new StreamWriter(LOGFILE_PATH, true))
                    {
                        var loginStr = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        var compNameStr = Environment.MachineName;
                        writer.WriteLine($"{datetime}, {compNameStr}, {loginStr}, {message}");
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Write a message to the status log textbox, a specified foreground text color.
        /// </summary>
        /// <param name="message">Message to write to the status log.</param>
        /// <param name="box">RichTextBox control to use for writing.</param>
        /// <param name="color">Foreground color of text.</param>
        /// <param name="writeToFile">Option to write to logfile</param>
        public static void Log(string message, RichTextBox box, Color color, bool writeToFile)
        {
            string LOGFILE_PATH = ConfigurationManager.AppSettings["LOGFILE_PATH"];
            var time = $"{DateTime.Now:hh:mm:sstt}";
            var datetime = time + " " + DateTime.Now.ToShortTimeString();
            var str = $"{time} > {message}";
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(str, color);
            }
            else
                box.AppendText(Environment.NewLine + str, color);

            if (writeToFile)
            {
                try
                {
                    using (var writer = new StreamWriter(LOGFILE_PATH, true))
                    {
                        var loginStr = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        var compNameStr = Environment.MachineName;
                        writer.WriteLine($"{datetime}, {compNameStr}, {loginStr}, {message}");
                    }
                }
                catch
                {
                }
            }
        }
    }
}