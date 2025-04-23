using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace CorrelationOrCausation
{
    class ScreenCap
    {
        public static Bitmap CaptureAllScreens()
        {
            Rectangle totalBounds = GetTotalScreenBounds();
            Bitmap screenshot = new Bitmap(totalBounds.Width, totalBounds.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.X - totalBounds.X, screen.Bounds.Y - totalBounds.Y, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                }
            }

            return screenshot;
        }

        private static Rectangle GetTotalScreenBounds()
        {
            int left = int.MaxValue, top = int.MaxValue, right = int.MinValue, bottom = int.MinValue;

            foreach (Screen screen in Screen.AllScreens)
            {
                left = Math.Min(left, screen.Bounds.Left);
                top = Math.Min(top, screen.Bounds.Top);
                right = Math.Max(right, screen.Bounds.Right);
                bottom = Math.Max(bottom, screen.Bounds.Bottom);
            }

            return new Rectangle(left, top, right - left, bottom - top);
        }

        public static Bitmap LoadBitmapFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new Bitmap(filePath);
            }
            throw new FileNotFoundException("Image file not found:", filePath);
        }

        public static unsafe Point? FindBitmapWithinBitmap(Bitmap source, Bitmap target, float tolerance)
        {
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData targetData = target.LockBits(new Rectangle(0, 0, target.Width, target.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int sourceStride = sourceData.Stride;
            int targetStride = targetData.Stride;
            int targetWidth = target.Width;
            int targetHeight = target.Height;

            byte* srcPtr = (byte*)sourceData.Scan0;
            byte* tgtPtr = (byte*)targetData.Scan0;

            for (int y = 0; y <= source.Height - targetHeight; y++)
            {
                for (int x = 0; x <= source.Width - targetWidth; x++)
                {
                    bool match = true;

                    for (int ty = 0; ty < targetHeight; ty++)
                    {
                        byte* srcRow = srcPtr + ((y + ty) * sourceStride) + (x * 3);
                        byte* tgtRow = tgtPtr + (ty * targetStride);

                        for (int tx = 0; tx < targetWidth; tx++)
                        {
                            int bDiff = Math.Abs(srcRow[0] - tgtRow[0]);
                            int gDiff = Math.Abs(srcRow[1] - tgtRow[1]);
                            int rDiff = Math.Abs(srcRow[2] - tgtRow[2]);

                            if (bDiff > tolerance || gDiff > tolerance || rDiff > tolerance)
                            {
                                match = false;
                                break;
                            }

                            srcRow += 3; // Move to next pixel in row
                            tgtRow += 3;
                        }
                        if (!match) break;
                    }

                    if (match)
                    {
                        source.UnlockBits(sourceData);
                        target.UnlockBits(targetData);
                        return new Point(x, y);
                    }
                }
            }

            source.UnlockBits(sourceData);
            target.UnlockBits(targetData);
            return null;
        }
    }
}
