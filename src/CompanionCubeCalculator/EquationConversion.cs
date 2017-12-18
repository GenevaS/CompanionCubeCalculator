/*
 * Equation Conversion Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
 * Corresponds to the Equation Conversion Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * 
 * Equation parsing is completed using the Precedence Climbing algorithm
 * from https://www.engr.mun.ca/~theo/Misc/exp_parsing.htm#climbing
 * ---------------------------------------------------------------------
 */

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompanionCubeCalculator
{
    public static class EquationConversion
    {
        private static List<string> variableList = new List<string>();
        private static bool ready = false;
    
        /* OPERATOR VARIABLES */
        private static List<string> unaryOpsSym = new List<string>();
        private static List<OperatorStruct> unaryOps = new List<OperatorStruct>();
        private static List<string> binaryOpsSym = new List<string>();
        private static List<OperatorStruct> binaryOps = new List<OperatorStruct>();
        private static List<string> leftTerminators = new List<string>();
        private static List<string> rightTerminators = new List<string>();

        private const string VARTOKEN = "VAR";
        private const string CONSTTOKEN = "CONST";
        private const string ENDTOKEN = "END";

        /* REGULAR EXPRESSION VARIABLES */
        private static readonly List<string> reservedChars = new List<string>() { "-", "\\", "]" };

        private static string variableStringPattern = "";
        private static string variableStringPatternOpen = "^[^0-9,";
        private static string variableStringPatternClose = "]+[0-9]*";

        private const string constantNumberStringPattern = "^-?[0-9]+";

        private static string implicitMultiplicationPattern = "";
        private static string implicitMultiplicationPatternOpen = "(?<const>-?[0-9]+)(?<var>[^0-9,";
        private static string implicitMultiplicationPatternClose = "]+[0-9]*)";
        private static string implicitMultiplicationReplacement = "${const}*${var}";
       
        /* GETTERS */
        public static bool IsReady()
        {
            return ready;
        }
        public static string[] GetVariableList()
        {
            return variableList.ToArray();
        }

        public static string GetVariableToken()
        {
            return VARTOKEN;
        }

        public static string GetConstToken()
        {
            return CONSTTOKEN;
        }

        /* SETTER */
        public static bool ConfigureParser(OperatorStruct[] ops, string[][] terminators)
        {
            bool success = true;
            string opList = "";
            string termList = "";

            if(ops.Length == 0)
            {
                frm_Main.UpdateLog("Error: No operators were passed to the parser." + System.Environment.NewLine);
                success = false;
            }
            else
            {
                foreach (OperatorStruct op in ops)
                {
                    if(op.IsUnary())
                    {
                        unaryOpsSym.Add(op.GetOperator());
                        unaryOps.Add(op);
                    }
                    else if(op.IsBinary())
                    {
                        binaryOpsSym.Add(op.GetOperator());
                        binaryOps.Add(op);
                    }
                    else
                    {
                        frm_Main.UpdateLog("Error: The parser cannot process the " + op.GetOperator() + " operator." + System.Environment.NewLine);
                        success = false;
                    }

                    // Ensuring that reserved characters are escaped for RE matching
                    opList += EscapeReservedCharacters(op.GetOperator()) + ",";
                }

                for (int i = 0; i < terminators.Length; i++)
                {
                    if(terminators[i][0] != "" && terminators[i][1] != "")
                    {
                        leftTerminators.Add(terminators[i][0]);
                        termList += EscapeReservedCharacters(terminators[i][0]) + ",";

                        rightTerminators.Add(terminators[i][1]);
                        termList += EscapeReservedCharacters(terminators[i][1]) + ",";
                    }
                    else
                    {
                        if(terminators[i][0] != "")
                        {
                            frm_Main.UpdateLog("Error: An unbalanced left terminator token was encountered (" + terminators[i][0] + ")." + System.Environment.NewLine);
                        }
                        else
                        {
                            frm_Main.UpdateLog("Error: An unbalanced right terminator token was encountered (" + terminators[i][1] + ")." + System.Environment.NewLine);
                        }
                        
                        success = false;
                    }
                }
            }

            if(success)
            {
                // Create the pattern for RE matching
                variableStringPattern += variableStringPatternOpen + opList.Substring(0, opList.Length) + termList.Substring(0, termList.Length - 1) + variableStringPatternClose;
                implicitMultiplicationPattern += implicitMultiplicationPatternOpen + opList.Substring(0, opList.Length - 1) + implicitMultiplicationPatternClose;
            }

            ready = success;
            return success;
        }

        public static void ResetEquationConversion()
        {
            variableList.Clear();

            unaryOpsSym.Clear();
            unaryOps.Clear();

            binaryOpsSym.Clear();
            binaryOps.Clear();

            leftTerminators.Clear();
            rightTerminators.Clear();

            variableStringPattern = "";
            implicitMultiplicationPattern = "";

            ready = false;

            return;
        }

        /* PARSING FUNCTIONS */
        public static EquationStruct MakeEquationTree(string equationIn)
        {
            EquationStruct node = null;
            string parseString;

            // The error flag will be true if a parsing error is encountered
            bool error = false;

            try
            {
                System.Convert.ToDouble(equationIn);
                node = new EquationStruct(CONSTTOKEN, equationIn, null, null);

                frm_Main.UpdateLog("Warning: The user equation is a constant value and the range will only include this value." + System.Environment.NewLine);
            }
            catch (System.FormatException)
            {
                // Replacing implicit multiplications of constants by variables with an explicit operator
                if (binaryOpsSym.Contains("*"))
                {
                    string temp = equationIn;
                    Regex rgx = new Regex(implicitMultiplicationPattern);
                    equationIn = rgx.Replace(equationIn, implicitMultiplicationReplacement);
                    if(equationIn != temp)
                    {
                        frm_Main.UpdateLog("Warning: Encountered an implicit multiplication of a constant value and a variable. Expanding with explicit operator." + System.Environment.NewLine);
                    }
                }

                // Parse the equation -> return null if an error occurs
                parseString = equationIn;
                node = ExpressionEquation(ref parseString, 0, ref error);
                if (!error)
                {
                    Expect(ref parseString, ENDTOKEN, ref error);
                    if(error)
                    {
                        frm_Main.UpdateLog("Error: Could not find the end of the equation." + System.Environment.NewLine);
                        node = null;
                    }
                }
                else
                {
                    node = null;
                }
            }

            return node;
        }

        private static EquationStruct ExpressionEquation(ref string equationIn, int prec, ref bool error)
        {
            EquationStruct node = AtomicEquation(ref equationIn, ref error);
            EquationStruct child = null;
            string op = "";
            int nextPrecedence = 0;

            if(!error)
            {
                while (binaryOpsSym.Contains(Next(ref equationIn)) && binaryOps[binaryOpsSym.IndexOf(Next(ref equationIn))].GetPrecedence() >= prec)
                {
                    op = Consume(ref equationIn, binaryOps[binaryOpsSym.IndexOf(Next(ref equationIn))].GetOperator().Length);
                    if(binaryOps[binaryOpsSym.IndexOf(op)].IsLeftAssociative())
                    {
                        nextPrecedence = binaryOps[binaryOpsSym.IndexOf(op)].GetPrecedence() + 1;
                    }
                    else
                    {
                        nextPrecedence = binaryOps[binaryOpsSym.IndexOf(op)].GetPrecedence();
                    }

                    child = ExpressionEquation(ref equationIn, nextPrecedence, ref error);
                    if(!error)
                    {
                        node = MakeNode(op, node, child);
                    }
                    else
                    {
                        node = null;
                    }
                    
                }
            } 
            else
            {
                node = null;
            }
            
            return node;
        }

        private static EquationStruct AtomicEquation(ref string equationIn, ref bool error)
        {
            EquationStruct node = null;
            EquationStruct child = null;
            string nextVar = GetNextVariableName(equationIn);
            string nextConst = GetNextConstant(equationIn);
            string op = "";
            int precedence;
            int index;

            if (unaryOpsSym.Contains(Next(ref equationIn)))
            {
                op = Next(ref equationIn);
                Consume(ref equationIn, op.Length);
                precedence = unaryOps[unaryOpsSym.BinarySearch(op)].GetPrecedence();

                child = ExpressionEquation(ref equationIn, precedence, ref error);
                if(!error)
                {
                    node = MakeNode(op, child, null);
                }
            }
            else if(leftTerminators.Contains(Next(ref equationIn)))
            {
                index = leftTerminators.IndexOf(Next(ref equationIn));
                Consume(ref equationIn, leftTerminators[index].Length);

                child = ExpressionEquation(ref equationIn, 0, ref error);
                if(!error)
                {
                    Expect(ref equationIn, rightTerminators[index], ref error);
                    if(!error)
                    {
                        node = MakeNode(leftTerminators[index] + rightTerminators[index], child, null);
                    }
                }
            }
            else if (nextVar != "")
            {
                node = MakeLeaf(nextVar);
                if(!variableList.Contains(nextVar))
                {
                    variableList.Add(nextVar);
                }
                Consume(ref equationIn, nextVar.Length);
            }
            else if (nextConst != "")
            {
                node = MakeLeaf(nextConst);
                Consume(ref equationIn, nextConst.Length);
            }
            // Either an unbalanced operator or unexpected case was encountered -> return null
            else
            {
                frm_Main.UpdateLog("Error: Unrecognized sequence encountered during Atomic Equation parsing. Remaining equation = " + equationIn + System.Environment.NewLine);
                error = true;
            }

            return node;
        }

        /* NODE GENERATION FUNCTIONS */
        private static EquationStruct MakeNode(string op, EquationStruct left, EquationStruct right)
        {
            return new EquationStruct(op, "", left, right);
        }

        private static EquationStruct MakeLeaf(string terminal)
        {
            EquationStruct leaf = null;

            try
            {
                double constant = System.Convert.ToDouble(terminal);
                leaf = new EquationStruct(CONSTTOKEN, terminal, null, null);
            }
            catch (System.FormatException)
            {
                leaf = new EquationStruct(VARTOKEN, terminal, null, null);
            }

            return leaf;
        }

        /* HELPER FUNCTIONS */
        private static string EscapeReservedCharacters(string token)
        {
            string esc = token;

            if(reservedChars.Contains(token))
            {
                esc = "\\" + token;
            }

            return esc;
        }

        private static string GetNextVariableName(string equationIn)
        {
            return Regex.Match(equationIn, variableStringPattern).Value;
        }

        private static string GetNextConstant(string equationIn)
        {
            return Regex.Match(equationIn, constantNumberStringPattern).Value;
        }

        /*
         * ---------------------------------------------------------------------------
         * Next
         * ---------------------------------------------------------------------------
         * Returns the next token in the string to be parsed.
         * The source string is not modified by this function.
         * ---------------------------------------------------------------------------
         */
        private static string Next(ref string equationIn)
        {
            string token = "";

            if (equationIn == "")
            {
                token = ENDTOKEN;
            }
            else
            {
                token = equationIn.Substring(0, 1);
            }

            return token;
        }

        /*
         * ---------------------------------------------------------------------------
         * Consume
         * ---------------------------------------------------------------------------
         * Returns the next token of size length in the string to be parsed.
         * The read token is removed from the source string.
         * ---------------------------------------------------------------------------
         */
        private static string Consume(ref string equationIn, int length)
        {
            string token = ENDTOKEN;

            if (equationIn != "")
            {
                token = equationIn.Substring(0, length);
                equationIn = equationIn.Substring(length);
            }

            return token;
        }

        /*
         * ---------------------------------------------------------------------------
         * Expect
         * ---------------------------------------------------------------------------
         * Returns the next token in the string to be parsed if it matches the token 
         * string tok. If the next token is not tok, this function throws an error.
         * The read token is removed from the source string if the function is 
         * successful.
         * ---------------------------------------------------------------------------
         */
        private static string Expect(ref string equationIn, string tok, ref bool error)
        {
            string token = "";

            if (Next(ref equationIn) == tok)
            {
                token = Consume(ref equationIn, tok.Length);
            }
            else
            {
                frm_Main.UpdateLog("Error: Could not find expected token " + tok + "." + System.Environment.NewLine);
                error = true;
            }

            return token;
        }
    }
}