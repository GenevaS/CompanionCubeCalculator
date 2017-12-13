/*
 * Equation Conversion and Data Structure Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/13
 * ---------------------------------------------------------------------
 */

using CompanionCubeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void TestSimpleEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            //
            EquationStruct equation = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3), new IntervalStruct("y", 4, 5) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(6, range.GetMinBound());
            Assert.AreEqual(8, range.GetMaxBound());

            //
            equation = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "4", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(6, range.GetMinBound());
            Assert.AreEqual(7, range.GetMaxBound());

            //
            equation = new EquationStruct("-", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3), new IntervalStruct("y", 4, 5) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-2, range.GetMinBound());
            Assert.AreEqual(-2, range.GetMaxBound());

            //
            equation = new EquationStruct("*", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3), new IntervalStruct("y", 4, 5) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(8, range.GetMinBound());
            Assert.AreEqual(15, range.GetMaxBound());

            //
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2.0, 3.0), new IntervalStruct("y", 4.0, 5.0) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(2.0/5.0, range.GetMinBound());
            Assert.AreEqual(3.0/4.0, range.GetMaxBound());

            //
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2.0, 3.0) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(9, range.GetMaxBound());

            //
            equation = new EquationStruct("^", "", new EquationStruct(constToken, "2", null, null), new EquationStruct(varToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2.0, 3.0) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(8, range.GetMaxBound());
        }
    }
}
