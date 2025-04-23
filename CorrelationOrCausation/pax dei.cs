using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CorrelationOrCausation
{
    class pax_dei
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        public bool start;

        public void kk()
        {
            Stopwatch resetty = new Stopwatch();
            bool reset = false;
            bool rightd = false;

            while (true)
            {
                if (!start)
                {
                    reset = false;
                    Thread.Sleep(1000);
                    resetty.Reset();
                    rightd = false;
                    continue;
                }
                if (!reset)
                {
                    Thread.Sleep(5000);
                    reset = true;
                }
                if (!rightd)
                {
                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
                    resetty.Restart();
                    rightd = true;
                }


                if (rightd)
                {
                    if (resetty.ElapsedMilliseconds > 50000)
                    {

                        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
                        Thread.Sleep(10000);
                        rightd = false;
                        continue;
                    }

                }



            }


        }
    }
}
