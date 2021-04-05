using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Image_Filtering
{
    public class OrderedDitheringFilter
    {
        public static int MatrixType;
        public static int GrayLevel;

        public static int[,] DitherMatrix2 = { { 1, 3 },
                                               { 4, 2 } };

        public static int[,] DitherMatrix3 = { { 3, 7, 4 },
                                               { 6, 1, 9 },
                                               { 2, 8, 5 } };

        public static int[,] DitherMatrix4 = { {  1,  3,  9, 11 },
                                               {  4,  2, 12, 10 },
                                               { 13, 15,  5,  7 },
                                               { 16, 14,  8,  6 } };

        public static int[,] DitherMatrix6 = { {  9, 11, 25, 27, 13, 15 },
                                               { 12, 10, 28, 26, 16, 14 },
                                               { 21, 23,  1,  3, 33, 35 },
                                               { 24, 22,  4,  2, 36, 34 },
                                               {  5,  7, 29, 31, 17, 19 },
                                               {  8,  6, 32, 30, 20, 18 } };

        public static Image GrayScale(Image image)
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
                int grayScale = (int)((pixelBuffer[i + 2] * 0.2989) + (pixelBuffer[i + 1] * 0.5870) + (pixelBuffer[i] * 0.1140));
                resultBuffer[i] = (byte)(grayScale);
                resultBuffer[i + 1] = (byte)(grayScale);
                resultBuffer[i + 2] = (byte)(grayScale);
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

        public static Image OrderedDithering(Image image)
        {
            int[,] DitherMatrix = OrderedDitheringFilter.DitherMatrix2;
            switch (OrderedDitheringFilter.MatrixType)
            {
                case 2:
                    DitherMatrix = OrderedDitheringFilter.DitherMatrix2;
                    break;
                case 3:
                    DitherMatrix = OrderedDitheringFilter.DitherMatrix3;
                    break;
                case 4:
                    DitherMatrix = OrderedDitheringFilter.DitherMatrix4;
                    break;
                case 6:
                    DitherMatrix = OrderedDitheringFilter.DitherMatrix6;
                    break;
                default:
                    DitherMatrix = OrderedDitheringFilter.DitherMatrix2;
                    break;
            }


            List<int> GrayLevels = new List<int>();
            for (int i = 0; i <= (GrayLevel - 1); i++)
            {
                GrayLevels.Add(i * 255 / (GrayLevel - 1));
            }

            Bitmap sourceBitmap = new Bitmap(GrayScale(image));
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

            for (int y = 0; y < sourceBitmap.Height; y++)
            {
                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    double DitherMatrixValue = DitherMatrix[x % MatrixType, y % MatrixType];
                    double Threshold = DitherMatrixValue / ((MatrixType * MatrixType) + 1d);
                    double Intensity = pixelBuffer[(y * sourceData.Stride + x * 4) + 2] / 255.0; // Red/255
                    double col = Math.Floor((GrayLevel - 1) * Intensity);
                    double Reminder = (GrayLevel - 1) * Intensity - col;
                    if (Reminder >= Threshold)
                    {
                        ++col;
                    }
                    resultBuffer[(y * sourceData.Stride + x * 4)] = (byte)((int)GrayLevels[(int)col]);
                    resultBuffer[(y * sourceData.Stride + x * 4) + 1] = (byte)((int)GrayLevels[(int)col]);
                    resultBuffer[(y * sourceData.Stride + x * 4) + 2] = (byte)((int)GrayLevels[(int)col]);
                    resultBuffer[(y * sourceData.Stride + x * 4) + 3] = pixelBuffer[(y * sourceData.Stride + x * 4) + 3];
                }
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
