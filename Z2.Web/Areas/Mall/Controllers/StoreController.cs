using System;
using System.Collections.Generic;
using System.IO;
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

namespace Z2.Web.Areas.Mall.Controllers
{
    [ModuleAction(ModuleId = "5BD705EC-57E3-4B2C-BDAA-C855669D424C", ModuleName = "店铺管理",DisplayNo = 3)]
    [AuthoritiesCheck]
    public class StoreController : HandlerLoginInfoController
    {
        private readonly MallStoreRep _storeRep = new MallStoreRep();
        // GET: Mall/Store
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _storeRep.GetList(keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),
            };
            return Content(result.ToJson());
        }
        //获取上传的文件名
        static string fileName = null;
        //获取最后一个\的索引
        private static int start = 0;
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = _storeRep.GetForm(keyValue);
            if (data != null)
            {
                start = data.Logo.LastIndexOf('\\');//索引
                fileName = data.Logo.Substring(start, data.Logo.Length - start);//文件名
            }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            _storeRep.DeleteForm(keyValue, "admin");
            return Success("删除成功！");
        }

        /// <summary>
        /// 加载Logo
        /// </summary>
        /// <param name="keyValue"> Id </param>
        /// <returns>返回文件流</returns>
        /// 
        public ActionResult LogoImage(string keyValue)
        {

            var data = _storeRep.GetForm(keyValue);
            var filePath = data.Logo;
            var fr = new FilePathResult(filePath, ContentTypeHelper.ContentType(".jpg"));
            return fr;

        }



        public ActionResult SubmitForm(MallStore mallStore)
        {
            if (Request.Files.Count <= 0)
            {
                return new EmptyResult();
            }

            var saveType = ConfigWrap.GetValue("FileUploadSaveType", 0);
            var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");
            var AllowFileExtension = ConfigWrap.GetValue("AllowFileExtension", "");
            var AllowFileSize = ConfigWrap.GetValue("AllowFileSize", 0);

            var dt = DateTime.Now;
            var storeModel = new MallStore();


            start = mallStore.Logo.Contains('\\') ? start : 0;
            if (!mallStore.Logo.Substring(start, mallStore.Logo.Length - start).Equals(fileName)) //未改变文件
            {
                for (int i = 0; i < Request.Files.Count;)
                {

                    var httpPostedFileBase = Request.Files[i];
                    storeModel.CreaterUserId = "admin";
                    storeModel.CreaterTime = DateTime.Now;
                    storeModel.Description = mallStore.Description;
                    storeModel.EnabledFlag = mallStore.EnabledFlag;
                    // storeModel.Logo = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                    storeModel.Logo = Guid.NewGuid().ToString("N") +
                                      SysFileModel.GetExtension(httpPostedFileBase.FileName); //获取指定的路径字符串的扩展名。
                    if (httpPostedFileBase != null)
                    {
                        if (saveType == 0)
                        {
                            var savePath = System.IO.Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(),
                                dt.Day.ToString(), storeModel.Logo);
                            System.IO.FileInfo fi = new System.IO.FileInfo(savePath);
                            if (!fi.Directory.Exists) //获取指示目录是否存在的值
                            {
                                System.IO.Directory.CreateDirectory(fi.DirectoryName); //在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                            }

                            //using (var f = System.IO.File.Create(savePath)) { }
                            httpPostedFileBase.SaveAs(savePath);
                            mallStore.Logo = savePath;
                        }
                        else
                        {
                            var bytes = new byte[httpPostedFileBase.ContentLength];
                            using (var sr = new System.IO.BinaryReader(httpPostedFileBase.InputStream))
                            {
                                sr.Read(bytes, 0, bytes.Length);
                            }

                            storeModel.Logo = bytes.ToString();
                        }

                        /*mbr.SubmitForm(mallbrand);*/ //将得到的数据（文件）保存到数据库中
                        //fileModelApp.SaveFile(fileModel);
                    }

                    // brandModel.Logo = Url.HttpRouteUrl("FileManage_GetFile", new { controller = "File", action = "GetFile", FileID = brandModel.Logo, FileName = brandModel.Logo });
                    break;
                }
            }

            _storeRep.SubmitForm(mallStore);
            //return View(brandModel);
            return Success("操作成功。");
        }
    }
}

