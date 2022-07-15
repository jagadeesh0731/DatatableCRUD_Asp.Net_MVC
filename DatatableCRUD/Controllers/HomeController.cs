using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatatableCRUD.Models;

namespace DatatableCRUD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (MvcCRUDDBEntities dc = new MvcCRUDDBEntities())
            {
                var employees = dc.Employees.OrderBy(a => a.Name).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MvcCRUDDBEntities dc = new MvcCRUDDBEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MvcCRUDDBEntities dc = new MvcCRUDDBEntities())
                {
                    if (emp.EmployeeID > 0)
                    {
                        //Edit
                        var v = dc.Employees.Where(a => a.EmployeeID == emp.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.Name = emp.Name;
                            v.Position = emp.Position;
                            v.Office = emp.Office;
                            v.Age = emp.Age;
                            v.Salary = emp.Salary;
                        }
                    }
                    else
                    {
                        //Save
                        dc.Employees.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MvcCRUDDBEntities dc = new MvcCRUDDBEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if(v!=null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (MvcCRUDDBEntities dc = new MvcCRUDDBEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if(v!=null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}