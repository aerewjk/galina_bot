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
    internal class ModelOrchestrator
    {
        //Заранее подготавливаем поля для создания объектов
        private CheckModelClass _modelCheck;
        private ChangeModelClass _modelChange;
        private ModelDownloading _modelDownloading;
        private TranscribeAudio _audioTranscribe;
        private SoundRecording _soundRecorder;
        private WhisperProcessor? _processor = null; //Объявляем поле для processor, который необходим для транскрибирования звука

        //Константы для работы модуля
        static string[] models = ["Tiny", "Base", "Small", "Medium", "LargeV3Turbo"]; //Массив с названиями доступных моделей
        static string modelFileName = "whisperVoiceModel.bin"; //Название файла, в котором хранится модель. Используется для скачивания и проверки
        static string? choosed_model; //Переменная, в которой хранится имя выбранной пользователем модели, создается заранее.
        
        public ModelOrchestrator() //Конструктор. Здесь создаются объекты классов
        {
            _modelCheck = new CheckModelClass();
            _modelChange = new ChangeModelClass();
            _modelDownloading = new ModelDownloading();
            _audioTranscribe = new TranscribeAudio();
            _soundRecorder = new SoundRecording();
        }
        public async Task ModelSetup() //Этот метод подготавливает модель к работе. Для начала он проверяет, существует ли уже файл модели, или еще нет. Если нет, то скачивает, а после - запускает Инициализацию и настройку, которая выплевывает в поля processor
        {
            bool isModelExists = _modelCheck.CheckModel(modelFileName); // Запуск метода, ищущий файл модели (Возвращает true или false в зависимости от того, найдет ее или нет)
            if (isModelExists == false)
            {
                choosed_model = _modelChange.ChangeModel(models); //Пользователь выбирает модель. Она возвращается в качестве строки с названием
                Task<bool> task = _modelDownloading.ModelDownloadAsync(modelFileName, choosed_model); //Активируем скачивание, передавая в метод пользовательский выбор и название, которым хотим обозвать файл модели
                bool result_downloading = await task;
                if (result_downloading)
                {
                    Console.WriteLine("Скачивание прошло успешно!");
                    await ModelInit();
                }
            }
            else
            {
                Console.WriteLine("Файл модели был найден");
                await ModelInit();
            }
        }
        private async Task ModelInit() //Создает и выплевывает в поле объект _processor
        {
            _processor = await InitializeModelClass.InitializeModel(modelFileName);
        }

        public async Task<string> ModelTranscrybe() //Транскрибирует переданный голосовой файл в формате .wav
        {
            if (_processor != null)
            {
                string filePath = _soundRecorder.GetTempFilePath(); //получение пути файла
                string recognizedText = await _audioTranscribe.TranscribeAudioAsync(_processor, filePath); //передача в метод расшифровки
                File.Delete(filePath); //Удаление файла после расшифровки
                return recognizedText;
            }
            else { return "Ошибка: Модель не была инициализирована"; }
        }
    }
}
