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
            var date = $"{DateTime.Now.ToShortDateString()}";
            var time = $"{DateTime.Now:hh:mm:sstt}";
            var datetime = date + " " + time;
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(message, Color.Black);
            }
            else
                box.AppendText(Environment.NewLine + message, Color.Black);

            if (writeToFile)
            {
                try
                {
                    using (FileStream fs = new FileStream(LOGFILE_PATH, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fs.Seek(0, SeekOrigin.End);
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            // To append, update the stream's position to the end of the file
                            WriteLine(message, writer);
                        }
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
            
            if (string.IsNullOrEmpty(box.Text))
            {
                box.AppendText(message, color);
            }
            else
                box.AppendText(Environment.NewLine + message, color);

            if (writeToFile)
            {
                try
                {
                    using (FileStream fs = new FileStream(LOGFILE_PATH, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        fs.Seek(0, SeekOrigin.End);
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            // To append, update the stream's position to the end of the file
                            WriteLine(message, writer);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static void WriteLine(string message, TextWriter w)
        {
            var date = $"{DateTime.Now.ToShortDateString()}";
            var time = $"{DateTime.Now:hh:mm:sstt}";
            var loginStr = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var compNameStr = Environment.MachineName;
            w.WriteLine($"{date} {time}, {compNameStr}, {loginStr}, {message}");
        }
    }
}