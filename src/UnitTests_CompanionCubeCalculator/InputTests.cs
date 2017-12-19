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
            // unittest-testgetlinedelimiter
            string test = Input.GetLineDelimiter();
            Assert.AreEqual(@"\r\n", test);

            // unittest-testgetfielddelimiter
            test = Input.GetFieldDelimiter();
            Assert.AreEqual(",", test);

            // unittest-testgetfiletypes
            string[] testFileTypes = Input.GetValidFileTypes();
            Assert.AreEqual(1, testFileTypes.Length);
            Assert.AreEqual("*.txt", testFileTypes[0]);
        }

        [TestMethod]
        public void TestGoodFile()
        {
            // unittest-fileinput
            string fileName = @"TestFiles/test.txt";
            string targetEq = "x+y";
            string targetIv = "x,2,4"+ System.Environment.NewLine + "y,3,5";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(targetEq, fileContents[0]);
            Assert.AreEqual(targetIv, fileContents[1]);

            // unittest-fileinputwithequals
            fileName = @"TestFiles/testWithEquals.txt";
            fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(targetEq, fileContents[0]);
            Assert.AreEqual(targetIv, fileContents[1]);
        }

        [TestMethod]
        public void TestEmptyFile()
        {
            // unittest-emptyfile
            string fileName = @"TestFiles/testempty.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestWrongFileType()
        {
            // unittest-invalidfiletype
            string fileName = @"TestFiles/test.tex";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestMissingEquation()
        {
            // unittest-input\_noFunctionFile
            string fileName = @"TestFiles/testNoEq.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestMissingFile()
        {
            // unittest-noFile
            string fileName = @"null.txt";

            string[] fileContents = Input.ReadFile(fileName);

            Assert.AreEqual(null, fileContents);
        }

        [TestMethod]
        public void TestInadequetePermission()
        {
            string fileName = @"TestFiles/test.txt";
            string[] fileContents;

            // unittest-badFileInput
            using (System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
            {
                fileContents = Input.ReadFile(fileName);
            }

            Assert.AreEqual(null, fileContents);
        }
    }
}
