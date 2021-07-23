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
    [ModuleAction(ModuleId = "F8E3E0A2-78C2-47BD-843D-95DB87663C7D", ModuleName = "店铺级别",DisplayNo = 5)]
    [AuthoritiesCheck]
    public class StoreRankController : HandlerLoginInfoController
    {


        // GET: Mall/StoreRank
        private readonly MallStoreRank _mdl = new MallStoreRank();
        private readonly MallStoreRankRep _mallStoreRankRep = new MallStoreRankRep();


        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = _mallStoreRankRep.GetList(keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count),
                page = gridParam.page,
                records = data.Count,
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows)
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
            var data = _mallStoreRankRep.GetForm(keyValue);
            //解决在未选择的情况下  抛异常
            if (data != null)
            {
                start = data.Avatar.LastIndexOf('\\');
                fileName = data.Avatar.Substring(start, data.Avatar.Length - start);//文件名
            }


            return Content(data.ToJson());
        }
        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var data = _mallStoreRankRep.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }

        public ActionResult SubmitForm(MallStoreRank mallStoreRank)
        {
            if (Request.Files.Count <= 0)
            {
                return new EmptyResult();
            }
            var saveType = ConfigWrap.GetValue("FileUploadSaveType", 0);
            var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");
            //var AllowFileExtension = ConfigWrap.GetValue("AllowFileExtension", "");
            var AllowFileSize = ConfigWrap.GetValue("AllowFileSize", 0);

            var dt = DateTime.Now;
            start = mallStoreRank.Avatar.Contains('\\') ? start : 0;
            if (!mallStoreRank.Avatar.Substring(start, mallStoreRank.Avatar.Length - start).Equals(fileName))//未改变文件
            {
                for (int i = 0; i < Request.Files.Count;)
                {
                    var httpPostedFileBase = Request.Files[i];
                    if (httpPostedFileBase != null)
                    {
                        _mdl.CreaterUserId = "admin";
                        _mdl.CreaterTime = DateTime.Now;
                        _mdl.Description = mallStoreRank.Description;
                        _mdl.EnabledFlag = mallStoreRank.EnabledFlag;
                        // _mdl.Avatar = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                        _mdl.Avatar = Guid.NewGuid().ToString("N") +
                                                         SysFileModel.GetExtension(httpPostedFileBase.FileName); //获取指定的路径字符串的扩展名。
                        if (saveType == 0)
                        {
                            var savePath = Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(),
                                dt.Day.ToString(), _mdl.Avatar);
                            FileInfo fileInfo = new FileInfo(savePath);
                            if (fileInfo.Directory != null && !fileInfo.Directory.Exists ) //获取指示目录是否存在的值
                            {
                                if (fileInfo.DirectoryName != null)
                                    Directory.CreateDirectory(fileInfo.DirectoryName); //在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                            }

                            httpPostedFileBase.SaveAs(savePath);
                            mallStoreRank.Avatar = savePath;
                        }
                        else
                        {
                            var bytes = new byte[httpPostedFileBase.ContentLength];
                            using (var sr = new BinaryReader(httpPostedFileBase.InputStream))
                            {
                                sr.Read(bytes, 0, bytes.Length);
                            }

                            _mdl.Avatar = bytes.ToString();
                        }


                    }

                    break;
                }
            }
            _mallStoreRankRep.SubmitForm(mallStoreRank);
            return Success("操作成功。");

        }

        /// <summary>
        /// 加载Logo
        /// </summary>
        /// <param name="keyValue"> Id </param>
        /// <returns>返回文件流</returns>
        public ActionResult LogoImage(string keyValue)
        {
            var data = _mallStoreRankRep.GetForm(keyValue);
            var filePath = data.Avatar;
            var fr = new FilePathResult(filePath, ContentTypeHelper.ContentType(".jpg"));
            return fr;
        }


    }
}