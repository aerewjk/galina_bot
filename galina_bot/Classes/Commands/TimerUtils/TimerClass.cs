using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace galina_bot.Classes.Commands.TimerUtils
{
    internal class TimerClass
    {
        public static System.Timers.Timer? _activeTimer;
        public static void setTimer(int seconds)
        {
            _activeTimer = new System.Timers.Timer(seconds*1000);
            _activeTimer.Elapsed += (sender, e) =>
            {
                _activeTimer.Stop();
                _activeTimer.Dispose();
                SystemSounds.Beep.Play();

                MessageBox.Show("Таймер истек!", "GalinaBot", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            };
            
            _activeTimer.Start();
        }
    }
}
