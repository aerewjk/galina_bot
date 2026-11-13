using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;


namespace galina_bot
{
    internal class CommandProcessing
    {
        //TODO: Пусть будет три основных листа с задачами: 1. Задача -> исполнение, простые, нетиповые задачи, которые существуют в единственном экземпляре. Например "Привет". Сначала текст будет анализироваться на это, потом - на сложные фразы, которые будут храниться в формате глагол + объект
        private readonly Dictionary<string, Action> _uniqueKeyActions = new() // Список с уникальными нетиповыми задачами
        {
            { "привет", () => SystemSounds.Beep.Play() }
        };
         
        private readonly Dictionary<string, string> _actions = new()
        {
            
        };

        public string NormalizeText(string input)
        {
            if (input != null)
            {
                string result = input.ToLower();
                result = result.Replace(",", "");
                result = result.Replace(".", "");
                result = result.Replace("-", "");
                result = result.Replace("?", "");
                result = result.Replace("!", "");
                return result;
            }


            return "Строка оказалась пустой";
        } //Убирает знаки препинания, ставит все в нижний регистр
    }
}
