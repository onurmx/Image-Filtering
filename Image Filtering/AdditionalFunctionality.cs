using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Image_Filtering
{
    public class AdditionalFunctionality
    {
        public static List<Point> Points = new List<Point>() { new Point(0, 255), new Point(255, 0) };
        public static List<int> FilterPoints = new List<int>();
        public static void CalculateFilterPoints()
        {
            for(int i = 0; i < 256; i++)
            {
                FilterPoints.Add(0);
            }
            for (int i = 0; i < Points.Count - 1; i++)
            {
                double slope = (double)(Points[i + 1].Y - Points[i].Y) / (double)(Points[i + 1].X - Points[i].X);
                FilterPoints[Points[i].X] = -(Points[i].Y - 255);
                FilterPoints[Points[i + 1].X] = -(Points[i + 1].Y - 255);
                for (int j = 1; j < Points[i + 1].X - Points[i].X; j++)
                {
                    FilterPoints[Points[i].X + j] = (byte)(255 - Math.Round(Points[i].Y + j * slope));
                }
            }
        }

        public static Image AdditionalFilter(Image image)
        {
            Bitmap pic = new Bitmap(image);
            for (int y = 0; (y <= (pic.Height - 1)); y++)
            {
                for (int x = 0; (x <= (pic.Width - 1)); x++)
                {
                    Color inv = pic.GetPixel(x, y);
                    inv = Color.FromArgb(255, (FilterPoints[inv.R]), (FilterPoints[inv.G]), (FilterPoints[inv.B]));
                    pic.SetPixel(x, y, inv);
                }
            }
            return pic;
        }
    }
}
