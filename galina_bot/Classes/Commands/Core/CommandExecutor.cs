using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.Core
{
    internal class CommandExecutor
    {
        public void CommandExecute(Action? meaning)//Метод, выполняет команду
        {
            if (meaning == null)
            {
                Console.WriteLine("Команды не обнаружено");
            }
            else 
            {
                meaning();
            }
        }
    }
}
