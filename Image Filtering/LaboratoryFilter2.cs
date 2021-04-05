using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Image_Filtering
{
    public class LaboratoryFilter2
    {
        public static Image ConvertToYCbCr(Image image)
        {
            Bitmap sourceBitmap = new Bitmap(image);
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0,
                                                                        0,
                                                                        sourceBitmap.Width,
                                                                        sourceBitmap.Height),
                                                                                           ImageLockMode.ReadOnly,
                                                                                           PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            for (int i = 0; i < pixelBuffer.Length; i += 4)
            {
                double Y = ((double)pixelBuffer[i] * 0.114) + ((double)pixelBuffer[i + 1] * 0.587) + ((double)pixelBuffer[i + 2] * 0.299);
                double Cb = 128.0 + ((double)pixelBuffer[i] * 0.5) - ((double)pixelBuffer[i + 1] * 0.331264) - ((double)pixelBuffer[i + 2] * 0.168736);
                double Cr = 128.0 - ((double)pixelBuffer[i] * 0.081312) - ((double)pixelBuffer[i + 1] * 0.418688) + ((double)pixelBuffer[i + 2] * 0.5);
                resultBuffer[i] = (byte)((int)(Cr));
                resultBuffer[i + 1] = (byte)((int)(Cb));
                resultBuffer[i + 2] = (byte)((int)(Y));
                resultBuffer[i + 3] = pixelBuffer[i + 3];
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0,
                                                                        0,
                                                                        resultBitmap.Width,
                                                                        resultBitmap.Height),
                                                                                           ImageLockMode.WriteOnly,
                                                                                           PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }

        public static Image ConvertToRGB(Image image)
        {
            Bitmap sourceBitmap = new Bitmap(image);
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0,
                                                                        0,
                                                                        sourceBitmap.Width,
                                                                        sourceBitmap.Height),
                                                                                           ImageLockMode.ReadOnly,
                                                                                           PixelFormat.Format32bppArgb);
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            for (int i = 0; i < pixelBuffer.Length; i += 4)
            {
                int R = (int)(((double)pixelBuffer[i + 2]) + (1.402 * (((double)pixelBuffer[i + 2]) - 128.0)));
                if (R > 255)
                {
                    R = 255;
                }
                if (R < 0)
                {
                    R = 0;
                }
                int G = (int)((double)pixelBuffer[i + 2] - 0.344136 * ((double)(pixelBuffer[i + 1] - 128)) - 0.714136 * ((double)(pixelBuffer[i] - 128)));
                if (G > 255)
                {
                    G = 255;
                }
                if (G < 0)
                {
                    G = 0;
                }
                int B = (int)((double)pixelBuffer[i + 2] + 1.772 * ((double)(pixelBuffer[i + 1] - 128)));
                if (B > 255)
                {
                    B = 255;
                }
                if (B < 0)
                {
                    B = 0;
                }
                resultBuffer[i] = (byte)(B);
                resultBuffer[i + 1] = (byte)(G);
                resultBuffer[i + 2] = (byte)(R);
                resultBuffer[i + 3] = pixelBuffer[i + 3];
            }
            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0,
                                                                        0,
                                                                        resultBitmap.Width,
                                                                        resultBitmap.Height),
                                                                                           ImageLockMode.WriteOnly,
                                                                                           PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}
