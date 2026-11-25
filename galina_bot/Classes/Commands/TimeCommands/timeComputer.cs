using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;
namespace galina_bot.Classes.Commands.TimeCommands
{
    internal class timeComputer
    {
        public static int timeCompute(string timeExpression)
        {
            try
            {
                var expression = new Expression(timeExpression);
                object result = expression.Evaluate();
                int intResult = Convert.ToInt32(result);
                return intResult;
            } catch (Exception) { Console.WriteLine("Возникла непредвиденная ошибка"); return 0; }
        }
    }
}
