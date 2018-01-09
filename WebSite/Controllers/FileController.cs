using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EPA.Project.WebSite.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/Get
        public ActionResult Get(string FileId)
        {
            FileId = FileId.Replace("/", "");
            FileId = FileId.Replace("\\", "");
            
            string FilePath = Server.MapPath("~/App_Data/UploadFile/" + FileId);

            if (System.IO.File.Exists(FilePath))
            {
                FileStream logFileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader logFileReader = new StreamReader(logFileStream);

                //string fileName = FilePath;
                //string strContentDisposition = String.Format("{0}; filename=\"{1}\"", "attachment", fileName);
                //Response.AddHeader("Content-Disposition", strContentDisposition);

                return File(logFileStream, EPA.Project.WebSite.Library.Utils.GetContentType(System.IO.Path.GetExtension(FileId)), FileId);
            }
            else
            {
                return Content(string.Empty);
            }
        }

         /// <summary>
        /// ckeditor上傳圖片
        /// </summary>
        /// <param name="upload">預設參數叫upload</param>
        /// <param name="CKEditorFuncNum"></param>
        /// <param name="CKEditor"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
       [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string result = "";
            if (upload!=null && upload.ContentLength>0)
            {
                string FileName = Library.Utils.GetObjectId() + System.IO.Path.GetExtension(upload.FileName).ToLower();
                //儲存圖片至Server
                upload.SaveAs(Server.MapPath("~/App_Data/UploadFile/" + FileName));

                var imageUrl = "http://52.187.122.112/File/Get?FileId=" + FileName;

                var vMessage = string.Empty;

               result = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + imageUrl + "\", \"" + vMessage + "\");</script></body></html>";

            }

            return Content(result);
        }

    }
}
