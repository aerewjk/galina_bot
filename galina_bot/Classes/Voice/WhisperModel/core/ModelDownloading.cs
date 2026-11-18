using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net.Ggml;

namespace galina_bot.Classes.Voice.WhisperModel.core
{
    internal class ModelDownloading
    {
        public async Task<bool> ModelDownloadAsync(string modelFileName, string model_name) //Скачивание модели
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
            return true;
        }
    }
}
