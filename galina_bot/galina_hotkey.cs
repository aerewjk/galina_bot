using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
public class KeyboardHook : IDisposable
{
    public event Action? OnHotKeyPressed;
    public event Action? OnHotKeyReleased;

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private bool _isRecording = false; 
    // Делегат для коллбэка
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // Импорт функций WinAPI
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    private IntPtr _hookId = IntPtr.Zero;
    private LowLevelKeyboardProc _proc;

    public KeyboardHook()
    {
        _proc = HookCallback;
        // Устанавливаем хук при создании объекта
        _hookId = SetHook(_proc);
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        // Получаем дескриптор текущего модуля для хука
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule? curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            // Проверяем клавишу "-" (VK_OEM_MINUS = 0xBD)
            if (vkCode == 0xBD)
            {
                bool isCtrlDown = (GetAsyncKeyState(0x11) & 0x8000) != 0;
                bool isAltDown = (GetAsyncKeyState(0x12) & 0x8000) != 0;

                if (isCtrlDown && isAltDown)
                {
                    if (wParam == (IntPtr)WM_KEYDOWN && _isRecording == false)
                    {
                        Console.WriteLine("Начинаю запись");
                        _isRecording = true;
                        OnHotKeyPressed.Invoke();
                    }
                    else if (wParam == (IntPtr)WM_KEYUP)
                    {
                        Console.WriteLine("Завершаю запись");
                        _isRecording = false;
                        OnHotKeyReleased.Invoke();
                    }
                }
            }
        }
        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    // Реализация IDisposable для снятия хука
    public void Dispose()
    {
        if (_hookId != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }
    }

}