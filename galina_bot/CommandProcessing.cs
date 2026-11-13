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
        public void ProcessCommand(string inputText) //Главный метод. Обнаружает команду и загружает на выполнение
        {
            string[] words = NormalizeText(inputText); // Создание массива, в котором будут храниться слова по отдельности

            foreach (string word in words) //Проверка на слова из списка уникальных команд
            {
                if (_uniqueKeyActions.TryGetValue(word, out Action? meaning))
                {
                    Console.WriteLine($"Обнаружена команда: {word} ");

                    ExecuteCommand(word);
                }
            }
        }
        private string[] NormalizeText(string input)//Метод, убирает знаки препинания, ставит все в нижний регистр
        {
            if (input != null)
            {
                string cleanText = input.ToLower();
                cleanText = cleanText.Replace(",", "");
                cleanText = cleanText.Replace(".", "");
                cleanText = cleanText.Replace("-", "");
                cleanText = cleanText.Replace("?", "");
                cleanText = cleanText.Replace("!", "");
                //cleanText = cleanText.Replace('"', "");
                Console.WriteLine($"Очищенный текст: {cleanText}");
                string[] phrase_words = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return phrase_words;
            }
            return Array.Empty<string>();
        } 
        private void ExecuteCommand(string command_target)//Метод, выполняет команду
        {
            switch (command_target)
            {
                case "привет": { SystemSounds.Beep.Play(); break; }
            }
        } 
        

    }
}
