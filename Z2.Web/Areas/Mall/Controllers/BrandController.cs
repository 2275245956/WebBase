using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Handler;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Core.Configs;
using Z2.Core.Utility;
using System.IO;
using Z2.Core.Filter;
using Z2.Core.Interface;
using Z2.Web.Filter;

namespace Z2.Web.Areas.Mall.Controllers
{
    [ModuleAction(ModuleId = "53773A64-A81A-47F1-80E0-3D56476A456B", ModuleName = "品牌管理",DisplayNo =1)]
    [AuthoritiesCheck]
    public class BrandController : HandlerLoginInfoController
    {
        private Mall_BrandRep mbr = new Mall_BrandRep();

        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = mbr.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = mbr.GetForm(keyValue);
            return Content(data.ToJson());
        }
        public  ActionResult LogoImage(string keyValue)
        {
            var data = mbr.GetForm(keyValue);
            var filePath = data.LogoPath;
            var fr = new FilePathResult(filePath, ContentTypeHelper.ContentType(".jpg"));
            return fr;
            #region 将图片路径转成字节发送给前台
            //if (!System.IO.File.Exists(filePath))
            //{
            //    return new HttpNotFoundResult();
            //}
            //FileStream fr = new FileStream(filePath, FileMode.Open);//将读取到的数据转成文件流形式
            //BinaryReader br = new BinaryReader(fr);
            //byte[] img = br.ReadBytes((int)fr.Length);//转字节
            //br.Close();
            //fr.Close();
            //return Json(Convert.ToBase64String(img));//转成base64位字符串
            #endregion
        }

        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            var data = mbr.GetList(keyword);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.Itemname;
                treeModel.parentId = item.Id;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        public ActionResult DeleteForm(string keyValue)
        {
            mbr.DeleteForm(keyValue, "admin");
            return Success("删除成功。");
        }

        [HttpPost]
        public ActionResult SubmitForm(Mall_brand mallbrand)
        {
            if (Request.Files.Count <= 0)
            {
                return new EmptyResult();
            }
            if (!string.IsNullOrEmpty(mallbrand.Id))
            {
                mallbrand.Deleteflag = true;
                mallbrand.Lastupdatetime = DateTime.Now;
                mallbrand.Lastupdateuserid = "admin";
                mbr.UpdateMallData(mallbrand);
                return Success("修改成功。");
            }
            else
            {
                var saveType = ConfigWrap.GetValue("FileUploadSaveType", 0);
                var uploadPath = ConfigWrap.GetValue("FileUploadPath", "");

                var AllowFileExtension = ConfigWrap.GetValue("AllowFileExtension", "");
                var AllowFileSize = ConfigWrap.GetValue("AllowFileSize", 0);

                var dt = DateTime.Now;
                var brandModel = new Mall_brand();

                for (int i = 0; i < Request.Files.Count;)
                {
                    var httpPostedFileBase = Request.Files[i];
                    brandModel.Createruserid = "admin";
                    brandModel.Creatertime = DateTime.Now;
                    brandModel.Description = mallbrand.Description;
                    brandModel.Enabledflag = mallbrand.Enabledflag;
                    brandModel.Logo = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                    brandModel.LogoPath = Guid.NewGuid().ToString("N") + SysFileModel.GetExtension(httpPostedFileBase.FileName);//获取指定的路径字符串的扩展名。
                    if (httpPostedFileBase != null)
                    {
                        if (saveType == 0)
                        {
                            var savePath = System.IO.Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), brandModel.Logo);
                            System.IO.FileInfo fi = new System.IO.FileInfo(savePath);
                            if (!fi.Directory.Exists)//获取指示目录是否存在的值
                            {
                                System.IO.Directory.CreateDirectory(fi.DirectoryName);//在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                            }

                            //using (var f = System.IO.File.Create(savePath)) { }
                            httpPostedFileBase.SaveAs(savePath);
                            mallbrand.LogoPath = savePath;
                        }
                        else
                        {
                            var bytes = new byte[httpPostedFileBase.ContentLength];
                            using (var sr = new System.IO.BinaryReader(httpPostedFileBase.InputStream))
                            {
                                sr.Read(bytes, 0, bytes.Length);
                            }
                            brandModel.LogoPath = bytes.ToString();
                        }
                        /*mbr.SubmitForm(mallbrand);*///将得到的数据（文件）保存到数据库中
                                                      //fileModelApp.SaveFile(fileModel);
                    }
                    // brandModel.Logo = Url.HttpRouteUrl("FileManage_GetFile", new { controller = "File", action = "GetFile", FileID = brandModel.Logo, FileName = brandModel.Logo });
                    break;
                }
                mallbrand.Deleteflag = true;
                mallbrand.Createruserid = "admin";
                mallbrand.Creatertime = DateTime.Now;
                mbr.SubmitForm(mallbrand);
                //return View(brandModel);
                return Success("操作成功。");
            }
        }
    }
    }

    

