using galina_bot.Classes.Commands.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static galina_bot.Classes.Commands.Dictionaries.CommandsDictionary;
namespace galina_bot.Classes.Commands.Core
{
    internal class CommandProcessor
    {
        public List<Action<string[]>>? CommandProcess(string[] words) //Главный метод. Обнаружает команду и загружает на выполнение
        {
            List<Action<string[]>> foundActions = new List<Action<string[]>>();
            foreach (string word in words) //Проверка каждого слова на наличие в словарях
            {
                if (uniqueKeyActions.TryGetValue(word, out Action<string[]>? meaning))//Проверяет на совпадение словарь с уникальными командами
                {
                    foundActions.Add(meaning);
                }
            }
            return foundActions;
        }
    }
}
