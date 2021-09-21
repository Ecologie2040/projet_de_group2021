using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using LazZiya.ImageResize;
namespace PortailIEPSM.Areas.Groupe_2.Models
{
    public class Upload
    {
        private readonly IWebHostEnvironment Env;

        public Upload(IWebHostEnvironment webHostEnvironment)
        {
            Env = webHostEnvironment;
        }

        public string UploadFile(IFormFile image)
        {
            if (image != null)
            {
                List<string> extensions = new List<string> { ".png", ".jpeg", ".jpg", ".gif" };
                string fileextension = Path.GetExtension(image.FileName);
                if (extensions.Contains(fileextension))
                {
                    List<int[]> tailles = new List<int[]> { new int[] { 1000, 450 }, new int[] { 420, 160 } };
                    var id = Guid.NewGuid().ToString();
                    string nom = id + image.FileName;
                    foreach (int[] taille in tailles)
                    {
                        string folder = "groupe2_uploads/" + taille[0] + "-" + id + image.FileName;
                        string serverFolder = Path.Combine(Env.WebRootPath, folder);

                        using (var memoryStream = new MemoryStream())
                        {
                            try
                            {
                                image.CopyTo(memoryStream);
                                using (var img = Image.FromStream(memoryStream))
                                {
                                    img.ScaleAndCrop(taille[0], taille[1]).SaveAs(serverFolder);
                                }
                            }
                            catch (ArgumentException)
                            {
                                return null;
                            }
                            memoryStream.Dispose();
                        }

                    }
                    return nom;
                } else
                {
                    return null;
                }

            }
            return null;
        }

        public string UploadSVG(IFormFile image)
        {
            if (image != null)
            {
                List<string> extensions = new List<string> { ".svg" };
                string fileextension = Path.GetExtension(image.FileName);
                if (extensions.Contains(fileextension))
                {
                        string folder = "groupe2_uploads/" + image.FileName;
                        string serverFolder = Path.Combine(Env.WebRootPath, folder);

                            try
                            {
                        using (FileStream test = File.Create(serverFolder))
                        {
                            image.CopyTo(test);
                        }

                            }
                            catch (ArgumentException)
                            {
                                return null;
                            }

                    return image.FileName;
                }
                else
                {
                    return null;
                }

            }
            return null;
        }
    }
}
