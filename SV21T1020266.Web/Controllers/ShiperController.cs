using Microsoft.AspNetCore.Mvc;
using SV21T1020266.BusinessLayers;
using SV21T1020266.DomainModels;

namespace SV21T1020266.Web.Controllers
{
    public class ShiperController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = BusinessLayers.CommonDataServices.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount / PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
            {
                pageCount += 1;
            }
            ViewBag.Page = page;
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            Shipper shipper = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", shipper);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Shipper? shipper = CommonDataServices.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            return View(shipper);
        }

        [HttpPost]
        public IActionResult Save(Shipper? data)
        {
            if (data.ShipperID == 0)
            {
                CommonDataServices.AddShipper(data);
            }
            else
            {
                CommonDataServices.UpdateShipper(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataServices.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var shipper = CommonDataServices.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataServices.IsUsedShipper(id);
            return View(shipper);
        }
    }
}
