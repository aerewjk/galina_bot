using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using static KeyboardHook;
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

            using (var keyboardHook = new KeyboardHook())
            {
                Console.WriteLine("Хук установлен");
                Console.WriteLine("\r\n Галя приветствует вас");
                Console.WriteLine("Для ввода команды зажмите Ctrl + Alt и зажмите - (Минус)");
                Console.WriteLine("После произнесения команды отожмите - (Минус)");
                
                Application.Run();
            }
        }
    }
}
