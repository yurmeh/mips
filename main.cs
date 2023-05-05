using MIPS;
using project_gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static MIPS.Program;
using static project_gui.ProgramGui;

namespace MIPS
{
    class Program
    {
      
        public static List<string> interpreter(List<string> ins_list, string re1, string re2, string re3)
        {
            // current ins: add,addi,sub,and,or,lw,c
            // gets list of instructions and regex expressions and splits the list to a list of opcodes and parameters
            List<string> result = new List<string>();
            Regex first = new Regex(re1);
            Regex second = new Regex(re2);
            Regex third = new Regex(re3);

            foreach (string a in ins_list)
            {
                if (first.IsMatch(a))
                {
                    MatchCollection matches = Regex.Matches(a, re1);
                    foreach (Match match in matches)
                    {
                        result.Add(match.Groups[1].Value);
                        result.Add(match.Groups[2].Value);
                        result.Add(match.Groups[3].Value);
                        result.Add(match.Groups[4].Value);
                    }

                }
                else
                {
                    if (second.IsMatch(a))
                    {
                        MatchCollection matches = Regex.Matches(a, re2);
                        foreach (Match match in matches)
                        {
                            result.Add(match.Groups[1].Value);
                            result.Add(match.Groups[2].Value);
                            result.Add(match.Groups[3].Value);
                            result.Add(match.Groups[4].Value);
                        }
                    }
                    else
                    {
                        MatchCollection matches = Regex.Matches(a, re3);
                        foreach (Match match in matches)
                        {
                            result.Add(match.Groups[1].Value);
                            result.Add(match.Groups[2].Value);
                            result.Add("");
                            result.Add("");
                        }
                    }
                }
            }

            return result;
        }

        public static class Globals
        {
            public static Dictionary<string, int> registers = new Dictionary<string, int>()
            {
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",0 },{"$a3",0 }, {"$t0",0 },{"$t1",0 },{"$t2",0 },{"$t3",0 }
            };
            public static Dictionary<string, int> regs_offset = new Dictionary<string, int>()
            {
                {"$zero",0 },{"$v0",1 },{"$v1",2 },{"$a0",3 },{"$a1",4 },{"$a2",5 },{"$a3",6 }, {"$t0",7 },{"$t1",8 },{"$t2",9 },{"$t3",10 }
            };

            public static int pc = 0;
            public static bool first_time = true;
            public static int count = 0;
            public static bool cs = false;
            public static List<string> load_to_pcb = new List<string> { "addi $zero,$zero,offset","sw $v0,1($zero)", "sw $v1,2($zero)", "sw $a0,3($zero)", "sw $a1,4($zero)", "sw $a2,5($zero)", "sw $a3,6($zero)", "sw $t0,7($zero)",
            "sw $t1,8($zero)","sw $t2,9($zero)","sw $t3,10($zero)","sub $zero,$zero,$zero"};

            public static List<string> load_to_reg = new List<string> {"addi $zero,$zero,offset","lw $v0,1($zero)", "lw $v1,2($zero)", "lw $a0,3($zero)", "lw $a1,4($zero)", "lw $a2,5($zero)", "lw $a3,6($zero)", "lw $t0,7($zero)",
            "lw $t1,8($zero)","lw $t2,9($zero)","lw $t3,10($zero)","sub $zero,$v0,$v0" };
            public static int cs_pc = 0;
            public static List<string> cs_instructions = new List<string>();
            public static string re1 = @"(\w+) (\S+),(\S+),(\S+)"; //add sub 
            public static string re2 = @"(\w+) (\S+),(\S+)\((\S+)\)"; //lw sw
            public static string re3 = @"(\w+) (\S+)";  // jump
            public static Stack<List<string>> ifid = new Stack<List<string>>();
            public static Stack<List<string>> idex = new Stack<List<string>>();
            public static Stack<List<string>> exmem = new Stack<List<string>>();
            public static Stack<List<string>> memwb = new Stack<List<string>>();
            public static Barrier b = new Barrier(5); // synchronize 5 parts
            public static Barrier b2 = new Barrier(6); // synchronize 5 parts with stopwatch
            public static Barrier b3 = new Barrier(2); // synchronize with cs
           
            public static List<string> instructionList = new List<string>(); // instruction list (loaded from files)
            public static int[] ram = new int[500];
            public static bool run = false;
            public static pcb[] procs = new pcb[10];
            public static int cur_pcb = -1;
            public static System.Timers.Timer aTimer = new System.Timers.Timer(300); // cycle clock
            public static System.Timers.Timer bTimer = new System.Timers.Timer(3000); // cs clock
            public static Stopwatch stopWatch = new Stopwatch(); // count time
            public static List<string> wb_ins = new List<string>();
            public static string alg = "";
            public static Queue<string> ongoing = new Queue<string>();
            public static Queue<List<string>> id_tor = new Queue<List<string>>();
            public static string v2_hazard = "";
            public static string v3_hazard = "";
            public static string v2_forward = "";
            public static string v3_forward = "";
            public static Barrier forwarding = new Barrier(4); // synchronize data hazards
            public static bool stall = false;
            public static readonly object ongoingLock = new object(); //lock for Globals.ongoing
            
            public static bool flush = false;
            public static bool ex = false;
            public static int id_branch = 0;
            public static int ex_branch = 0;
            public static Barrier b_flush = new Barrier(4); // synchronize flushing
            public static int ongoing_count = 0;
            public static string ctrl_tech = "ex_stage";
            public static bool advanced_control_flag=false;
            public static bool started = false;
            public static bool ended = false;
            public static string if_string = "";      
            public static string id_string = "";
            public static string ex_string = "";
            public static string mem_string = "";
            public static string wb_string = "";
            public static bool not_complete=true;
            public static string control_alg = "";
            public static bool take_branch=true;
            public static int predictor = 0;
            public static bool branch_check = false;
            public static bool final_flag = false;
            public static string mem_reg = "";
            public static bool srt_flag = false;


        }

