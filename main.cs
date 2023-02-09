using MIPS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MIPS
{
    class Program
    {
        static void Main(string[] args)
        {

           
            MainRun();
            Thread.Sleep(120000);
            /// something fucked up with finishing up when using srt



        }
        public static List<string> interpreter(List<string> ins_list, string re1, string re2, string re3)
        {
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
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",0 },{"$a3",0 }, {"$t0",0 },{"$t1",0 },{"$t2",0 },{"$t3",0 },{"$gp",0 },{"$fp",0 },{"$sp",0 },{"$ra",0 }

            };

            public static int pc = 0;
            public static bool first_time = true;
            public static int count = 0;
            public static int newcount = 0;
            public static bool cs = false;
            public static List<string> load_to_pcb = new List<string> { "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","addi $zero,$zero,offset","nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","sw $v0,1($zero)", "sw $v1,2($zero)", "sw $a0,3($zero)", "sw $a1,4($zero)", "sw $a2,5($zero)", "sw $a3,6($zero)", "sw $t0,7($zero)",
            "sw $t1,8($zero)","sw $t2,9($zero)","sw $t3,10($zero)","sw $gp,11($zero)","sw $fp,12($zero)","sw $sp,13($zero)",
            "sw $ra,14($zero)","sub $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero"};
            public static List<string> load_to_reg = new List<string> { "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","addi $zero,$zero,offset","nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero","lw $v0,1($zero)", "lw $v1,2($zero)", "lw $a0,3($zero)", "lw $a1,4($zero)", "lw $a2,5($zero)", "lw $a3,6($zero)", "lw $t0,7($zero)",
            "lw $t1,8($zero)","lw $t2,9($zero)","lw $t3,10($zero)","lw $gp,11($zero)","lw $fp,12($zero)","lw $sp,13($zero)",
            "lw $ra,14($zero)","sub $zero,$v0,$v0", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero", "nop $zero,$zero,$zero" };
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
            public static List<string> instructionList = new List<string> { "sub $v1,$t2,$t3" }; // instruction list (loaded from files)
            public static int[] ram = new int[500];
            public static bool run = true;
            public static pcb[] procs = new pcb[3];
            public static int cur_pcb = -1;
            public static System.Timers.Timer aTimer = new System.Timers.Timer(700); // cycle clock
            public static System.Timers.Timer bTimer = new System.Timers.Timer(3000); // cs clock
            public static Stopwatch stopWatch = new Stopwatch(); // count time
            public static List<string> wb_ins = new List<string>();
            public static List<string> log = new List<string>();
            public static string alg = "fcfs";
        }

        public static void IF()
        {
            Console.WriteLine("if");

            while (Globals.run)
            {
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
                        pc = Globals.cs_pc;
                        instructions = Globals.cs_instructions;
                        if (pc < instructions.Count)
                        {
                            List<string> instruction = new List<string>()
                            { instructions[pc], instructions[pc + 1], instructions[pc + 2], instructions[pc + 3], "cs" };
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
                    else
                    {
                        if (pc < instructions.Count)
                        {
                            Globals.newcount++;
                            List<string> instruction = new List<string>()
                            { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3],Globals.cur_pcb.ToString() };

                            Globals.pc += 4;
                            Globals.b.SignalAndWait();
                            Globals.ifid.Push(instruction);
                        }

                     

                        //else
                        //{
                        //    Console.WriteLine("We need this 1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        //    Globals.procs[Globals.cur_pcb].SetState("dead");
                        //    Thread t = new Thread(() => choose_pcb("RR", "mem"));
                        //    t.Start();
                        //    Globals.b.SignalAndWait();
                        //}
                    }
                }
                else
                {
                    if (pc < instructions.Count)
                    {
                        Globals.newcount++;
                        if (pc + 4 == instructions.Count)
                        {
                         
                            Globals.procs[Globals.cur_pcb].SetState("dead");
                            Globals.bTimer.Stop();
                            Thread t = new Thread(() => choose_pcb(Globals.alg, "mem"));
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
                        Globals.b.SignalAndWait();
                    }
                }
            
                Globals.b2.SignalAndWait();
            }
        }

        public static void ID()
        {
            Console.WriteLine("id");
            while (Globals.run)
            {
                if (Globals.ifid.Count != 0)
                {
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
                            break;
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
                    Globals.idex.Push(instruction);
                }
                else
                    Globals.b.SignalAndWait();
              
                Globals.b2.SignalAndWait();
            }
        }

        public static void EX()
        {
            Console.WriteLine("ex");
            while (Globals.run)
            {
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
                                instruction[1] = "equal";
                            else
                                instruction[1] = "not";
                            output = new List<string>() { instruction[0], instruction[1], instruction[3], instruction.Last() };
                            break;
                        case "bne":
                            if (int.Parse(instruction[1]) - int.Parse(instruction[2]) == 0)
                                instruction[1] = "equal";
                            else
                                instruction[1] = "not";
                            output = new List<string>() { instruction[0], instruction[1], instruction[3], instruction.Last() };
                            break;
                        case "slt":
                            if (int.Parse(instruction[1]) < int.Parse(instruction[2]))
                                instruction[2] = "less";
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
            Console.WriteLine("mem");
            while (Globals.run)
            {
                if (Globals.exmem.Count != 0)
                {
                   
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

                        Globals.ram[address] = int.Parse(instruction[1]);
                        int value = Globals.ram[address];
                        newInstruction = new List<string>() { instruction[0], instruction[1], value.ToString(), instruction.Last() };
                       
                    }

                    Globals.b.SignalAndWait();
                    if (flag)
                        Globals.memwb.Push(newInstruction);
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
            Console.WriteLine("wb");
            while (Globals.run)
            {
                if (Globals.memwb.Count != 0)
                {

                 
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

        public static void MainRun()
        {
            System.IO.File.WriteAllText("log.txt", string.Empty);

            string[] array = { "TextFile1.txt", "TextFile2.txt" };
            List<string> names = new List<string>(array);
            pcb_creation(file_load(names));

            Thread t = new Thread(() => choose_pcb("srt", "reg"));
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
        public static void choose_pcb(string algorithm, string load)
        {
            Console.WriteLine("choose pcb");
            Globals.alg = algorithm;
            Globals.cs = true;
            int next_pcb = Globals.cur_pcb + 1;
            int count = 0;
            bool flag2 = true;

            switch (algorithm)
            {
                case "RR":
                    if (load == "reg")
                    {
                        bool flag = true;
                        while (flag)
                        {
                            count++;
                            if (count > Globals.procs.Length + 2)
                            {
                                flag = false;
                                flag2 = false;
                                Globals.cs = false;
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
                        if (flag2)
                        {
                            pcb_load_to_reg(next_pcb, "RR");
                        }

                    }
                    else
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
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
                                flag2 = false;
                                Globals.cs = false;
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
                        if (flag2)
                        {
                            pcb_load_to_reg(next_pcb, "fcfs");
                        }

                    }
                    else
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
                    break;
                case "srt":
                    if (load == "reg")
                    {

                        int current = 0;
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
                                
                                    if (size > max_value)
                                    {
                                        max_value = size;
                                        max_index = i;
                                    }
                                }
                            }
                        }
                        if (Globals.procs[max_index].GetSize() - Globals.procs[max_index].GetPc() > 0)
                        {
                           
                            pcb_load_to_reg(max_index, "srt");
                        }
                        else
                            Globals.cs = false;


                    }
                    else
                        pcb_load_to_mem(Globals.cur_pcb, algorithm);
                    break;
                default:
                    break;


            }


        }
        public static void pcb_load_to_mem(int pcb_num, string alg)
        {

            Globals.bTimer.Stop();
            Globals.cs = true;
            int offset = Globals.cur_pcb * 33;
            Console.WriteLine("loading to mem with offset " + offset);
            List<string> instructions = new List<string>();
            foreach (string item in Globals.load_to_pcb)
            {
                instructions.Add(item.Replace("offset", offset.ToString()));

            }
            Globals.cs_instructions = interpreter(instructions, Globals.re1, Globals.re2, Globals.re3);
            Globals.procs[Globals.cur_pcb].SetPc(Globals.pc);
            Globals.b3.SignalAndWait();
            Globals.b3.SignalAndWait();
            Console.WriteLine("finished loading to memory");

            choose_pcb(alg, "reg");
        }

        public static void pcb_load_to_reg(int pcb_num, string alg)
        {

            int offset = pcb_num * 33;
            Console.WriteLine("loading to reg with offset " + offset);
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
            Console.WriteLine("finished loading to regs");

            Globals.cur_pcb = pcb_num;
            Globals.cs = false;
            if (alg == "RR")
            {
                if (Globals.first_time)
                {
                    SetTimer_cs();
                    Globals.first_time = false;
                }
                else
                    Globals.bTimer.Start();
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
            // Create a timer with a two second interval.
            Globals.aTimer.Start();
            // Hook up the Elapsed event for the timer.
            Globals.aTimer.Elapsed += OnTimedEvent;
            Globals.aTimer.AutoReset = true;
            Globals.aTimer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("event");
            TimeSpan ts = Globals.stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            string if_string = "";
            string id_string = "";
            string ex_string = "";
            string mem_string = "";
            string wb_string = "";
            string line = "-------------------";

            if (Globals.ifid.Count != 0)
            {
                string ifid_ins = Globals.ifid.Peek()[0];
                string pcb_num = Globals.ifid.Peek().Last();
                if_string = "if worked on instruction " + ifid_ins + " pcb " + pcb_num;

            }
            else
                if_string = "if was idle";
            Console.WriteLine(if_string);
            if (Globals.idex.Count != 0)
            {
                string idex_ins = Globals.idex.Peek()[0];
                string pcb_num = Globals.idex.Peek().Last();
                id_string = "id worked on instruction " + idex_ins + " pcb " + pcb_num;

            }
            else
                id_string = "id was idle";
            Console.WriteLine(id_string);
            if (Globals.exmem.Count != 0)
            {
                string exmem_ins = Globals.exmem.Peek()[0];
                string pcb_num = Globals.exmem.Peek().Last();
                ex_string = "ex worked on instruction " + exmem_ins + " pcb " + pcb_num;

            }
            else
                ex_string = "ex was idle";
            Console.WriteLine(ex_string);
            if (Globals.memwb.Count != 0)
            {
                string memwb_ins = Globals.memwb.Peek()[0];
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
            Action<object> action = (object obj) =>
            {

                string[] readText = File.ReadAllLines(obj.ToString(), Encoding.UTF8);
                string[] newText = new string[] { if_string, id_string, ex_string, mem_string, wb_string, line };
                string[] combined = readText.Concat(newText).ToArray();
                File.WriteAllLines(obj.ToString(), combined, Encoding.UTF8);
            };

            // Create a task but do not start it.
            Task t1 = new Task(action, "log.txt");


            t1.Start();



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
                    Globals.aTimer.Dispose();
                    Globals.bTimer.Stop();
                    Globals.bTimer.Dispose();
                  


                }
            }
        }

        private static void SetTimer_cs()
        {
            Console.WriteLine("set timer cs");
            Globals.bTimer.Start();
            // Hook up the Elapsed event for the timer.
            Globals.bTimer.Elapsed += OnTimedEvent_cs;
            Globals.bTimer.AutoReset = true;
            Globals.bTimer.Enabled = true;
        }
        private static void OnTimedEvent_cs(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("event cs");
            if (Globals.run)
            {
                choose_pcb("RR", "mem");
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
    }
}
