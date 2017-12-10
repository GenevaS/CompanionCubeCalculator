/*
 * Equation Conversion Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/09
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
    
        /* OPERATOR VARIABLES */
        private static List<string> unaryOpsSym = new List<string>();
        private static List<OperatorStruct> unaryOps = new List<OperatorStruct>();
        private static List<string> binaryOpsSym = new List<string>();
        private static List<OperatorStruct> binaryOps = new List<OperatorStruct>();
        private static List<string> leftTerminators = new List<string>();
        private static List<string> rightTerminators = new List<string>();

        private const string VARTOKEN = "VAR";
        private const string CONSTTOKEN = "CONST";

        /* REGULAR EXPRESSION VARIABLES */
        private static readonly List<string> reservedChars = new List<string>() { "-", "\\", "]" };
        private static string variableStringPattern = "^[^0-9,";
        private static string variableStringPatternMid = "]*[^";
        private static string variableStringPatternClose = "]+";
        private const string constantNumberStringPattern = "^[0-9]+";
        private static string implicitMultiplicationPattern = "(?<const>[0-9]+)(?<var>[^0-9,";
        private static string implicitMultiplicationPatternClose = "]+)";
        private static string implicitMultiplicationReplacement = "${const}*${var}";
        private const string ENDTOKEN = "END";

        /* GETTERS */
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

            if(ops.Length == 0)
            {
                frm_Main.UpdateLog("Error: No operators were passed to the parser.");
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
                        frm_Main.UpdateLog("Error: The parser cannot process the " + op.GetOperator() + " operator.");
                        success = false;
                    }

                    // Ensuring that reserved characters are escaped for RE matching
                    opList += EscapeReservedCharacters(op.GetOperator()) + ",";
                }

                for (int i = 0; i < terminators.Length; i++)
                {
                    if(terminators[i][1] != "")
                    {
                        leftTerminators.Add(terminators[i][0]);
                        opList += EscapeReservedCharacters(terminators[i][0]) + ",";

                        rightTerminators.Add(terminators[i][1]);
                        opList += EscapeReservedCharacters(terminators[i][1]) + ",";
                    }
                    else
                    {
                        frm_Main.UpdateLog("Error: An unbalanced terminator token was encountered (" + terminators[i][0] + ").");
                        success = false;
                    }
                }
            }

            if(success)
            {
                // Create the pattern for RE matching
                variableStringPattern += opList.Substring(0, opList.Length - 1) + variableStringPatternMid + opList.Substring(0, opList.Length - 1) + variableStringPatternClose;
                implicitMultiplicationPattern += opList.Substring(0, opList.Length - 1) + implicitMultiplicationPatternClose;
                frm_Main.UpdateLog(variableStringPattern + System.Environment.NewLine);
            }

            return success;
        }

        /* PARSING FUNCTIONS */
        public static EquationStruct MakeEquationTree(string equationIn)
        {
            EquationStruct node = null;

            try
            {
                System.Convert.ToDouble(equationIn);
                node = new EquationStruct(CONSTTOKEN, equationIn, null, null);

                frm_Main.UpdateLog("Warning: The user equation is a constant value and the range will only include this value.");
            }
            catch (System.FormatException)
            {
                // Replacing implicit multiplications of constants by variables with an explicit operator
                if (binaryOpsSym.Contains("*"))
                {
                    Regex rgx = new Regex(implicitMultiplicationPattern);
                    equationIn = rgx.Replace(equationIn, implicitMultiplicationReplacement);
                }

                node = ExpressionEquation(ref equationIn, 0);
                if (node != null)
                {
                    Expect(equationIn, ENDTOKEN);
                }
            }

            return node;
        }

        private static EquationStruct ExpressionEquation(ref string equationIn, int prec)
        {
            EquationStruct node = AtomicEquation(ref equationIn);
            EquationStruct child = null;
            string op = "";
            int nextPrecedence = 0;

            if(node != null)
            {
                while (binaryOpsSym.Contains(Next(equationIn)) && binaryOps[binaryOpsSym.IndexOf(Next(equationIn))].GetPrecedence() >= prec)
                {
                    op = Consume(ref equationIn, binaryOps[binaryOpsSym.IndexOf(Next(equationIn))].GetOperator().Length);
                    if(binaryOps[binaryOpsSym.IndexOf(op)].IsLeftAssociative())
                    {
                        nextPrecedence = binaryOps[binaryOpsSym.IndexOf(op)].GetPrecedence() + 1;
                    }
                    else
                    {
                        nextPrecedence = binaryOps[binaryOpsSym.IndexOf(op)].GetPrecedence();
                    }

                    child = ExpressionEquation(ref equationIn, nextPrecedence);
                    if(child != null)
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
                equationIn = "";
            }
            
            return node;
        }

        private static EquationStruct AtomicEquation(ref string equationIn)
        {
            EquationStruct node = null;
            EquationStruct child = null;
            string nextVar = GetNextVariableName(equationIn);
            string nextConst = GetNextConstant(equationIn);
            string op = "";
            int precedence;
            int index;

            if (unaryOpsSym.Contains(Next(equationIn)))
            {
                op = Consume(ref equationIn, op.Length);
                precedence = unaryOps[unaryOpsSym.BinarySearch(op)].GetPrecedence();

                child = ExpressionEquation(ref equationIn, precedence);
                if(child != null)
                {
                    node = MakeNode(op, child, null);
                }
            }
            else if(leftTerminators.Contains(Next(equationIn)))
            {
                index = leftTerminators.IndexOf(Next(equationIn));
                Consume(ref equationIn, leftTerminators[index].Length);

                child = ExpressionEquation(ref equationIn, 0);
                if(child != null)
                {
                    Expect(equationIn, rightTerminators[index]);
                    node = MakeNode(leftTerminators[index] + rightTerminators[index], child, null);
                }
            }
            else if (nextVar != "")
            {
                node = MakeLeaf(nextVar);
                variableList.Add(nextVar);
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
                //throw new System.ArgumentException("Error: Unrecognized sequence encountered during Atomic Equation parsing." + System.Environment.NewLine);
                frm_Main.UpdateLog("Error: Unrecognized sequence encountered during Atomic Equation parsing. Remaining equation = " + equationIn + System.Environment.NewLine);
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
        private static string Next(string equationIn)
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
        private static string Expect(string equationIn, string tok)
        {
            string token = "";

            if (Next(equationIn) == tok)
            {
                token = Consume(ref equationIn, tok.Length);
            }
            else
            {
                throw new System.ArgumentException("Error: Could not find expected token " + tok + ".");
            }

            return token;
        }
    }
}