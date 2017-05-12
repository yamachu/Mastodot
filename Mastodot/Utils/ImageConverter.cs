using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Mastodot.Utils
{
    public class ImageConverter
    {
        /// <summary>
        /// Base64Encoded image from byte array.
        /// </summary>
        /// <returns>The encoded image string from byte array.</returns>
        /// <param name="image">byte formated Image.</param>
        public static string Base64EncodedImageFromByteArray(byte[] image)
        {
            var mime = GetMime(image);

            // ToDo: Handling unsupported format
            if (mime == "")
            {

            }

            return DataURIFormat(mime, Convert.ToBase64String(image));
        }
#if !NETSTANDARD1_1
        /// <summary>
        /// Base64Encoded image from file.
        /// </summary>
        /// <returns>The encoded image string from file.</returns>
        /// <param name="filePath">File path.</param>
        public static async Task<string> Base64EncodedImageFromFile(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var image = new byte[fs.Length];
                await fs.ReadAsync(image, 0, (int)fs.Length).ConfigureAwait(false);

                var mime = GetMime(image);
                if (mime == "")
                {
                    mime = $"image/{filePath.Split('.').Last()}";
                }

                return DataURIFormat(mime, Convert.ToBase64String(image));
            }
        }
#endif
        private static string GetMime(byte[] image)
        {
            if (image.Take(3).ToArray().Equals(new byte[] { 0xFF, 0xD8, 0xFF }))
            {
                return "image/jpeg";
            }
            if (image.Take(8).ToArray().Equals(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
            {
                return "image/png";
            }
            if (image.Take(4).ToArray().Equals(new byte[] { 0x47, 0x49, 0x46 }))
            {
                return "image/gif";
            }
            if (image.Take(2).ToArray().Equals(new byte[] { 0x4D, 0x42 }))
            {
                return "image/bmp";
            }

            return "";
        }

        private static string DataURIFormat(string mime, string base64string)
        => $"data:{mime};base64,{base64string}";
    }
}
