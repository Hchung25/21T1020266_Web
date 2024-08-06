using Microsoft.AspNetCore.Mvc;
using SV21T1020266.BusinessLayers;
using SV21T1020266.DomainModels;

namespace SV21T1020266.Web.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = BusinessLayers.CommonDataServices.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

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
            ViewBag.Title = "Bổ sung khách hàng";

            Customer customer = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit", customer);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";

            Customer? customer = CommonDataServices.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Save (Customer? data)
        {
            //TODO: Kiểm tra dữ liệu đầu vào có hợp lệ hay không!!? => Ghi task list
            if (data.CustomerID == 0)
            {
                CommonDataServices.AddCustomer(data);
            }else
            {
                CommonDataServices.UpdateCustomer(data);
            }    
            return RedirectToAction("Index");           
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                    CommonDataServices.DeleteCustomer(id);
                    return RedirectToAction("Index");
            }
            var customer = CommonDataServices.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataServices.IsUsedCustomer(id);
            return View(customer);
        }
    }
}
