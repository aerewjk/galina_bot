using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.TimeCommands
{
    internal class timeParserClass
    {
        public static string timeParse(string TimePhrase)
        {
            TimePhrase = TimePhrase.Replace("секунду", "1 +");
            TimePhrase = TimePhrase.Replace("минуту", "60 +");
            TimePhrase = TimePhrase.Replace("час", "3600 +");

            TimePhrase = Regex.Replace(TimePhrase,@"секунд\w*", "* 1 +");
            TimePhrase = Regex.Replace(TimePhrase, @"минут\w*", "* 60 +");
            TimePhrase = Regex.Replace(TimePhrase, @"час\w*", "* 3600 +");
            TimePhrase = TimePhrase.Replace("и", "");
            TimePhrase = TimePhrase.Replace(" ", "");
            TimePhrase = TimePhrase.TrimEnd('+');
            return TimePhrase;
        }
    }
}
