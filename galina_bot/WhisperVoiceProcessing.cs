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
        public async Task InitializeModel() //метод, который будет вызываться при старте программы. Он скачивает модель, если ее еще нет, и подготавливает к работе
        {
            if (!File.Exists(modelFileName)) //Если файл с моделью не обнаружен
            {
                Console.WriteLine("Файл модели не был обнаружен. Скачивание модели Base, подождите, пожалуйста!");
                Task<string> task = ModelDownloadAsync();
                string result_download = await task;
                Console.WriteLine(result_download);

            }
            else { Console.WriteLine("Файл модели был обнаружен -> Скачивание не требуется"); }
        }

        static async Task<string> ModelDownloadAsync()
        {
            using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(GgmlType.Base); //Скачивает файл модели
            using var fileWriter = File.OpenWrite(modelFileName); //Называет его согласно указанному названию
            await modelStream.CopyToAsync(fileWriter);
            return "Скачивание модели завершено";
        }

    }
}
