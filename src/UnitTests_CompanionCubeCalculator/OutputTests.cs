/*
 * Output Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/14
 * ---------------------------------------------------------------------
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanionCubeCalculator;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class OutputTests
    {
        [TestMethod]
        public void TestPrintIntervals()
        {
            IntervalStruct interval = new IntervalStruct("x", 3,5, true, true);
            Assert.AreEqual("x = [3, 5]", Output.PrintInterval(interval, true));

            interval = new IntervalStruct("x", 3, 5, false, false);
            Assert.AreEqual("x = (3, 5)", Output.PrintInterval(interval, true));

            interval = new IntervalStruct("x", 3, 5, true, false);
            Assert.AreEqual("x = [3, 5)", Output.PrintInterval(interval, true));

            interval = new IntervalStruct("x", 3, 5, false, true);
            Assert.AreEqual("x = (3, 5]", Output.PrintInterval(interval, true));

            interval = new IntervalStruct("", 3, 3, false, false);
            Assert.AreEqual("CONST: 3", Output.PrintInterval(interval, true));

            interval = new IntervalStruct("", 3, 3, false, false);
            Assert.AreEqual("CONST: 3", Output.PrintInterval(interval, false));
        }

        [TestMethod]
        public void TestPrintEquation()
        {
            string varToken = EquationConversion.GetVariableToken();

            EquationStruct equation = new EquationStruct("+", "", new EquationStruct("/", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)), new EquationStruct(varToken, "x", null, null));
            string target = "";
            target += "+- {+}" + System.Environment.NewLine;
            target += "|   +- {/}" + System.Environment.NewLine;
            target += "|   |  +- {VAR} y" + System.Environment.NewLine;
            target += "|   |  +- {VAR} z" + System.Environment.NewLine;
            target += "|   +- {VAR} x" + System.Environment.NewLine;

            Assert.AreEqual(System.Text.RegularExpressions.Regex.Replace(target, @"\s+", ""), System.Text.RegularExpressions.Regex.Replace(Output.PrintEquationTree(equation), @"\s+", ""));
        }
    }
}
