using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static KeyboardHook;
using galina_bot.utils;
using galina_bot.Classes.Commands;
using galina_bot.Classes.Voice.WhisperModel;
using Whisper.net;
namespace galina_bot
{
    internal class Program
    {
        private static readonly SemaphoreSlim _processingLock = new SemaphoreSlim(1, 1);
        static async Task Main(string[] args)
        {
            bool flag_IsRecording = false;

            //Создание объектов классов для работы
            var soundRecorder = new SoundRecording(); //Объект записи звука
            var commandOrchestrator = new CommandOrchestrator(); //Оркестратор класса работы с Командами
            var modelOrchestrator = new ModelOrchestrator(); //Оркестратор класса работы с Моделью

            await modelOrchestrator.ModelSetup();//Скачивание модели, если ее нет

            using (var keyboardHook = new KeyboardHook()) //Запуск чтения хоткея
            {
                keyboardHook.OnHotKeyPressed += () => //Если хоткей зажат
                {
                    if (flag_IsRecording != true)
                    {
                        soundRecorder.StartRecording(); //Метод запуска записи
                        flag_IsRecording = true;
                        Console.WriteLine($"Мы в StartRecording. Состояние флага {flag_IsRecording}");
                    }
                };
                keyboardHook.OnHotKeyReleased += () => //Если хоткей отжат
                {
                    Task.Run(async () => // Запуск асинхронной задачи
                    {
                        await _processingLock.WaitAsync();

                        try
                        {
                            soundRecorder.StopRecordingAndProcess(); //Метод остановки записи

                            string recognizedText = await modelOrchestrator.ModelTranscrybe(); //Получает распознанную строку

                            Console.WriteLine(recognizedText); //Передача распознанного текста в консоль

                            await commandOrchestrator.CommandOrchestrate(recognizedText); //Исполняет команды, которые найдет в тексте
                        }
                        finally { _processingLock.Release(); flag_IsRecording = false; }

                            Console.WriteLine($"Мы в StopRecording. Состояние флага {flag_IsRecording}");
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