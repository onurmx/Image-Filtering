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
    public class ConvolutionFilters
    {
        public static Bitmap ConvolutionFilter(Bitmap sourceBitmap, double[,] FilterMatrix, double Factor)
        {
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
            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;
            int filterWidth = FilterMatrix.GetLength(1);
            int filterHeight = FilterMatrix.GetLength(0);
            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;
            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            blue += (double)(pixelBuffer[calcOffset]) * FilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            green += (double)(pixelBuffer[calcOffset + 1]) * FilterMatrix[filterY + filterOffset, filterX + filterOffset];
                            red += (double)(pixelBuffer[calcOffset + 2]) * FilterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }
                    blue = Factor * blue;
                    green = Factor * green;
                    red = Factor * red;
                    if (blue > 255)
                    {
                        blue = 255;
                    }
                    else if (blue < 0)
                    {
                        blue = 0;
                    }


                    if (green > 255)
                    {
                        green = 255;
                    }
                    else if (green < 0)
                    {
                        green = 0;
                    }


                    if (red > 255)
                    {
                        red = 255;
                    }
                    else if (red < 0)
                    {
                        red = 0;
                    }
                    resultBuffer[byteOffset]     = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
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

        public static Image Blur(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            double Factor = 1.0 / 9.0;
            double[,] Kernel = new double[,] { { 1.0, 1.0, 1.0},
                                               { 1.0, 1.0, 1.0},
                                               { 1.0, 1.0, 1.0}};
            bmp = ConvolutionFilter(bmp, Kernel, Factor);
            return bmp;
        }

        public static Image GaussianBlur(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            double Factor = 1.0 / 8.0;
            double[,] Kernel = new double[,] { { 0.0, 1.0, 0.0},
                                               { 1.0, 4.0, 1.0},
                                               { 0.0, 1.0, 0.0}};
            bmp = ConvolutionFilter(bmp, Kernel, Factor);
            return bmp;
        }

        public static Image Sharpen(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            double Factor = 1.0;
            double[,] Kernel = new double[,] { {  0.0, -1.0,  0.0},
                                               { -1.0,  5.0, -1.0},
                                               {  0.0, -1.0,  0.0}};
            bmp = ConvolutionFilter(bmp, Kernel, Factor);
            return bmp;
        }

        public static Image EdgeDetection(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            double Factor = 1.0;
            double[,] Kernel = new double[,] { { -1.0, -1.0, -1.0},
                                               { -1.0,  8.0, -1.0},
                                               { -1.0, -1.0, -1.0}};
            bmp = ConvolutionFilter(bmp, Kernel, Factor);
            return bmp;
        }

        public static Image Emboss(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            double Factor = 1.0;
            double[,] Kernel = new double[,] { { -1.0, -1.0, 0.0},
                                               { -1.0,  1.0, 1.0},
                                               {  0.0,  1.0, 1.0}};
            bmp = ConvolutionFilter(bmp, Kernel, Factor);
            return bmp;
        }
    }
}
