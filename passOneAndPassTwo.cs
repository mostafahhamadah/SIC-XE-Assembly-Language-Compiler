using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace sicXeProjectFinal
{
    class passOne
    {
        static List<String> Lines = new List<string>();
        static List<String> modifiedLines = new List<String>();
        static List<String> labels = new List<string>();
        static List<String> Mnemonics = new List<string>();
        static List<String> firstOperand = new List<string>();
        static List<String> secondOperand = new List<string>();
        static List<String> locations = new List<string>();
        static List<String> literals = new List<string>();
        static int counter = 0;
        static List<String> modifiedLabels = new List<String>();
        static List<String> modifiedMnemonics = new List<string>();
        static List<String> modifiedFirstOperand = new List<String>();
        static List<String> modifiedSecondOperand = new List<string>();
        static List<String> objectCode = new List<String>();
        static string baseValue ="33";
        static void readSrcCode()
        {
            try
            {
                StreamReader sr = new StreamReader(@"E:\sicXeProjectFinal\c#\sicXeProjectFinal\src.txt");
                Lines.Add(sr.ReadLine());
                while (Lines[counter] != null)
                {
                    getParts((String)Lines[counter]);
                    Lines.Add(sr.ReadLine());
                    counter++;
                }
                sr.Close();
                ModifyForLiterals();
                Console.ReadLine();

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
        static void getParts(String s)
        {
            String[] s1 = s.Replace('\t', ' ').Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (!s1[0].Equals("-")) {

                labels.Add(s1[0]);
            }
            else
            {
                labels.Add(s1[0]);
            }
            Mnemonics.Add(s1[1]);
            if (s1[2][0] == '=')
            {
                literals.Add((String)s1[2]);
            } else
            {
                literals.Add("-");
            }
            if (!s1[2].Contains(","))
            {
                firstOperand.Add(s1[2]);
                secondOperand.Add("-");
            } else
            {
                for (int t = 0; t < s1[2].Length; t++)
                {
                    if (s1[2][t] == ',')
                    {
                        firstOperand.Add(s1[2].Substring(0, t));
                        secondOperand.Add(s1[2].Substring(t + 1));
                        break;
                    }
                }
            }
        }

        static void ModifyForLiterals()
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Mnemonics[i].Equals("LTORG") || Mnemonics.Equals("END"))
                {
                    modifiedLines.Add((String)Lines[i]);
                    int tempCounter = 0;
                    for (int c = 0; c < literals.Count; c++)
                    {

                        if (literals[c][0] == '=')
                        {
                            modifiedLines.Add("*\t" + (String)literals[c + tempCounter] + "\t-");
                            literals.Insert(c + tempCounter, "-");
                            tempCounter++;
                        }
                    }
                    tempCounter = 0;

                }
                else
                {
                    modifiedLines.Add((String)Lines[i]);
                }
            }
        }

        //getPartsAfterModification
        static void getPartsModified()
        {
            for (int c = 0; c < modifiedLines.Count; c++)
            {
                String[] s1 = modifiedLines[c].Replace('\t', ' ').Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!s1[0].Equals("-") && !s1[0].Equals("*"))
                {
                    modifiedLabels.Add(s1[0]);
                }
                else
                {
                    modifiedLabels.Add(s1[0]);
                }
                modifiedMnemonics.Add(s1[1]);


                if (!s1[2].Contains(","))
                {
                    modifiedFirstOperand.Add(s1[2]);
                    modifiedSecondOperand.Add("-");
                }
                else
                {
                    for (int t = 0; t < s1[2].Length; t++)
                    {
                        if (s1[2][t] == ',')
                        {
                            modifiedFirstOperand.Add(s1[2].Substring(0, s1[2].Length-2));
                            modifiedSecondOperand.Add(s1[2].Substring(t + 1));
                            break;
                        }
                    }
                }
            }
        }
        static string convertfromHextoBinary(char s)
        {
            switch (s) {
                case '0': return "0000"; break;
                case '1': return "0001"; break;
                case '2': return "0010"; break;
                case '3': return "0011"; break;
                case '4': return "0100"; break;
                case '5': return "0101"; break;
                case '6': return "0110"; break;
                case '7': return "0111"; break;
                case '8': return "1000"; break;
                case '9': return "1001"; break;
                case 'a': return "1010"; break;
                case 'A': return "1010"; break;
                case 'b': return "1011"; break;
                case 'B': return "1011"; break;
                case 'c': return "1100"; break;
                case 'C': return "1100"; break;
                case 'd': return "1101"; break;
                case 'D': return "1101"; break;
                case 'e': return "1110"; break;
                case 'E': return "1110"; break;
                case 'f': return "1111"; break;
                case 'F': return "1111"; break;
                default: return "Wrong Digit";
            }

        }
        static List<String> sTEMPA = new List<string>();
        static void generatePassOne()
        {
            String firstAddress = ((String)modifiedFirstOperand[0]);
            locations.Add(modifiedFirstOperand[0]);
            locations.Add(firstAddress);

            for(int i = 2; i < modifiedLines.Count; i++)
            {
                if (isFormatOne(modifiedMnemonics[i-1]))
                {
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, 1);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (isFormatTwo(modifiedMnemonics[i-1]))
                {
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, 2);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (modifiedMnemonics[i - 1][0] == '+' && i != 0 || modifiedMnemonics[i - 1][0] == '$' && i != 0)
                {
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, 4);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (modifiedMnemonics[i - 1][0] == '&' || (!isDirective(modifiedMnemonics[i - 1]) && modifiedMnemonics[i - 1][0] != '=' && modifiedMnemonics[i - 1][0] != '-' &&
                     !modifiedMnemonics[i - 1].Equals("RESW") && !modifiedMnemonics[i - 1].Equals("RESB") && !modifiedMnemonics[i-1].Equals("BYTE")))
                {
                    int prevLoc = Int32.Parse(locations[i-1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, 3);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (modifiedMnemonics[i - 1].Equals("RESW"))
                {
                    int multiple = Int32.Parse(modifiedFirstOperand[i - 1], System.Globalization.NumberStyles.Integer);
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, multiple*3);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (modifiedMnemonics[i - 1].Equals("RESB"))
                {
                    int multiple = Int32.Parse(modifiedFirstOperand[i - 1], System.Globalization.NumberStyles.Integer) * 1;
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, multiple);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                }
                else if (modifiedMnemonics[i - 1].Equals("BYTE"))
                {
                    if (modifiedFirstOperand[i - 1][0] == 'X')
                    {
                        int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                        String temps = modifiedFirstOperand[i - 1].Substring(2,modifiedFirstOperand[i-1].Length-3);
                        string curr = HexAdd(prevLoc,  1);
                        locations.Add(Int32.Parse(curr).ToString("X"));
                    }
                    else if (modifiedFirstOperand[i - 1][0] == 'C')
                    {
                        int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                        String temps = modifiedFirstOperand[i - 1].Substring(2,3);
                        string curr = HexAdd(prevLoc, temps.Length);
                        locations.Add(Int32.Parse(curr).ToString("X"));
                    }
                }
               else if (modifiedMnemonics[i - 1].Equals("WORD"))
                {
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc, 3);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));
                } 
                else if (modifiedMnemonics[i-1][0]=='=')
                {
                    int length = modifiedMnemonics[i - 1].Length;
                    int prevLoc = Int32.Parse(locations[i - 1], System.Globalization.NumberStyles.HexNumber);
                    string currLoc = HexAdd(prevLoc,length-4);
                    locations.Add(Int32.Parse(currLoc).ToString("X"));

                }
                else if (modifiedMnemonics[i-1].Contains("-") && modifiedLabels[i-1].Contains("*"))
                {
                    locations.Add(locations[i - 1]);
                }
                else
                {
                    locations.Add(locations[i-1]);
                }
            }
        }

        static string convertFromBinaryToHex(String s)
        {
            int i = 0;
            String result = "";
            while(i<s.Length)
            {
                switch (s.Substring(i,4))
                {
                    case "0000":result += "0";break;
                    case "0001":result += "1";break;
                    case "0010":result += "2";break;
                    case "0011":result += "3";break;
                    case "0100":result += "4";break;
                    case "0101":result += "5";break;
                    case "0110":result += "6";break;
                    case "0111":result += "7";break;
                    case "1000":result += "8";break;
                    case "1001":result += "9";break;
                    case"1010":result += "A";break;
                    case "1011":result += "B";break;
                    case "1100":result += "C";break;
                    case "1101":result += "D";break;
                    case "1110":result += "E";break;
                   default:result =result+ "F";break;
             
                }
                i = i + 4;
            }
            return result;

        }

        static void generateObCode()
        {
            int n, i, x, b, p, e;
            for (int counter = 0; counter < modifiedMnemonics.Count; counter++)
            {

                optable instruction = new optable(modifiedMnemonics[counter]);
                if (isDirective(modifiedMnemonics[counter]))
                {
                    objectCode.Add("-");
                }
                else if (modifiedMnemonics[counter].Equals("RESW") || modifiedMnemonics[counter].Equals("RESB"))
                {
                    objectCode.Add("-");
                }
                else if (modifiedMnemonics[counter].Equals("BYTE") && modifiedFirstOperand[counter][0] == 'X')
                {
                    String hexValue = modifiedFirstOperand[counter].Substring(2, 2);
                    objectCode.Add(hexValue);
                }
                else if (modifiedMnemonics[counter].Equals("BYTE") && modifiedFirstOperand[counter][0] == 'C')
                {
                    String charValue = modifiedFirstOperand[counter].Substring(2, 3);
                    string temp = "";
                    foreach (var c in charValue)
                    {
                        temp = temp + (int)c;
                    }
                    objectCode.Add(temp);
                } 
                else if (modifiedMnemonics[counter].Equals("WORD"))
                {
                    int multiple = Int32.Parse(modifiedFirstOperand[counter], System.Globalization.NumberStyles.HexNumber);
                    string hexMultiple = multiple.ToString("X");
                    objectCode.Add(hexMultiple);
                }
                else if (modifiedMnemonics[counter][0] == '=')
                {
                    string charValue = modifiedMnemonics[counter].Substring(3, 3);
                    string temp = "";
                    foreach (var c in charValue)
                    {
                        temp = temp + (int)c;
                    }
                    objectCode.Add(temp);
                }
                else if (instruction.size==1)
                {
                    objectCode.Add(instruction.oCode);
                }
                else if (instruction.size == 2)
                {
                    String regOne = regNumber(modifiedFirstOperand[counter]);
                    String regTwo = regNumber(modifiedSecondOperand[counter]);
                    objectCode.Add(instruction.oCode + "" + convertFromBinaryToHex(regOne + regTwo));
                }
                else if (instruction.size == 3)
                {
                    e = 0;
                    int c = 0;
                    switch (modifiedFirstOperand[counter][0])
                    {
                        case '#':
                            n = 0; i = 1; p = 0; b = 0;
                            x = 0;
                            String targetAddres = modifiedFirstOperand[counter].Substring(1);
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres = locations[c];
                                }
                            }

                            char ObCodeDigitOne = instruction.oCode[0];
                            char obCodeDigitTwo = instruction.oCode[1];
                            String byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            string byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            ;
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + targetAddres.Substring(1).PadLeft(3, '0')); break;

                        case '=':
                            n = 1; i = 1; x = 0;
                            targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedMnemonics[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres +=locations[c];
                                }
                            }
                            // calculating the displacement
                            String disp = HexSub(targetAddres, locations[counter + 1]);
                            int displacment = Int32.Parse(disp, System.Globalization.NumberStyles.HexNumber);
                            if (displacment <= 2047 && displacment >= -2048)
                            {
                                p = 1;
                                b = 0;
                            }
                            else
                            {
                                disp = HexSub(targetAddres, baseValue);
                                b = 1;
                                p = 0;
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + disp.PadLeft(3,'0')); break;

                        case '@':
                            n = 1; i = 0; x = 0;
                            String firstTargetAddress = "";

                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    firstTargetAddress+=locations[c];
                                    break;
                                }
                            }
                            string RealTargetAddress = modifiedFirstOperand[c - 1];
                            for (int j = 0; j < modifiedLabels.Count - 1; j++)
                            {
                                if (modifiedLabels[j].Equals(RealTargetAddress))
                                {
                                    RealTargetAddress+=locations[j];
                                    break;
                                }
                            }
                            disp = HexSub(RealTargetAddress.PadLeft(3, '0'), locations[counter+1]);
                            displacment = Int32.Parse(disp, System.Globalization.NumberStyles.HexNumber);
                            if (displacment <= 2047 && displacment >= -2048)
                            {
                                p = 1;
                                b = 0;
                            }
                            else
                            {
                                disp = HexSub(RealTargetAddress, baseValue);
                                b = 1;
                                p = 0;
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + disp.PadLeft(3, '0')); 
                            break;
                        case '-':
                            objectCode.Add(instruction.oCode.PadRight(6, '0'));
                            break;
                        default:
                            n = 1; i = 1;
                            if (modifiedSecondOperand[counter][0]!='X')
                            {
                                x = 0;
                            }
                            else
                            {
                                x = 1;
                            }
                            targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres+=locations[c];
                                }
                            }
                            // calculating the displacement
                            disp = HexSub(targetAddres, locations[counter+1]);
                            displacment = Int32.Parse(disp, System.Globalization.NumberStyles.HexNumber);
                            if (displacment <= 2047 && displacment >= -2048)
                            {
                                p = 1;
                                b = 0;
                            }
                            else
                            {
                                disp = HexSub(targetAddres, baseValue);
                                b = 1;
                                p = 0;
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (disp.PadLeft(3, '0')));
                            break;
                    }
                }
                else if (instruction.size == 4)
                {
                    switch (modifiedFirstOperand[counter][0])
                    {
                        case '#':
                            n = 0; i = 1; x = 0; b = 0; p = 0; e = 1;
                            int c = 0;
                            String targetAddres = modifiedFirstOperand[counter].Substring(1);
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres +=locations[c];
                                }
                            }
                            char ObCodeDigitOne = instruction.oCode[0];
                            char obCodeDigitTwo = instruction.oCode[1];
                            String byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            string byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);

                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + targetAddres.PadLeft(5,'0')); break;
                        case '=':
                            n = 1; i = 1; x = 0; b = 0; p = 0; e = 1;
                            targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedMnemonics[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres = locations[c];
                                }
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (targetAddres.PadLeft(5,'0'))); break;
                        case '-':
                            int var = 0;
                            objectCode.Add(instruction.oCode.PadRight(8, '0')); break;
                        case '@':
                            n = 1; i = 0; x = 0; b = 0; p = 0; e = 1;
                            String firstTargetAddress = "";
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    firstTargetAddress = locations[c];
                                    break;
                                }
                            }
                            string RealTargetAddress = modifiedFirstOperand[c - 1];
                            for (int j = 0; j < modifiedLabels.Count - 1; j++)
                            {
                                if (modifiedLabels[j].Equals(RealTargetAddress))
                                {
                                    RealTargetAddress = locations[j];
                                    break;
                                }
                            }

                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (RealTargetAddress.PadLeft(5, '0'))); break;
                        default:
                            n = 1; i = 1; p = 0; b = 0; e = 1;
                            if (modifiedSecondOperand[counter][0] == 'X')
                            {
                                x = 1;
                            }
                            else
                            {
                                x = 0;
                            }
                            targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres += locations[c];
                                }
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            ;
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (targetAddres.PadLeft(5,'0'))); break;
                    }
                }
                else if (instruction.size == 5)
                {
                    string targetAddres = "";
                    int c;
                    //getting the targetAddress>>
                    for (c = 0; c < modifiedLabels.Count - 1; c++)
                    {
                        if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                        {
                            targetAddres += locations[c];
                        }
                    }
                    // calculating the displacement
                    string disp = HexSub(targetAddres, locations[counter + 1]);
                    int displacment = Int32.Parse(disp, System.Globalization.NumberStyles.HexNumber);
                    if (displacment <= 2047 && displacment >= -2048)
                    {
                        p = 1;
                        b = 0;
                    }
                    else
                    {
                        disp = HexSub(targetAddres, baseValue);
                        b = 1;
                        p = 0;
                    }
                    displacment = Int32.Parse(disp, System.Globalization.NumberStyles.HexNumber);
                    if (displacment % 2 == 0)
                    {
                        n = 1;
                    }
                    else
                    {
                        n = 0;
                    }
                    if (displacment > 0)
                    {
                        i = 0;
                        e = 0;
                    }
                    else if (displacment < 0)
                    {
                        i = 1;
                        e = 0;
                    }
                    else
                    {
                        i = 1;
                        e = 1;
                    }
                    if (modifiedSecondOperand[counter][0] != '-')
                    {
                        x = 1;
                    }
                    else
                    {
                        x = 0;
                    }
                    char ObCodeDigitOne = instruction.oCode[0];
                    char obCodeDigitTwo = instruction.oCode[1];
                    string byteOne = convertfromHextoBinary(ObCodeDigitOne);
                    string byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                    objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (disp.PadLeft(3, '0')));
                   
                }
                else if (instruction.size == 6)
                {
                    switch (modifiedFirstOperand[counter][0])
                    {
                        case '#':
                            n = 0; i = 1; x = 0;
                            int c = 0;
                            String targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    targetAddres=locations[c];
                                }
                            }
                            int address = Int32.Parse(targetAddres, System.Globalization.NumberStyles.HexNumber);
                            if (address % 2 == 0)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = 0;
                            }
                            if (address == 0)
                            {
                                p = 0;
                            }
                            else
                            {
                                p = 1;
                            }
                            if (targetAddres.Equals(baseValue))
                            {
                                e = 0;
                            }
                            else
                            {
                                e = 1;
                            }
                            if (modifiedSecondOperand[counter][0] != 'X')
                            {
                                x = 0;
                            }
                            else
                            {
                                x = 1;
                            }
                            char ObCodeDigitOne = instruction.oCode[0];
                            char obCodeDigitTwo = instruction.oCode[1];
                            String byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            string byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);

                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (targetAddres.PadLeft(5, '0'))); break;
                        case '-':
                            objectCode.Add(instruction.oCode.PadRight(8, '0'));
                            break;
                        case '@':
                            n = 1; i = 0; x = 0;
                            String firstTargetAddress = "";
                            for (c = 0; c < modifiedLabels.Count; c++)
                            {
                                if (modifiedLabels[c].Equals(modifiedFirstOperand[counter]))
                                {
                                    firstTargetAddress = locations[c];
                                    break;
                                }
                            }
                            string RealTargetAddress = modifiedFirstOperand[c - 1];
                            for (int j = 0; j < modifiedLabels.Count - 1; j++)
                            {
                                if (modifiedLabels[j].Equals(RealTargetAddress))
                                {
                                    RealTargetAddress = locations[j];
                                    break;
                                }
                            }
                            address = Int32.Parse(RealTargetAddress, System.Globalization.NumberStyles.HexNumber);
                            if (address % 2 == 0)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = 0;
                            }
                            if (address == 0)
                            {
                                p = 0;
                            }
                            else
                            {
                                p = 1;
                            }
                            if (RealTargetAddress.Equals(baseValue))
                            {
                                e = 0;
                            }
                            else
                            {
                                e = 1;
                            }
                            if (modifiedSecondOperand[counter][0] != 'X')
                            {
                                x = 0;
                            }
                            else
                            {
                                x = 1;
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (RealTargetAddress.PadLeft(5, '0'))); break;

                        default:
                            n = 1; i = 1;
                            if (modifiedSecondOperand[counter][0] == 'X')
                            {
                                x = 1;
                            }
                            else
                            {
                                x = 0;
                            }
                            targetAddres = "";
                            //getting the targetAddress>>
                            for (c = 0; c < modifiedLabels.Count - 1; c++)
                            {
                                if (modifiedLabels[c] == modifiedFirstOperand[counter])
                                {
                                    targetAddres = locations[c];
                                    break;
                                }
                            }
                            address = Int32.Parse(targetAddres, System.Globalization.NumberStyles.HexNumber);
                            if (address % 2 == 0)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = 0;
                            }
                            if (address == 0)
                            {
                                p = 0;
                            }
                            else
                            {
                                p = 1;
                            }
                            if (targetAddres.Equals(baseValue))
                            {
                                e = 0;
                            }
                            else
                            {
                                e = 1;
                            }
                            ObCodeDigitOne = instruction.oCode[0];
                            obCodeDigitTwo = instruction.oCode[1];
                            byteOne = convertfromHextoBinary(ObCodeDigitOne);
                            byteTwo = convertfromHextoBinary(obCodeDigitTwo).Substring(0, 2);
                            objectCode.Add(convertFromBinaryToHex(byteOne + byteTwo + n + i + x + b + p + e) + (targetAddres.PadLeft(5, '0')));
                            break;
                    }
                }
                else
                {
                    objectCode.Add("-");
                }
            }


        }



        static void setBase()
        {
            for (int i = 0; i < modifiedMnemonics.Count; i++)
            {
                if (modifiedMnemonics[i].Equals("BASE"))
                {
                    for(int c = 0; c < modifiedLabels.Count; i++)
                    {
                        if (modifiedLabels[c].Equals(modifiedFirstOperand[i]))
                        {
                            baseValue = locations[c];
                        }
                    }
                }
            }
        }


        static String regNumber(string s)
        {
            char s1 = s[0];
            switch (s1)
            {
                case 'A':
                case '-':    
                    return "0000";
                case 'X': return "0001";
                case 'L': return "0010";
                case 'B': return "0011";
                case 'S': return "0100";
                case 'T': return "0101";
                case 'F': return "0110";
                default:  return "0000";
            }
        }


        static bool isDirective(String s)
        {
            bool f = true;
            String[] directives = new string[7];
            directives[0] = "START";
            directives[1] = "END";
            directives[2] = "EQU";
            directives[3] = "BASE";
            directives[4] = "LTORG";
            directives[5] = "EXTDEF";
            directives[6] = "ESTREF";
           for(int i = 0; i < directives.Length; i++)
            {
                if (directives[i].Equals(s))
                {
                    f = true;
                    return f;
                }
                f = false;
            }
            return f;
        }
        static void  getLiteral()
        {
             List<String> ltrl = new List<string>();
            ltrl.Insert(0, "literal\tlocation\tvalue");
            for(int i = 0; i < modifiedMnemonics.Count; i++)
            {
                if (modifiedMnemonics[i].Contains("="))
                {
                    string temps = modifiedMnemonics[i - 1].Substring(2, 3);
                    string temp = "";
                    foreach (var c in temps)
                    {
                        temp= temp+(int)c;
                    }
                    ltrl.Add(modifiedMnemonics[i]+"\t"+locations[i]+"\t\t"+temp);
                }
            }
            File.WriteAllLines(@"literalTable.txt",ltrl);
        }
        static void getSymbolTable()
        {
            List<String> temp = new List<string>();
               
            for (int i=0;i<modifiedLabels.Count;i++)
            {
                if(!modifiedLabels[i].Contains("-") && !modifiedLabels[i].Contains("*"))
                {
                    temp.Add(modifiedLabels[i] + "\tat Location\t" + locations[i]);
                }
            }
            File.WriteAllLines(@"symTable.txt",temp);
        }
        
        static bool isFormatOne(String mnemonic)
        {
            bool firstFormat = false;
            List<String> formatOne = new List<String>();
            formatOne.Add("FIX");
            formatOne.Add("FLOAT");
            formatOne.Add("HIO");
            formatOne.Add("NORM");
            formatOne.Add("SIO");
            formatOne.Add("TIO");
            for(int i = 0; i < formatOne.Count; i++)
            {
                if (mnemonic.Equals(formatOne[i]))
                {
                    firstFormat = true;
                }
            }
            return firstFormat;
        }
        static bool isFormatTwo(String mnemonic)
        {
            bool secondFormat = false;
            List<String> formatTwo = new List<string>();
            formatTwo.Add("ADDR");
            formatTwo.Add("CLEAR");
            formatTwo.Add("COMPR");
            formatTwo.Add("DIVR");
            formatTwo.Add("MULR");
            formatTwo.Add("RMO");
            formatTwo.Add("SHIFTL");
            formatTwo.Add("SHIFTR");
            formatTwo.Add("SUBR");
            formatTwo.Add("SVC");
            formatTwo.Add("TIXR");
            for(int i = 0; i < formatTwo.Count; i++)
            {
                if (mnemonic.Equals(formatTwo[i]))
                {
                    secondFormat = true;
                    break;
                }
            }
            return secondFormat;

        }
            public   static void Main()
        {
             readSrcCode();
             File.WriteAllLines(@"modifiedSrcCode.txt", modifiedLines);
             getPartsModified();      
             generatePassOne();
             setBase();
            generateObCode();
            getLiteral();
            getSymbolTable();
            List<String> Locs = new List<string>();
             Locs.Insert(0, "Location\t  Source Statement\t\tObject Code");
            
            for (int c = 1; c <= modifiedLines.Count; c++)
            {
                Locs.Insert(c, locations[c-1]+"\t\t"+modifiedLines[c-1]+"\t\t"+objectCode[c-1]);
            }
             File.WriteAllLines(@"PassTwo.txt",Locs );
             File.WriteAllLines(@"tesssssssst.txt",sTEMPA);
             File.WriteAllLines(@"operandOne.txt",modifiedFirstOperand);
             File.WriteAllLines(@"secondOperand.txt",modifiedSecondOperand);
             getLiteral();
             getSymbolTable();
            
            File.WriteAllLines(@"objectCode.txt",objectCode);
        }

         static String HexAdd(int temp, int temp2)
        {
            int resultInt = temp + temp2;
            String result = "" + resultInt;
            return result;

        }
        static String HexSub(String p, String t)
        {
            int v = Int32.Parse(p, System.Globalization.NumberStyles.HexNumber);
            int y = Int32.Parse(t, System.Globalization.NumberStyles.HexNumber);
            int diff = v - y;
            if (y > v)
            {
                return diff.ToString("X").Substring(diff.ToString().Length-3,3);
            }else
            {
                return diff.ToString("X").PadLeft(3,'0');

            }
        }
    }
}