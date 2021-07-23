using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Z2.Core;
using Z2.Core.DataBases;
using Z2.Core.Interface;
using Z2.Core.Operator;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Areas.PM.Controllers;

namespace Z2.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ModuleRep _rep = new ModuleRep();
        private readonly SysModule _sysModule = new SysModule();
        private readonly ModuleRep _moduleRep = new ModuleRep();
        private readonly SysWidget _sysWidget = new SysWidget();
        private readonly CMS_WidgetBaseRep _sysWidgetRep = new CMS_WidgetBaseRep();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //修改标识：范文强  2018/10/15
            //修改描述：动态加载程序集 注册模块

            #region 自动注册模块  1.0  反射

            //    //Dll文件所在的文件夹
            //    string dllFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            //    //加载文件路径下的dll文件
            //    string[] dllFiles = Directory.GetFiles(dllFilePath, "*.dll");
            //    //找到Z2开头的dll文件
            //    foreach (string item in dllFiles)
            //    {
            //        //获取到Z2.Web
            //        if (Path.GetFileName(item).StartsWith("Z2"))
            //        {
            //            //加载程序集
            //            Assembly ass = Assembly.LoadFile(item);
            //            //获取所有的类
            //            Type[] types = ass.GetExportedTypes();

            //            foreach (Type tt in types)
            //            {
            //                //找到所有继承字AreaRegistration的子类
            //                if (tt.IsSubclassOf(typeof(AreaRegistration)) && !tt.IsAbstract)
            //                {
            //                    //初始化对象
            //                    object obj = Activator.CreateInstance(tt);
            //                    //根节点的文件夹（Area）名称
            //                    string rootAreaName = tt.GetProperty("AreaName").GetValue(obj, null).ToString();
            //                    //根节点Id
            //                    string rootId = tt.GetProperty("PlugInId").GetValue(obj, null).ToString();
            //                    //根节点的名称
            //                    string rootName = tt.GetProperty("PlugInName").GetValue(obj, null).ToString();
            //                    //根节点添加到数据库中
            //                    Operate(rootId, "0", rootName);

            //                    //功能模块类所在的命名空间
            //                    string namesp = tt.Namespace + ".Controllers";
            //                    //加载某个节点下的模块
            //                    Assembly allClass = Assembly.GetExecutingAssembly();
            //                    //遍历每个类型
            //                    foreach (Type cla in allClass.GetTypes())
            //                    {
            //                        if (cla.Namespace == namesp)
            //                        {
            //                            string mid = string.Empty;
            //                            string mName = string.Empty;
            //                            //找具有相应特性标签的类  获取相应的特性值
            //                            ModuleActionAttribute moduleAttr = cla.GetCustomAttribute(typeof(ModuleActionAttribute)) as ModuleActionAttribute;
            //                            if (moduleAttr != null)
            //                            {
            //                                mid = moduleAttr.ModuleId;
            //                                mName = moduleAttr.ModuleName;
            //                                //将注册的模块添加到数据库中
            //                                //1.先判断是否已经存在在数据库中
            //                                Operate(mid, rootId, mName);

            //                            }//end if
            //                            else
            //                                continue;

            //                        }//end  if
            //                    }//end  foreach

            //                }//end if

            //            }//end foreach

            //        }//end if 

            //    }//end foreach

            #endregion

            #region 自动注册模块  2.0 

            //所有实现IAppPlugIn的所有类
            var allCompleteCla = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IAppPlugin))))
                .ToArray();
            //遍历所有的实现类
            foreach (var item in allCompleteCla)
            {
                var obj = Activator.CreateInstance(item) as IAppPlugin;
                if (obj == null) continue;
                var rootId = obj.PlugInId; //模块Id
                var rootName = obj.PlugInName; //模块名称
                var routeName = obj.RouteName; //路由名
                string namesp = item.Namespace + ".Controllers";
                Operate(rootId, "0", rootName, "", 0, 0);
                //加载所有继承自HandlerLoginInfoController的Controllers  
                var allControllers = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(Controller))))
                    .ToArray();
                //遍历每个控制器类
                foreach (var cla in allControllers)
                {
                    if (cla.Namespace != namesp) continue;
                    var moduleAttr = cla.GetCustomAttribute(typeof(ModuleActionAttribute)) as ModuleActionAttribute;
                    if (moduleAttr != null && !moduleAttr.Manual)
                    {
                        var mId = moduleAttr.ModuleId;
                        var mName = moduleAttr.ModuleName;
                        var DisplayNo = moduleAttr.DisplayNo;
                        //拼接链接路径 
                        var controller = cla.Name.Substring(0, cla.Name.Length - 10); //只获取Controller前面的部分
                        var url = RouteTable.Routes[routeName] as Route;
                        var urlAddress = obj.GetUrl(url.Url.Split('/')[0], controller);

                        //将注册的模块添加到数据库中
                        if (moduleAttr.ParentModuleId != null) //三级菜单
                            Operate(mId, moduleAttr.ParentModuleId, mName, urlAddress, 2, DisplayNo);
                        else //二级菜单
                            Operate(mId, rootId, mName, urlAddress, 1, DisplayNo);

                    } //end if
                    else
                        continue;
                } //end  foreach
            } //end  foreach

            #endregion

            #region 注册 组件


            var allViewModel = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IWidgetModel))))
                .ToArray();

            foreach (var viewModel in allViewModel)
            {

                var genericType = typeof(WidgetBaseController<>);
                var conType = genericType.MakeGenericType(viewModel);
                var allWidgets = AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(conType)))
              .ToArray();
                foreach (var item in allWidgets)
                {
                    var widget = item.GetCustomAttribute(typeof(WidgetAttribute)) as WidgetAttribute;
                    if (widget == null) continue;
                    _sysWidget.WidgetId = widget.WidgetKey;
                    _sysWidget.Name = widget.WidgetTitle;
                    _sysWidget.Icon = widget.WidgetIcon;
                    var WidgetArea = widget.WidgetArea;
                    _sysWidget.UrlAddress = $"~/{WidgetArea}/{item.Name.Substring(0, item.Name.Length - 10)}/Index";
                    InsertWidgets(_sysWidget);
                }
            }
            #endregion

        }

        /// <summary>
        /// 封装 插入数据库
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="pId">父Id</param>
        /// <param name="name">名称</param>
        /// <param name="urlAddress">链接地址</param>
        /// <param name="layer">菜单层级 0:父节点 1:一级子节点 2:二级子节点</param>
        private void Operate(string moduleId, string pId, string name, string urlAddress, int layer, int Displayno)
        {
            DbUtility db = DbUtility.GetInstance();
            _sysModule.Id = moduleId; //默认为模块Id    无需自动生成
            _sysModule.ModuleId = moduleId;
            _sysModule.ParentId = pId;
            _sysModule.Name = name;
            _sysModule.EnabledFlag = true; //默认有效 true
            _sysModule.Target = pId == "0" ? "expand" : "iframe"; //expand:无页面  iframe：框架页
            _sysModule.UrlAddress = urlAddress;
            _sysModule.Layers = layer; //菜单层级 
            _sysModule.DisplayNo = Displayno;

            //数据库中不存在id  也不存在Pid
            var existFlag = _rep.CheckExist(moduleId, pId);
            if (existFlag == "None")
            {
                if (pId != "0") //框架页添加链接
                {
                    _moduleRep.AddDefaultFunc(db, _sysModule.ModuleId);
                }

                _rep.AddModule(_sysModule);
            }
            //id存在 但是Pid不存在
            else if (existFlag == "NotEuqal")
            {
                //更新
                _rep.UpdateModule(_sysModule);
            }
            //id和Pid都存在
            else if (existFlag == "Exist")
            {
                //更新
                //_rep.UpdateModuleS(_sysModule);
            }

        }


        /// <summary>
        /// 插入组件
        /// </summary>
        /// <param name="widgets"></param>
        private void InsertWidgets(SysWidget widgets)
        {

            widgets.DeleteFlag = false;
            widgets.EnabledFlag = true;

            if (!_sysWidgetRep.CheckExist(widgets))//判断是否存在
            {
                widgets.CreaterTime = DateTime.Now;
                widgets.CreaterUserId = "admin";
                _sysWidgetRep.InsertData(widgets);
            }
            else
            {
                widgets.LastUpdateTime = DateTime.Now;
                widgets.LastUpdateUserId = "admin";
                _sysWidgetRep.ChangeWidgetInfo(widgets);
            }


        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var LogWriter = Core.Logger.LogFactory.GetLogger();
            var currentUser = Core.Operator.OperatorProvider.Provider.GetCurrent();
            Exception ex = Server.GetLastError();
            LogWriter.Error(new Core.Logger.LogContent()
            {
                Account = currentUser?.UserId,
                IPAddress = HttpContext.Current.Request.UserHostAddress,
                ModuleId = "Application",
                ModuleName = "",
                Result = "error",
                Message = ex?.Message,
                Description = "",
                Source = ex?.Source,
                StackTrace = ex?.StackTrace,
                TargetSite = ""
            });
            Application[HttpContext.Current.Request.UserHostAddress.ToString()] = ex;
#if RELEASE
            Server.ClearError();
#endif
            //UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            //Response.Redirect(url.Action("HttpError", "Home"), false);
        }
    }
}

