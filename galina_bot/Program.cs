using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using static galina_bot.galina_hotkey;
namespace galina_bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Галя на связи!");
            Console.WriteLine("Для того, чтобы запустить запись команды для Гали зажмите и произнесите команду с зажатым сочетанием клавиш crtl + alt + - (минус) ");

            using (var hotKeyWindow = new HotKeyWindow())
            {
                if (!hotKeyWindow.Register())
                {
                    Console.WriteLine("> Невозможно зарегистрировать горячую клавишу");
                    return;
                }

                while (true)
                {
                    Application.DoEvents();
                    Thread.Sleep(500);
                }
            }
        }
    }
}
