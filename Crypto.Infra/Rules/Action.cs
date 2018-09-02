using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infra.Rules
{
    public enum OperatorType { GreaterThan, LessThan, Multiply}

    public class Action : IAction
    {
        public OperatorType Operator { get; set; }


        public bool Calculate(decimal value, decimal refValue, decimal threshold=0)
        {

            switch (Operator)
            {
                case OperatorType.GreaterThan:
                    if (value > (refValue + (refValue * threshold)))
                        return true;
                    else
                        return false;

                case OperatorType.LessThan:
                    if (value < (refValue - (refValue * threshold)))
                        return true;
                    else
                        return false;

                case OperatorType.Multiply:
                    if (value > (refValue * threshold))
                        return true;
                    else
                        return false;

                default:
                    return false;

            }


            return false;
        }
    }
}
