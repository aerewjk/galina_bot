using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;

using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.Dictionaries
{
    public class EasyCommandsDictionary
    {
        public static readonly Dictionary<string, Action> uniqueKeyActions = new() // Список с уникальными нетиповыми задачами
        {
            { "привет", () => SystemSounds.Beep.Play() },
            { "галя", () => SystemSounds.Hand.Play() }
        };
    }
}
