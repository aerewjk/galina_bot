using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.Classes.Commands.Dictionaries
{
    internal class CommandActions
    {
        public static void searchInBrowser(string[] words)
        {
            int index = Array.IndexOf(words, "найди");
            var queryWords = words.Skip(index + 1);
            string searchQuery = string.Join(" ", queryWords);

            string url = "https://www.google.com/search?q=" + searchQuery;
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Вот эта строчка делает всю магию
            });

        }

    }
}
