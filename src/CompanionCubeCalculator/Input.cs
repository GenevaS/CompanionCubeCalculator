/*
 * Input Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
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
        private static string lineDelimeter = System.Environment.NewLine;
        private static string fieldDelimiter = ",";
        private static string[] validFileTypes = new string[] { ".txt" };

        /* GETTERS */
        public static string GetLineDelimiter()
        {
            return BuildLiteralString(lineDelimeter);
        }

        public static string GetFieldDelimiter()
        {
            return BuildLiteralString(fieldDelimiter);
        }

        public static string[] GetValidFileTypes()
        {
            return validFileTypes;
        }

        /* FILE I/O */
        public static string[] ReadFile(string fileName)
        {
            string[] inputs = null;

            if (File.Exists(fileName))
            {
                try
                {
                    if (fileName.Contains(".txt"))
                    {
                        using (StreamReader inStream = new StreamReader(fileName, Encoding.UTF8))
                        {
                            string line = inStream.ReadLine();

                            if (line != null)
                            {
                                if (line.Contains(fieldDelimiter.ToString()))
                                {
                                    frm_Main.UpdateLog("Error: The first line of the file is not an equation or the equation contains ','." + System.Environment.NewLine);
                                }
                                else
                                {
                                    inputs = new string[2];
                                    if (line.Contains("="))
                                    {
                                        inputs[0] = Regex.Split(RemoveWhitespace(line, false), "=")[1];
                                    }
                                    else
                                    {
                                        inputs[0] =RemoveWhitespace(line, false);
                                    }

                                    line = inStream.ReadToEnd();
                                    inputs[1] = RemoveWhitespace(line, true);

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
                catch (System.Exception)
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

        public static string RemoveWhitespace(string line, bool preserveSpecialWhitespace)
        {
            string conditionedLine;

            if(preserveSpecialWhitespace)
            {
                conditionedLine = Regex.Replace(line, @"[^\S\r\n\t]+", "");
            }
            else
            {
                conditionedLine = Regex.Replace(line, @"\s+", "");
            }

            return conditionedLine;
        }

        /* HELPER FUNCTIONS */
        private static string BuildLiteralString(string line)
        {
            string output = "";

            foreach (char c in line)
            {
                switch (c)
                {
                    case '\r':
                        output += @"\r";
                        break;
                    case '\n':
                        output += @"\n";
                        break;
                    default:
                        output += c;
                        break;
                }
            }

            return output;
        }
    }
}