/*
 * Variable Consolidation Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/12
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
        public void TestFailedConfig()
        {
            OperatorStruct[] ops = new OperatorStruct[] { };
            bool success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", ops, Solver.GetValidTerminators());
            Assert.AreEqual(false, success);

            string[][] terminators = new string[][] { new string[] { "(", "" } };
            success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), terminators);
            Assert.AreEqual(false, success);

            terminators = new string[][] { new string[] { "", ")" } };
            success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), terminators);
            Assert.AreEqual(false, success);
        }

        [TestMethod]
        public void TestSimpleInputs()
        {
            string varToken = EquationConversion.GetVariableToken();

            Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), Solver.GetValidTerminators());
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
            // test-input variableNotInFunction
            string varToken = EquationConversion.GetVariableToken();

            Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5\nz,6,7", Solver.GetValidOperators(), Solver.GetValidTerminators());
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
            // test-input noDomain
            bool success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3", Solver.GetValidOperators(), Solver.GetValidTerminators());
            
            Assert.AreEqual(false, success);
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
