using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace MP4_to_MP3
{
    public partial class Form1 : Form
    {
        string[] filesToConvert;
        string[] filesToShow;
        string destinationDir;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                //before
                listBox1.Items.Clear();

                long fileSizes = 0;
                filesToShow = openFileDialog1.SafeFileNames;
                filesToConvert = openFileDialog1.FileNames;
                for (int i = 0; i < filesToConvert.Length; i++)
                {
                    listBox1.Items.Add(openFileDialog1.SafeFileNames[i] + " (" + new FileInfo(filesToConvert[i]).Length / 1024 / 1024 + " MB)");
                    fileSizes += new FileInfo(filesToConvert[i]).Length / 1024 / 1024;
                }
                textBox1.Text = new FileInfo(filesToConvert[0]).DirectoryName;
                label2.Text = filesToConvert.Length.ToString() + " " + (filesToConvert.Length > 1 ? "files" : "file") + ", total size is " + fileSizes + " MB";

                if (folderBrowserDialog1.SelectedPath != "")
                    button3.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
                destinationDir = folderBrowserDialog1.SelectedPath;
                if (openFileDialog1.FileName != "")
                    button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            long fileSizes = 0;
            bool ok = true;
            for(int i = 0; i < filesToConvert.Length && ok; i++)
            {
                if(File.Exists(folderBrowserDialog1.SelectedPath + "\\" + filesToShow[i].Substring(0, filesToShow[i].Length - 1) + "3"))
                {
                    ok = false;
                    MessageBox.Show("File " + filesToShow[i].Substring(0, filesToShow[i].Length - 1) + "3" + " exists!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            for(int i=0;i<filesToConvert.Length && ok;i++)
            {
                button3.Text = "Converting "+ filesToShow[i];
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = @"/C cd " + Application.StartupPath + " && ffmpeg -i \"" + filesToConvert[i] + " \" \"" + folderBrowserDialog1.SelectedPath + "\\" + filesToShow[i].Substring(0, filesToShow[i].Length - 1) + "3\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    button3.Text = "Convert";
                    listBox2.Items.Add(filesToShow[i] + " CONVERTED  " + new FileInfo(folderBrowserDialog1.SelectedPath + "\\" + filesToShow[i].Substring(0, filesToShow[i].Length - 1) + "3").Length / 1024 / 1024 + " MB now");
                    fileSizes += new FileInfo(folderBrowserDialog1.SelectedPath + "\\" + filesToShow[i].Substring(0, filesToShow[i].Length - 1) + "3").Length / 1024 / 1024;
                    if (i== filesToConvert.Length - 1)
                    {
                        MessageBox.Show(filesToConvert.Length + " files have been converted, total size is "+ fileSizes+" MB", "Success :)", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                } else
                {
                    listBox2.Items.Add(filesToShow[i] + " FAILED TO CONVERT");
                }
            }
            
        }
    }
}
