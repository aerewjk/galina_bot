using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;

namespace galina_bot.Classes.Voice.WhisperModel.core
{
    internal class InitializeModelClass
    {
        public static Task<WhisperProcessor> InitializeModel(string modelFileName) //проверка на наличие модели, настройка модели для работы. Запускать при старте работы
        {
            try
            {
                var _whisperFactory = WhisperFactory.FromPath(modelFileName);
                var _processor = _whisperFactory.CreateBuilder()
                    .WithLanguage("ru")
                    .Build();
                return Task.FromResult(_processor);
            }
            catch (Exception)
            {
                Console.WriteLine("Файл модели поврежден. необходима переустановка");
                File.Delete(modelFileName);
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                return null;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            }
        }
    }
}
