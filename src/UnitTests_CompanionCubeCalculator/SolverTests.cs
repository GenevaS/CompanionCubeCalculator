/*
 * Range Solver Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/14
 * ---------------------------------------------------------------------
 */

using CompanionCubeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class OperatorStructTests
    {
        [TestMethod]
        public void TestOperatorConstructor()
        {
            OperatorStruct op = new OperatorStruct("+", 1, false, true, false, true);

            Assert.AreEqual("+", op.GetOperator());
            Assert.AreEqual(1, op.GetPrecedence());
            Assert.AreEqual(false, op.IsUnary());
            Assert.AreEqual(true, op.IsBinary());
            Assert.AreEqual(false, op.IsTernary());
            Assert.AreEqual(true, op.IsLeftAssociative());
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: Cannot have an operator with no representative symbol.")]
        public void TestMissingOpSymbol()
        {
            OperatorStruct op = new OperatorStruct("", 1, false, true, false, true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: Precedence values must be greater than 0.")]
        public void TestLowOpPrecedence()
        {
            OperatorStruct op = new OperatorStruct("+", 0, false, true, false, true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: An operator cannot be overloaded to be unary, binary, and ternary.")]
        public void TestOpType1()
        {
            OperatorStruct op = new OperatorStruct("+", 1, true, true, false, true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: An operator cannot be overloaded to be unary, binary, and ternary.")]
        public void TestOpType2()
        {
            OperatorStruct op = new OperatorStruct("+", 1, false, true, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: An operator cannot be overloaded to be unary, binary, and ternary.")]
        public void TestOpType3()
        {
            OperatorStruct op = new OperatorStruct("+", 1, true, false, true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: Operators must be assigned a number of operands type.")]
        public void TestNoOpType()
        {
            OperatorStruct op = new OperatorStruct("+", 1, false, false, false, true);
        }
    }

    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void TestMissingIntervals()
        {
            string varToken = EquationConversion.GetVariableToken();
            EquationStruct equation = new EquationStruct(varToken, "x", null, null);
            IntervalStruct[] intervals = new IntervalStruct[] { };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        [TestMethod]
        public void TestAtomicEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            EquationStruct equation = new EquationStruct(constToken, "42", null, null);
            IntervalStruct[] intervals = new IntervalStruct[] { };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(42, range.GetMinBound());
            Assert.AreEqual(42, range.GetMaxBound());

            equation = new EquationStruct(varToken, "x", null, null);
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(2, range.GetMinBound());
            Assert.AreEqual(3, range.GetMaxBound());
        }

        [TestMethod]
        public void TestAdditionEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            // test-calculate addition
            EquationStruct equation = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(5, range.GetMinBound());
            Assert.AreEqual(9, range.GetMaxBound());

            // 
            equation = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "4", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(6, range.GetMinBound());
            Assert.AreEqual(7, range.GetMaxBound());

            // 
            equation = new EquationStruct("+", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(varToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        [TestMethod]
        public void TestSubtractionEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            // test-calculate subtraction 
            EquationStruct equation = new EquationStruct("-", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-1, range.GetMinBound());
            Assert.AreEqual(-1, range.GetMaxBound());

            // 
            equation = new EquationStruct("-", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(varToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        [TestMethod]
        public void TestMultiplicationEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            // test-calculate multiplication1
            EquationStruct equation = new EquationStruct("*", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(6, range.GetMinBound());
            Assert.AreEqual(20, range.GetMaxBound());

            // test-calculate multiplication2
            equation = new EquationStruct("*", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -1, 3, true, true), new IntervalStruct("y", -3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-9, range.GetMinBound());
            Assert.AreEqual(15, range.GetMaxBound());

            // test-calculate multiplication3
            equation = new EquationStruct("*", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -3, -1, true, true), new IntervalStruct("y", -5, -2, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(2, range.GetMinBound());
            Assert.AreEqual(15, range.GetMaxBound());

            // 
            equation = new EquationStruct("*", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(varToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        [TestMethod]
        public void TestDivisionEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            // test-calculate divisionPositiveDivisor1
            EquationStruct equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(2.0 / 5, range.GetMinBound());
            Assert.AreEqual(4.0 / 3, range.GetMaxBound());

            // test-calculate divisionPositiveDivisor2
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 0, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(0, range.GetMinBound());
            Assert.AreEqual(4.0 / 3, range.GetMaxBound());

            // test-calculate divisionPositiveDivisor3
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -1, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-1.0 / 3, range.GetMinBound());
            Assert.AreEqual(4.0 / 3, range.GetMaxBound());

            // test-calculate divisionPositiveDivisor5
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, -1, true, true), new IntervalStruct("y", 3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-2.0 / 3, range.GetMinBound());
            Assert.AreEqual(-1.0 / 5, range.GetMaxBound());

            // test-calculate divisionPositiveDivisor4
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, 0, true, true), new IntervalStruct("y", 3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-2.0 / 3, range.GetMinBound());
            Assert.AreEqual(0, range.GetMaxBound());

            // test-calculate divisionNegativeDivisor1
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -5, -3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4.0 / -3, range.GetMinBound());
            Assert.AreEqual(2.0 / -5, range.GetMaxBound());

            // test-calculate divisionNegativeDivisor2
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 0, 4, true, true), new IntervalStruct("y", -5, -3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4.0 / -3, range.GetMinBound());
            Assert.AreEqual(0, range.GetMaxBound());

            // test-calculate divisionNegativeDivisor3
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -1, 4, true, true), new IntervalStruct("y", -5, -3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4.0 / -3, range.GetMinBound());
            Assert.AreEqual(-1.0 / -3, range.GetMaxBound());

            // test-calculate divisionNegativeDivisor4
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, 0, true, true), new IntervalStruct("y", -5, -3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(0, range.GetMinBound());
            Assert.AreEqual(-2.0 / -3, range.GetMaxBound());

            // test-calculate divisionNegativeDivisor5
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, -1, true, true), new IntervalStruct("y", -5, -3, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(-1.0 / -5, range.GetMinBound());
            Assert.AreEqual(-2.0 / -3, range.GetMaxBound());

            // test-parse divisionMixedIntervalDivisor 
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // test-parse divisionMixedIntervalDivisorZero1
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 0, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // test-parse divisionMixedIntervalDivisorZero2
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 0, 3, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // test-calculate mixedDivisorComponent
            // Ideally, I would like this to return a partial answer
            equation = new EquationStruct("/", "", new EquationStruct(varToken, "x", null, null), new EquationStruct("()", "", new EquationStruct("*", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, -1, true, true), new IntervalStruct("y", 3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // 
            equation = new EquationStruct("/", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(varToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        [TestMethod]
        public void TestExponentEquations()
        {
            string varToken = EquationConversion.GetVariableToken();
            string constToken = EquationConversion.GetConstToken();

            // test-calculate intervalAsExponents
            EquationStruct equation = new EquationStruct("^", "", new EquationStruct(constToken, "2", null, null), new EquationStruct(varToken, "x", null, null));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalAsExponentsInvalidBase
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "1", null, null), new EquationStruct(constToken, "x", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -4, -2, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // test-parse intervalWithExponent1 
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "3", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(8, range.GetMinBound());
            Assert.AreEqual(64, range.GetMaxBound());

            // test-parse intervalWithExponent2
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalWithExponent3
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 0, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(0, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalWithExponent4
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -2, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(0, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalWithExponent5
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", -4, -2, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalWithInvalidExponent1
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "2.1", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(4, range.GetMinBound());
            Assert.AreEqual(16, range.GetMaxBound());

            // test-parse intervalWithInvalidExponent2
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "-1", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // test-parse intervalWithInvalidExponent2
            equation = new EquationStruct("^", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(constToken, "y", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);

            // 
            equation = new EquationStruct("^", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(constToken, "4", null, null));
            intervals = new IntervalStruct[] { new IntervalStruct("y", -3, 5, true, true), new IntervalStruct("z", -1, 1, true, true) };

            range = Solver.FindRange(equation, intervals);

            Assert.AreEqual(null, range);
        }

        /* Operational Precedence tests
         * x+y−z == f(V) = (x+y)−z,x = [2,4],y = [3,5],z = [2,2] 
         * x∗y/z == f(V) = (x∗y)/z,x = [2,4],y = [3,5],z = [2,2] 
         * x + y∗z == x + (y∗z),x = [2,4],y = [3,5],z = [2,2] 
         * 2x ∗y == (2x)∗y,x = [2,4],y = [3,5] 
         * x2 ∗y == (x2)∗y,x = [2,4],y = [3,5] 
         * x∗(y + z),x = [2,4],y = [3,5],z = [2,2] == [10,28]
         */
    }
}