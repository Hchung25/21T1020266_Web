using Microsoft.AspNetCore.Mvc;
using SV21T1020266.BusinessLayers;
using SV21T1020266.DomainModels;

namespace SV21T1020266.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = BusinessLayers.CommonDataServices.ListOfCategory(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount / PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
            {
                pageCount += 1;
            }
            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.RowCount = rowCount;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            Category category = new Category()
            {
                CategoryID = 0
            };
            return View("Edit", category);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin loại hàng";

            Category? category = CommonDataServices.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Save(Category? data)
        {
            //TODO: Kiểm tra dữ liệu đầu vào có hợp lệ không
            if (data.CategoryID == 0)
            {
                CommonDataServices.AddCategory(data);
            }else
            {
                CommonDataServices.UpdataCategory(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataServices.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var category = CommonDataServices.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
