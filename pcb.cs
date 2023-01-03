using System;
using System.Collections.Generic;
using System.Text;

namespace MIPS
{
    internal class pcb
    {
        private  Dictionary<string, int> registers;
        private int pc;
        private int size;
        private string state;
        
        private  List<string> instructionList;
        private  int[] ram = new int[100];
        public pcb(List<string> instructions,string state)
        {
            this.instructionList = instructions;
            this.pc = 0;
            this.state = state;
            this.registers= new Dictionary<string, int>()
            {
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",5 },{"$a3",5 }, {"$t0",5 },{"$t1",0 },{"$t2",2 },{"$t3",3 },{"$t4",0 },{"$t5",0 },{"$t6",1 },{"$t7",2},
                {"$s0",0 },{"$s1",0 },{"$s2",0 },{"$s3",0 },{"$s4",0 },{"$s5",0 },{"$s6",0 },{"$s7",0 },{"$t8",0 },{"$t9",0 },{"$gp",0 },{"$fp",0 },{"$sp",0 },{"$ra",0 }

            };
        }
        
        public string GetState()
        {
            return this.state;
        }
        public List<string> GetIns()
        {
            return this.instructionList;
        }
        public int GetPc()
        {
            return this.pc;
        }

        public void SetPc(int pc)
        {
            this.pc = pc;
        }
        public void SetState(string state)
        {
            this.state = state;
        }

    }
}
