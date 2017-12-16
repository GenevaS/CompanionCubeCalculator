/*
 * Input Tests
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
    public class InputTests
    {
        [TestMethod]
        public void TestGetters()
        {
            string test = Input.GetLineDelimiter();
            Assert.AreEqual(@"\r\n", test);

            test = Input.GetFieldDelimiter();
            Assert.AreEqual(",", test);

            string[] testFileTypes = Input.GetValidFileTypes();
            Assert.AreEqual(1, testFileTypes.Length);
            Assert.AreEqual("*.txt", testFileTypes[0]);
        }

        [TestMethod]
        public void TestGoodFile()
        {
            string fileName = @"TestFiles/test.txt";
            string targetEq = "x+y";
            string targetIv = "x,2,4"+ System.Environment.NewLine + "y,3,5";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(targetEq, fileContents[0]);
            Assert.AreEqual(targetIv, fileContents[1]);

            fileName = @"TestFiles/testWithEquals.txt";
            fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(targetEq, fileContents[0]);
            Assert.AreEqual(targetIv, fileContents[1]);
        }

        [TestMethod]
        public void TestEmptyFile()
        {
            string fileName = @"TestFiles/testempty.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestWrongFileType()
        {
            string fileName = @"TestFiles/test.tex";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestMissingEquation()
        {
            string fileName = @"TestFiles/testNoEq.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestMissingFile()
        {
            string fileName = @"null.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestInadequetePermission()
        {
            string fileName = @"TestFiles/test.txt";
            string[] fileContents;

            using (System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
            {
                fileContents = Input.ReadFile(fileName);
            }

            Assert.AreEqual(null, fileContents);
        }
    }
}
