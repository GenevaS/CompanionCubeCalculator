/*
 * Equation Conversion Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/09 (Incomplete)
 * Corresponds to the Equation Conversion Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * 
 * Equation parsing is completed using the Precedence Climbing algorithm
 * from https://www.engr.mun.ca/~theo/Misc/exp_parsing.htm#climbing
 * ---------------------------------------------------------------------
 */

using System.Linq;
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

        /* REGULAR EXPRESSION VARIABLES */
        private static readonly List<string> reservedChars = new List<string>() { "-", "\\", "]" };
        private static string variableStringPattern = "^[^0-9][^";
        private const string constantNumberStringPattern = "^[0-9]+";
        private const string ENDTOKEN = "END";

        /* GETTERS */
        public static string[] GetVariableList()
        {
            return variableList.ToArray();
        }

        /* SETTER */
        public static bool ConfigureParser(OperatorStruct[] ops)
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

                    if(reservedChars.Contains(op.GetOperator()))
                    {
                        opList += "\\" + op.GetOperator() + ",";
                    }
                    else
                    {
                        opList += op.GetOperator() + ",";
                    }
                }
            }

            if(success)
            {
                variableStringPattern += opList.Substring(0, opList.Length - 1) + "]*";
                frm_Main.UpdateLog("String Pattern: " + variableStringPattern + System.Environment.NewLine);
            }

            return success;
        }

        public static EquationStruct ParseEquation(string equationIn)
        {
            EquationStruct node = null;

            node = ExpressionEquation(ref equationIn, 0);
            Expect(equationIn, ENDTOKEN);

            return node;
        }

        private static EquationStruct ExpressionEquation(ref string equationIn, int prec)
        {
            EquationStruct node = AtomicEquation(ref equationIn);
            EquationStruct child = null;
            string op = "";
            int nextPrecedence = 0;

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
                    node = MakeNode(op, node, child);
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

            if (unaryOpsSym.Contains(Next(equationIn)))
            {
                op = Consume(ref equationIn, op.Length);
                precedence = unaryOps[unaryOpsSym.BinarySearch(op)].GetPrecedence();
                child = ExpressionEquation(ref equationIn, precedence);
                node = MakeNode(op, child, null);
            }
            else if(Next(equationIn) == "(")
            {
                Consume(ref equationIn, 1);
                child = ExpressionEquation(ref equationIn, 0);
                Expect(equationIn, ")");
                node = MakeNode("()", child, null);
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
            else
            {
                throw new System.ArgumentException("Error: Invalid sequence encountered during Atomic Equation parsing.");
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
                leaf = new EquationStruct("CONST", terminal, null, null);
            }
            catch (System.FormatException)
            {
                leaf = new EquationStruct("VAR", terminal, null, null);
            }

            return leaf;
        }

        /* HELPER FUNCTIONS */
        private static string GetNextVariableName(string equationIn)
        {
            return Regex.Match(equationIn, variableStringPattern).Value;
        }

        private static string GetNextConstant(string equationIn)
        {
            return Regex.Match(equationIn, constantNumberStringPattern).Value;
        }

        private static int FindIndexForSymbol(string sym, List<OperatorStruct> options)
        {
            int index = -1;
            
            foreach (OperatorStruct os in options)
            {
                if(sym == os.GetOperator())
                {
                    index = options.IndexOf(os);
                    break;
                }
            }

            return index;
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
         * Returns the next token in the string to be parsed.
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

        /*
        
        
        public static EquationStruct MakeEquationTree(string userEquation, OperatorStruct[] supportedOperations, string[] supportedTerminators)
        {
            EquationStruct eqRoot = null;
            string[] components;
            double constant;

            try
            {
                constant = System.Convert.ToDouble(userEquation);
                eqRoot = new EquationStruct("Const", "var" + userEquation, null, null);
                variableList.Add("var" + userEquation);

                frm_Main.UpdateLog("Warning: The user equation is a constant value and the range will only include this value.");
            }
            catch(System.FormatException)
            {
                for (int i = supportedOperations.Length - 1; i >= 0; i--)
                {
                    components = userEquation.Split(supportedOperations[i].ToCharArray()[0]);
                    for (int j = 0; j < components.Length; j++)
                    {
                        frm_Main.UpdateLog(components[j] + System.Environment.NewLine);
                    }
                }
            }

            return eqRoot;
        }

        */
    }
}