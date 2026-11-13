using galina_bot.utils;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;

namespace galina_bot.Classes.Voice
{
    internal partial class WhisperVoiceProcessing
    {
        static string modelFileName = "whisperVoiceModel.bin"; //Название файла, в котором хранится модель
        static string? choosed_model;
        static string[] models = ["Tiny", "Base", "Small", "Medium", "LargeV3Turbo"];

        private WhisperFactory? _whisperFactory;
        private WhisperProcessor? _processor;
        public async Task InitializeModel() //проверка на наличие модели, настройка модели для работы. Запускать при старте работы
        {
            if (!File.Exists(modelFileName)) //Если файл с моделью не обнаружен
            {
                choosed_model = ChangeModel();
                Task<string> task = ModelDownloadAsync(choosed_model); //Установка модели
                string result_download = await task; //Запуск и ожидание
                Console.WriteLine($"\n{result_download}"); //После завершения - вывод в консоль

            }
            else { Console.WriteLine("Файл модели был обнаружен -> Скачивание не требуется"); }
            try
            {
                _whisperFactory = WhisperFactory.FromPath(modelFileName);
                _processor = _whisperFactory.CreateBuilder()
                    .WithLanguage("ru")
                    .Build();
            }
            catch (Whisper.net.WhisperModelLoadException) 
            { 
                Console.WriteLine("Файл модели поврежден. необходима переустановка");
                File.Delete(modelFileName);
                await InitializeModel();
            }
        }
        static async Task<string> ModelDownloadAsync(string model_name) //Скачивание модели
        {
            GgmlType _model_name = Enum.Parse<GgmlType>(model_name, ignoreCase: true);
            using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(_model_name); //Скачивает файл модели
            using var fileWriter = File.Create(modelFileName); //Создает/перезаписывает файл согласно указанному названию

            // Пытаемся узнать общий размер (может быть null, если сервер не сообщил Content-Length)
            long? totalBytes = modelStream.CanSeek ? modelStream.Length : null;

            var buffer = new byte[81920]; // 80 КБ — стандартный размер буфера .NET
            long totalRead = 0;
            int bytesRead;

            while ((bytesRead = await modelStream.ReadAsync(buffer)) > 0)
            {
                await fileWriter.WriteAsync(buffer, 0, bytesRead);
                totalRead += bytesRead;

                double mbRead = totalRead / 1024.0 / 1024.0;
                if (totalBytes.HasValue && totalBytes.Value > 0)
                {
                    double mbTotal = totalBytes.Value / 1024.0 / 1024.0;
                    double percent = (double)totalRead / totalBytes.Value * 100;
                    Console.Write($"\rСкачано: {mbRead:0.00} / {mbTotal:0.00} MB ({percent:0.0}%)   ");
                }
                else
                {
                    Console.Write($"\rСкачано: {mbRead:0.00} MB   ");
                }
            }

            Console.WriteLine(); //Перевод строки после завершения
            return "Скачивание модели завершено";
        }
        public async Task<string> TranscribeAudioAsync(string filePath) //Функция обработки звука
        {
            using var fileStream = File.OpenRead(filePath);
            string fullText = "";
            if (_processor != null)
            {
                await foreach (var result in _processor.ProcessAsync(fileStream))
                {
                    fullText += result.Text + " ";
                }
                return fullText.Trim();
            }
            return "";
        }
        static string ChangeModel() // Функция выбора модели для скачивания
        {
            Console.WriteLine("Файл модели не был обнаружен. Необходимо скачивание! \n Введите номер модели, которую хотите скачать и нажмите Enter. \n 1. Tiny \n 2. Base (recommended) \n 3. Small (recommended+) \n 4. medium \n 5. large-v3-turbo \n");
            PrintModelTableClass.PrintModelTable(); // Фунция из утилиты, рисует красивую таблицу с характеристиками моделей

            try
            {
                Console.Write("> ");
                int numOfChoosed_model = Convert.ToInt16(Console.ReadLine()); // Пользователь вводит число
                Console.WriteLine($"Была выбрана модель: {models[numOfChoosed_model - 1]}");
                return models[numOfChoosed_model - 1]; //Метод возвращает строку с названием выбранной модели (ради обращения к массиву вычитали 1)
            }
            catch (Exception) //Выброс исключения для некорректного ввода
            {
                Console.WriteLine("Введите корректную цифру \n");
                return ChangeModel(); //Метод повторно вызывает сам себя
            }
            
        }

    }
}
