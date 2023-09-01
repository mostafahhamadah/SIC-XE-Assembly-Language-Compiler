using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sicXeProjectFinal
{
    class optable
    {

        static List<String> mnemonic = new List<String>();
        static List<String> Format = new List<String>();
        static List<String> opCode = new List<String>();
        public string mnm = "";
        public int size = 0;
        public string oCode = "";

         static int counter = 0; 
         void readOpTable()
        { 
            String line;
            
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(@"E:\sicXeProjectFinal\c#\sicXeProjectFinal\bin\optab.txt");
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                   
                    //  String s = getFormat(line);

                    getMnemonic(line);
                    getFormat(line);
                    getOpCode(line);
                    
                    //Read the next line
                    counter++;
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();             
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            
        }


      static void getMnemonic(String s)
        {
            String[] s1 = s.Replace('\t', ' ').Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            mnemonic.Add(s1[0]);
            

        }
        static void getFormat(String s)
        {
            String[] s1 = s.Replace('\t', ' ').Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            Format.Add(s1[1]);


        }
        static void getOpCode(String s)
        {
            String[] s1 = s.Replace('\t', ' ').Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            opCode.Add(s1[2]);
        }

            public optable(String mn)
        {

            bool found = false;
            readOpTable();
            for (int i=0;i< mnemonic.Count; i++)
            {
                if (mn.Equals(mnemonic[i]))
                {
                    this.mnm = (String)mnemonic[i];
                    this.size = Int32.Parse(Format[i], System.Globalization.NumberStyles.HexNumber); ;
                    this.oCode = (String)opCode[i];
                    found = true; 
                }
             
            } 
        }


    }
}
