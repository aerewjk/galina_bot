using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.Classes.Voice.WhisperModel.core
{

    internal class CheckModelClass
    {
        public bool CheckModel(string modelFileName)
        {
            if (!File.Exists(modelFileName)) //Если файл с моделью не обнаружен
            {
                return false;
            }
            return true;
        }
    }
}
