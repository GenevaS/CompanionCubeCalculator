/*
 * Variable Consolidation Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
 * ---------------------------------------------------------------------
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanionCubeCalculator;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class VariableConsolidationTests
    {
        [TestMethod]
        public void TestInit()
        {
            // unittest-consolidatetestinit
            bool success = Consolidate.Initialize();

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void TestFailedConfig()
        {
            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // unittest-consolidatenoops
            OperatorStruct[] ops = new OperatorStruct[] { };
            int success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", ops, Solver.GetValidTerminators(), System.Environment.NewLine, ",");
            Assert.AreEqual(-1, success);

            // unittest-consolidatenorightterm
            string[][] terminators = new string[][] { new string[] { "(", "" } };
            success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), terminators, System.Environment.NewLine, ",");
            Assert.AreEqual(-1, success);

            // unittest-consolidatenoleftterm
            terminators = new string[][] { new string[] { "", ")" } };
            success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), terminators, System.Environment.NewLine, ",");
            Assert.AreEqual(-1, success);
        }

        [TestMethod]
        public void TestSimpleInputs()
        {
            string varToken = EquationConversion.GetVariableToken();

            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // unittest - consolidatesimpleinputs
            Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), Solver.GetValidTerminators(), "\n", ",");
            EquationStruct eqRoot = Consolidate.GetEquationStruct();
            IntervalStruct[] vars = Consolidate.GetIntervalStructList();

            EquationStruct targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] targetIntervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3, true, true), new IntervalStruct("y", 4, 5, true, true) };

            Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(eqRoot));
            Assert.AreEqual(targetIntervals[0].GetVariableName(), vars[0].GetVariableName());
            Assert.AreEqual(targetIntervals[0].GetMinBound(), vars[0].GetMinBound());
            Assert.AreEqual(targetIntervals[0].GetMaxBound(), vars[0].GetMaxBound());

            Assert.AreEqual(targetIntervals[1].GetVariableName(), vars[1].GetVariableName());
            Assert.AreEqual(targetIntervals[1].GetMinBound(), vars[1].GetMinBound());
            Assert.AreEqual(targetIntervals[1].GetMaxBound(), vars[1].GetMaxBound());

            Assert.AreEqual(2, vars.Length);
        }

        [TestMethod]
        public void TestExtraVariable()
        {
            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // test-input variableNotInFunction
            string varToken = EquationConversion.GetVariableToken();

            Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5\nz,6,7", Solver.GetValidOperators(), Solver.GetValidTerminators(), "\n", ",");
            EquationStruct eqRoot = Consolidate.GetEquationStruct();
            IntervalStruct[] vars = Consolidate.GetIntervalStructList();

            EquationStruct targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] targetIntervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3, true, true), new IntervalStruct("y", 4, 5, true, true) };

            Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(eqRoot));
            Assert.AreEqual(targetIntervals[0].GetVariableName(), vars[0].GetVariableName());
            Assert.AreEqual(targetIntervals[0].GetMinBound(), vars[0].GetMinBound());
            Assert.AreEqual(targetIntervals[0].GetMaxBound(), vars[0].GetMaxBound());

            Assert.AreEqual(targetIntervals[1].GetVariableName(), vars[1].GetVariableName());
            Assert.AreEqual(targetIntervals[1].GetMinBound(), vars[1].GetMinBound());
            Assert.AreEqual(targetIntervals[1].GetMaxBound(), vars[1].GetMaxBound());

            Assert.AreEqual(2, vars.Length);
        }

        [TestMethod]
        public void TestMissingVariable()
        {
            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // test-input noDomain
            int success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3", Solver.GetValidOperators(), Solver.GetValidTerminators(), System.Environment.NewLine, ",");
            
            Assert.AreEqual(-2, success);
        }

        [TestMethod]
        public void TestVarExtraction()
        {
            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // unittest-consolidateextractvariables
            string[] vars = Consolidate.ExtractVariablesFromEquation("x+y");

            Assert.AreEqual(2, vars.Length);
            Assert.AreEqual("x", vars[0]);
            Assert.AreEqual("y", vars[1]);
        }

        [TestMethod]
        public void TestIncompleteEquation()
        {
            EquationConversion.ResetEquationConversion();
            Consolidate.Initialize();

            // unittest-consolidateincompleteequation
            int successCode = Consolidate.ConvertAndCheckInputs("", "x,2,3\n", Solver.GetValidOperators(), Solver.GetValidTerminators(), Input.GetLineDelimiter(), Input.GetFieldDelimiter());
            Assert.AreEqual(-3, successCode);
        }


        /* HELPER FUNCTIONS */
        private static string PrintEquation(EquationStruct node)
        {
            string equation = "";

            if (node.GetLeftOperand() == null)
            {
                equation = node.GetOperator() + ": " + node.GetVariableName();
            }
            else if (node.GetRightOperand() == null)
            {
                equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ")";
            }
            else
            {
                equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ", " + PrintEquation(node.GetRightOperand()) + ")";
            }

            return equation;
        }
    }
}
