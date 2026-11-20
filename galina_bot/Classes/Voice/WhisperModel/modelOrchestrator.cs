using galina_bot.Classes.Commands.Core;
using galina_bot.Classes.Voice;
using galina_bot.Classes.Voice.WhisperModel;
using galina_bot.Classes.Voice.WhisperModel.core;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
namespace galina_bot.Classes.Voice.WhisperModel
{
    internal class modelOrchestrator
    {
        private CheckModelClass modelCheck;
        private ChangeModelClass modelChange;
        private ModelDownloading modelDownloading;
        private TranscribeAudio audioTranscribe;
        private SoundRecording soundRecorder;
        static string[] models = ["Tiny", "Base", "Small", "Medium", "LargeV3Turbo"]; //Массив с названиями моделей
        static string modelFileName = "whisperVoiceModel.bin"; //Название файла, в котором хранится модель. Используется для скачивания и проверки
        static string? choosed_model; //Переменная, в которой хранится имя выбранной пользователем модели, создается заранее.
        public WhisperProcessor processor;

        public modelOrchestrator()
        {
            modelCheck = new CheckModelClass();
            modelChange = new ChangeModelClass();
            modelDownloading = new ModelDownloading();
            audioTranscribe = new TranscribeAudio();
            soundRecorder = new SoundRecording();
        }
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
                    await ModelInit();
                    Console.WriteLine("Процессор создан");
                }
            }
            else 
            {
                Console.WriteLine("Файл модели был найден");
                await ModelInit();
                Console.WriteLine("Процессор создан");
            }
        }

        private async Task ModelInit()
        {
            processor = await InitializeModelClass.InitializeModel(modelFileName);
            
        }

        public async Task<string> ModelTranscrybe()
        {
            string filePath = soundRecorder.GetTempFilePath();
            Console.WriteLine($"{filePath} {modelFileName}");
            string recognizedText = await audioTranscribe.TranscribeAudioAsync(processor, filePath);
            File.Delete(filePath);
            return recognizedText;
        }

        
    }
    
}
