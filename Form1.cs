using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using System.Threading.Tasks;


namespace project_gui
{
    public partial class Form1 : Form
    {
        public static int tab_count=1;
        public static Form1 my_form;
        public Form1()
        {
            InitializeComponent();
            Form1.my_form = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
           
            MIPS.Program.Globals.control_alg = control_alg.Text;

            Console.WriteLine(v0.Text);
            
            MIPS.Program.Globals.aTimer.Interval = Double.Parse(cyclebox.Text);
            MIPS.Program.Globals.bTimer.Interval = Double.Parse(cscyclebox.Text);
            
            
            MIPS.Program.MainRun(alg.Text.ToLower());
        }
        public void SetLabelText(string newText)
        {
            
                cyclebox.Text = newText;
            
            
        }
        public void Set_id_branch(string newText)
        {

            id_branch.Text = newText;


        }
        public void Set_alu_branch(string newText)
        {

            alu_branch.Text = newText;


        }
      
        public void SetSelectedTab(int pcb)
        {

            instructiontabs.SelectedIndex = pcb;


        }
        public string GetAlg()
        {

            return alg.Text;


        }
        public void Setmemtext(string newText)
        {

            memtext.Text = newText;


        }
        public void Addtolog(string newText)
        {

            if (logtext.Text == "")
            {
                logtext.Text = newText;
            }
            else
                logtext.Text = logtext.Text +"\n"+ newText;


        }
        public void SetMemory(string value,string index)
        {
            Console.WriteLine("a1");
            string memorytext=memory.Text;
            int indexofindex=memorytext.IndexOf("#"+index);
            int indexofdots=memorytext.IndexOf(":", indexofindex);
            int indexofend = memorytext.IndexOf("\n", indexofindex);
            Console.WriteLine("a2");
            memory.Text=memorytext.Substring(0,indexofdots+1)+value+ memorytext.Substring(indexofend,memory.Text.Length-indexofend);
            Console.WriteLine(indexofindex);
            Console.WriteLine(indexofdots);
            Console.WriteLine(indexofdots);


        }
        public void Setwbtext(string newText)
        {

            wbtext.Text = newText;


        }
        public void SetALUText(string newText)
        {

            alutext.Text = newText;


        }
        public void SetALUdest(string newText)
        {

            aludest.Text = newText;


        }
        public void Cleandh( )
        {

            dhtext.Text = "";


        }
        public void SettorLabel(string newText)
        {

            torlabel.Text = newText;


        }
        public void Setforwardingtext(string newText)
        {

            forwardinglabel.Text = newText;


        }
        public void Setdhtext(string newText)
        {

            dhtext.Text = newText;


        }
        public void SetRegLabel(string newText)
        {

            registerslabel.Text = newText;


        }
        public void Setv0(string newText)
        {

            v0.Text = newText;


        }

        public void Setlabelcs(string newText)
        {

            cslabel.Text = newText;


        }
        

        public void SetRegisters(string register, string value)
        {

            switch (register)
            {
                case "$v0":
                    v0.Text = value;
                    break;
                case "$v1":
                    v1.Text = value;
                    break;
                case "$a0":
                    a0.Text = value;
                    break;
                case "$a1":
                    a1.Text = value;
                    break;
                case "$a2":
                    a2.Text = value;
                    break;
                case "$a3":
                    a3.Text = value;
                    break;
                case "$t0":
                    t0.Text = value;
                    break;
                case "$t1":
                    t1.Text = value;
                    break;
                case "$t2":
                    t2.Text = value;
                    break;
                case "$t3":
                    t3.Text = value;
                    break;
                default:
                    break;
            }
        }
        public void SetCurInsText(string newText)
        {

            label25.Text = newText;


        }
        public void SetRunTime(string newText)
        {

            runtimebox.Text = newText;


        }
        public void SetPc(string newText)
        {

            label24.Text = newText;


        }


        private void label1_Click_3(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MIPS.Program.Globals.pc = 100000;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string title = "File " + Form1.tab_count;
                bool first = false;
                if (instructiontabs.SelectedTab == tabPage4)
                {
                    first = true;
                    title= "File " + (instructiontabs.TabCount ).ToString();
                }
                Form1.tab_count++;
                
                TabPage myTabPage = new TabPage(title);
                instructiontabs.TabPages.Add(myTabPage);
                var rich = new RichTextBox();
                myTabPage.Controls.Add(rich);
                rich.Dock = DockStyle.Fill;
                var fileStream = ofd.OpenFile();
                string content = "";
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string fileContent = reader.ReadToEnd();
                    content = fileContent;
                    rich.Text = fileContent;
                    
                }
                if (first)
                {
                    instructiontabs.TabPages.Remove(tabPage4);
                }
                string[] lines = content.Split('\n');
                bool dollarflag = true;
                int index = 0;
                while (dollarflag)
                {
                    lines[index] = lines[index].Remove(lines[index].Length - 1);
                    
                    if (lines[index]!="$")
                    {
                        index++;
                    }
                    else
                    {
                        dollarflag = false;
                    }
                }
                   
                
                List<string> lines2 = new List<string>(lines);
                

                instructiontabs.SelectedTab = myTabPage;
                if (MIPS.Program.Globals.run)
                {
                    if (MIPS.Program.Globals.alg=="srt")
                    {
                        if (MIPS.Program.Globals.cs)
                        {
                            if (MIPS.Program.Globals.mem_reg=="mem")
                            {
                                MIPS.Program.addProc(lines2);
                            }
                            else
                            {
                                MIPS.Program.Globals.srt_flag = true;
                                MIPS.Program.addProc(lines2);
                                MIPS.Program.Globals.b3.SignalAndWait();
                            }
                        }
                        else
                        {
                           
                            MIPS.Program.addProc(lines2);
                            MIPS.Program.thread_choose_pcb("srt", "mem");

                        }
                    }
                    else
                    {
                        MIPS.Program.addProc(lines2);
                    }
                }
                
                
            }
        }
        

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void button2_Click(object sender, EventArgs e)
        {
            MIPS.Program.Globals.control_alg = control_alg.Text;
            MIPS.Program.Globals.alg = alg.Text.ToLower();
            MIPS.Program.StepRun();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (instructiontabs.TabCount>0)
            {
                TabPage tab_remove = instructiontabs.SelectedTab;
                instructiontabs.TabPages.Remove(tab_remove);
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void v0_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void alg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
