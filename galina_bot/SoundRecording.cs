using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
    public class SoundRecording
    {
        private WasapiRecorder? _recorder;
        private WaveFileWriter? _writer;
        private string _tempFilePath = "temp_voice.wav";

        public void StartRecording()
    {
        //Создание рекордера с указанием нужного формата записи
        _recorder = new WasapiRecorderBuilder()
            .WithFormat(new WaveFormat(16000, 1))
            .Build();

        _writer = new WaveFileWriter(_tempFilePath, _recorder.WaveFormat);

        //Подписка на событие
        _recorder.DataAvailable += (buffer, flags, devicePosition, qpcPosition) =>
        {
            _writer.Write(buffer);
        };

        _recorder.StartRecording();
    }

    public void StopRecordingAndProcess()
    {
        if (_recorder != null)
        {
            _recorder.StopRecording();
            _recorder.Dispose();
            _recorder = null;
        }

        if (_writer != null)
        {
            _writer.Dispose();
            _writer = null;
        }

        Console.WriteLine("Запись завершена, отправка в Whisper.");
    }

    public string GetTempFilePath() => _tempFilePath;
}


