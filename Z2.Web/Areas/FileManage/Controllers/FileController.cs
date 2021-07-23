using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Configs;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Utility;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Areas.FileManage.Controllers
{
    [ModuleAction(ModuleId = "64A1C550-2C61-4A8C-833D-ACD0C012260F", ModuleName ="文件管理",DisplayNo = 1)]
    [AuthoritiesCheck]
    public class FileController : HandlerLoginInfoController
    {
        private SysFileModelReg fileModelApp = new SysFileModelReg();
        // GET: FileManage/File
        [HttpPost]
        public ActionResult UploadFile(SysFileModel fileEntity)
        { 
            if (Request.Files.Count <= 0)
            {
                return new EmptyResult();
            }
            if (!string.IsNullOrEmpty(fileEntity.FileID))
            {
                fileEntity.LastUpdateTime = DateTime.Now;
                fileEntity.LastUpdateUserId = "admin";
                var b1 = fileModelApp.UpdateFileData(fileEntity);
                return Success("修改成功。");
            }
            if (fileEntity == null)
            {
                return new EmptyResult();
            }
            //文件上传保存方式 0：路径；1：DB
            var saveType = ConfigWrap.GetValue("FileUploadPathType", 0);
            var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");

            //**********判读文件扩展名和文件大小**********
            var AllowFileExtension = ConfigWrap.GetValue("AllowFileExtension", "");
            var AllowFileSize = ConfigWrap.GetValue("AllowFileSize", 0);

            var dt = DateTime.Now;
            var fileModel = new SysFileModel();
            for (var i = 0; i < Request.Files.Count;)
            {
                var httpPostedFileBase = Request.Files[i];
                fileModel.FileID = ResultHelper.NewId();
                fileModel.CreaterUserId = "admin";
                fileModel.CreaterTime = DateTime.Now;
                fileModel.Description = fileEntity.Description;
                fileModel.EnabledFlag = fileEntity.EnabledFlag;
                fileModel.OrgFilePath = fileEntity.OrgFilePath;
                fileModel.OrgFileName = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                fileModel.FileName = Guid.NewGuid().ToString("N") + SysFileModel.GetExtension(httpPostedFileBase.FileName);

                if (httpPostedFileBase != null)
                {
                    if (saveType == 0)
                    {
                        var savePath = System.IO.Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), fileModel.FileName);
                        System.IO.FileInfo fi = new System.IO.FileInfo(savePath);
                        if (!fi.Directory.Exists)
                        {
                            System.IO.Directory.CreateDirectory(fi.DirectoryName);
                        }
                        //using (var f = System.IO.File.Create(savePath)){ }
                        httpPostedFileBase.SaveAs(savePath);
                        fileModel.FilePath = savePath;
                    }
                    else
                    {
                        var bytes = new byte[httpPostedFileBase.ContentLength];
                        using (var sr = new System.IO.BinaryReader(httpPostedFileBase.InputStream))
                        {
                            sr.Read(bytes, 0, bytes.Length);
                        }
                        fileModel.FileData = bytes;
                    }
                    fileModelApp.AddFileData(fileModel);//将得到的数据（文件）保存到数据库中
                    //fileModelApp.SaveFile(fileModel);
                }
                //★File Download URL 文件DownloadUrl属性：下载文件
                fileModel.DownloadUrl = Url.RouteUrl("FileManage_GetFile", new { controller = "File", action = "GetFile", FileID = fileModel.FileID, FileName = fileModel.FileName });
                //fileModels.Files.Add(fileModel);
                break;
            }
            return View(fileModel);
        }
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetFile(string FileID, string FileName)
        {
            var fileinfo = fileModelApp.GetFile(FileID, "");
            // return Content(fileinfo.ToJson());
            /*
             2018/10/22  范文强  修改  
             分页数据
             */
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(fileinfo.Count()),
                page = gridParam.page,
                records = fileinfo.Count(),
                rows = fileinfo.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());

            //if (string.IsNullOrEmpty(fileinfo.FileID))
            //{
            //    return Content("");
            //}

            //if (fileinfo.SaveType == 0)
            //{
            //    if (!System.IO.File.Exists(fileinfo.FilePath))
            //    {
            //        return new HttpNotFoundResult();
            //    }
            //    var fr = new FilePathResult(fileinfo.FilePath, ContentTypeHelper.ContentType(fileinfo.Extension));
            //    return fr;
            //}
            //else if (fileinfo.SaveType == 1)
            //{
            //    if (fileinfo.FileData == null)
            //    {
            //        return new HttpNotFoundResult();
            //    }
            //    var bytes = fileinfo.FileData;
            //    var fcr = new FileContentResult(bytes, ContentTypeHelper.ContentType(fileinfo.Extension));
            //    return fcr;
            //}
            //else
            //{
            //    return new HttpNotFoundResult();
            //}

        }
        public ActionResult DownloadFile(string FileID)
        {
            var fileinfo = fileModelApp.GetFile("", FileID)[0];
            if (string.IsNullOrEmpty(fileinfo.FileID))
            {
                return new HttpNotFoundResult();
            }

            if (fileinfo.SaveType == 0)
            {
                if (!System.IO.File.Exists(fileinfo.FilePath))
                {
                    return new HttpNotFoundResult();
                }
                var fr = new FilePathResult(fileinfo.FilePath, ContentTypeHelper.ContentType(fileinfo.Extension)) { FileDownloadName = fileinfo.OrgFileName };
                return fr;
            }
            else if (fileinfo.SaveType == 1)
            {
                if (fileinfo.FileData == null)
                {
                    return new HttpNotFoundResult();
                }
                var bytes = fileinfo.FileData;
                var fcr = new FileContentResult(bytes, ContentTypeHelper.ContentType(fileinfo.Extension));
                return fcr;
            }
            else
            {
                return new HttpNotFoundResult();
            }
        }
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var fileinfo = fileModelApp.GetFile("", keyValue);
            return Content(fileinfo[0].ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysFileModel fileEntity, string keyValue)
        {
            fileEntity.CreaterUserId = "admin";
            fileEntity.LastUpdateUserId = "admin";
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    var b1 = fileModelApp.UpdateFileData(fileEntity, keyValue);
            //    return Success("修改成功。");
            //}
            //var b2 = fileModelApp.AddFileData(fileEntity);
            return Success("添加成功。");
        }
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DelFileData(string keyValue)
        {
            var fileModel = new SysFileModel();
            fileModel.FileID = keyValue;
            fileModel.DeleteTime = DateTime.Now;
            fileModel.DeleteUserId = "admin";
            var bl = fileModelApp.DelFileData(fileModel);
            return Success("删除成功。");
        }

    }
}