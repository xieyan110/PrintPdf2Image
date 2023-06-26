using System;
using System.IO;
using System.Drawing;
using PdfiumViewer;
using System.Drawing.Imaging;

namespace PrintPdf2Image
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConvertPDFtoImage("C:\\Users\\TXHZ-C021\\Desktop\\xxxxx.pdf", "C:\\Users\\TXHZ-C021\\Desktop\\");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="imagePath"></param>
        public static void ConvertPDFtoImage(string pdfPath, string imagePath)
        {
            using (var document = PdfDocument.Load(pdfPath))
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    int dpi = 600; // 设置分辨率为 600 DPI
                    float scale = dpi / 72f;
                    int width = (int)(document.PageSizes[i].Width * scale);
                    int height = (int)(document.PageSizes[i].Height * scale);
                    using (var pageImage = document.Render(i, width, height, dpi, dpi, true))
                    {
                        var imageFormat = ImageFormat.Jpeg;
                        var encoder = imageFormat == ImageFormat.Jpeg ? Encoder.Quality : Encoder.Compression;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(encoder, 100L);

                        var fileName = Path.GetFileNameWithoutExtension(pdfPath) + "-" + (i + 1) + "." + imageFormat.ToString().ToLower();
                        var outputPath = Path.Combine(imagePath, fileName);
                        pageImage.Save(outputPath, GetEncoder(imageFormat), encoderParameters);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}

