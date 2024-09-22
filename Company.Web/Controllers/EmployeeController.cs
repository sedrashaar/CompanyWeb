using Company.Data.Entities;
using Company.Services.Helper;
using Company.Services.Interfaces;
using Company.Services.Interfaces.Employee.Dto;
using Company.Services.Services;
using Company.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace Company.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
       

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
         
        }
        public IActionResult Index(string searchInp)
        {   //ViewBag,ViewTemp,TempData
            //ViewBag.Message = "Hello From EmployeeDto Index(ViewBag)";

            //ViewData["TextMessage"] = "Hello From EmployeeDto Index (ViewData)";


            IEnumerable<EmployeeDto> employees = new List<EmployeeDto>();
            if(string.IsNullOrEmpty(searchInp))
            {
                employees = _employeeService.GetAll();
            }
            else
            {
                employees = _employeeService.GetEmployeesByName(searchInp);
            }
            return View(employees);
          

        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            //ViewBag,ViewTemp,TempData
            ViewBag.Departments = _departmentService.GetAll();
            return View();
        }
       
        [HttpPost]
        public IActionResult Create(EmployeeDto EmployeeDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _employeeService.Add(EmployeeDto);
                    return RedirectToAction(nameof(Index));      
                }
                return View(EmployeeDto);
            }
            catch (Exception ex)
            {
                return View(EmployeeDto);
            }
        }


        public IActionResult Details(int? id, string viewName = "Details")
        {
            var EmployeeDto = _employeeService.GetById(id);
            if (EmployeeDto is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            return View(viewName, EmployeeDto);

        }
        public IActionResult Update(int? id)
        {
            if (id is null)
                return BadRequest();

            return Details(id, "Update");
        }
        [HttpPost]
        public IActionResult Update(int? id, EmployeeDto EmployeeDto)
        {
            if (EmployeeDto.Id != id)
                return RedirectToAction("NotFoundPage", null, "Home");

            _employeeService.Update(EmployeeDto);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var EmployeeDto = _employeeService.GetById(id);
            if (EmployeeDto is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            _employeeService.Delete(EmployeeDto);

            if (!string.IsNullOrEmpty(EmployeeDto.ImageUrl))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", "Images");
                var filePath = Path.Combine(folderPath, EmployeeDto.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
