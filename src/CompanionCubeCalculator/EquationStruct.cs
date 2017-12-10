/*
 * Equation Data Structure
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/11/30
 * Corresponds to EquationStruct MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    public class EquationStruct
    {
        private string operatr;
        private string variableName;
        private EquationStruct leftOperand;
        private EquationStruct rightOperand;

        /* CONSTRUCTOR */
        public EquationStruct(string op, string vName, EquationStruct eStruct1, EquationStruct eStruct2)
        {
            operatr = op;
            variableName = vName;

            // Operands are allowed to be null
            leftOperand = eStruct1;
            rightOperand = eStruct2;

            return;
        }

        /* GETTERS */
        public string GetOperator()
        {
            return operatr;
        }

        public string GetVariableName()
        {
            return variableName;
        }

        public EquationStruct GetLeftOperand()
        {
            return leftOperand;
        }

        public EquationStruct GetRightOperand()
        {
            return rightOperand;
        }

        /* SETTERS */
        public void SetLeftOperand(EquationStruct eStruct)
        {
            leftOperand = eStruct;
            return;
        }

        public void SetRightOperand(EquationStruct eStruct)
        {
            rightOperand = eStruct;
            return;
        }
    }
}