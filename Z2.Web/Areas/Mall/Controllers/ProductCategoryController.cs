using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Configs;
using Z2.Core.Handler;
using Z2.Core.Web;
using Z2.Model.Mall;
using Z2.Repository.Mall;
using System.IO;
using Z2.Core.Filter;
using Z2.Core.Interface;
using Z2.Core.WebApp.Model;
using Z2.Core.Utility;
using Z2.Web.Filter;

namespace Z2.Web.Areas.Mall.Controllers
{
    [ModuleAction(ModuleId = "A0353F7D-97E4-464E-8CF1-08DA1220EED8", ModuleName = "商品类别",DisplayNo = 2)]
    [AuthoritiesCheck]
    public class ProductCategoryController : HandlerLoginInfoController
    {
        private ProductCategoryRep pcr = new ProductCategoryRep();
        // GET: Mall/ProductCategory
        /// <summary>
        /// 初始化界面数据绑定（treeviw 树状形式）
        /// </summary>
        /// <param name="keyword">传入搜索框中的相应参数</param>
        /// <returns></returns>
        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridjson(string keyword)
        {
            var data = pcr.GetProductsList(keyword);
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChild = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;//判断data中是否有数据
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChild;//是否有子叶(级)
                treeModel.parentId = item.ParentId;
                treeModel.expanded = hasChild;//是否能伸展
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());//返回treeList树状集合并转换成Json发送给前台
        }



        /// <summary>
        /// 给select（下拉菜单）控件绑定数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var keyword = "";
            var data = pcr.GetProductsList(keyword);
            var treeList = new List<TreeSelectModel>();//创建变量树形集合存放查询到的结果
            foreach (var item in data)//将查询到的结果遍历给树形集合
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id;
                treeModel.text = item.ItemName;
                treeModel.parentId = item.ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());//将树形集合转换成json格式
        }


        /// <summary>
        /// 提交数据并保存至服务器+数据修改
        /// </summary>
        /// <param name="products">products 实体</param>
        /// <param name="keyValue">参数，用于分辨新增界面和修改界面</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitForm(ProductCategory products, string keyValue)
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
            var productsModel = new ProductCategory();
            for (int i = 0; i < Request.Files.Count;)//将收到的数据遍历给对应的字段
            {
                var httpPostedFileBase = Request.Files[i];//定义变量接收客户端上载文件的集合
                productsModel.CreaterUserId = "admin";
                productsModel.CreaterTime = DateTime.Now;
                productsModel.Description = products.Description;
                productsModel.EnabledFlag = products.EnabledFlag;
                productsModel.CategoryImg = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
                productsModel.Path = Guid.NewGuid().ToString("N") + SysFileModel.GetExtension(httpPostedFileBase.FileName);//获取客户端上载完文件的名称包括文件路径
                productsModel.DisplayNo = products.DisplayNo;
                if (httpPostedFileBase != null)
                {
                    if (saveType == 0)
                    {
                        var savePath = System.IO.Path.Combine(uploadPath, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), productsModel.CategoryImg);
                        System.IO.FileInfo fi = new System.IO.FileInfo(savePath);//创建文件路径保存地址
                        if (!fi.Directory.Exists)//获取指定目录是否存在
                        {
                            System.IO.Directory.CreateDirectory(fi.DirectoryName);// 在指定路径创建所有目录和子目录,获取表示目录的完整路径的字符串
                        }
                        httpPostedFileBase.SaveAs(savePath);//待保存文件的名称，保存上载文件的内容
                        products.Path = savePath;
                    }
                    else
                    {
                        var bytes = new byte[httpPostedFileBase.ContentLength];//文件长度
                        using (var sr = new System.IO.BinaryReader(httpPostedFileBase.InputStream))//InputStream  用于读取文件内容的对象
                        {
                            sr.Read(bytes,0,bytes.Length);//从字节数组的指定位置点开始，从流中读取指定字节数
                        }
                        productsModel.Path = bytes.ToString();
                    }
                }
                break;
            }
            products.DeleteFlag = true;
            products.CreaterTime = DateTime.Now;
            products.CreaterUserId = "admin";
            pcr.AddImgFileData(products,keyValue);
            return Success("操作成功");
        }



        /// <summary>
        /// 显示数据详情
        /// </summary>
        /// <param name="keyValue">用于判断选择的哪一行数据</param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetDetalisJson(string keyValue)
        {
            var data = pcr.GetDetails(keyValue);
            return Content(data.ToJson());
        }



        /// <summary>
        /// 详情界面展示该数据中所带有的照片信息
        /// </summary>
        /// <param name="keyValue">判断当前数据是哪一行</param>
        /// <returns></returns>
        public ActionResult GetProductImg(string keyValue)
        {
            var data = pcr.GetDetails(keyValue);
            var proImg = data.Path;
            var fr = new FilePathResult(proImg,ContentTypeHelper.ContentType(".jpg"));
            return fr;
        }
        
        /// <summary>
        ///删除功能
        /// </summary>
        /// <param name="keyValue">    </param>
        /// <returns></returns>
        public ActionResult DeleteForm(string keyValue)
        {
            pcr.GetDelete(keyValue, "admin");
            return Success("删除成功。");
        }
        public ActionResult TestPage()
        {
            return View();
        }
    }
}