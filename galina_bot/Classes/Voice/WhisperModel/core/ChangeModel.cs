using galina_bot.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace galina_bot.Classes.Voice.WhisperModel.core
{

    internal class ChangeModelClass
    {

        public string ChangeModel(string[] models) // Функция выбора модели для скачивания
        {
            Console.WriteLine("Файл модели не был обнаружен. Необходимо скачивание! \n Введите номер модели, которую хотите скачать и нажмите Enter. \n 1. Tiny \n 2. Base (recommended) \n 3. Small (recommended+) \n 4. medium \n 5. large-v3-turbo \n");
            PrintModelTableClass.PrintModelTable(); // Фунция из утилиты, рисует красивую таблицу с характеристиками моделей

            try
            {
                Console.Write("> ");
                int numOfChoosed_model = Convert.ToInt16(Console.ReadLine()); // Пользователь вводит число
                Console.WriteLine($"Была выбрана модель: {models[numOfChoosed_model - 1]}");
                return models[numOfChoosed_model - 1]; //Метод возвращает строку с названием выбранной модели (ради обращения к массиву вычитали 1)
            }
            catch (Exception) //Выброс исключения для некорректного ввода
            {
                Console.WriteLine("Введите корректную цифру \n");
                return ChangeModel(models); //Метод повторно вызывает сам себя
            }

        }
    }
}