        public static void IF()
        {

            
            while (Globals.run)
            {
                Globals.if_string = "IF: Idle";

                string proccess = "";
                if (Globals.cs)
                {
                    proccess = "CS";
                }
                else
                    proccess = Globals.cur_pcb.ToString();
                if (Globals.ended)
                {
                    proccess = "CS";
                }
               
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setlabelcs(proccess)));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetCurInsText("")));
              
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetPc(Globals.pc.ToString())));
              
                Globals.count++;
                int pc = Globals.pc;
                List<string> instructions = Globals.instructionList;
               
                if (Globals.cs)
                {
                    if (Globals.cs_pc == 0)
                    {
                        // I double check in case cs is cancelled when there is only one process left
                        Globals.b3.SignalAndWait();
                    }

                    if (Globals.cs)
                    {
                        Console.WriteLine("nigger");
                        //cs instructions
                        pc = Globals.cs_pc;
                        instructions = Globals.cs_instructions;
                        if (pc < instructions.Count)
                        {
                            List<string> instruction = new List<string>()
                            { instructions[pc], instructions[pc + 1], instructions[pc + 2], instructions[pc + 3], "cs" };
                            Globals.cs_pc += 4;
                            Globals.b.SignalAndWait();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetCurInsText(instruction[0] + " " + instruction[1] + " " + instruction[2] + " " + instruction[3])));
                            Globals.if_string= "IF: "+instruction[0] + ", " + instruction[1] + ", " + instruction[2] + ", " + instruction[3]+", cs";
                            Globals.ifid.Push(instruction);
                            if (pc + 4 == instructions.Count)
                            {
                                Globals.cs_pc = 0;
                                Globals.b3.SignalAndWait();
                            }
                        }
                    }
                    else
                    {
                        // if about to cs but realizes only on proc left
                        if (pc < instructions.Count)
                        {
                            List<string> instruction = new List<string>()
                            { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3],pc.ToString(),Globals.cur_pcb.ToString() };
                            Globals.pc += 4;
                            Globals.b.SignalAndWait();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetCurInsText(instruction[0] + " " + instruction[1] + " " + instruction[2] + " " + instruction[3])));
                            Globals.if_string ="IF: "+ instruction[0] + ", " + instruction[1] + ", " + instruction[2] + ", " + instruction[3] + ", "+Globals.cur_pcb;
                            Globals.ifid.Push(instruction);
                        }
                    }
                }
                else
                {
                    // regular instructions
                    if (pc < instructions.Count)
                    {

                        if (pc + 4 == instructions.Count)
                        {
                            Globals.procs[Globals.cur_pcb].SetState("dead");
                            Globals.bTimer.Stop();
                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
                            Globals.not_complete = false;
                            t.Start();
                        }

                        List<string> instruction = new List<string>()
                        { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3] ,pc.ToString(),Globals.cur_pcb.ToString()};
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.SetCurInsText(instruction[0] +" "+ instruction[1] + " " + instruction[2] + " " + instruction[3])));
                        Globals.pc += 4;
                        Globals.b.SignalAndWait();
                        Globals.if_string = "IF: "+instruction[0] + ", " + instruction[1] + ", " + instruction[2] + ", " + instruction[3] + ", " + Globals.cur_pcb;

                        Globals.ifid.Push(instruction);
                    }
                    else
                    {
                        
                        Globals.b.SignalAndWait();
                    }
                }
                Globals.b_flush.SignalAndWait();
                Globals.b2.SignalAndWait();
               
                Globals.b2.SignalAndWait();
            }
        }

        public static void ID()
        {

            while (Globals.run)
            {                                     
                List<string> instruction = new List<string>() { "idle", "idle" }; // what is this?
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegLabel("")));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setdhtext("")));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Set_alu_branch("")));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Set_id_branch("")));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setforwardingtext(""))); // reseting form               
                string sw_origin=""; // used later because I override the v1 for sw instructions

                if (Globals.ifid.Count != 0 || Globals.id_tor.Count != 0)
                {                 
                    if (Globals.ifid.Count != 0)
                    {
                        List<string> instruction1 = Globals.ifid.Pop();
                        Globals.id_tor.Enqueue(instruction1); 
                    }                    
                    instruction = Globals.id_tor.Dequeue();
                    Form1.my_form.Invoke(new Action(() => Form1.my_form.SettorLabel(Globals.id_tor.Count.ToString())));
                    // puts instruction from ifid to queue and takes the first one from the queue                   

                    string op = instruction[0];
                    string v1 = instruction[1];
                    string v2 = instruction[2];
                    string v3 = instruction[3];
                    bool v2_flag = false;
                    bool v3_flag = false;
                    int index2 = 0;
                    int index3 = 0;
                    bool stall = false;
                    bool dh = false;

                    //preparing for dh check
                    switch (op)
                    {
                        case "sw":
                            sw_origin = instruction[1];
                            instruction[1] = instruction[2];
                            instruction[2] = v1;
                            break;
                        case "jr":
                            instruction[2] = instruction[1];
                            break;
                        case "beq":
                            instruction[1] = v3;
                            instruction[3] = v1;
                            break;
                        case "bne":
                            instruction[1] = v3;
                            instruction[3] = v1;
                            break;
                        default:
                            break;


                    }
                   
                                  
                    if (op != "nop" && (Globals.ongoing.Contains(instruction[2]) || Globals.ongoing.Contains(instruction[3])))
                    {
                        Console.WriteLine("yep i am hereeeeeeeeeeeee");
                        dh = true;                                                                 
                        string[] ongoing = queuetoarraylocked(Globals.ongoing);
                                              
                        if (Globals.ongoing.Contains(instruction[2]))
                        {
                            index2 = Array.IndexOf(ongoing, instruction[2]);
                            if (ongoing[index2 - 1] == "lw"  )
                            {
                                if (index2 == 1)
                                    stall = true;
                                else
                                    v2_flag = true;
                            }
                            else
                                v2_flag = true;
                        }
                        if (Globals.ongoing.Contains(instruction[3]))
                        {

                            index3 = Array.IndexOf(ongoing, instruction[3]);
                            if (ongoing[index3 - 1] == "lw" )
                            {
                                if (index3 == 1)
                                    stall = true;
                                else
                                    v3_flag = true;
                            }
                            else
                                v3_flag = true;
                        }

                        if (stall)
                        {
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setdhtext(op+", Stall because "+v1+" is used but not yet loaded by lw")));
                            Globals.stall = true;
                            List<string> nop = new List<string>() { "nop", "$nop", "$nop", "$nop", instruction.Last() };
                            List<string> output = new List<string>();
                            foreach (string item in instruction)
                            {
                                output.Add(item);
                            }
                            pushFirst(output); // don't remember why I did it this way but otherwise it crashes                                                   
                            Globals.forwarding.SignalAndWait();
                            Globals.forwarding.SignalAndWait();
                            Globals.id_string = "ID: Stall because "+v1+" is used but not yet loaded by lw";
                            Globals.b.SignalAndWait();
                            Globals.idex.Push(nop);                            
                        }
                        else
                        {
                            if (v2_flag)
                            {
                                Console.WriteLine("yes i am here");
                                switch (index2)
                                {
                                    case 1:
                                        Globals.v2_hazard = "ex";
                                        break;
                                    case 3:
                                        Globals.v2_hazard = "mem";
                                        break;
                                    case 5:

                                        Globals.v2_hazard = "wb";
                                        break;
                                    default:
                                        break;
                                }

                            }
                            if (v3_flag)
                            {
                                switch (index3)
                                {
                                    case 1:
                                        Globals.v3_hazard = "ex";
                                        break;
                                    case 3:
                                        Globals.v3_hazard = "mem";
                                        break;
                                    case 5:
                                        Globals.v2_hazard = "wb";
                                        break;
                                    default:
                                        break;
                                }

                            }
                            // Checking in what part the instruction with the correct value is
                            Globals.forwarding.SignalAndWait();
                            Globals.forwarding.SignalAndWait();
                            //if (op == "sw" || op == "bne" || op == "beq" || op == "jr")
                            //{
                            //    v2_flag = false;
                            //    v3_flag = false;
                            //}
                            string dhtext = "";
                            if (v2_flag)
                            {
                                instruction[2] = Globals.v2_forward;
                                Globals.v2_hazard = "no";
                                
                                switch (op)
                                {
                                    case "sw":
                                        dhtext += v1 + " = " + instruction[2];
                                        break;
                                    case "jr":
                                        dhtext += v1 + " = " + instruction[2];
                                        break;
                                    default:
                                        dhtext += v2 + " = " + instruction[2];
                                        break;


                                }
                                
                            }
                            if (v3_flag)
                            {
                                instruction[3] = Globals.v3_forward;
                                Globals.v3_hazard = "no";
                                switch (op)
                                {
                                    case "beq":
                                        dhtext += v1 + " = " + instruction[3];
                                        break;
                                    case "bne":
                                        dhtext += v1 + " = " + instruction[3];
                                        break;
                                    default:
                                        dhtext += v3 + " = " + instruction[3];
                                        break;


                                }
                                
                                
                            }
                            if (dhtext != "")
                            {
                                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setforwardingtext(dhtext)));
                            }
                            

                        }
                    }
                    

                    switch (op)
                    {
                        case "add":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "addi":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();                        
                            break;
                        case "sub":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();                         
                            break;
                        case "beq":
                           
                            if (!v2_flag)
                                instruction[2] = Globals.registers[instruction[2]].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[instruction[3]].ToString();
                            switch (Globals.control_alg)
                            {
                                case "1-Bit":
                                    if (Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.id_branch =int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;

                                    }
                                    
                                    break;
                                case "2_Bit":
                                    if (Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.id_branch = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                                    }
                                    break;
                                case "Static":
                                    if (int.Parse(instruction[4])<0)
                                    {
                                        Globals.flush = true;
                                        Globals.id_branch = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "bne":
                            Console.WriteLine("i am at bne");
                            if (!v2_flag)
                                instruction[2] = Globals.registers[instruction[2]].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[instruction[3]].ToString();
                            switch (Globals.control_alg)
                            {
                                case "1-Bit":
                                    if (Globals.take_branch)
                                    {
                                        Console.WriteLine("1bit");
                                        Globals.flush = true;
                                        Globals.id_branch = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                                    }

                                    break;
                                case "2-Bit":
                                    if (Globals.take_branch)
                                    {
                                        Console.WriteLine("2bit");
                                        Globals.flush = true;
                                        Globals.id_branch = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                                    }
                                    break;
                                case "Static":
                                    if (int.Parse(instruction[1]) < 0)
                                    {
                                        Console.WriteLine("staticcccc");
                                        Globals.flush = true;
                                        Globals.id_branch = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                                    }
                                    break;
                                default:
                                    
                                    break;
                            }
                            Console.WriteLine("control alg is ");
                            Console.WriteLine(Globals.control_alg);
                            break;
                        case "slt":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "lw":
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "sw":                          
                            if (!v2_flag)
                                instruction[2] = Globals.registers[instruction[2]].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[instruction[3]].ToString();                          
                            instruction = new List<string>() { op, "$"+sw_origin, instruction[2], instruction[1], instruction[3], instruction.Last() };                                                                                     
                            break;
                        case "jr":                           
                            if (!v2_flag)
                                instruction[2] = Globals.registers[instruction[2]].ToString();
                            instruction[1] = "jrr";
                            Globals.flush=true;
                            Globals.id_branch = (int.Parse(instruction[2])-1)*4;                            
                            break;
                        case "and":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "or":
                            if (!v2_flag)
                                instruction[2] = Globals.registers[v2].ToString();
                            if (!v3_flag)
                                instruction[3] = Globals.registers[v3].ToString();
                            break;
                    }                  
                    if (v2_flag&&v3_flag)
                    {
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Setdhtext("need to forward " + v2 + "and "+v3+" because their value changed but not yet wrote back to registers")));
                    }
                    else
                    {
                        if (v2_flag)
                        {
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setdhtext("need to forward " + v2 + " because it's value changed but not yet wrote back to registers")));
                        }
                        if (v3_flag)
                        {
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setdhtext("need to forward " + v3 + " because it's value changed but not yet wrote back to registers")));
                        }
                    }
                   
                    string reglabel = "";
                    if (!v2_flag && op!="lw" && op != "jr" && op != "sw")
                        reglabel += v2 + " = " + Globals.registers[v2].ToString() + "\n";
                    if (!v3_flag && op != "addi" && op!="jr" && op!="bne" && op != "beq")
                        reglabel += v3 + " = " + Globals.registers[v3].ToString();
                    if (op == "jr" || op=="bne" || op=="beq")
                    {
                        reglabel += v1 + " = " + Globals.registers[v1];
                    }                   
                    Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegLabel(reglabel)));

                    if (!Globals.stall)
                    {
                        if (!dh)
                        {
                            Globals.forwarding.SignalAndWait();
                            Globals.forwarding.SignalAndWait();
                        }

                        string next = "ID: "+op+", ";
                        if (!v2_flag && op != "lw" && op != "jr" && op != "sw")
                        {
                            if (op!="addi")
                            {
                                next += v2 + " = " + Globals.registers[v2].ToString() + ", ";
                            }
                            else
                                next += v2 + " = " + Globals.registers[v2].ToString();
                        }
                        if (v2_flag)
                        {
                            if (op=="addi")
                            {
                                next += "Forwarded " + Globals.v2_forward ;
                            }
                            else
                                next += "Forwarded " + Globals.v2_forward + ", ";
                            
                        }
                        if (v3_flag)
                        {
                            next += "Forwarded " + Globals.v3_forward;
                        }

                        if (!v3_flag && op != "addi" && op != "jr" && op != "bne" && op != "beq")
                            next += v3 + " = " + Globals.registers[v3].ToString();
                        if (op == "jr" || op == "bne" || op == "beq")
                        {
                            next += v1 + " = " + Globals.registers[v1];
                        }
                        Globals.id_string = next;
                        Globals.b.SignalAndWait();
                        Globals.idex.Push(instruction);  
                    }
                }
                else
                {
                    Globals.id_string = "ID: Idle";
                    Globals.forwarding.SignalAndWait();
                    Globals.forwarding.SignalAndWait();
                    Globals.b.SignalAndWait();

                }     
                
                Globals.b_flush.SignalAndWait();
                if (Globals.flush)
                {
                    if (!Globals.ex)
                    {
                        Globals.id_tor.Clear();
                        Globals.ifid.Clear();
                        Globals.pc = Globals.id_branch;
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Set_id_branch("Taking branch")));
                    }
                    else
                    {
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Set_alu_branch("Wrong prediction. Taking 2nd branch")));
                        Globals.id_tor.Clear();
                        Globals.ifid.Clear();
                        Globals.idex.Clear();
                        Globals.pc = Globals.ex_branch;
                        Globals.ex = false;
                        Clean2();
                    }
                    Globals.flush = false;
                    if (Globals.cs && !Globals.final_flag)
                    { Globals.cs = false;
                        
                        Globals.cs_pc = 0;
                        Globals.b3.SignalAndWait();
                    }

                }
                
                Globals.b2.SignalAndWait();
                if (!Globals.stall)
                {
                    if (!Globals.flush)
                    {
                        queuedequeuelocked("in", instruction[0], instruction[1]);
                    }
                    else
                        queuedequeuelocked("in", "flush", "flush");

                }
                else
                {
                    
                    
                    queuedequeuelocked("in", "nop", "$nop");
                    Globals.stall = false;
                   
                }                                                                 
                Globals.b2.SignalAndWait();
            }
        }

        public static void EX()
        {
            while (Globals.run)
            {
                Globals.ex_string = "Ex: Idle";
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText("")));
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUdest("")));
                if (Globals.idex.Count != 0)
                {
                    List<string> instruction = Globals.idex.Pop();
                    int result;
                    List<string> output = new List<string>();
                    switch (instruction[0])
                    {
                        case "add":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            Globals.ex_string = instruction[2] + "+" + instruction[3] + "=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            break;
                        case "addi":
                          
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            Globals.ex_string=instruction[2] + "+" + instruction[3] + "=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            break;
                        case "nop":
                            output = new List<string>() { instruction[0], instruction[1], instruction.Last() };
                            Globals.ex_string = "nop";
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText("")));
                            break;
                        case "sub":
                            result = int.Parse(instruction[2]) - int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            Globals.ex_string = instruction[2] + "-" + instruction[3] + "=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));

                            break;
                        case "beq":
                            Globals.ex_string = instruction[3] + "-" + instruction[2] + "==0";
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            if (int.Parse(instruction[3]) - int.Parse(instruction[2]) == 0)
                                instruction[2] = "equal";
                            else
                                instruction[2] = "not";

                            
                            int dest = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                            switch (Globals.control_alg)
                            {
                                case "Static":
                                    if (instruction[2] == "equal" && int.Parse(instruction[1])>0)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = dest;

                                        
                                    }
                                    if (instruction[2] == "not" && int.Parse(instruction[1]) < 0)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = int.Parse(instruction[4]) + 4;
                                        if (Globals.ex_branch >= Globals.instructionList.Count)
                                        {
                                            Globals.procs[Globals.cur_pcb].SetState("dead");
                                            Globals.bTimer.Stop();
                                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
                                            Globals.not_complete = false;
                                            t.Start();
                                            Globals.final_flag = true;
                                        }
                                    }
                                    break;
                                default:
                                    if (instruction[2]=="equal" && !Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = dest;
                                        Globals.predictor ++;
                                    }
                                    if (instruction[2] == "not" && Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = int.Parse(instruction[4]) + 4;
                                        Globals.predictor ++;
                                        if (Globals.ex_branch >= Globals.instructionList.Count)
                                        {
                                            Globals.procs[Globals.cur_pcb].SetState("dead");
                                            Globals.bTimer.Stop();
                                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
                                            Globals.not_complete = false;
                                            t.Start();
                                            Globals.final_flag = true;
                                        }
                                    }
                                    if (Globals.control_alg=="1-Bit")
                                    {
                                        Globals.take_branch = !Globals.take_branch;
                                    }
                                    if (Globals.control_alg == "2-Bit" && Globals.predictor == 2)
                                    {
                                        Globals.take_branch = !Globals.take_branch;
                                        Globals.predictor = 0;
                                    }
                                    break;
                            }
                         

                            output = new List<string>() { instruction[0], dest.ToString(), instruction[2], instruction.Last() };
                            break;
                        case "bne":
                            Globals.ex_string = instruction[3] + "-" + instruction[2] + "!=0";
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            if (int.Parse(instruction[3]) - int.Parse(instruction[2]) == 0)
                                instruction[2] = "equal";
                            else
                                instruction[2] = "not";
                            
                            int dest2 = int.Parse(instruction[4]) + int.Parse(instruction[1]) * 4;
                            switch (Globals.control_alg)
                            {
                                case "Static":
                                    if (instruction[2] == "not" && int.Parse(instruction[1]) > 0)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = dest2;

                                    }
                                    if (instruction[2] == "equal" && int.Parse(instruction[1]) < 0)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = int.Parse(instruction[4]) + 4;
                                        Console.WriteLine("ex branch is "+Globals.ex_branch);
                                        Console.WriteLine(Globals.instructionList.Count);
                                        if (Globals.ex_branch >= Globals.instructionList.Count)
                                        {
                                            Globals.procs[Globals.cur_pcb].SetState("dead");
                                            Globals.bTimer.Stop();
                                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
                                            Globals.not_complete = false;
                                            t.Start();
                                            Globals.final_flag = true;
                                        }
                                    }
                                    break;
                                default:
                                    if (instruction[2] == "not" && !Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = dest2;
                                        Globals.predictor++;
                                    }
                                    if (instruction[2] == "equal" && Globals.take_branch)
                                    {
                                        Globals.flush = true;
                                        Globals.ex = true;

                                        Globals.ex_branch = int.Parse(instruction[4]) + 4;
                                        Globals.predictor++;
                                        if (Globals.ex_branch >= Globals.instructionList.Count)
                                        {
                                            Globals.procs[Globals.cur_pcb].SetState("dead");
                                            Globals.bTimer.Stop();
                                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
                                            Globals.not_complete = false;
                                            t.Start();
                                            Globals.final_flag = true;
                                        }
                                    }
                                    if (Globals.control_alg == "1-Bit")
                                    {
                                        Globals.take_branch = !Globals.take_branch;
                                    }
                                    if (Globals.control_alg == "2-Bit" && Globals.predictor == 2)
                                    {
                                        Globals.take_branch = !Globals.take_branch;
                                        Globals.predictor = 0;
                                    }
                                    break;
                            }
                            output = new List<string>() { instruction[0], dest2.ToString(), instruction[2], instruction.Last() };
                            break;
                        case "slt":
                            if (int.Parse(instruction[2]) < int.Parse(instruction[3]))
                            {
                                instruction[2] = "1";
                                Globals.ex_string = instruction[2] + "<" + instruction[3];
                                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));

                            }

                            else
                            {
                                Globals.ex_string = instruction[2] + ">" + instruction[3];
                                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                                instruction[2] = "0";
                            }
                                
                            output = instruction;
                            break;
                        case "lw":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            Globals.ex_string = instruction[2] + "+" + instruction[3] + "=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "sw":
                            result = int.Parse(instruction[4]) + int.Parse(instruction[3]);
                            
                            output = new List<string>() { instruction[0], instruction[1], instruction[2], result.ToString(), instruction.Last() };
                            Globals.ex_string = instruction[4] + "+" + instruction[3] + "=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            break;
                        case "jr":
                            output = instruction;
                            result = ((int.Parse(output[2]) - 1) * 4);
                            Globals.ex_string = "((" + instruction[2] + "-1)*4)=" + result.ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            output[2] = result.ToString();
                            
                            break;
                        case "and":
                            logical binary_num = new logical(instruction[2]);
                            output = new List<string>() { instruction[0], instruction[1], binary_num.logical_and(instruction[3]), instruction.Last() };
                            Globals.ex_string = instruction[2] + "&&" + instruction[3] + "=" + binary_num.logical_and(instruction[3]).ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            break;
                        case "or":
                            logical binary_num1 = new logical(instruction[2]);
                            output = new List<string>() { instruction[0], instruction[1], binary_num1.logical_or(instruction[3]), instruction.Last() };
                            Globals.ex_string = instruction[2] + "||" + instruction[3] + "=" + binary_num1.logical_or(instruction[3]).ToString();
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetALUText(Globals.ex_string)));
                            break;
                    }
                    Globals.ex_string = "EX: " + output[0] +", " + Globals.ex_string;
                    Globals.forwarding.SignalAndWait();
                    if (Globals.v2_hazard == "ex")
                    {
                        Globals.v2_forward = output[2];
                        Globals.ex_string+=", Forwarded "+output[2];
                        
                    }
                    if (Globals.v3_hazard == "ex")
                    {
                        Globals.v3_forward = output[2];
                        Globals.ex_string += ", Forwarded " + output[2];

                    }
                    Globals.forwarding.SignalAndWait();
                    Globals.b.SignalAndWait();
                    Globals.exmem.Push(output);
                }
                else
                {
                    
                    Globals.forwarding.SignalAndWait();
                    Globals.forwarding.SignalAndWait();
                    Globals.b.SignalAndWait();

                }
                Globals.b_flush.SignalAndWait();
                Globals.b2.SignalAndWait();
                
                Globals.b2.SignalAndWait();
            }
        }

        public static void MEM()
        {
            while (Globals.run)
            {
                Globals.mem_string = "MEM: ";
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setmemtext("")));
                
                if (Globals.exmem.Count != 0)
                {
                    
                    List<string> instruction = Globals.exmem.Pop();
                    Globals.mem_string += instruction[0];
                    if (instruction[0] == "lw")
                    {
                        int address = int.Parse(instruction[2]);
                        int value = Globals.ram[address];
                        instruction = new List<string>() { instruction[0], instruction[1], value.ToString(), instruction.Last() };
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Setmemtext("Loading value:" + value.ToString() + " to " + instruction[1] + " from RAM[" + address.ToString() + "]")));
                        Globals.mem_string += ", Loading value:" + value.ToString() + " to " + instruction[1] + " from RAM[" + address.ToString() + "]";


                    }

                    if (instruction[0] == "sw")
                    {
                        int address = int.Parse(instruction[3]);
                        Globals.ram[address] = int.Parse(instruction[2]);
                        Console.WriteLine("savingg " + instruction[2] +" to address " +address);
                        int value = Globals.ram[address];
                        string register = instruction[1].Substring(1, instruction[1].Length - 1);
                        instruction = new List<string>() { instruction[0], instruction[1], value.ToString(), instruction.Last() };
                        Globals.mem_string += ", Storing value:" + instruction[2].ToString() + " from " + register + " to RAM[" + address.ToString() + "]";
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Setmemtext("Storing value:" + instruction[2].ToString() + " from " + register + " to RAM[" + address.ToString() + "]")));
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.SetMemory(instruction[2],address.ToString())));



                    }

                    Globals.forwarding.SignalAndWait();
                    if (Globals.v2_hazard == "mem")
                    {
                        Globals.v2_forward = instruction[2];
                       
                        Globals.mem_string += ", Forwarded " + instruction[2];
                        
                        


                    }
                    if (Globals.v3_hazard == "mem")
                    {
                        Globals.v3_forward = instruction[2];
                        Globals.mem_string += ", Forwarded " + instruction[2];


                    }
                    Globals.forwarding.SignalAndWait();
                    Globals.b.SignalAndWait();
                    
                    Globals.memwb.Push(instruction);
                }
                else
                {
                    Globals.mem_string = "MEM: Idle";
                    Globals.forwarding.SignalAndWait();
                    Globals.forwarding.SignalAndWait();
                    Globals.b.SignalAndWait();
                }
                
                
                Globals.b2.SignalAndWait();
              
                Globals.b2.SignalAndWait();
            }
        }

        public static void WB()
        {
            while (Globals.run)
            {
                Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext("")));
                if (Globals.memwb.Count != 0)
                {

                    List<string> instruction = Globals.memwb.Pop();
                    Globals.forwarding.SignalAndWait();

                    if (Globals.v2_hazard == "wb")
                    {
                        Globals.v2_forward = instruction[2];

                        //Globals.wb_string += ", Forwarded " + instruction[2];




                    }
                    if (Globals.v3_hazard == "wb")
                    {
                        Globals.v3_forward = instruction[2];
                        //Globals.mem_string += ", Forwarded " + instruction[2];


                    }
                    Globals.forwarding.SignalAndWait();
                    switch (instruction[0])
                    {
                        case "lw":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string =   instruction[1] + "=" + instruction[2];

                            break;
                        case "sw":
                            Globals.b.SignalAndWait();
                            Globals.wb_string = "WB: sw";
                            break;
                        case "nop":
                            Globals.b.SignalAndWait();
                            Globals.wb_string = "WB: nop";
                            break;
                        case "add":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string =  instruction[1] + "=" + instruction[2];

                            break;
                        case "addi":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string = instruction[1] + "=" + instruction[2];
                            break;
                        case "sub":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string =  instruction[1] + "=" + instruction[2];

                            break;
                        case "jr":
                            Globals.b.SignalAndWait();
                           
                            Globals.wb_string="WB: jr";
                            break;
                        case "beq":
                          
                            Globals.wb_string = "WB: beq";
                            break;
                        case "bne":
                           
                            Globals.wb_string = "WB: bne";

                            Globals.b.SignalAndWait();
                            break;
                        case "slt":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string =  instruction[1] + "=" + instruction[2];

                            break;
                        case "and":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string =  instruction[1] + "=" + instruction[2];

                            break;
                        case "or":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRegisters(instruction[1], instruction[2])));
                            Form1.my_form.Invoke(new Action(() => Form1.my_form.Setwbtext(instruction[1] + "=" + instruction[2])));
                            Globals.wb_string = instruction[1] + "=" + instruction[2];

                            break;
                    }
                    if (instruction[1]!="bne" && instruction[1] != "beq" && instruction[1] != "jr" && instruction[1] != "nop" && instruction[1] != "sw")
                    {
                        Globals.wb_string = "WB: " + instruction[0] + ", " + Globals.wb_string;
                    }
                    string[] arr = { instruction[0], instruction.Last() };
                    Globals.wb_ins = new List<string>(arr);
                    // split wb instead of repeating it and add op
                }
                else
                {
                    Globals.wb_string = "WB: Idle";
                    Globals.forwarding.SignalAndWait();
                    Globals.forwarding.SignalAndWait();
                    string[] arr = { "no" };
                    Globals.wb_ins = new List<string>(arr);
                    Globals.b.SignalAndWait();
                }
              
                Globals.b_flush.SignalAndWait();
                
                Globals.b2.SignalAndWait();
              
                
                queuedequeuelocked("out", "", "");
                
                Globals.b2.SignalAndWait();
                lock (Globals.ongoingLock)
                {
                    foreach (string item in Globals.ongoing)
                    {
                        Console.WriteLine(item);
                    }
                }

            }
        }

        public static void MainRun(string alg)
        {
            Globals.run = true;
            Console.WriteLine("the real alg is "+alg);
            Globals.ongoing.Enqueue("lol");
            Globals.ongoing.Enqueue("lol");
            Globals.ongoing.Enqueue("lol");
            Globals.ongoing.Enqueue("lol");
            Globals.ongoing.Enqueue("lol");
            Globals.ongoing.Enqueue("lol");
          
            System.IO.File.WriteAllText("log.txt", string.Empty);
            string[] array = { "TextFile1.txt", "TextFile2.txt" };
            List<string> names = new List<string>(array);
            Globals.ram[100] = 6;

            pcb_creation(file_load_gui());
            Thread t = new Thread(() => choose_pcb(alg, "reg"));
            Console.WriteLine("alg is "+Globals.alg);
            t.Start();

            Thread ifThread = new Thread(IF);
            Thread idThread = new Thread(ID);
            Thread exThread = new Thread(EX);
            Thread memThread = new Thread(MEM);
            Thread wbThread = new Thread(WB);

            Globals.stopWatch.Start();
            Thread stopwatch_thread = new Thread(Update_runtime);
            stopwatch_thread.Start();

            ifThread.Start();
            idThread.Start();
            exThread.Start();
            memThread.Start();
            wbThread.Start();

            SetTimer();
        }
        public static void StepRun()
        {
            if (!Globals.started)
            {
                Globals.run = true;
                Globals.started = true;
                Globals.ongoing.Enqueue("lol");
                Globals.ongoing.Enqueue("lol");
                Globals.ongoing.Enqueue("lol");
                Globals.ongoing.Enqueue("lol");
                Globals.ongoing.Enqueue("lol");
                Globals.ongoing.Enqueue("lol");

                System.IO.File.WriteAllText("log.txt", string.Empty);
                string[] array = { "TextFile1.txt", "TextFile2.txt" };
                List<string> names = new List<string>(array);
                Globals.ram[100] = 6;

                pcb_creation(file_load_gui());
                Thread t = new Thread(() => choose_pcb(Globals.alg, "reg"));
                Console.WriteLine("alg is " + Globals.alg);
                t.Start();

                Thread ifThread = new Thread(IF);
                Thread idThread = new Thread(ID);
                Thread exThread = new Thread(EX);
                Thread memThread = new Thread(MEM);
                Thread wbThread = new Thread(WB);

                 
                
                ifThread.Start();
                idThread.Start();
                exThread.Start();
                memThread.Start();
                wbThread.Start();
            }
            else
            {
                if (Globals.ifid.Count + Globals.idex.Count + Globals.exmem.Count + Globals.memwb.Count != 0)
                {
                    
                    Globals.b2.SignalAndWait();
                    Form1.my_form.Invoke(new Action(() => Form1.my_form.Addtolog(Globals.if_string + "\n" + Globals.id_string + "\n" + Globals.ex_string + "\n" + Globals.mem_string + "\n" + Globals.wb_string + "\n")));

                    Globals.b2.SignalAndWait();



                }
                else
                {
                    if (Globals.run)
                    {
                        Form1.my_form.Invoke(new Action(() => Form1.my_form.Addtolog(Globals.if_string + "\n" + Globals.id_string + "\n" + Globals.ex_string + "\n" + Globals.mem_string + "\n" + Globals.wb_string + "\n")));

                        Console.WriteLine("bish im here");
                        Globals.run = false;
                        Globals.aTimer.Stop();
                        end();
                        

                        Globals.b2.SignalAndWait();
                       
                        Globals.bTimer.Stop();
                        
                    }
                }
            }
             
            

            
        }
        public static List<List<string>> file_load(List<string> file_names)
        {
            List<List<string>> instructions = new List<List<string>>();
            foreach (string name in file_names)
            {
                string[] lines = File.ReadAllLines(name);
                List<string> lines2 = new List<string>(lines);
                instructions.Add(lines2);
            }

            return instructions;
        }
        public static List<List<string>> file_load_gui()
        {
            List<List<string>> instructions = new List<List<string>>();
            foreach (TabPage tab in project_gui.Form1.my_form.instructiontabs.TabPages)
            {
                string[] lines = tab.Controls[0].Text.Split('\n');
                List<string> lines2 = new List<string>(lines);
                instructions.Add(lines2);

            }
            return instructions;
        }
        public static void pcb_creation(List<List<string>> instructions)
        {
            int count = 0;
            foreach (List<string> single_pcb in instructions)
            {
                if (single_pcb[0] == "set")
                {
                    List<string> reg_set = new List<string>();
                    int index = 0;
                    while (single_pcb[index] != "$")
                    {
                        reg_set.Add(single_pcb[index]);
                       
                        index++;

                    }

                    single_pcb.RemoveRange(0, index + 1);
                    pcb new_pcb = new pcb(single_pcb, "ready", reg_set, count);
                    Globals.procs[count] = new_pcb;
                }
                else
                {
                    pcb new_pcb = new pcb(single_pcb, "ready", count);
                    Globals.procs[count] = new_pcb;
                }

                count++;
            }
        }
        public static void addProc(List<string> instructions)
        {
            Console.WriteLine("first");
            foreach (string item in instructions)
            {
                Console.WriteLine(item);
                Console.WriteLine(item.Length);
            }
            Console.WriteLine("last");
                int count = 0;
                while (Globals.procs[count] != null)
                {
                    count++;
               
                }
                if (instructions[0] == "set")
                {
                Console.WriteLine("we know we need to set");
                    List<string> reg_set = new List<string>();
                    int index = 0;
                    while (instructions[index] != "$")
                    {
                        reg_set.Add(instructions[index]);

                        index++;

                    }
           
                    instructions.RemoveRange(0, index + 1);
               
              
                    pcb new_pcb = new pcb(instructions, "ready", reg_set, count);
            
                    Globals.procs[count] = new_pcb;
                   
                }
                else
                {
                Console.WriteLine("instructions0 is " + instructions[0]);
                Console.WriteLine("we dont know!");
                    pcb new_pcb = new pcb(instructions, "ready", count);
                    Globals.procs[count] = new_pcb;
                }
            Console.WriteLine("add proc finished");
                
           
           




        }

        public static void choose_pcb(string algorithm, string load)
        {


            Globals.alg = algorithm;
            Globals.cs = true;
            
            int next_pcb = Globals.cur_pcb + 1;
            int count = 0;
            bool exist = true;
            Console.WriteLine("dolly");
            Console.WriteLine("alg is "+algorithm );
            switch (algorithm)
            {
                case "rr":
                    if (true)
                    {
                        bool kepp_search = true;
                        while (kepp_search)
                        {
                            count++;
                            if (count >= Globals.procs.Length )
                            {
                                kepp_search = false;
                                exist = false;
                                

                            }

                            if (Globals.procs[next_pcb] != null && kepp_search)
                            {
                                if (Globals.procs[next_pcb].GetState() == "ready")
                                    kepp_search = false;

                                
                            }
                            if (kepp_search)
                            {
                                next_pcb++;
                                if (next_pcb == Globals.procs.Length)
                                {
                                    next_pcb = 0;
                                }
                            }
                        }
                        

                    }
                    if (load == "reg")
                    {

                        if (exist)
                        {
                            pcb_load_to_reg(next_pcb, "rr");
                        }
                        else
                        {
                            Globals.cs = false;
                            Globals.ended = true;
                        }

                    }
                    else
                    {
                        if (exist)
                        {
                            Console.WriteLine("it exists!!");
                            Console.WriteLine("next pcb is "+next_pcb);
                            pcb_load_to_mem(Globals.cur_pcb, algorithm);
                        }
                        else
                        {
                            Console.WriteLine("it does not exist!!!!!!!");
                            if (Globals.not_complete==true)
                            {
                                Globals.cs = false;
                                
                            }
                            else
                                pcb_load_to_mem(Globals.cur_pcb, algorithm);
                        }
                    }
                        
                    break;
                case "fcfs":
                    if (load == "reg")
                    {
                        bool flag = true;
                        while (flag)
                        {
                            count++;
                            if (count > Globals.procs.Length + 2)
                            {
                                flag = false;
                                exist = false;
                                Globals.cs = false;
                                Globals.ended = true;

                            }

                            if (Globals.procs[next_pcb] != null)
                            {
                                if (Globals.procs[next_pcb].GetState() == "ready")
                                    flag = false;

                                if (Globals.cur_pcb == next_pcb)
                                {
                                    exist = false;
                                    Globals.cs = false;
                                    
                                }
                            }
                            if (flag)
                            {
                                next_pcb++;
                                if (next_pcb >= Globals.procs.Length)
                                {
                                    next_pcb = 0;
                                }
                            }
                        }
                        if (exist)
                        {
                            pcb_load_to_reg(next_pcb, "fcfs");
                        }
                    }
                    else
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
                    break;
                case "sjn":
                    if (load == "reg")
                    {
                        int max_index = 0;
                        int max_value = Globals.procs[0].GetSize() - Globals.procs[0].GetPc();
                        int size = 0;
                        for (int i = 1; i < Globals.procs.Length; i++)
                        {
                            if (Globals.procs[i] != null)
                            {
                                if (Globals.procs[i].GetState() != "dead")
                                {
                                    size = Globals.procs[i].GetSize() - Globals.procs[i].GetPc();
                                    if (size < max_value)
                                    {
                                        max_value = size;
                                        max_index = i;
                                    }
                                }
                            }
                        }
                        if (Globals.procs[max_index].GetSize() - Globals.procs[max_index].GetPc() > 0)
                        {

                            pcb_load_to_reg(max_index, "sjn");
                        }
                        else
                        { 
                            Globals.cs = false;
                            Globals.ended = true;
                        }
                           
                    }
                    else
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
                    break;
                case "srt":
                    if (load == "reg")
                    {
                        int max_index = 0;
                        int max_value = Globals.procs[0].GetSize() - Globals.procs[0].GetPc();
                        int size = 0;
                        Console.WriteLine("max value is" + max_value);
                        for (int i = 1; i < Globals.procs.Length; i++)
                        {

                            if (Globals.procs[i] != null)
                            {
                                Console.WriteLine("i did go in");
                                if (Globals.procs[i].GetState() != "dead")
                                {
                                    Console.WriteLine("also here");
                                    Console.WriteLine("bushhh");
                                    size = Globals.procs[i].GetSize() - Globals.procs[i].GetPc();
                                    Console.WriteLine("size is " + size);
                                    if (size < max_value)
                                    {
                                        Console.WriteLine("i exchanged");
                                        max_value = size;
                                        max_index = i;
                                    }
                                }
                            }
                        }
                        if (Globals.procs[max_index].GetSize() - Globals.procs[max_index].GetPc() > 0)
                        {

                            pcb_load_to_reg(max_index, "srt");
                            Console.WriteLine("the index is " + max_index);
                        }
                        else
                        {
                            Globals.cs = false;
                            Globals.ended = true;
                        }

                    }
                    else
                    {
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
                    }
                       
                    break;
                default:
                    break;
            }
        }
        public static void pcb_load_to_mem(int pcb_num, string alg)
        {
            Globals.mem_reg = "mem";
            Globals.bTimer.Stop();
            Globals.cs = true;
            
            int offset = Globals.cur_pcb * 15;
            Console.WriteLine("loading to mem with offset " + offset);
            List<string> instructions = new List<string>();
            foreach (string item in Globals.load_to_pcb)
            {
                instructions.Add(item.Replace("offset", offset.ToString()));
            }
            Globals.cs_instructions = interpreter(instructions, Globals.re1, Globals.re2, Globals.re3);
            Globals.procs[Globals.cur_pcb].SetPc(Globals.pc);
            Console.WriteLine("negro");
            Globals.b3.SignalAndWait();
            Console.WriteLine("yes we are here");
            Globals.b3.SignalAndWait();
            Console.WriteLine("finished loading to memory");
            if (Globals.cs)
                choose_pcb(alg, "reg");
        }

        public static void pcb_load_to_reg(int pcb_num, string alg)
        {
            Globals.mem_reg = "reg";
            int offset = pcb_num * 15;
            Console.WriteLine("loading to reg with offset " + offset);
            List<string> instructions = new List<string>();
            foreach (string item in Globals.load_to_reg)
            {
                instructions.Add(item.Replace("offset", offset.ToString()));
                
            }
            Globals.cs_instructions = interpreter(instructions, Globals.re1, Globals.re2, Globals.re3);
           
            Globals.instructionList = interpreter(Globals.procs[pcb_num].GetIns(), Globals.re1, Globals.re2, Globals.re3);

            Globals.b3.SignalAndWait();
            Globals.b3.SignalAndWait();
            Globals.pc = Globals.procs[pcb_num].GetPc();
            Console.WriteLine("finished loading to regs");
            
            Globals.cur_pcb = pcb_num;
            Form1.my_form.Invoke(new Action(() => Form1.my_form.SetSelectedTab(Globals.cur_pcb)));
            Globals.cs = false;
            
            if (alg == "rr")
            {
                if (Globals.first_time)
                {
                    SetTimer_cs();
                    Globals.first_time = false;
                }
                else
                    Globals.bTimer.Start();
            }
            if (Globals.srt_flag)
            {
                Globals.cs = true;
                Console.WriteLine("righttttttt");
                int index = 0;
                while (Globals.procs[index]!=null)
                {
                    index++;
                }
                index--;
                Thread t2 = new Thread(() => MIPS.Program.pcb_load_to_reg(index,"srt"));
                t2.Start();
                Globals.srt_flag = false;
            }
            
        }

        public static void end()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("RAM at " + i + "is " + Globals.ram[i]);
            }
            

        }

        private static void SetTimer()
        {
            Globals.aTimer.Start();
            Globals.aTimer.Elapsed += OnTimedEvent;
            Globals.aTimer.AutoReset = true;
            Globals.aTimer.Enabled = true;
        }
        
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (Globals.ifid.Count + Globals.idex.Count + Globals.exmem.Count + Globals.memwb.Count != 0)
            {
              
                Globals.b2.SignalAndWait();
               
            }
            else
            {
                if (Globals.run)
                {
                    
                    Globals.run = false;
                    Globals.aTimer.Stop();
                    end();
                    Globals.b2.SignalAndWait();
                    
                    Globals.bTimer.Stop();
                    Form1.my_form.Invoke(new Action(() => Form1.my_form.Setlabelcs("")));

                }
            }
            
            Console.WriteLine("pc is " + Globals.pc);
            TimeSpan ts = Globals.stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Form1.my_form.Invoke(new Action(() => Form1.my_form.Addtolog("RunTime " + elapsedTime+"\n"+Globals.if_string+"\n" + Globals.id_string + "\n" + Globals.ex_string + "\n" + Globals.mem_string + "\n" + Globals.wb_string + "\n")));


            string if_string = "";
            string id_string = "";
            string ex_string = "";
            string mem_string = "";
            string wb_string = "";
            string line = "-------------------";
            if (Globals.ifid.Count != 0)
            {

                string ifid_ins = "";
                foreach (string item in Globals.ifid.Peek())
                {
                    ifid_ins += item;
                    ifid_ins += " ";
                }
                
                string pcb_num = Globals.ifid.Peek().Last();
                if_string = "if worked on instruction " + ifid_ins + " pcb " + pcb_num;

            }
            else
                if_string = "if was idle";
            Console.WriteLine(if_string);
            if (Globals.idex.Count != 0)
            {
                string idex_ins = "";
                foreach (string item in Globals.idex.Peek())
                {
                    idex_ins += item;
                    idex_ins += " ";
                }
                string pcb_num = Globals.idex.Peek().Last();
                id_string = "id worked on instruction " + idex_ins + " pcb " + pcb_num;

            }
            else
                id_string = "id was idle";
            Console.WriteLine(id_string);
            if (Globals.exmem.Count != 0)
            {
                string exmem_ins = "";
                foreach (string item in Globals.exmem.Peek())
                {
                    exmem_ins += item;
                    exmem_ins += " ";
                }
                string pcb_num = Globals.exmem.Peek().Last();
                ex_string = "ex worked on instruction " + exmem_ins + " pcb " + pcb_num;

            }
            else
                ex_string = "ex was idle";
            Console.WriteLine(ex_string);
            if (Globals.memwb.Count != 0)
            {
                string memwb_ins = "";
                foreach (string item in Globals.memwb.Peek())
                {
                    memwb_ins += item;
                    memwb_ins += " ";
                }
                string pcb_num = Globals.memwb.Peek().Last();
                mem_string = "mem worked on instruction " + memwb_ins + " pcb " + pcb_num;

            }
            else
                mem_string = "mem was idle";
            Console.WriteLine(mem_string);
            if (Globals.wb_ins[0] == "no")
            {
                wb_string = "wb was idle";
            }
            else
            {
                wb_string = "wb worked on instruction " + Globals.wb_ins[0] + " pcb " + Globals.wb_ins.Last();
            }
            Console.WriteLine(wb_string);
            Console.WriteLine(line);
           

            //Action<object> action = (object obj) =>
            //{

            //    string[] readText = File.ReadAllLines(obj.ToString(), Encoding.UTF8);
            //    string[] newText = new string[] { if_string, id_string, ex_string, mem_string, wb_string, line };
            //    string[] combined = readText.Concat(newText).ToArray();
            //    File.WriteAllLines(obj.ToString(), combined, Encoding.UTF8);
            //};
            

            //Task t1 = new Task(action, "log.txt");
            //t1.Start();
            string[] readText = File.ReadAllLines("log.txt", Encoding.UTF8);
            string[] newText = new string[] { if_string, id_string, ex_string, mem_string, wb_string, line };
            string[] combined = readText.Concat(newText).ToArray();
            File.WriteAllLines("log.txt", combined, Encoding.UTF8);


            Globals.b2.SignalAndWait();
        }

        private static void SetTimer_cs()
        {
            Console.WriteLine("set timer cs");
            Globals.bTimer.Start();
            Globals.bTimer.Elapsed += OnTimedEvent_cs;
            Globals.bTimer.AutoReset = true;
            Globals.bTimer.Enabled = true;
        }
        public static void Update_runtime()
        {
            while (Globals.run)
            {
                TimeSpan ts = Globals.stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Form1.my_form.Invoke(new Action(() => Form1.my_form.SetRunTime(elapsedTime.ToString())));
            }
        }
        private static void OnTimedEvent_cs(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("event cs");
            if (Globals.run)
            {
                choose_pcb("rr", "mem");
            }
        }
        public static void PrintRegs()
        {
            string first = String.Format("$v0:{0} $v1:{1}", Globals.registers["$v0"], Globals.registers["$v1"]);
            string second = String.Format("$a0:{0} $a1:{1} $a2:{2} $a3:{3}", Globals.registers["$a0"], Globals.registers["$a1"], Globals.registers["$a2"], Globals.registers["$a3"]);
            string third = String.Format("$t0:{0} $t1:{1} $t2:{2} $t3:{3}", Globals.registers["$t0"], Globals.registers["$t1"], Globals.registers["$t2"], Globals.registers["$t3"]);
            Console.WriteLine(first);
            Console.WriteLine(second);
            Console.WriteLine(third);
        }

        public static void pushFirst(List<string> instruction)
        {
            if (Globals.id_tor.Count == 0)
            {
                Globals.id_tor.Enqueue(instruction);
            }
            else
            {
                List<string>[] strings = Globals.id_tor.ToArray();
                Globals.id_tor.Clear();
                Globals.id_tor.Enqueue(instruction);
                foreach (List<string> item in strings)
                {
                    Globals.id_tor.Enqueue(item);
                }
            }
           
        }
        public static string[] queuetoarraylocked(Queue<string> tor)
        {
            lock (Globals.ongoingLock)
            {
                string[] ongoing = Globals.ongoing.ToArray();
                Array.Reverse(ongoing);
                return ongoing;
            }

        }

        public static void queuedequeuelocked(string inorout, string input1, string input2)
        {
            lock (Globals.ongoingLock)
            {
                if (inorout == "in")
                {
                    Globals.ongoing.Enqueue(input2);
                    Globals.ongoing.Enqueue(input1);
                }
                else
                {
                    

                       
                            Globals.ongoing.Dequeue();
                            Globals.ongoing.Dequeue();
                        
                        
                    
                }
            }
        }
        public static void Clean2()
        {
            lock (Globals.ongoingLock)
            {
                string[] ongoing = Globals.ongoing.ToArray();
                Globals.ongoing.Clear();
                for (int i = 0; i < ongoing.Length-2; i++)
                {
                    Globals.ongoing.Enqueue(ongoing[i]);
                }
                Globals.ongoing.Enqueue("clean");
                Globals.ongoing.Enqueue("clean");


            }

        }
    }
}
