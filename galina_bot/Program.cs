using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using static galina_bot.galina_hotkey;
using static galina_bot.WhisperVoiceProcessing;
namespace galina_bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Косметика для консоли
            Console.WriteLine("Галя на связи!");
            Console.WriteLine("Для того, чтобы запустить запись команды для Гали зажмите и произнесите команду с зажатым сочетанием клавиш crtl + alt + - (минус) ");

            var voiceProcessing = new WhisperVoiceProcessing();
            await voiceProcessing.InitializeModel();

            using (var hotKeyWindow = new HotKeyWindow()) //Проверка условий и запуск проверки хоткея
            {
                if (!hotKeyWindow.Register()) //В случае, если хоткей будет занят системно - выйдет ошибка
                {
                    Console.WriteLine("> Невозможно зарегистрировать горячую клавишу");
                    return;
                }

                while (true) //Вечный цикл опроса 
                {
                    Application.DoEvents();
                    Thread.Sleep(500);
                }
            }
        }
    }
}
