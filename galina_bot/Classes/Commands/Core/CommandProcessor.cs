using galina_bot.Classes.Commands.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static galina_bot.Classes.Commands.Dictionaries.EasyCommandsDictionary;
namespace galina_bot.Classes.Commands.Core
{
    internal class CommandProcessor
    {
        public Action? CommandProcess(string[] words) //Главный метод. Обнаружает команду и загружает на выполнение
        {
            foreach (string word in words) //Проверка каждого слова на наличие в словарях
            {
                if (uniqueKeyActions.TryGetValue(word, out Action? meaning))//Проверяет на совпадение словарь с уникальными командами
                {
                    return meaning;
                }
            }
            return null;
        }
    }
}
