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
        const string modelFileName = "ggm1-base.bin"; //Название файла, в котором хранится модель

        private WhisperFactory? _whisperFactory;
        private WhisperProcessor? _processor;
        public async Task InitializeModel() //метод, который будет вызываться при старте программы. Он скачивает модель, если ее еще нет
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
            using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(GgmlType.Base); //Скачивает файл модели
            using var fileWriter = File.OpenWrite(modelFileName); //Называет его согласно указанному названию
            await modelStream.CopyToAsync(fileWriter);
            return "Скачивание модели завершено";
        }

        public async Task<string> TranscribeAudioAsync(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            string fullText = "";

            await foreach (var result in _processor.ProcessAsync(fileStream))
            {
                fullText += result.Text + " ";
            }
            return fullText.Trim();
        }
    }
}
