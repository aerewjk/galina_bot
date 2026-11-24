using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using galina_bot.Classes.Commands.Dictionaries;
using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.Dictionaries
{
    public class EasyCommandsDictionary
    {
        public static readonly Dictionary<string, Action<string[]>> uniqueKeyActions = new() // Список с уникальными нетиповыми задачами
        {
            { "привет", (args) => SystemSounds.Beep.Play() },
            { "галя", (args) => SystemSounds.Hand.Play() },
            {"найди", CommandActions.searchInBrowser}
        };
    }
}
