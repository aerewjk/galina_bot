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
        static async Task Main(string[] args)
        {
            //Создание объектов классов для работы
            var soundRecorder = new SoundRecording(); //Объект записи звука
            var commandOrchestrator = new CommandOrchestrator(); //Оркестратор класса работы с Командами
            var modelOrchestrator = new ModelOrchestrator(); //Оркестратор класса работы с Моделью

            await modelOrchestrator.ModelSetup();//Скачивание модели, если ее нет

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

                        string recognizedText = await modelOrchestrator.ModelTranscrybe(); //Получает распознанную строку

                        Console.WriteLine(recognizedText); //Передача распознанного текста в консоль

                        bool lastCommandStatus = commandOrchestrator.CommandOrchestrate(recognizedText); //Сохраняет статус команды (Найдена/не найдена)

                        if (lastCommandStatus) { Console.WriteLine("Команда обнаружена"); } else { Console.WriteLine("Команда не обнаружена"); } //Выводит результат поиска
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