using PhotoSauce.MagicScaler;
using System.IO;
using System.Text;

namespace ITProductECommerce.Helpers
{
    public class Util
    {
        public static string UploadImage(IFormFile image, string folder)
        {
            var imgOptions = new ProcessImageSettings
            {
                Width = 500,
                Height = 350,
                ResizeMode = CropScaleMode.Crop,
                SaveFormat = FileFormat.Jpeg,
                JpegQuality = 100,
                JpegSubsampleMode = ChromaSubsampleMode.Subsample420,
            };

            var categoryImgOptions = new ProcessImageSettings
            {
                Width = 600,
                Height = 400,
                ResizeMode = CropScaleMode.Crop,
                SaveFormat = FileFormat.Jpeg,
                JpegQuality = 100,
                JpegSubsampleMode = ChromaSubsampleMode.Subsample420,
            };

            if (folder.Equals("Categories"))
            {
                try
                {
                    var type = image.FileName.Substring(image.FileName.LastIndexOf("."));
                    var fileName = $"img_{DateTime.Now.ToString("dd-MM-yy-HH-mm-ss")}{type}";

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img",
                            folder, fileName);
                    using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                    {
                        MagicImageProcessor.ProcessImage(image.OpenReadStream(), myfile, categoryImgOptions);
                    }
                    return fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return string.Empty;
                }
            }
            else
            {
                try
                {
                    var type = image.FileName.Substring(image.FileName.LastIndexOf("."));
                    var fileName = $"img_{DateTime.Now.ToString("dd-MM-yy-HH-mm-ss")}{type}";

                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img",
                            folder, fileName);
                    using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                    {
                        MagicImageProcessor.ProcessImage(image.OpenReadStream(), myfile, imgOptions);
                    }
                    return fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return string.Empty;
                }
            }
        }

        public static void RemoveImage (IFormFile image, string folder)
        {
            try
            {
                var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img",
                        folder, image.FileName);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Sinh chuỗi Random Key
        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"asdjghsy!@#nfhasdjsuauAD#$%^TGYYHaknjdnsdaAASFGGASWWED&^%$VBNM";
            var sb = new StringBuilder();
            var rd = new Random();

            for(int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
    }
}
