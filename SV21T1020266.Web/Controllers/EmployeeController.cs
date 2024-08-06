using Microsoft.AspNetCore.Mvc;
using SV21T1020266.BusinessLayers;
using SV21T1020266.DomainModels;

namespace SV21T1020266.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = BusinessLayers.CommonDataServices.ListOfEmployee(out rowCount, page, PAGE_SIZE, searchValue ?? "");
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
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Employee? employee = CommonDataServices.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        [HttpPost]
        public IActionResult Save(Employee? data)
        {
            if (data.EmployeeID == 0)
            {
                CommonDataServices.AddEmployee(data);
            }
            else
            {
                CommonDataServices.UpdateEmployee(data);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataServices.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var employee = CommonDataServices.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataServices.IsWorkingEmployee(id);
            return View(employee);
        }
    }
}
