using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot
{
    internal class CommandProcessing
    {
        private readonly List<string> _keyActions = new()
        { 
            "привет"
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
        }
    }
}
