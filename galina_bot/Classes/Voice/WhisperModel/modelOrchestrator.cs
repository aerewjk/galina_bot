using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
using galina_bot.Classes.Voice.WhisperModel;
using galina_bot.Classes.Voice.WhisperModel.core;
using galina_bot.Classes.Voice;
namespace galina_bot.Classes.Voice.WhisperModel
{
    internal class modelOrchestrator
    {
        private CheckModelClass modelCheck;
        private ChangeModelClass modelChange;
        private ModelDownloading modelDownloading;
        private TranscribeAudio audioTranscribe;
        private SoundRecording soundRecorder;

        private WhisperFactory? _whisperFactory;
        private WhisperProcessor? _processor;

        static string[] models = ["Tiny", "Base", "Small", "Medium", "LargeV3Turbo"]; //Массив с названиями моделей
        static string modelFileName = "whisperVoiceModel.bin"; //Название файла, в котором хранится модель. Используется для скачивания и проверки
        static string? choosed_model; //Переменная, в которой хранится имя выбранной пользователем модели, создается заранее.


        public async Task ModelSetup()
        {
             bool isModelExists = modelCheck.CheckModel(modelFileName); // Запуск метода, ищущий файл модели (Возвращает true или false в зависимости от того, найдет ее или нет)
            if (isModelExists == false)
            {
                choosed_model = modelChange.ChangeModel(models); //Пользователь выбирает модель. Она возвращается в качестве строки с названием
                Task<bool> task = modelDownloading.ModelDownloadAsync(modelFileName, choosed_model);
                bool result_downloading = await task;
                if (result_downloading) 
                {
                    Console.WriteLine("Скачивание прошло успешно!");
                    ModelInit();
                }
            }
            else 
            {
                Console.WriteLine("Файл модели был найден");
                await ModelInit();
            }
        }

        private async Task<WhisperProcessor> ModelInit()
        {
            WhisperProcessor _processor = await InitializeModelClass.InitializeModel(modelFileName);
            return _processor;
        }

        public async Task<string> ModelTranscrybe()
        {
            string filePath = soundRecorder.GetTempFilePath();
            string recognizedText = await audioTranscribe.TranscribeAudioAsync(_processor, filePath);
            File.Delete(filePath);
            return recognizedText;
        }

        
    }
    
}
