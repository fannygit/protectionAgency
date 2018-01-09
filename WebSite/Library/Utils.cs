using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EPA.Project.WebSite.Library
{

    public static class Utils
    {

        public static string[] SplitToArrary(this string value, string splitString)
        {
            return value.Split(new string[] { splitString }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BannerImgFile"></param>
        /// <param name="imgMaxWidth"></param>
        /// <param name="imgMaxHeight"></param>
        /// <param name="errorMsg"></param>
        /// <param name="ImgName"></param>
        /// <returns></returns>
        public static bool SaveAndCheckImg(HttpPostedFileBase BannerImgFile, int imgMaxWidth, int  imgMaxHeight, out string errorMsg, out string ImgName)
        {
            ImgName = string.Empty;
            errorMsg = string.Empty;

            //判斷是否有選擇上傳檔案
            if (BannerImgFile == null)
            {
                errorMsg = "並未選擇上傳檔案";
                return false;
            }
            //判斷檔案大小
            if (BannerImgFile.ContentLength <= 0)
            {
                errorMsg = "檔案大小不可為0";
                return false;
            }
            //判斷檔案名稱
            // 取得副檔名
            string fileType = BannerImgFile.FileName.Split('.').Last().ToUpper();
            if (!(fileType.Equals("JPG") || fileType.Equals("JPEG") || fileType.Equals("PNG")))
            {
                errorMsg = "只接受圖片檔";
                return false;
            }
            // 取得上傳檔案
            Image img = Image.FromStream(BannerImgFile.InputStream);
            // 判斷圖片大小是否符合
            if (!(img.Height <= imgMaxHeight && img.Width <= imgMaxWidth))
            {
                errorMsg = string.Format("圖片尺寸需介於{0}*{1}", imgMaxHeight, imgMaxWidth);
                return false;
            }
            else
            {
                //存在某個資料夾
                //BannerImg=檔名
            }

            return true;
                
        }

        /// <summary>
        /// 隨機產生 Hash256 編碼
        /// </summary>
        /// <param name="oString"></param>
        /// <returns></returns>
        public static string ToHash256Random(this string oString)
        {
            HMACSHA256 hash256 = new HMACSHA256();

            byte[] b = System.Text.Encoding.Default.GetBytes(oString);

            byte[] h = hash256.ComputeHash(b);
            string e = string.Empty;

            foreach (var r in h)
            {
                e += r.ToString("x2");
            }
            return e;
        }

        public static string ToHash256(this string oString)
        {
            return ToHash256(oString, null);
        }
        /// <summary>
        /// 產生 Hash256 編碼
        /// </summary>
        /// <param name="oString"></param>
        /// <returns></returns>
        public static string ToHash256(this string oString, string PrivateKey)
        {
            //string _key = "EPA@1688";

            if (string.IsNullOrEmpty(PrivateKey))
            {
                PrivateKey = "EPA@1688";
            }

            byte[] _keyByte = System.Text.Encoding.Default.GetBytes(PrivateKey);

            HMACSHA256 hash256 = new HMACSHA256(_keyByte);

            byte[] b = System.Text.Encoding.Default.GetBytes(oString);

            byte[] h = hash256.ComputeHash(b);
            string e = string.Empty;

            foreach (var r in h)
            {
                e += r.ToString("x2");
            }
            return e;
        }


        /// <summary>
        /// 將字串轉成 Base 64
        /// </summary>
        /// <param name="oString"></param>
        /// <returns></returns>
        public static string ToBase64(this string oString)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(oString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解碼 Base 64 成一般文字
        /// </summary>
        /// <param name="oString"></param>
        /// <returns></returns>
        public static string FromBase64(this string oString)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(oString));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetAbsoluteFilePath(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    return System.IO.Path.Combine(GetBaseUrl(), "App/File/Get?fileName=" + fileName);
                }
                catch
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        /// <summary>
        /// 藉由副檔名, 取得 Content Type 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetContentType(string fileName)
        {

            string contentType = "application/octetstream";

            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)

                contentType = registryKey.GetValue("Content Type").ToString();

            return contentType;

        }

        public static List<SelectListItem> ConvertEnumToSelectListItem<T>()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (string strEnum in Enum.GetNames(typeof(T)).ToList<string>())
            {
                items.Add(new SelectListItem()
                {
                    Text = strEnum,
                    Value = strEnum
                });
            }

            return items;
        }

        private static char[] postfix = { '+', '-', '_' };
        private static object _object = new object();
        //取得 Object Key
        public static string GetObjectId()
        {
            TimeSpan timespan = TimeSpan.FromTicks(DateTime.Now.Ticks);
            int processId = new Random(Guid.NewGuid().GetHashCode()).Next();

            //ProcessModelInfo.GetCurrentProcessInfo().ProcessID;
            lock (_object)
            {
                int inc = 0;
                if (HttpContext.Current.Cache["Inc"] != null)
                {
                    inc = Convert.ToInt32(HttpContext.Current.Cache["Inc"]);
                    inc += 1;
                    HttpContext.Current.Cache["Inc"] = inc;
                }
                else
                {
                    HttpContext.Current.Cache["Inc"] = 1;
                }

                //String.Format("{0:X}", timespan.Ticks);
                string str_result = string.Format("{0}{1}{2}",
                        String.Format("{0:x}", timespan.Seconds),
                        String.Format("{0:x}", processId),
                        String.Format("{0:x}", inc)
                    );
                return str_result;
            }

        }

        public static void CreateDirectory(string dir)
        {
            try
            {
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("建立資料夾錯誤:" + dir, ex);
            }
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <typeparam name="T">要更新的 Model</typeparam>
        /// <param name="Data">Model 的實體</param>
        /// <param name="FolderPath">資料夾路徑</param>
        /// <param name="UpdateProperty">要更新的欄位</param>
        /// <param name="SavedFile">Http Post 過來的類別</param>
        public static void SaveFile<T>(T Data, string FolderPath, string UpdateProperty, HttpPostedFileBase SavedFile)
        {
            if (SavedFile != null)
            {
                string DirectoryPath = FolderPath;
                string FileName = Library.Utils.GetObjectId() + System.IO.Path.GetExtension(SavedFile.FileName).ToLower();
                Library.Utils.CreateDirectory(DirectoryPath);
                SavedFile.SaveAs(System.IO.Path.Combine(DirectoryPath, FileName));

                System.Reflection.PropertyInfo p = typeof(T).GetProperty(UpdateProperty);
                if (p != null)
                {
                    p.SetValue(Data, FileName, null);
                }
            }
        }

        /// <summary>
        /// 產生APP縮圖
        /// </summary>
        /// <param name="sourcefile"></param>
        /// <param name="destinationfile"></param>
        /// <param name="width"></param>
        public static void GenerateAppThumbNail(string sourcefile, string destinationfile, int width)
        {
            System.Drawing.Image image =
               System.Drawing.Image.FromFile(sourcefile);
            int srcWidth = image.Width;
            int srcHeight = image.Height;
            int thumbWidth = width;
            int thumbHeight;
            Bitmap bmp;

            thumbHeight = (int)((float)thumbWidth / (float)srcWidth * (float)srcHeight);
            bmp = new Bitmap(thumbWidth, thumbHeight);

            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            System.Drawing.Rectangle rectDestination =
                   new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
            bmp.Save(destinationfile);
            bmp.Dispose();
            image.Dispose();
        }



        //http://stackoverflow.com/questions/8489705/c-sharp-imageformat-to-string
        //imageformat to string 
        private static readonly Dictionary<Guid, string> _knownImageFormats =
            (from p in typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public)
             where p.PropertyType == typeof(ImageFormat)
             let value = (ImageFormat)p.GetValue(null, null)
             select new { Guid = value.Guid, Name = value.ToString() })
            .ToDictionary(p => p.Guid, p => p.Name);

        static string GetImageFormatName(ImageFormat format)
        {
            string name;
            if (_knownImageFormats.TryGetValue(format.Guid, out name))
                return name;
            return null;
        }

        /// <summary>
        /// 轉 Base64 Encode To 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="model"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public static void SaveBase64ToImage<T>(T model, string FolderPath, string UpdateProperty, string Base64String)
        {
            if (!string.IsNullOrEmpty(Base64String))
            {
                Image img = Base64ToImage(Base64String);
                if (img != null)
                {
                    string ext = string.Empty;
                    if (img.RawFormat != null)
                    {
                        ext = string.Format(".{0}", GetImageFormatName(img.RawFormat));
                    }
                    string DirectoryPath = FolderPath;
                    string FileName = Library.Utils.GetObjectId() + ext;
                    Library.Utils.CreateDirectory(DirectoryPath);
                    //img.SaveAs(System.IO.Path.Combine(DirectoryPath, FileName));
                    string strAbsPath = System.IO.Path.Combine(FolderPath, FileName);
                    img.Save(strAbsPath, img.RawFormat);

                    System.Reflection.PropertyInfo p = typeof(T).GetProperty(UpdateProperty);
                    if (p != null)
                    {
                        p.SetValue(model, FileName, null);
                    }

                }
            }

        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}