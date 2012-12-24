using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;

namespace TagThis.Controllers
{
    public class GetThumbController : Controller
    {
        //
        // GET: /GetThumb/

        public void Index(string u, int width)
        {
            u = u.Replace("hqdefault.jpg", "0.jpg");
            u = u.Replace("default.jpg", "0.jpg");

            Response.Clear();

            if (string.IsNullOrEmpty(u) == false)
            {
                try
                {

                    string filePath = u;

                    WebClient l_WebClient = new WebClient();
                    byte[] l_imageBytes = l_WebClient.DownloadData(filePath);
                    MemoryStream l_stream = new MemoryStream(l_imageBytes);
                    Image image = Image.FromStream(l_stream);


                    string contentType = "image/png";
                    string extension = Path.GetExtension(filePath).ToLower();
                    switch (extension)
                    {
                        case ".gif":
                            contentType = "image/gif";
                            break;
                        case ".jpg":
                            contentType = "image/jpeg";
                            break;
                        case ".png":
                            contentType = "image/png";
                            break;
                    }

                    Response.ContentType = contentType;


                    ImageResizer.ResizeSettings rs = new ImageResizer.ResizeSettings();
                    rs.Width = width;

                    //rs.MaxHeight = 500;


                    //ImageResizer.ImageBuilder.Current.Build(image, "~/a.png", rs);  stores it locally

                    MemoryStream ms = new MemoryStream();
                    ImageResizer.ImageBuilder.Current.Build(image, ms, rs);


                    //write to amazon s3



                    //write to response
                    ms.WriteTo(Response.OutputStream);

                    Response.Flush();
                }
                catch { };
            }
            
        }

    }
}
