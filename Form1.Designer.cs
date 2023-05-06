using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static MIPS.Program;


namespace project_gui
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.settings = new System.Windows.Forms.GroupBox();
            this.control_alg = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.alg = new System.Windows.Forms.ComboBox();
            this.alglabel = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cscyclebox = new System.Windows.Forms.TextBox();
            this.cscyclelabel = new System.Windows.Forms.Label();
            this.runtimebox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.runtimelabel = new System.Windows.Forms.Label();
            this.cyclelabel = new System.Windows.Forms.Label();
            this.cyclebox = new System.Windows.Forms.TextBox();
            this.v0 = new System.Windows.Forms.TextBox();
            this.instructions = new System.Windows.Forms.GroupBox();
            this.instructionspanel = new System.Windows.Forms.Panel();
            this.closebutton = new System.Windows.Forms.Button();
            this.instructiontabs = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.logtext = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.memory = new System.Windows.Forms.RichTextBox();
            this.registers = new System.Windows.Forms.GroupBox();
            this.registerspanel = new System.Windows.Forms.Panel();
            this.t3 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.t2 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.t1 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.t0 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.a3 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.a2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.a1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.a0 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.v1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.accountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.wbtext = new System.Windows.Forms.Label();
            this.memtext = new System.Windows.Forms.Label();
            this.aludest = new System.Windows.Forms.Label();
            this.alutext = new System.Windows.Forms.Label();
            this.forwardinglabel = new System.Windows.Forms.Label();
            this.torlabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dhtext = new System.Windows.Forms.Label();
            this.registerslabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cslabel = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.id_branch = new System.Windows.Forms.Label();
            this.alu_branch = new System.Windows.Forms.Label();
            this.settings.SuspendLayout();
            this.instructions.SuspendLayout();
            this.instructionspanel.SuspendLayout();
            this.instructiontabs.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.registers.SuspendLayout();
            this.registerspanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // settings
            // 
            this.settings.Controls.Add(this.control_alg);
            this.settings.Controls.Add(this.label29);
            this.settings.Controls.Add(this.alg);
            this.settings.Controls.Add(this.alglabel);
            this.settings.Controls.Add(this.label23);
            this.settings.Controls.Add(this.label22);
            this.settings.Controls.Add(this.cscyclebox);
            this.settings.Controls.Add(this.cscyclelabel);
            this.settings.Controls.Add(this.runtimebox);
            this.settings.Controls.Add(this.label5);
            this.settings.Controls.Add(this.runtimelabel);
            this.settings.Controls.Add(this.cyclelabel);
            this.settings.Controls.Add(this.cyclebox);
            this.settings.Font = new System.Drawing.Font("Arial", 16F);
            this.settings.Location = new System.Drawing.Point(12, 27);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(220, 188);
            this.settings.TabIndex = 1;
            this.settings.TabStop = false;
            this.settings.Text = "Settings";
            // 
            // control_alg
            // 
            this.control_alg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.control_alg.Font = new System.Drawing.Font("Arial", 11F);
            this.control_alg.FormattingEnabled = true;
            this.control_alg.Items.AddRange(new object[] {
            "Static",
            "1-Bit",
            "2-Bit"});
            this.control_alg.Location = new System.Drawing.Point(99, 151);
            this.control_alg.Name = "control_alg";
            this.control_alg.Size = new System.Drawing.Size(92, 25);
            this.control_alg.TabIndex = 17;
            this.control_alg.SelectedIndex = 0;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Arial", 11F);
            this.label29.Location = new System.Drawing.Point(6, 152);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(78, 17);
            this.label29.TabIndex = 16;
            this.label29.Text = "Control Alg";
            // 
            // alg
            // 
            this.alg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alg.Font = new System.Drawing.Font("Arial", 11F);
            this.alg.FormattingEnabled = true;
            this.alg.Items.AddRange(new object[] {
            "FCFS",
            "RR",
            "SJN",
            "SRT"});
            this.alg.Location = new System.Drawing.Point(99, 120);
            this.alg.Name = "alg";
            this.alg.Size = new System.Drawing.Size(92, 25);
            this.alg.TabIndex = 15;
            this.alg.SelectedIndex = 0;
            this.alg.SelectedIndexChanged += new System.EventHandler(this.alg_SelectedIndexChanged);
            // 
            // alglabel
            // 
            this.alglabel.AutoSize = true;
            this.alglabel.Font = new System.Drawing.Font("Arial", 11F);
            this.alglabel.Location = new System.Drawing.Point(6, 121);
            this.alglabel.Name = "alglabel";
            this.alglabel.Size = new System.Drawing.Size(92, 17);
            this.alglabel.TabIndex = 14;
            this.alglabel.Text = "Schedule Alg";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Arial", 11F);
            this.label23.Location = new System.Drawing.Point(197, 63);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(16, 17);
            this.label23.TabIndex = 12;
            this.label23.Text = "s";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Arial", 11F);
            this.label22.Location = new System.Drawing.Point(197, 93);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(16, 17);
            this.label22.TabIndex = 11;
            this.label22.Text = "s";
            // 
            // cscyclebox
            // 
            this.cscyclebox.Font = new System.Drawing.Font("Arial", 11F);
            this.cscyclebox.Location = new System.Drawing.Point(99, 90);
            this.cscyclebox.Name = "cscyclebox";
            this.cscyclebox.Size = new System.Drawing.Size(92, 24);
            this.cscyclebox.TabIndex = 10;
            this.cscyclebox.Text = "3000";
            // 
            // cscyclelabel
            // 
            this.cscyclelabel.AutoSize = true;
            this.cscyclelabel.Font = new System.Drawing.Font("Arial", 11F);
            this.cscyclelabel.Location = new System.Drawing.Point(6, 90);
            this.cscyclelabel.Name = "cscyclelabel";
            this.cscyclelabel.Size = new System.Drawing.Size(70, 17);
            this.cscyclelabel.TabIndex = 9;
            this.cscyclelabel.Text = "CS Cycle";
            // 
            // runtimebox
            // 
            this.runtimebox.Font = new System.Drawing.Font("Arial", 11F);
            this.runtimebox.Location = new System.Drawing.Point(99, 60);
            this.runtimebox.Name = "runtimebox";
            this.runtimebox.Size = new System.Drawing.Size(92, 24);
            this.runtimebox.TabIndex = 8;
            this.runtimebox.Text = "00:00:00:00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11F);
            this.label5.Location = new System.Drawing.Point(197, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "s";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // runtimelabel
            // 
            this.runtimelabel.AutoSize = true;
            this.runtimelabel.Font = new System.Drawing.Font("Arial", 11F);
            this.runtimelabel.Location = new System.Drawing.Point(6, 60);
            this.runtimelabel.Name = "runtimelabel";
            this.runtimelabel.Size = new System.Drawing.Size(63, 17);
            this.runtimelabel.TabIndex = 6;
            this.runtimelabel.Text = "Runtime";
            // 
            // cyclelabel
            // 
            this.cyclelabel.AutoSize = true;
            this.cyclelabel.Font = new System.Drawing.Font("Arial", 11F);
            this.cyclelabel.Location = new System.Drawing.Point(6, 35);
            this.cyclelabel.Name = "cyclelabel";
            this.cyclelabel.Size = new System.Drawing.Size(45, 17);
            this.cyclelabel.TabIndex = 4;
            this.cyclelabel.Text = "Cycle";
            this.cyclelabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // cyclebox
            // 
            this.cyclebox.Font = new System.Drawing.Font("Arial", 11F);
            this.cyclebox.Location = new System.Drawing.Point(99, 30);
            this.cyclebox.Name = "cyclebox";
            this.cyclebox.Size = new System.Drawing.Size(92, 24);
            this.cyclebox.TabIndex = 2;
            this.cyclebox.Text = "300";
            this.cyclebox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // v0
            // 
            this.v0.Font = new System.Drawing.Font("Arial", 11F);
            this.v0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.v0.Location = new System.Drawing.Point(41, 8);
            this.v0.Name = "v0";
            this.v0.ReadOnly = true;
            this.v0.Size = new System.Drawing.Size(92, 24);
            this.v0.TabIndex = 1;
            this.v0.Text = "0";
            this.v0.TextChanged += new System.EventHandler(this.v0_TextChanged);
            // 
            // instructions
            // 
            this.instructions.Controls.Add(this.instructionspanel);
            this.instructions.Font = new System.Drawing.Font("Arial", 16F);
            this.instructions.Location = new System.Drawing.Point(12, 224);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(220, 337);
            this.instructions.TabIndex = 2;
            this.instructions.TabStop = false;
            this.instructions.Text = "Instructions";
            this.instructions.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // instructionspanel
            // 
            this.instructionspanel.Controls.Add(this.closebutton);
            this.instructionspanel.Controls.Add(this.instructiontabs);
            this.instructionspanel.Controls.Add(this.openbutton);
            this.instructionspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instructionspanel.Location = new System.Drawing.Point(3, 28);
            this.instructionspanel.Name = "instructionspanel";
            this.instructionspanel.Size = new System.Drawing.Size(214, 306);
            this.instructionspanel.TabIndex = 0;
            // 
            // closebutton
            // 
            this.closebutton.Font = new System.Drawing.Font("Arial", 14F);
            this.closebutton.Location = new System.Drawing.Point(42, 5);
            this.closebutton.Name = "closebutton";
            this.closebutton.Size = new System.Drawing.Size(30, 30);
            this.closebutton.TabIndex = 2;
            this.closebutton.Text = "x";
            this.closebutton.UseVisualStyleBackColor = true;
            this.closebutton.Click += new System.EventHandler(this.button5_Click);
            // 
            // instructiontabs
            // 
            this.instructiontabs.Controls.Add(this.tabPage4);
            this.instructiontabs.Font = new System.Drawing.Font("Arial", 11F);
            this.instructiontabs.Location = new System.Drawing.Point(6, 41);
            this.instructiontabs.Name = "instructiontabs";
            this.instructiontabs.SelectedIndex = 0;
            this.instructiontabs.Size = new System.Drawing.Size(204, 274);
            this.instructiontabs.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.richTextBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(196, 244);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Example";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 11F);
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(190, 238);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "- Try adding instruction files using the button above\n- Once you add a new file t" +
    "his text will disappear\n";
            // 
            // openbutton
            // 
            this.openbutton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.openbutton.Font = new System.Drawing.Font("Arial", 16F);
            this.openbutton.Location = new System.Drawing.Point(6, 5);
            this.openbutton.Name = "openbutton";
            this.openbutton.Size = new System.Drawing.Size(30, 30);
            this.openbutton.TabIndex = 1;
            this.openbutton.Text = "+";
            this.openbutton.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(374, 485);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 53);
            this.button1.TabIndex = 3;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(534, 485);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 53);
            this.button2.TabIndex = 4;
            this.button2.Text = "Step";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(687, 485);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(127, 53);
            this.button3.TabIndex = 5;
            this.button3.Text = "Reset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.logtext);
            this.groupBox3.Location = new System.Drawing.Point(942, 349);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(365, 312);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // logtext
            // 
            this.logtext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logtext.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.logtext.Location = new System.Drawing.Point(3, 16);
            this.logtext.Name = "logtext";
            this.logtext.Size = new System.Drawing.Size(359, 293);
            this.logtext.TabIndex = 0;
            this.logtext.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Location = new System.Drawing.Point(942, 190);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 157);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Memory";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.memory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 138);
            this.panel1.TabIndex = 0;
            // 
            // memory
            // 
            this.memory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memory.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.memory.Location = new System.Drawing.Point(0, 0);
            this.memory.Name = "memory";
            this.memory.Size = new System.Drawing.Size(205, 138);
            this.memory.TabIndex = 0;
            this.memory.Text = resources.GetString("memory.Text");
            // 
            // registers
            // 
            this.registers.Controls.Add(this.registerspanel);
            this.registers.Font = new System.Drawing.Font("Arial", 16F);
            this.registers.Location = new System.Drawing.Point(939, 27);
            this.registers.Name = "registers";
            this.registers.Size = new System.Drawing.Size(214, 157);
            this.registers.TabIndex = 8;
            this.registers.TabStop = false;
            this.registers.Text = "Registers";
            // 
            // registerspanel
            // 
            this.registerspanel.AutoScroll = true;
            this.registerspanel.Controls.Add(this.t3);
            this.registerspanel.Controls.Add(this.label21);
            this.registerspanel.Controls.Add(this.t2);
            this.registerspanel.Controls.Add(this.label26);
            this.registerspanel.Controls.Add(this.t1);
            this.registerspanel.Controls.Add(this.label27);
            this.registerspanel.Controls.Add(this.t0);
            this.registerspanel.Controls.Add(this.label28);
            this.registerspanel.Controls.Add(this.a3);
            this.registerspanel.Controls.Add(this.label13);
            this.registerspanel.Controls.Add(this.a2);
            this.registerspanel.Controls.Add(this.label12);
            this.registerspanel.Controls.Add(this.a1);
            this.registerspanel.Controls.Add(this.label10);
            this.registerspanel.Controls.Add(this.a0);
            this.registerspanel.Controls.Add(this.label9);
            this.registerspanel.Controls.Add(this.label4);
            this.registerspanel.Controls.Add(this.label11);
            this.registerspanel.Controls.Add(this.v0);
            this.registerspanel.Controls.Add(this.v1);
            this.registerspanel.Controls.Add(this.label7);
            this.registerspanel.Controls.Add(this.label8);
            this.registerspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerspanel.Location = new System.Drawing.Point(3, 28);
            this.registerspanel.Name = "registerspanel";
            this.registerspanel.Size = new System.Drawing.Size(208, 126);
            this.registerspanel.TabIndex = 0;
            this.registerspanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // t3
            // 
            this.t3.Font = new System.Drawing.Font("Arial", 11F);
            this.t3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.t3.Location = new System.Drawing.Point(41, 253);
            this.t3.Name = "t3";
            this.t3.ReadOnly = true;
            this.t3.Size = new System.Drawing.Size(92, 24);
            this.t3.TabIndex = 33;
            this.t3.Text = "0";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Arial", 11F);
            this.label21.Location = new System.Drawing.Point(3, 256);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(28, 17);
            this.label21.TabIndex = 34;
            this.label21.Text = "$t3";
            // 
            // t2
            // 
            this.t2.Font = new System.Drawing.Font("Arial", 11F);
            this.t2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.t2.Location = new System.Drawing.Point(41, 226);
            this.t2.Name = "t2";
            this.t2.ReadOnly = true;
            this.t2.Size = new System.Drawing.Size(92, 24);
            this.t2.TabIndex = 27;
            this.t2.Text = "0";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Arial", 11F);
            this.label26.Location = new System.Drawing.Point(3, 229);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(28, 17);
            this.label26.TabIndex = 28;
            this.label26.Text = "$t2";
            // 
            // t1
            // 
            this.t1.Font = new System.Drawing.Font("Arial", 11F);
            this.t1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.t1.Location = new System.Drawing.Point(41, 199);
            this.t1.Name = "t1";
            this.t1.ReadOnly = true;
            this.t1.Size = new System.Drawing.Size(92, 24);
            this.t1.TabIndex = 31;
            this.t1.Text = "0";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Arial", 11F);
            this.label27.Location = new System.Drawing.Point(3, 202);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(28, 17);
            this.label27.TabIndex = 32;
            this.label27.Text = "$t1";
            // 
            // t0
            // 
            this.t0.Font = new System.Drawing.Font("Arial", 11F);
            this.t0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.t0.Location = new System.Drawing.Point(41, 171);
            this.t0.Name = "t0";
            this.t0.ReadOnly = true;
            this.t0.Size = new System.Drawing.Size(92, 24);
            this.t0.TabIndex = 29;
            this.t0.Text = "0";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Arial", 11F);
            this.label28.Location = new System.Drawing.Point(3, 174);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(28, 17);
            this.label28.TabIndex = 30;
            this.label28.Text = "$t0";
            // 
            // a3
            // 
            this.a3.Font = new System.Drawing.Font("Arial", 11F);
            this.a3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.a3.Location = new System.Drawing.Point(41, 144);
            this.a3.Name = "a3";
            this.a3.ReadOnly = true;
            this.a3.Size = new System.Drawing.Size(92, 24);
            this.a3.TabIndex = 25;
            this.a3.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 11F);
            this.label13.Location = new System.Drawing.Point(3, 147);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 17);
            this.label13.TabIndex = 26;
            this.label13.Text = "$a3";
            // 
            // a2
            // 
            this.a2.Font = new System.Drawing.Font("Arial", 11F);
            this.a2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.a2.Location = new System.Drawing.Point(41, 117);
            this.a2.Name = "a2";
            this.a2.ReadOnly = true;
            this.a2.Size = new System.Drawing.Size(92, 24);
            this.a2.TabIndex = 19;
            this.a2.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 11F);
            this.label12.Location = new System.Drawing.Point(3, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 17);
            this.label12.TabIndex = 20;
            this.label12.Text = "$a2";
            // 
            // a1
            // 
            this.a1.Font = new System.Drawing.Font("Arial", 11F);
            this.a1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.a1.Location = new System.Drawing.Point(41, 90);
            this.a1.Name = "a1";
            this.a1.ReadOnly = true;
            this.a1.Size = new System.Drawing.Size(92, 24);
            this.a1.TabIndex = 23;
            this.a1.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 11F);
            this.label10.Location = new System.Drawing.Point(3, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 17);
            this.label10.TabIndex = 24;
            this.label10.Text = "$a1";
            // 
            // a0
            // 
            this.a0.Font = new System.Drawing.Font("Arial", 11F);
            this.a0.ForeColor = System.Drawing.SystemColors.WindowText;
            this.a0.Location = new System.Drawing.Point(41, 62);
            this.a0.Name = "a0";
            this.a0.ReadOnly = true;
            this.a0.Size = new System.Drawing.Size(92, 24);
            this.a0.TabIndex = 21;
            this.a0.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 11F);
            this.label9.Location = new System.Drawing.Point(3, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 17);
            this.label9.TabIndex = 22;
            this.label9.Text = "$a0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11F);
            this.label4.Location = new System.Drawing.Point(3, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "$v1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 11F);
            this.label11.Location = new System.Drawing.Point(3, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 17);
            this.label11.TabIndex = 18;
            this.label11.Text = "$v0";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // v1
            // 
            this.v1.Font = new System.Drawing.Font("Arial", 11F);
            this.v1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.v1.Location = new System.Drawing.Point(41, 35);
            this.v1.Name = "v1";
            this.v1.ReadOnly = true;
            this.v1.Size = new System.Drawing.Size(92, 24);
            this.v1.TabIndex = 9;
            this.v1.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11F);
            this.label7.Location = new System.Drawing.Point(6, -54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "$v1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 11F);
            this.label8.Location = new System.Drawing.Point(6, -28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "$a0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 11F);
            this.label6.Location = new System.Drawing.Point(946, -17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "$v0";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountToolStripMenuItem,
            this.compareToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1484, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // accountToolStripMenuItem
            // 
            this.accountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bitchToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            this.accountToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.accountToolStripMenuItem.Text = "Account";
            // 
            // bitchToolStripMenuItem
            // 
            this.bitchToolStripMenuItem.Name = "bitchToolStripMenuItem";
            this.bitchToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.bitchToolStripMenuItem.Text = "bitch";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(101, 22);
            this.toolStripMenuItem2.Text = "2";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(101, 22);
            this.toolStripMenuItem3.Text = "3";
            // 
            // compareToolStripMenuItem
            // 
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compareToolStripMenuItem.Text = "Compare";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.alu_branch);
            this.panel3.Controls.Add(this.id_branch);
            this.panel3.Controls.Add(this.wbtext);
            this.panel3.Controls.Add(this.memtext);
            this.panel3.Controls.Add(this.aludest);
            this.panel3.Controls.Add(this.alutext);
            this.panel3.Controls.Add(this.forwardinglabel);
            this.panel3.Controls.Add(this.torlabel);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.dhtext);
            this.panel3.Controls.Add(this.registerslabel);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.cslabel);
            this.panel3.Controls.Add(this.label25);
            this.panel3.Controls.Add(this.label24);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.label19);
            this.panel3.Controls.Add(this.textBox15);
            this.panel3.Controls.Add(this.label18);
            this.panel3.Controls.Add(this.textBox14);
            this.panel3.Controls.Add(this.label17);
            this.panel3.Controls.Add(this.textBox13);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.textBox12);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(238, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(701, 456);
            this.panel3.TabIndex = 11;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // wbtext
            // 
            this.wbtext.AutoSize = true;
            this.wbtext.Location = new System.Drawing.Point(544, 92);
            this.wbtext.Name = "wbtext";
            this.wbtext.Size = new System.Drawing.Size(0, 13);
            this.wbtext.TabIndex = 24;
            // 
            // memtext
            // 
            this.memtext.AutoSize = true;
            this.memtext.Location = new System.Drawing.Point(429, 144);
            this.memtext.Name = "memtext";
            this.memtext.Size = new System.Drawing.Size(0, 13);
            this.memtext.TabIndex = 23;
            // 
            // aludest
            // 
            this.aludest.AutoSize = true;
            this.aludest.Location = new System.Drawing.Point(330, 130);
            this.aludest.Name = "aludest";
            this.aludest.Size = new System.Drawing.Size(0, 13);
            this.aludest.TabIndex = 22;
            // 
            // alutext
            // 
            this.alutext.AutoSize = true;
            this.alutext.Location = new System.Drawing.Point(333, 91);
            this.alutext.Name = "alutext";
            this.alutext.Size = new System.Drawing.Size(0, 13);
            this.alutext.TabIndex = 21;
            // 
            // forwardinglabel
            // 
            this.forwardinglabel.AutoSize = true;
            this.forwardinglabel.Location = new System.Drawing.Point(296, 356);
            this.forwardinglabel.Name = "forwardinglabel";
            this.forwardinglabel.Size = new System.Drawing.Size(0, 13);
            this.forwardinglabel.TabIndex = 20;
            // 
            // torlabel
            // 
            this.torlabel.AutoSize = true;
            this.torlabel.Location = new System.Drawing.Point(157, 320);
            this.torlabel.Name = "torlabel";
            this.torlabel.Size = new System.Drawing.Size(13, 13);
            this.torlabel.TabIndex = 19;
            this.torlabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Waiting: ";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // dhtext
            // 
            this.dhtext.AutoSize = true;
            this.dhtext.Location = new System.Drawing.Point(206, 58);
            this.dhtext.Name = "dhtext";
            this.dhtext.Size = new System.Drawing.Size(0, 13);
            this.dhtext.TabIndex = 17;
            // 
            // registerslabel
            // 
            this.registerslabel.AutoSize = true;
            this.registerslabel.Location = new System.Drawing.Point(173, 137);
            this.registerslabel.Name = "registerslabel";
            this.registerslabel.Size = new System.Drawing.Size(0, 13);
            this.registerslabel.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(3, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Process";
            // 
            // cslabel
            // 
            this.cslabel.AutoSize = true;
            this.cslabel.Location = new System.Drawing.Point(52, 213);
            this.cslabel.Name = "cslabel";
            this.cslabel.Size = new System.Drawing.Size(32, 13);
            this.cslabel.TabIndex = 14;
            this.cslabel.Text = "False";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(19, 162);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(0, 13);
            this.label25.TabIndex = 13;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(49, 34);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(0, 13);
            this.label24.TabIndex = 12;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.Location = new System.Drawing.Point(296, 337);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 15);
            this.label20.TabIndex = 11;
            this.label20.Text = "Forwarding";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Location = new System.Drawing.Point(555, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(62, 15);
            this.label19.TabIndex = 10;
            this.label19.Text = "Write Back";
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(518, 5);
            this.textBox15.Multiline = true;
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(11, 313);
            this.textBox15.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label18.Location = new System.Drawing.Point(440, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 15);
            this.label18.TabIndex = 8;
            this.label18.Text = "Data Memory";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(412, 5);
            this.textBox14.Multiline = true;
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(11, 313);
            this.textBox14.TabIndex = 7;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Location = new System.Drawing.Point(333, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 15);
            this.label17.TabIndex = 6;
            this.label17.Text = "ALU";
            this.label17.Click += new System.EventHandler(this.label17_Click);
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(316, 5);
            this.textBox13.Multiline = true;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(11, 313);
            this.textBox13.TabIndex = 5;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Location = new System.Drawing.Point(198, 107);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 15);
            this.label16.TabIndex = 4;
            this.label16.Text = "Registers";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Location = new System.Drawing.Point(176, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 15);
            this.label15.TabIndex = 3;
            this.label15.Text = "Hazard Detection Unit";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(146, 4);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(11, 313);
            this.textBox12.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Location = new System.Drawing.Point(19, 133);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(98, 15);
            this.label14.TabIndex = 1;
            this.label14.Text = "Instruction Memory";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(19, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "PC:";
            // 
            // id_branch
            // 
            this.id_branch.AutoSize = true;
            this.id_branch.Location = new System.Drawing.Point(173, 213);
            this.id_branch.Name = "id_branch";
            this.id_branch.Size = new System.Drawing.Size(0, 13);
            this.id_branch.TabIndex = 25;
            // 
            // alu_branch
            // 
            this.alu_branch.AutoSize = true;
            this.alu_branch.Location = new System.Drawing.Point(333, 245);
            this.alu_branch.Name = "alu_branch";
            this.alu_branch.Size = new System.Drawing.Size(0, 13);
            this.alu_branch.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 729);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.registers);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.instructions);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.settings.ResumeLayout(false);
            this.settings.PerformLayout();
            this.instructions.ResumeLayout(false);
            this.instructionspanel.ResumeLayout(false);
            this.instructiontabs.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.registers.ResumeLayout(false);
            this.registerspanel.ResumeLayout(false);
            this.registerspanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.GroupBox settings;
        public System.Windows.Forms.GroupBox instructions;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.GroupBox registers;
        public System.Windows.Forms.Label cyclelabel;
        public System.Windows.Forms.TextBox cyclebox;
        public System.Windows.Forms.TextBox v0;
        public System.Windows.Forms.Label runtimelabel;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Panel registerspanel;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox v1;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox runtimebox;
        public System.Windows.Forms.Panel instructionspanel;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem bitchToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        public System.Windows.Forms.Button openbutton;
        public System.Windows.Forms.TabControl instructiontabs;
        public System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.Button closebutton;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.TextBox textBox12;
        public System.Windows.Forms.Label label14;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox textBox13;
        public System.Windows.Forms.Label label16;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.Label label20;
        public System.Windows.Forms.Label label19;
        public System.Windows.Forms.TextBox textBox15;
        public System.Windows.Forms.Label label18;
        public System.Windows.Forms.TextBox textBox14;
        public System.Windows.Forms.Label alglabel;
        public System.Windows.Forms.Label label23;
        public System.Windows.Forms.Label label22;
        public System.Windows.Forms.TextBox cscyclebox;
        public System.Windows.Forms.Label cscyclelabel;
        public System.Windows.Forms.Label label24;
        public System.Windows.Forms.Label label25;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label cslabel;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label registerslabel;
        public System.Windows.Forms.Label dhtext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label torlabel;
        private System.Windows.Forms.Label forwardinglabel;
        public System.Windows.Forms.Label alutext;
        public System.Windows.Forms.Label aludest;
        public System.Windows.Forms.Label memtext;
        public System.Windows.Forms.Label wbtext;
        private System.Windows.Forms.RichTextBox memory;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox t3;
        public System.Windows.Forms.Label label21;
        public System.Windows.Forms.TextBox t2;
        public System.Windows.Forms.Label label26;
        public System.Windows.Forms.TextBox t1;
        public System.Windows.Forms.Label label27;
        public System.Windows.Forms.TextBox t0;
        public System.Windows.Forms.Label label28;
        public System.Windows.Forms.TextBox a3;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox a2;
        public System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox a1;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox a0;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox logtext;
        private System.Windows.Forms.ComboBox alg;
        private System.Windows.Forms.ComboBox control_alg;
        public System.Windows.Forms.Label label29;
        public System.Windows.Forms.Label alu_branch;
        public System.Windows.Forms.Label id_branch;
    }
}

