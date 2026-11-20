using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whisper.net;
namespace galina_bot.Classes.Voice.WhisperModel.core
{
    internal class TranscribeAudio
    {
        public async Task<string> TranscribeAudioAsync(WhisperProcessor _processor, string filePath) //Функция обработки звука
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
