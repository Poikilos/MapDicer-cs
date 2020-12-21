using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapDicer
{
    class ByteMap
    {
        private byte[] pixels = null;
        private int bytesPP;
        private int stride;
        private int width;
        private int height;
        private int totalBytes;
        public ByteMap(int width, int height, int bytesPerPixel, bool fill, Color color)
        {
            this.width = width;
            this.height = height;
            this.bytesPP = bytesPerPixel;
            this.stride = width * bytesPerPixel;
            this.totalBytes = this.stride * this.height;
            this.pixels = new byte[this.totalBytes];
            if (fill)
            {
                Fill(color);
            }
        }
        public void SetPixel(int x, int y, Color color)
        {
            int lineIndex = y * stride;
            int pxIndex = lineIndex + x * bytesPP;
            this.pixels[pxIndex] = color.B;
            this.pixels[pxIndex + 1] = color.G;
            this.pixels[pxIndex + 2] = color.B;
            this.pixels[pxIndex + 3] = color.A;
        }
        public void Fill(Color color)
        {
            int lineIndex = 0;
            int pxIndex;
            byte b = color.B;
            byte g = color.G;
            byte r = color.R;
            byte a = color.A;

            for (int y = 0; y < height; y++)
            {
                pxIndex = lineIndex;
                for (int x = 0; x < width; x++)
                {
                    pixels[pxIndex] = b;
                    pixels[pxIndex + 1] = g;
                    pixels[pxIndex + 2] = r;
                    pixels[pxIndex + 3] = a;
                    pxIndex += bytesPP;
                }
                lineIndex += stride;
            }
        }
        public void WriteTo(WriteableBitmap wb)
        {
            Int32Rect dstRect = new Int32Rect(0, 0, width, height);
            wb.WritePixels(dstRect, pixels, stride, 0);
        }
        /// <summary>
        /// Write all pixels to the destination.
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="dstRect"></param>
        public void WriteTo(WriteableBitmap wb, Int32Rect dstRect)
        {
            wb.WritePixels(dstRect, pixels, stride, 0);
        }
        public void WritePixelTo(WriteableBitmap wb, int x, int y)
        {
            Int32Rect dstRect = new Int32Rect(x, y, 1, 1);
            int lineIndex = y * stride;
            int pxIndex = lineIndex + x * bytesPP;
            wb.WritePixels(dstRect, pixels, stride, pxIndex);
        }
        public static void WritePixelTo(WriteableBitmap wb, int x, int y, Color color)
        {
            // See
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.writepixels?view=net-5.0
            byte[] pixels = { color.B, color.G, color.R, color.A };
            Int32Rect rect = new Int32Rect(x, y, 1, 1);
            int pxIndex = 0;
            int stride = 4;
            wb.WritePixels(rect, pixels, stride, pxIndex);
        }

        internal static void SaveWriteableBitmap(string path, WriteableBitmap wb)
        {
            // ^ Use a different name to prevent infinite recursion
            // Clone converts a WriteableBitmap if the target is an implemented type.
            ByteMap.Save(path, wb.Clone());
        }
        /// <summary>
        /// Save the bitmapsource as a png file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source">Any bitmap source such as from a WriteableBitmap's overloaded Clone method.</param>
        /// <returns></returns>
        internal static string Save(string path, BitmapSource source)
        {
            if (Path.GetExtension(path).ToLower() != SettingController.MapblockImageDotExt)
            {
                throw new ApplicationException(String.Format("The file {0} doesn't have the extension {1}.",
                                                             path, SettingController.MapblockImageDotExt));
            }
            string error = "";
            if (path != string.Empty)
            {
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(source));
                        encoder.Save(stream);
                    }
                }
                catch (System.IO.DirectoryNotFoundException ex)
                {
                    error = String.Format("The directory was not ensured to exist: {0}", path);
                }
            }
            return error;
        }
    }
}
