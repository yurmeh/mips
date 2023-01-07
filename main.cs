using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Timers;
using MIPS;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ConsoleApp98
{
    class Program
    {
        static void Main(string[] args)
        {



            MainRun(1);




        }
        public static List<string> interpreter(List<string> ins_list, string re1, string re2, string re3)
        {
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
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",5 },{"$a3",5 }, {"$t0",5 },{"$t1",0 },{"$t2",2 },{"$t3",3 },{"$t4",0 },{"$t5",0 },{"$t6",1 },{"$t7",2},
                {"$s0",0 },{"$s1",0 },{"$s2",0 },{"$s3",0 },{"$s4",0 },{"$s5",0 },{"$s6",0 },{"$s7",0 },{"$t8",0 },{"$t9",0 },{"$gp",0 },{"$fp",0 },{"$sp",0 },{"$ra",0 }

            };
           
            public static int pc = 0;
            public static bool first_time = true;
            public static int count = 0;
            public static int newcount = 0;
            public static bool cs = false;
            public static List<string> load_to_pcb = new List<string> { "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","addi $zero,$zero,offset","sw $v0,0($zero)", "sw $v1,1($zero)", "sw $a0,2($zero)", "sw $a1,3($zero)", "sw $a2,4($zero)", "sw $a3,5($zero)", "sw $t0,6($zero)",
            "sw $t1,7($zero)","sw $t2,8($zero)","sw $t3,9($zero)","sw $t4,10($zero)","sw $t5,11($zero)","sw $t6,12($zero)","sw $t7,13($zero)","sw $s0,14($zero)","sw $s1,15($zero)","sw $s2,16($zero)",
            "sw $s3,17($zero)","sw $s4,18($zero)","sw $s5,19($zero)","sw $s6,20($zero)","sw $s7,21($zero)","sw $t8,22($zero)","sw $t9,23($zero)","sw $gp,24($zero)","sw $fp,25($zero)","sw $sp,26($zero)",
            "sw $ra,27($zero)","sub $zero,$zero,$zero"};
            public static List<string> load_to_reg = new List<string> { "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","addi $zero,$zero,offset","lw $v0,0($zero)", "lw $v1,1($zero)", "lw $a0,2($zero)", "lw $a1,3($zero)", "lw $a2,4($zero)", "lw $a3,5($zero)", "lw $t0,6($zero)",
            "lw $t1,7($zero)","lw $t2,8($zero)","lw $t3,9($zero)","lw $t4,10($zero)","lw $t5,11($zero)","lw $t6,12($zero)","lw $t7,13($zero)","lw $s0,14($zero)","lw $s1,15($zero)","lw $s2,16($zero)",
            "lw $s3,17($zero)","lw $s4,18($zero)","lw $s5,19($zero)","lw $s6,20($zero)","lw $s7,21($zero)","lw $t8,22($zero)","lw $t9,23($zero)","lw $gp,24($zero)","lw $fp,25($zero)","lw $sp,26($zero)",
            "lw $ra,27($zero)","sub $zero,$v0,$v0"};
            
            public static int cs_pc = 0;
            public static List<string> cs_instructions = new List<string>();
            public static string re1 = @"(\w+) (\S+),(\S+),(\S+)"; //add sub registers
            public static string re2 = @"(\w+) (\S+),(\S+)\((\S+)\)"; //lw
            public static string re3 = @"(\w+) (\S+)";  // jump
            public static Stack<List<string>> ifid = new Stack<List<string>>();
            public static Stack<List<string>> idex = new Stack<List<string>>();
            public static Stack<List<string>> exmem = new Stack<List<string>>();
            public static Stack<List<string>> memwb = new Stack<List<string>>();
            public static Barrier b = new Barrier(5);
            public static Barrier b2 = new Barrier(6);
            public static Barrier b3 = new Barrier(2);
            public static List<string> instructionList = new List<string> {  "sub $v1,$t2,$t3" };
            public static int[] ram = new int[500];
            public static bool run = true;
            public static pcb[] procs = new pcb[3];
            public static int cur_pcb = -1;
            public static System.Timers.Timer aTimer = new System.Timers.Timer(700);
            public static System.Timers.Timer bTimer = new System.Timers.Timer(3000);
            public static Stopwatch stopWatch = new Stopwatch();
            public static List<string> wb_ins = new List<string>();




            public static List<string> log = new List<string>();
            
        }

        public static void IF()
        {
            while (Globals.run)
            {
                Globals.count++;
                

                int pc = Globals.pc;
                List<string> instructions = Globals.instructionList;
                
                if (Globals.cs)
                {
                  
                    
                    if (Globals.cs_pc == 0)
                    {
                       
                        Globals.b3.SignalAndWait();
                    }

                    if (Globals.cs)
                    {
                        pc = Globals.cs_pc;
                        instructions = Globals.cs_instructions;
                        if (pc < instructions.Count)
                        {



                            List<string> instruction = new List<string>()
                { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3],"cs" };
                            Globals.cs_pc += 4;
                           
                           

                            Globals.b.SignalAndWait();

                            Globals.ifid.Push(instruction);
                            if (pc + 4 == instructions.Count)
                            {
                                
                                Globals.cs_pc = 0;


                                Globals.b3.SignalAndWait();


                            }

                        }
                    }
                    else {
                       
                        if (pc < instructions.Count)
                        {
                           
                            Globals.newcount++;

                            

                         
                            List<string> instruction = new List<string>()
                { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3],Globals.cur_pcb.ToString() };
                            Globals.pc += 4;
                           

                            Globals.b.SignalAndWait();

                            Globals.ifid.Push(instruction);

                        }
                        else
                        {
                            
                            Globals.procs[Globals.cur_pcb].SetState("dead");
                            Thread t = new Thread(() => choose_pcb("RR", "mem"));
                            t.Start();
                            Globals.b.SignalAndWait();
                        }

                    }
                    



                }
                else {

                    if (pc < instructions.Count)
                    {
                       
                        Globals.newcount++;

                       
                        if (pc + 4 == instructions.Count)
                        {
                           
                            Globals.procs[Globals.cur_pcb].SetState("dead");
                            Globals.bTimer.Stop();
                            Thread t = new Thread(() => choose_pcb("RR", "mem"));
                            t.Start();


                        }


                        List<string> instruction = new List<string>()
                { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3] ,Globals.cur_pcb.ToString()};
                        Globals.pc += 4;
                      

                        Globals.b.SignalAndWait();

                        Globals.ifid.Push(instruction);

                    }
                    else
                    {
                        
                        
                       
                        Globals.procs[Globals.cur_pcb].SetState("dead");
                        choose_pcb("RR", "mem");
                        Globals.b.SignalAndWait();
                    }
                }

                
                Globals.b2.SignalAndWait();
            }

        }
        public static void ID()
        {
            while (Globals.run)
            {
                if (Globals.ifid.Count != 0)
                {
                    Globals.log.Add("2");
                    List<string> instruction = Globals.ifid.Pop();
                    string op = instruction[0];
               
                    string v1 = instruction[1];
                    string v2 = instruction[2];
                    string v3 = instruction[3];
                    switch (op)
                    {
                        case "add":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "nop":
                            break;
                        case "addi":                          
                            instruction[2] = Globals.registers[v2].ToString();
                            break;
                        case "sub":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "beq":
                            instruction[1] = Globals.registers[v1].ToString();
                            instruction[2] = Globals.registers[v2].ToString();
                            break;
                        case "bne":
                            instruction[1] = Globals.registers[v1].ToString();
                            instruction[2] = Globals.registers[v2].ToString();
                            break;
                        case "slt":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "lw":
                            instruction[3] = Globals.registers[v3].ToString();
                            break ;
                        case "sw":
                            instruction[3] = Globals.registers[v3].ToString();
                            instruction[1] = Globals.registers[v1].ToString();
                            break;
                        case "j":
                            instruction[1] = Globals.registers[v1].ToString();
                            break;
                        case "and":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "or":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                    }
                    Globals.b.SignalAndWait();
                    Globals.log.Add("5");
                    Globals.idex.Push(instruction);

                }
                else
                    Globals.b.SignalAndWait();
                Globals.b2.SignalAndWait();
            }

        }
        public static void EX()
        {
            while (Globals.run)
            {
                if (Globals.idex.Count != 0)
                {
                    Globals.log.Add("3");
                    List<string> instruction = Globals.idex.Pop();
                    int result;
                  
                    List<string> output = new List<string>();
                    switch (instruction[0])
                    {


                        case "add":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "addi":
                           
                            
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "nop":
                            output = new List<string>() { instruction[0], instruction[1], instruction.Last() };
                            break;
                        case "sub":
                            result = int.Parse(instruction[2]) - int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "beq":
                            if (int.Parse(instruction[1]) - int.Parse(instruction[2]) == 0)
                            {
                                instruction[1] = "equal";


                            }
                            else
                                instruction[1] = "not";
                            output = new List<string>() { instruction[0], instruction[1], instruction[3], instruction.Last() };
                            break;
                        case "bne":
                            if (int.Parse(instruction[1]) - int.Parse(instruction[2]) == 0)
                            {
                                instruction[1] = "equal";


                            }
                            else
                                instruction[1] = "not";
                            output = new List<string>() { instruction[0], instruction[1], instruction[3], instruction.Last() };
                            break;
                        case "slt":
                            if (int.Parse(instruction[1]) < int.Parse(instruction[2]) )
                            {
                                instruction[2] = "less";


                            }
                            else
                                instruction[1] = "not";
                            output = new List<string>() { instruction[0], instruction[1], instruction[3], instruction.Last() };
                            break;
                        case "lw":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "sw":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString(), instruction.Last() };
                            break;
                        case "j":
                            output = new List<string>() { instruction[0], instruction[1], instruction.Last() };
                            break;
                        case "and":
                            logical binary_num = new logical(instruction[2]);
                            output = new List<string>() { instruction[0], instruction[1], binary_num.logical_and(instruction[3]), instruction.Last() };
                            break;
                        case "or":
                            logical binary_num1 = new logical(instruction[2]);
                            output = new List<string>() { instruction[0], instruction[1], binary_num1.logical_or(instruction[3]), instruction.Last() };
                            break;


                    }
                    Globals.b.SignalAndWait();

                    Globals.exmem.Push(output);

                }
                else
                    Globals.b.SignalAndWait();
                Globals.b2.SignalAndWait();
            }

        }

        public static void MEM()
        {
            while (Globals.run)
            {
                if (Globals.exmem.Count != 0)
                {
                    Globals.log.Add("4");
                    bool flag = false;
                    List<string> newInstruction = new List<string>();
                    List<string> instruction = Globals.exmem.Pop();
             
                    if (instruction[0] == "lw")
                    {
                        flag = true;
                        int address = int.Parse(instruction[2]);
                        int value = Globals.ram[address];
                        newInstruction = new List<string>() { instruction[0], instruction[1], value.ToString(), instruction.Last() };

                    }

                    if (instruction[0] == "sw")
                    {
                        flag = true;
                        int address = int.Parse(instruction[2]);
                        int value = Globals.ram[address];
                        Globals.ram[address] = int.Parse(instruction[1]);
                        newInstruction = new List<string>() { instruction[0], instruction[1], value.ToString(), instruction.Last() };

                    }

                    Globals.b.SignalAndWait();
                    if (flag)
                    {
                        Globals.memwb.Push(newInstruction);

                    }
                    else
                        Globals.memwb.Push(instruction);


                }
                else
                    Globals.b.SignalAndWait();
                Globals.b2.SignalAndWait();
            }



        }

        public static void WB()
        {
            while (Globals.run)
            {
                if (Globals.memwb.Count != 0)
                {
                    Globals.log.Add("5");
                    List<string> instruction = Globals.memwb.Pop();
                   

                    switch (instruction[0])
                    {
                        case "lw":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "sw":
                            Globals.b.SignalAndWait();
                            break;
                        case "nop":
                            Globals.b.SignalAndWait();
                            break;
                        case "add":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "addi":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "sub":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "j":
                            Globals.b.SignalAndWait();
                            Globals.pc = int.Parse(instruction[2]);
                            break;
                        case "beq":
                            if (instruction[1] == "equal")
                            {
                                Globals.b.SignalAndWait();

                                Globals.pc = Globals.pc + int.Parse(instruction[2]);
                            }
                            break;
                        case "bne":
                            if (instruction[1] == "not")
                            {
                                Globals.b.SignalAndWait();

                                Globals.pc = Globals.pc + int.Parse(instruction[2]);
                            }
                            break;
                        case "slt":
                            if (instruction[1] == "less")
                            {
                                Globals.b.SignalAndWait();

                                Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            }
                            break;
                        case "and":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "or":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;


                    }
                    string[] arr = { instruction[0], instruction.Last() };
                    Globals.wb_ins = new List<string>(arr);
                }
                else
                {
                    string[] arr = { "no" };
                    Globals.wb_ins = new List<string>(arr);
                    Globals.b.SignalAndWait();
                }
                    
                Globals.b2.SignalAndWait();
            }



        }

        public static void MainRun(int cycle)
        {
           
            string[] array = { "TextFile1.txt","TextFile2.txt" };
            List<string> names = new List<string>(array);
            pcb_creation(file_load(names));
            Thread t = new Thread(() => choose_pcb("RR", "reg"));
            t.Start();
          
            Thread ifThread = new Thread(IF);
            Thread idThread = new Thread(ID);
            Thread exThread = new Thread(EX);
            Thread memThread = new Thread(MEM);
            Thread wbThread = new Thread(WB);

            Globals.stopWatch.Start();
            ifThread.Start();

            idThread.Start();
            exThread.Start();
            memThread.Start();
            wbThread.Start();
           

            SetTimer();
            Thread.Sleep(600);
            










            //context switch:
            // works with timer
            // when timer runs out
            // starts putting NOP instructions and making sure pc stays same until pipe are empty
            // saves pc registers 
            // calls function to decide next runner
            // loads pcb
            // starts running and timers


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
        public static void pcb_creation(List<List<string>> instructions)
        {
            int count = 0;
            foreach (List<string> single_pcb in instructions)
            {
                pcb new_pcb = new pcb(single_pcb,"ready");
                Globals.procs[count] = new_pcb;
                count++;
            }

        }
        public static void choose_pcb(string algorithm,string load)
        {
            Globals.cs = true;
            int next_pcb = Globals.cur_pcb + 1;
            int count = 0;
          
            bool flag2=true;
            switch (algorithm)
            {
                case "RR":
                    
                    bool flag = true;
                    while (flag)
                    {
                        
                        count++;
                        if (count > Globals.procs.Length + 2)
                        {
                            flag = false;
                            flag2 = false;
                            Globals.cs = false;
                            //Globals.run = false;
                            //Globals.b2.SignalAndWait();
                            //Globals.aTimer.Stop();
                            //Globals.aTimer.Dispose();
                            ////Globals.bTimer.Stop();
                            ////Globals.bTimer.Dispose();

                        }
                        if (Globals.procs[next_pcb] != null)
                        {
                            if (Globals.procs[next_pcb].GetState() == "ready")
                                flag = false;
                            if (Globals.cur_pcb == next_pcb)
                            {
                                flag2 = false;
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
                    break;
                   
                default:
                    break;
            
            
            }
            if (flag2)
            {
                
                if (load == "reg")
                {
                    pcb_load_to_reg(next_pcb);
                }
                else
                    pcb_load_to_mem(next_pcb);
            }
            




        }
        public static void pcb_load_to_mem(int pcb_num)
        {
            Globals.bTimer.Stop();
            
           
            Globals.cs = true;
            int offset =  Globals.cur_pcb * 33;
            List<string> instructions=new List<string>();
            foreach (string item in Globals.load_to_pcb)
            {
                instructions.Add(item.Replace("offset",offset.ToString()));

            }

         
            Globals.cs_instructions = interpreter(instructions,Globals.re1,Globals.re2,Globals.re3);
            Globals.procs[Globals.cur_pcb].SetPc(Globals.pc);
            Globals.b3.SignalAndWait();
            Globals.b3.SignalAndWait();
           
            choose_pcb("RR", "reg");
            

        }

        public static void pcb_load_to_reg(int pcb_num)
        {
                
            int offset = pcb_num * 33;
            List<string> instructions = new List<string>();
            foreach (string item in Globals.load_to_reg)
            {
                instructions.Add(item.Replace("offset", offset.ToString()));

            }
          
            Globals.cs_instructions = interpreter(instructions, Globals.re1, Globals.re2, Globals.re3);          
            Globals.instructionList = interpreter(Globals.procs[pcb_num].GetIns(), Globals.re1, Globals.re2, Globals.re3);
          
            Globals.pc = Globals.procs[pcb_num].GetPc();
           
            
            Globals.b3.SignalAndWait();
            
            Globals.b3.SignalAndWait();
            Globals.cur_pcb = pcb_num;
            Globals.cs = false;

           
            if (Globals.first_time)
            {
                SetTimer_cs();
                Globals.first_time = false;
            }
            else
                Globals.bTimer.Start();
            




        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            Globals.aTimer.Start();
            // Hook up the Elapsed event for the timer.
            Globals.aTimer.Elapsed += OnTimedEvent;
            Globals.aTimer.AutoReset = true;
            Globals.aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TimeSpan ts = Globals.stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Console.WriteLine("RunTime " + elapsedTime);
            if (Globals.ifid.Count != 0)
            {
                string ifid_ins = Globals.ifid.Peek()[0];
                string pcb_num = Globals.ifid.Peek().Last();
                Console.WriteLine("if worked on instruction " + ifid_ins + " pcb " + pcb_num);
            }
            else
                Console.WriteLine("if was idle");
            if (Globals.idex.Count != 0)
            {
                string idex_ins = Globals.idex.Peek()[0];
                string pcb_num = Globals.idex.Peek().Last();
                Console.WriteLine("id worked on instruction " + idex_ins + " pcb " + pcb_num);
            }
            else
                Console.WriteLine("id was idle");
            if (Globals.exmem.Count != 0)
            {
                string exmem_ins = Globals.exmem.Peek()[0];
                string pcb_num = Globals.exmem.Peek().Last();
                Console.WriteLine("ex worked on instruction " + exmem_ins + " pcb " + pcb_num);
            }
            else
                Console.WriteLine("ex was idle");
            if (Globals.memwb.Count != 0)
            {
                string memwb_ins = Globals.memwb.Peek()[0];
                string pcb_num = Globals.memwb.Peek().Last();
                Console.WriteLine("mem worked on instruction " + memwb_ins + " pcb " + pcb_num);
            }
            else
                Console.WriteLine("mem was idle");

            if (Globals.wb_ins[0] == "no")
                Console.WriteLine("wb was idle");
            else
            {
                Console.WriteLine("wb worked on instruction " + Globals.wb_ins[0] + " pcb " + Globals.wb_ins.Last());
            }



            Console.WriteLine("-------------------");




            if (Globals.ifid.Count + Globals.idex.Count + Globals.exmem.Count + Globals.memwb.Count != 0)
            {
                Globals.b2.SignalAndWait();
                
            }

            else
            {
                if (Globals.run)
                {
                   
                    Globals.run = false;
                    Globals.b2.SignalAndWait();
                    Globals.aTimer.Stop();
                    Globals.aTimer.Dispose();
                    Globals.bTimer.Stop();
                    Globals.bTimer.Dispose();
                }
                
                


            }
        }

        private static void SetTimer_cs()
        {
           
            //if (Globals.ifid.Count + Globals.idex.Count + Globals.exmem.Count + Globals.memwb.Count != 0)
            //{
            //    Globals.bTimer.Stop();
            //}
            // Create a timer with a two second interval.
            Globals.bTimer.Start();
            // Hook up the Elapsed event for the timer.
            Globals.bTimer.Elapsed += OnTimedEvent_cs;
            Globals.bTimer.AutoReset = true;
            Globals.bTimer.Enabled = true;
        }
        private static void OnTimedEvent_cs(Object source, ElapsedEventArgs e)
        {
            
            if (Globals.run)
            {
                choose_pcb("RR","mem");
            }

          
            
        }

      
    }
}
