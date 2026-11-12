using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Policy;

namespace galina_bot
{
    internal static class galina_hotkey
    {
        public class HotKeyWindow : NativeWindow, IDisposable
        {
            private const int WM_HOTKEY = 0x0312;
            private const int HOTKEY_ID = 9000;

            //Импорт функций WinAPI
            [DllImport("user32.dll")]
            private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

            [DllImport("user32.dll")]
            private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

            //Модификаторы (Хоткей ctrl + alt + -)
            private const uint VK_OEM_MINUS = 0xBD; //Код клавиши -
            private const uint MOD_CONTROL = 0x0002; //Код клавиши Ctrl
            private const uint MOD_ALT = 0x0001; //Код клавиши Alt
            private const uint MOD_NOREPEAT = 0x4000; //Модификатор, чтобы при зажатой комбинации не повторялось миллион раз

            public HotKeyWindow() //Здесь создается невидимое окно, которое будет слушать хоткей
            {
                Console.WriteLine("Создано окно");
                CreateHandle(new CreateParams()); //Задание параметров (никакие - т.е. оно невидимо)
            }

            public bool Register() //Регистратору передаются коды клавиш и модификатор NOREPEAT
            {
                Console.WriteLine("Хоткей зарегистрирован");
                return RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_ALT | MOD_NOREPEAT, VK_OEM_MINUS);
            }

            protected override void WndProc(ref Message m) //Процесс, замечающий хоткей
            {
                if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID) //Это условие срабатывает при хоткее
                {
                    Console.WriteLine("Хоткей нажат");
                }

                base.WndProc(ref m);
            }

            public void Dispose() //Сборщик мусора после закрытия бота
            {
                UnregisterHotKey(this.Handle, HOTKEY_ID);
                DestroyHandle();
            }
        }
    } 
}
