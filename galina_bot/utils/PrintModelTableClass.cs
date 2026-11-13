using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.utils
{
    public class PrintModelTableClass
    {
        public static void PrintModelTable()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Задаем ширину для каждой колонки
            const int wModel = 18;    // Модель
            const int wParams = 12;   // Параметры
            const int wWeight = 15;   // Вес файла (стало намного компактнее)
            const int wRam = 20;      // RAM/VRAM
            const int wSpeed = 24;    // Скорость

            string v = new('-', wModel + wParams + wWeight + wRam + wSpeed + 12);
            string separator = v;

            // Вывод заголовка
            Console.WriteLine(separator);
            Console.WriteLine($"{"Модель",-wModel} | {"Параметры",-wParams} | {"Вес файла",-wWeight} | {"RAM / VRAM",-wRam} | {"Скорость",-wSpeed}");
            Console.WriteLine(separator);

            // Вывод данных (оставлен только вес GGML-файлов)
            Console.WriteLine($"{"tiny",-wModel} | {"39 млн",-wParams} | {"~75 МБ",-wWeight} | {"~270–300 МБ",-wRam} | {"~10x (максимальная)",-wSpeed}");
            Console.WriteLine($"{"base (recommend)",-wModel} | {"74 млн",-wParams} | {"~140 МБ",-wWeight} | {"~390–500 МБ",-wRam} | {"~7x",-wSpeed}");
            Console.WriteLine($"{"small (recommend+)",-wModel} | {"244 млн",-wParams} | {"~480 МБ",-wWeight} | {"~850 МБ – 1 ГБ",-wRam} | {"~4x",-wSpeed}");
            Console.WriteLine($"{"medium",-wModel} | {"769 млн",-wParams} | {"~1,5 ГБ",-wWeight} | {"~2,1–2,5 ГБ",-wRam} | {"~2x",-wSpeed}");
            Console.WriteLine($"{"large-v3-turbo",-wModel} | {"809 млн",-wParams} | {"~570 МБ",-wWeight} | {"~1,7–2,3 ГБ",-wRam} | {"~8x",-wSpeed}");
           

            Console.WriteLine(separator);
        }
    }
}
