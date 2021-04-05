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
    public class QuantizationFilter
    {
        public static int DivisorR;
        public static int DivisorG;
        public static int DivisorB;

        public static List<List<int>> RegionsR = new List<List<int>>();
        public static List<List<int>> RegionsG = new List<List<int>>();
        public static List<List<int>> RegionsB = new List<List<int>>();

        public static void CalculateRegions(List<List<int>> RegionsList, int Divisor)
        {
            int StepSize = 255 / Divisor;
            for (int i = 0; i < Divisor; i++)
            {
                List<int> tmp = new List<int>();
                tmp.Add(i * (StepSize + 1));
                tmp.Add(((i + 1) * (StepSize)) + i);
                RegionsList.Add(tmp);
            }
        }

        public static int FindCorrespondColorValue(List<List<int>> RegionsList, int ColorValue)
        {
            int AverageColor = ColorValue;
            foreach (var Region in RegionsList)
            {
                if (Region.First() < ColorValue && ColorValue < Region.Last())
                {
                    AverageColor = (Region.First() + Region.Last()) / 2;
                    break;
                }
            }
            return AverageColor;
        }

        public static Image UniformQuantization(Image image)
        {
            RegionsR.Clear();
            RegionsG.Clear();
            RegionsB.Clear();
            CalculateRegions(RegionsR, DivisorR);
            CalculateRegions(RegionsG, DivisorG);
            CalculateRegions(RegionsB, DivisorB);
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

            for (int y = 0; y < sourceBitmap.Height; y++)
            {
                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    resultBuffer[(y * sourceData.Stride + x * 4)] = (byte)(FindCorrespondColorValue(RegionsB, pixelBuffer[(y * sourceData.Stride + x * 4)]));
                    resultBuffer[(y * sourceData.Stride + x * 4) + 1] = (byte)(FindCorrespondColorValue(RegionsG, pixelBuffer[(y * sourceData.Stride + x * 4) + 1]));
                    resultBuffer[(y * sourceData.Stride + x * 4) + 2] = (byte)(FindCorrespondColorValue(RegionsR, pixelBuffer[(y * sourceData.Stride + x * 4) + 2]));
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
