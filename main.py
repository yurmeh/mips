using System;
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

namespace ConsoleApp117
{
    class Program
    {
        static void Main(string[] args)
        {
            Globals.instructionList = interpreter(Globals.instructionList, Globals.re1, Globals.re2, Globals.re3);
            Console.WriteLine(Globals.instructionList.Count);
            Globals.ram[10] = 100;
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
            public static int count = 0;
            public static string re1 = @"(\w+) (\S+),(\S+),(\S+)";
            public static string re2 = @"(\w+) (\S+),(\S+)\((\S+)\)";
            public static string re3 = @"(\w+) (\S+)";
            public static Stack<List<string>> ifid = new Stack<List<string>>();
            public static Stack<List<string>> idex = new Stack<List<string>>();
            public static Stack<List<string>> exmem = new Stack<List<string>>();
            public static Stack<List<string>> memwb = new Stack<List<string>>();
            public static Barrier b = new Barrier(5);
            public static Barrier b2 = new Barrier(6);
            public static List<string> instructionList = new List<string> {  "beq $s6,$s7,4","add $v1,$t2,$t3", "lw $a0,5($a2)", "add $v0,$t6,$t7", "add $v0,$t6,$t7", "add $a2,$a3,$t0", "add $v0,$t6,$t7", "add $a2,$a3,$t0", "add $a2,$a3,$t0", "add $a2,$a3,$t0", "add $a2,$a3,$t0", "add $a2,$a3,$t0" };
            public static int[] ram = new int[100];
            public static bool run = true;
            public static System.Timers.Timer aTimer = new System.Timers.Timer(2000);

            public static List<string> log = new List<string>();
        }

        public static void IF()
        {
            while (Globals.run)
            {

                int pc = Globals.pc;
                List<string> instructions = Globals.instructionList;
                Console.WriteLine("the pc is " + pc);
                Console.WriteLine("the instruction length is " + instructions.Count);
                if (pc < instructions.Count)
                {
                   

                    List<string> instruction = new List<string>()
                { instructions[pc], instructions[pc+1], instructions[pc+2], instructions[pc+3] };
                    Globals.pc += 4;
                    Console.WriteLine("if working on " + instruction[0]);

                    Globals.b.SignalAndWait();

                    Globals.ifid.Push(instruction);

                }
                else
                    Globals.b.SignalAndWait();
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
                    Console.WriteLine("id working on " + instruction[0]);
                    string v1 = instruction[1];
                    string v2 = instruction[2];
                    string v3 = instruction[3];
                    switch (op)
                    {
                        case "add":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "sub":
                            instruction[2] = Globals.registers[v2].ToString();
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "beq":
                            instruction[1] = Globals.registers[v1].ToString();
                            instruction[2] = Globals.registers[v2].ToString();
                            break;
                        case "lw":
                            instruction[3] = Globals.registers[v3].ToString();
                            break;
                        case "j":
                            instruction[1] = Globals.registers[v1].ToString();
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
                    Console.WriteLine("ex working on " + instruction[0]);
                    List<string> output = new List<string>();
                    switch (instruction[0])
                    {


                        case "add":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString() };
                            break;
                        case "sub":
                            result = int.Parse(instruction[2]) - int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString() };
                            break;
                        case "beq":
                            if (int.Parse(instruction[1]) - int.Parse(instruction[2]) == 0)
                            {
                                instruction[1] = "equal";


                            }
                            else
                                instruction[1] = "not";
                           
                            output = new List<string>() { instruction[0], instruction[1],instruction[3] };
                            break;
                        case "lw":
                            result = int.Parse(instruction[2]) + int.Parse(instruction[3]);
                            output = new List<string>() { instruction[0], instruction[1], result.ToString() };
                            break;
                        case "j":
                            output = new List<string>() { instruction[0], instruction[1] };
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
                    Console.WriteLine("mem working on " + instruction[0]);
                    if (instruction[0] == "lw")
                    {
                        flag = true;
                        int address = int.Parse(instruction[2]);
                        int value = Globals.ram[address];
                        newInstruction = new List<string>() { instruction[0], instruction[1], value.ToString() };

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
                    Console.WriteLine("wb working on " + instruction[0]);
                    foreach (string item in instruction)
                    {
                        Console.WriteLine(item);
                    }
                    switch (instruction[0])
                    {
                        case "lw":
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "add":
                            Globals.log.Add(instruction[2]);
                            Globals.b.SignalAndWait();
                            Globals.registers[instruction[1]] = int.Parse(instruction[2]);
                            break;
                        case "sub":
                            Globals.log.Add(instruction[2]);
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
                                Console.WriteLine("beq"+instruction[2]);
                                Globals.pc = Globals.pc  + int.Parse(instruction[2]);
                            }
                            break;

                    }

                }
                else
                    Globals.b.SignalAndWait();
                Globals.b2.SignalAndWait();
            }



        }

        public static void MainRun(int cycle)
        {


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


            SetTimer();

            Thread.Sleep(25000);
            Console.WriteLine(Globals.registers["$v0"]);
            Console.WriteLine(Globals.registers["$v1"]);
            Console.WriteLine(Globals.registers["$a0"]);
            Console.WriteLine(Globals.pc);




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

            if (Globals.ifid.Count + Globals.idex.Count + Globals.exmem.Count + Globals.memwb.Count != 0)
            {
                Globals.b2.SignalAndWait();
            }

            else
            {
                Console.WriteLine("i stopped");
                Console.WriteLine(Globals.ifid.Count.ToString(),  Globals.idex.Count.ToString() , Globals.exmem.Count.ToString() , Globals.memwb.Count.ToString());
                Globals.run = false;
                Globals.b2.SignalAndWait();
                Globals.aTimer.Stop();
                Globals.aTimer.Dispose();
            }
        }
    }
}
