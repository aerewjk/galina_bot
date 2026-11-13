using galina_bot.Classes.Commands;
using galina_bot.Classes.Voice;
using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static galina_bot.Classes.Commands.CommandProcessing;
using static galina_bot.Classes.Voice.WhisperVoiceProcessing;
using static KeyboardHook;
namespace galina_bot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var soundRecorder = new SoundRecording(); //Объект записи звука
            var voiceProcessing = new WhisperVoiceProcessing(); //Объект обработки звука
            var commandProcess = new CommandProcessing();
            await voiceProcessing.InitializeModel();//Скачивание модели, если ее нет

            using (var keyboardHook = new KeyboardHook()) //Запуск чтения хоткея
            {
                keyboardHook.OnHotKeyPressed += () => //Если хоткей зажат
                {
                    soundRecorder.StartRecording(); //Метод запуска записи
                };
                keyboardHook.OnHotKeyReleased += () => //Если хоткей отжат
                {
                    Task.Run(async () => // Запуск асинхронной задачи
                    {
                         soundRecorder.StopRecordingAndProcess(); //Метод остановки записи

                        string filePath = soundRecorder.GetTempFilePath(); // Передача пути звукового файла в метод

                        string recognizedText = await voiceProcessing.TranscribeAudioAsync(filePath); //Передача файла в метод и получение распознавания

                        Console.WriteLine(recognizedText); //Передача распознанного текста в консоль
                        commandProcess.ProcessCommand(recognizedText);
                    });
                };
                //Открытая инструкция для пользователя
                Console.WriteLine("\r\n Галя приветствует вас");
                Console.WriteLine("Для ввода команды зажмите Ctrl + Alt и зажмите - (Минус)");
                Console.WriteLine("После произнесения команды отожмите - (минус)");
                
                Application.Run(); //Поддержание программы в запущенном состоянии
            }//Общий процесс программы. Здесь работает все
        }
    }
}