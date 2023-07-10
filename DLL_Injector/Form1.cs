using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;

namespace DLL_Injector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DisplayProcess();
        }

        public void DisplayProcess()
        {
            try
            {
                Process[] pc = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
                comboBox1.Items.Clear();
                foreach (Process p in pc)
                {
                    comboBox1.Items.Add(p.ProcessName);
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Dll Injector", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string dll_File { get; set; } 

        Point lastPoint;

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DisplayProcess();
        }

        private void injectDll_Click(object sender, EventArgs e)
        {
            Injector dllInjector = new Injector(comboBox1.Text, dll_File);
            dllInjector.InjectDll();
        }

        private void selectDll_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                dll_File = openFileDialog1.FileName;
                pathToDll.Text = dll_File;
            }
        }
    }
}
