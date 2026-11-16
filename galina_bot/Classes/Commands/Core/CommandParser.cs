using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.Core
{
    internal class CommandParser
    {
        public string[] CommandParse(string input)//Метод, убирает знаки препинания, ставит все в нижний регистр
        {
            if (input != null)
            {
                string cleanText = input.ToLower();
                cleanText = cleanText.Replace(",", "");
                cleanText = cleanText.Replace(".", "");
                cleanText = cleanText.Replace("-", "");
                cleanText = cleanText.Replace("?", "");
                cleanText = cleanText.Replace("!", "");
                cleanText = cleanText.Replace("*", "");
                cleanText = cleanText.Replace("[", "");
                cleanText = cleanText.Replace("]", "");

                string[] phrase_words = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return phrase_words;
            }
            return Array.Empty<string>();
        }
    }
}
