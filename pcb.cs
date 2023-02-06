using System.Collections.Generic;
using static ConsoleApp98.Program;

namespace MIPS
{
    internal class pcb
    {
        private Dictionary<string, int> regs;
        private int pc;
        private int size;
        private string state;
        private int id;

        private List<string> instructionList;
        private int[] ram = new int[100];
        public pcb(List<string> instructions, string state, int id)
        {
            this.instructionList = instructions;
            this.pc = 0;
            this.state = state;
            this.id = id;
            this.regs = new Dictionary<string, int>()
            {
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",0},{"$a3",0 }, {"$t0",0 },{"$t1",0 },{"$t2",0 },{"$t3",0 },{"$gp",0 },{"$fp",0 },{"$sp",0 },{"$ra",0 }

            };
            this.id = id;
            int offset = id * 33;
            int index = offset;
            foreach (int item in this.regs.Values)
            {
                Globals.ram[offset] = item;
                offset++;
            }
            this.size = instructions.Count;
        }
        public pcb(List<string> instructions, string state,List<string> settings,int id)
        {
            this.id = id; 
            this.instructionList = instructions;
            this.pc = 0;
            this.state = state;
            this.regs = new Dictionary<string, int>()
            {
                {"$zero",0 },{"$v0",0 },{"$v1",0 },{"$a0",0 },{"$a1",0 },{"$a2",0},{"$a3",0 }, {"$t0",0 },{"$t1",0 },{"$t2",0 },{"$t3",0 },{"$gp",0 },{"$fp",0 },{"$sp",0 },{"$ra",0 }

            };
            string name = "";
            string value = "";
            foreach (string pair in settings)
            {
                if (pair != "set")
                {
                    string[] words = pair.Split(':');                    
                    this.regs[words[0]] = int.Parse(words[1]);                  
                }            
            }
          
            int offset = id * 33;
            int index = offset;
            foreach (int item in this.regs.Values)
            {
                Globals.ram[offset] = item;
                offset++;
            }
            this.size = instructions.Count;
        }

        public string GetState()
        {
            return this.state;
        }
        public int GetSize()
        {
            return this.size;
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
