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
            var soundRecorder = new SoundRecording(); //Сервис записи звука
            
            //Скачивание модели, если ее нет
            var voiceProcessing = new WhisperVoiceProcessing();
            await voiceProcessing.InitializeModel();

            
            using (var keyboardHook = new KeyboardHook()) //Запуск хука
            {
                keyboardHook.OnHotKeyPressed += () =>
                {
                    soundRecorder.StartRecording();
                };

                keyboardHook.OnHotKeyReleased += () =>
                {
                    Task.Run(async () =>
                    {
                        soundRecorder.StopRecordingAndProcess();

                        string filePath = soundRecorder.GetTempFilePath();

                        string recognizedText = await voiceProcessing.TranscribeAudioAsync(filePath);

                        Console.WriteLine(recognizedText);
                    });
                };



                Console.WriteLine("Хук установлен");
                Console.WriteLine("\r\n Галя приветствует вас");
                Console.WriteLine("Для ввода команды зажмите Ctrl + Alt и зажмите - (Минус)");
                Console.WriteLine("После произнесения команды отожмите - (минус)");
                
                Application.Run(); //Поддержание программы
            }
        }
    }
}
