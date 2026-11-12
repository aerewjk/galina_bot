using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot
{
    internal class SoundRecording
    {
        private WasapiRecorder _recorder;
        private WaveFileWriter _writer;
        private string _tempFilePath = "temp_voice.wav";

        private void StartRecording()
    {
        // Создаем рекордер с нужным форматом для Whisper (16kHz, моно)
        // Если не указать WithFormat, будет использован формат микшера устройства (обычно 32-bit float)
        _recorder = new WasapiRecorderBuilder()
            .WithFormat(new WaveFormat(16000, 1))
            .Build();

        _writer = new WaveFileWriter(_tempFilePath, _recorder.WaveFormat);

        // Подписываемся на событие. Span валиден только внутри этого колбэка!
        _recorder.DataAvailable += (buffer, flags, devicePosition, qpcPosition) =>
        {
            _writer.Write(buffer);
        };

        _recorder.StartRecording();
        Console.WriteLine("🎤 Начало записи (WasapiRecorder)...");
    }

    private void StopRecordingAndProcess()
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
}
}

