/*
 * Input Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/14
 * Corresponds to Input Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CompanionCubeCalculator
{
    public static class Input
    {
        private static char lineDelimeter = '\n';
        private static char fieldDelimiter = ',';
        private static string[] validFileTypes = new string[] { ".txt" };
    
        /* GETTERS */
        public static char GetLineDelimiter()
        {
            return lineDelimeter;
        }

        public static char GetFieldDelimiter()
        {
            return fieldDelimiter;
        }

        public static string[] GetValidFileTypes()
        {
            return validFileTypes;
        }

        /* FILE I/O */
        public static string[] ReadFile(string fileName)
        {
            string[] inputs = null;

            if(File.Exists(fileName))
            {
                try
                {
                    if(fileName.Contains(".txt"))
                    {
                        using (StreamReader inStream = new StreamReader(fileName, Encoding.UTF8))
                        {
                            string line = inStream.ReadLine();

                            if(line != null)
                            {
                                if(line.Contains(fieldDelimiter.ToString()))
                                {
                                    frm_Main.UpdateLog("Error: The first line of the file is not an equation or the equation contains ','." + System.Environment.NewLine);
                                }
                                else
                                {
                                    inputs = new string[2];
                                    inputs[0] = Regex.Replace(line, @"\s+", "");

                                    line = inStream.ReadToEnd();
                                    if (line != null)
                                    {
                                        inputs[1] = Regex.Replace(line, @"[^\S\r\n]+", "");
                                    }
                                    else
                                    {
                                        inputs[1] = "";
                                    }
                                    
                                }
                            }
                            else
                            {
                                frm_Main.UpdateLog("Error: The file is empty." + System.Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        frm_Main.UpdateLog("Error: Cannot read files of this type." + System.Environment.NewLine);
                    }
                    
                }
                catch(System.Exception)
                {
                    frm_Main.UpdateLog("Error: The file could not be read." + System.Environment.NewLine);
                }
            }
            else
            {
                frm_Main.UpdateLog("Error: The specified file does not exist." + System.Environment.NewLine);
            }

            return inputs;
        }
    }
}