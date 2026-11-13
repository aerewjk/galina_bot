using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;
namespace galina_bot
{
    internal partial class WhisperVoiceProcessing
    {
        const string modelFileName = "whisperVoiceModel.bin"; //Название файла, в котором хранится модель

        private WhisperFactory? _whisperFactory;
        private WhisperProcessor? _processor;
        public async Task InitializeModel() //проверка на наличие модели, настройка модели для работы. Запускать при старте работы
        {
            if (!File.Exists(modelFileName)) //Если файл с моделью не обнаружен
            {
                Console.WriteLine("Файл модели не был обнаружен. Скачивание модели Base, подождите, пожалуйста!");

                Task<string> task = ModelDownloadAsync(); //Назначение таски
                string result_download = await task; //Запуск и ожидание
                Console.WriteLine(result_download); //После завершения - вывод в консоль

            }
            else { Console.WriteLine("Файл модели был обнаружен -> Скачивание не требуется"); }

            _whisperFactory = WhisperFactory.FromPath(modelFileName);
            _processor = _whisperFactory.CreateBuilder()
                .WithLanguage("ru")
                .Build();
        }
        static async Task<string> ModelDownloadAsync() //Скачивание модели
        {
            using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(GgmlType.Small); //Скачивает файл модели
            using var fileWriter = File.Create(modelFileName); //Создает/перезаписывает файл согласно указанному названию

            // Пытаемся узнать общий размер (может быть null, если сервер не сообщил Content-Length)
            long? totalBytes = modelStream.CanSeek ? modelStream.Length : null;

            var buffer = new byte[81920]; // 80 КБ — стандартный размер буфера .NET
            long totalRead = 0;
            int bytesRead;

            while ((bytesRead = await modelStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
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
    }
}
