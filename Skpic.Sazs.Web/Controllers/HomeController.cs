using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Skpic.Sazs.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>首页</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexTest()
        {
            return View();
        }
        /// <summary>登录</summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>登录</summary>
        /// <returns></returns>
        public ActionResult LoginSoft()
        {
            return View();
        }
        /// <summary>锁屏</summary>
        /// <returns></returns>
        public ActionResult LockScreen()
        {
            return View();
        }
        /// <summary>日历</summary>
        /// <returns></returns>
        public ActionResult Calendar()
        {
            return View();
        }
        /// <summary>客户简介</summary>
        /// <returns></returns>
        public ActionResult UserProfile()
        {
            return View();
        }
        /// <summary>收件箱</summary>
        /// <returns></returns>
        public ActionResult InBox()
        {
            return View();
        }
        /// <summary>图表</summary>
        /// <returns></returns>
        public ActionResult Charts()
        {
            return View();
        }
        /// <summary>google地图</summary>
        /// <returns></returns>
        public ActionResult GoogleMaps()
        {
            return View();
        }
        /// <summary>Vector地图</summary>
        /// <returns></returns>
        public ActionResult VectorMaps()
        {
            return View();
        }
        /// <summary>可拖拽的</summary>
        /// <returns></returns>
        public ActionResult PortletDraggable()
        {
            return View();
        }
        /// <summary>全面的</summary>
        /// <returns></returns>
        public ActionResult PortletGeneral()
        {
            return View();
        }

        public ActionResult TableAdvanced()
        {
            return View();
        }

        public ActionResult TableBasic()
        {
            return View();
        }

        public ActionResult TableEditable()
        {
            return View();
        }

        public ActionResult TableManaged()
        {
            return View();
        }

        public ActionResult TableResponsive()
        {
            return View();
        }
        /// <summary>常见问题</summary>
        /// <returns></returns>
        public ActionResult Faq()
        {
            return View();
        }
        /// <summary>搜索</summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }
        /// <summary>时间轴</summary>
        /// <returns></returns>
        public ActionResult TimeLine()
        {
            return View();
        }

        public ActionResult LayoutHorizontalSidebarMenu()
        {
            return View();
        }
        public ActionResult LayoutHorizontalSidebarMenu2()
        {
            return View();
        }
        public ActionResult LayoutPromo()
        {
            return View();
        }
        /// <summary>上传</summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Upload(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称

                    fileData.SaveAs(filePath + saveName);
                    return Json(new { Success = true, FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
