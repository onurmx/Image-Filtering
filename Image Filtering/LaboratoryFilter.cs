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
    public class LaboratoryFilter
    {
        public static int findMid(Bitmap image, int j, int i)
        {
            List<int> arr = new List<int>();
            arr.Add(image.GetPixel(j + 1, i).R);
            arr.Add(image.GetPixel(j + 1, i - 1).R);
            arr.Add(image.GetPixel(j, i - 1).R);
            arr.Add(image.GetPixel(j - 1, i - 1).R);
            arr.Add(image.GetPixel(j - 1, i).R);
            arr.Add(image.GetPixel(j - 1, i + 1).R);
            arr.Add(image.GetPixel(j, i + 1).R);
            arr.Add(image.GetPixel(j + 1, i + 1).R);
            arr.Add(image.GetPixel(j, i).R);
            arr = arr.OrderBy(p => p).ToList();
            return arr[4];
        }

        public static Image MedianFilter(Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            Bitmap buffer = new Bitmap(image.Width, image.Height);
            Color color;
            for(int i=0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if((i==0)||(i==bitmap.Height - 1) || (j == 0) || (j == bitmap.Width - 1))
                    {
                        continue;
                    }
                    else
                    {
                        int median = LaboratoryFilter.findMid(bitmap, j, i);
                        color = Color.FromArgb(median, median, median);
                        buffer.SetPixel(j, i, color);
                    }
                }
            }
            return buffer;
        }
    }
}
