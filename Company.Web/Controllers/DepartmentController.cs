using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Company.Services.Interfaces.Department.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();

            //TempData.Keep("TextTempMessage");

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepartmentDto());
        }

        [HttpPost]
        public IActionResult Create(DepartmentDto DepartmentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Add(DepartmentDto);
                    TempData["TextTempMessage"] = "Hello From Employee Index (TempData)";
                    return RedirectToAction(nameof(Index)); //subsequent
                }
                ModelState.AddModelError("DepartmentError", "ValidationError");
                return View(DepartmentDto);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("DepartmentError", ex.Message);
                return View(DepartmentDto);
            }
        }
        public IActionResult Details(int? id , string viewName="Details") 
        {
             var DepartmentDto = _departmentService.GetById(id);
             if (DepartmentDto is null)
               return RedirectToAction("NotFoundPage",null,"Home");

               return View(viewName,DepartmentDto);
            
        }
        public IActionResult Update(int? id)
        {
            if (id is null)
                return BadRequest();

            return Details(id,"Update");
        }
        [HttpPost]
        public IActionResult Update(int? id,DepartmentDto DepartmentDto)
        {
            if (DepartmentDto.Id != id.Value)
                return RedirectToAction("NotFoundPage", null , "Home");

            _departmentService.Update(DepartmentDto);
   
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id) 
        {
            if (id is null)
                return BadRequest();

            var DepartmentDto = _departmentService.GetById(id);
            if (DepartmentDto is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            _departmentService.Delete(DepartmentDto);

            return RedirectToAction(nameof(Index));
        }

        // bahwl 3la ad m'dar ani el controller bykonsh maktob feh haga
    }
}
