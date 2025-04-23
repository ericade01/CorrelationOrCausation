using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace CorrelationOrCausation
{
    class Api
    {
        #region dlls
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);
        #endregion

        #region hotkeys
        public bool[] altHotkeys = new bool[10];

        public void StartListeningForAllAltHotkeys()
        {
            for (int i = 0; i <= 9; i++)
            {
                int index = i; // local copy for thread
                Thread t = new Thread(() =>
                {
                    Keys key = Keys.D0 + index;
                    while (true)
                    {
                        try
                        {
                            if ((GetAsyncKeyState(Keys.Menu) & 0x8000) != 0 &&
                                (GetAsyncKeyState(key) & 0x8000) != 0)
                            {
                                altHotkeys[index] = !altHotkeys[index];
                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception) { }
                        Thread.Sleep(50);
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
        }
        #endregion

        #region clipboard
        public void SendClipboardTextToForegroundWindow()
        {
            try
            {
                string clipboardText = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    SendKeys.SendWait(clipboardText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clipboard access error: " + ex.Message);
            }
        }

        public string GetClipboardText()
        {
            try
            {
                return Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clipboard read error: " + ex.Message);
                return string.Empty;
            }
        }

        public void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clipboard write error: " + ex.Message);
            }
        }
        #endregion

        #region copypasta
        public void SendCtrlA()
        {
            SendKeys.SendWait("^a");
        }

        public void SendCtrlC()
        {
            SendKeys.SendWait("^c");
        }

        public void SendCtrlV()
        {
            SendKeys.SendWait("^v");
        }
        #endregion

        #region mousestuff
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        public void MouseLeftDown() => mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
        public void MouseLeftUp() => mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        public void MouseRightDown() => mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
        public void MouseRightUp() => mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        public void MouseMiddleDown() => mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
        public void MouseMiddleUp() => mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);

        public void ScrollUp(int ticks, int delayMs = 10)
        {
            for (int i = 0; i < ticks; i++)
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 120, UIntPtr.Zero);
                Thread.Sleep(delayMs);
            }
        }

        public void ScrollDown(int ticks, int delayMs = 10)
        {
            for (int i = 0; i < ticks; i++)
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, unchecked((uint)-120), UIntPtr.Zero);
                Thread.Sleep(delayMs);
            }
        }

        public Point GetCursorPosition()
        {
            GetCursorPos(out Point pt);
            return pt;
        }

        public void MoveCursorTo(Point target, int speed)
        {
            Point start = GetCursorPosition();
            int steps = 50;

            for (int i = 1; i <= steps; i++)
            {
                double t = (double)i / steps;
                double curvedT = (1 - Math.Cos(t * Math.PI)) / 2; // cosine interpolation for smoothness

                int x = (int)(start.X + (target.X - start.X) * curvedT);
                int y = (int)(start.Y + (target.Y - start.Y) * curvedT);
                SetCursorPos(x, y);
                Thread.Sleep(speed);
            }
        }


        public void MouseClick()
        {
            MouseLeftDown();
            Thread.Sleep(50);
            MouseLeftUp();
        }

        #endregion

        #region bitmapops
        public Bitmap CaptureAllScreens()
        {
            Rectangle bounds = Rectangle.Empty;
            foreach (var screen in Screen.AllScreens)
            {
                bounds = Rectangle.Union(bounds, screen.Bounds);
            }

            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }
            return bmp;
        }

        public Bitmap CaptureRegion(Point topLeft, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(topLeft, Point.Empty, new Size(width, height));
            }
            return bmp;
        }

        public Bitmap ConvertToGrayscale(Bitmap original)
        {
            Bitmap gray = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(gray))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] {0.3f, 0.3f, 0.3f, 0, 0},
                        new float[] {0.59f, 0.59f, 0.59f, 0, 0},
                        new float[] {0.11f, 0.11f, 0.11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return gray;
        }

        public Bitmap RemoveNoise(Bitmap original)
        {
            Bitmap result = new Bitmap(original.Width, original.Height);
            for (int y = 1; y < original.Height - 1; y++)
            {
                for (int x = 1; x < original.Width - 1; x++)
                {
                    Color c = original.GetPixel(x, y);
                    int avgR = 0, avgG = 0, avgB = 0;
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            Color nc = original.GetPixel(x + i, y + j);
                            avgR += nc.R;
                            avgG += nc.G;
                            avgB += nc.B;
                        }
                    }
                    avgR /= 9; avgG /= 9; avgB /= 9;
                    result.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }
            return result;
        }

        public unsafe Point FindBitmap(Bitmap haystack, Bitmap needle, float tolerance)
        {
            using (Bitmap hBmp = haystack.Clone(new Rectangle(0, 0, haystack.Width, haystack.Height), PixelFormat.Format24bppRgb))
            using (Bitmap nBmp = needle.Clone(new Rectangle(0, 0, needle.Width, needle.Height), PixelFormat.Format24bppRgb))
            {
                int w1 = hBmp.Width;
                int h1 = hBmp.Height;
                int w2 = nBmp.Width;
                int h2 = nBmp.Height;

                BitmapData haystackData = hBmp.LockBits(new Rectangle(0, 0, w1, h1), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                BitmapData needleData = nBmp.LockBits(new Rectangle(0, 0, w2, h2), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                int stride1 = haystackData.Stride;
                int stride2 = needleData.Stride;

                byte* scan0_1 = (byte*)haystackData.Scan0;
                byte* scan0_2 = (byte*)needleData.Scan0;

                Rectangle virtualBounds = SystemInformation.VirtualScreen;

                for (int y = 0; y <= h1 - h2; y++)
                {
                    for (int x = 0; x <= w1 - w2; x++)
                    {
                        bool match = true;

                        for (int j = 0; j < h2 && match; j++)
                        {
                            byte* ptr1 = scan0_1 + (y + j) * stride1 + x * 3;
                            byte* ptr2 = scan0_2 + j * stride2;

                            for (int i = 0; i < w2 * 3; i++)
                            {
                                float diff = Math.Abs(ptr1[i] - ptr2[i]) / 255f;
                                if (diff > tolerance)
                                {
                                    match = false;
                                    break;
                                }
                            }
                        }

                        if (match)
                        {
                            hBmp.UnlockBits(haystackData);
                            nBmp.UnlockBits(needleData);
                            return new Point(x + virtualBounds.X, y + virtualBounds.Y);
                        }
                    }
                }

                hBmp.UnlockBits(haystackData);
                nBmp.UnlockBits(needleData);
                return Point.Empty;
            }
        }
        #endregion


        #region keyboardstubb
        public void SendTabKey()
        {
            SendKeys.SendWait("{TAB}");  // Sends a Tab key press
        }

        public void SendEnterKey()
        {
            SendKeys.SendWait("{ENTER}");
        }

        #endregion

    }
}
