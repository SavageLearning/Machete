using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace Machete.Domain
{
    public class Image : Record
    {
        //public int ID { get; set; }
        public byte[] ImageData { get; set; }
        [StringLength(30)]
        public string ImageMimeType { get; set; }
        [StringLength(255)]
        public string filename { get; set; }
        public byte[] Thumbnail { get; set; }
        [StringLength(30)]
        public string ThumbnailMimeType { get; set; }
        [StringLength(30)]
        public string parenttable { get; set; }
        [StringLength(20)]
        public string recordkey { get; set; }

        //public void make_thumbnail()
        //{
        //    Thumbnail = ResizeImage(ImageData, 100, 100);
        //}
        //private Bitmap ResizeImage(Stream streamImage, int maxWidth, int maxHeight)
        //{
        //    Bitmap originalImage = new Bitmap(streamImage);
        //    int newWidth = originalImage.Width;
        //    int newHeight = originalImage.Height;
        //    double aspectRatio = (double)originalImage.Width / (double)originalImage.Height;

        //    if (aspectRatio <= 1 && originalImage.Width > maxWidth)
        //    {
        //        newWidth = maxWidth;
        //        newHeight = (int)Math.Round(newWidth / aspectRatio);
        //    }
        //    else if (aspectRatio > 1 && originalImage.Height > maxHeight)
        //    {
        //        newHeight = maxHeight;
        //        newWidth = (int)Math.Round(newHeight * aspectRatio);
        //    }

        //    Bitmap newImage = new Bitmap(originalImage, newWidth, newHeight);

        //    Graphics g = Graphics.FromImage(newImage);
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
        //    g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

        //    originalImage.Dispose();

        //    return newImage;
        //}
    }
}
