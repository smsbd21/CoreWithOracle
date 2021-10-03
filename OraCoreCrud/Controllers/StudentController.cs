using System.Linq;
using OraCoreCrud.Models;
using OraCoreCrud.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OraCoreCrud.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService oService;
        public StudentController(IStudentService _Service)
        {
            oService = _Service;
        }
        public IActionResult Index()
        {
            IEnumerable<Student> oStud = oService.GetStudents();
            return View(oStud);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student oStud)
        {
            oService.AddStudent(oStud);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Student oStud = oService.GetStudentById(id);
            return View(oStud);
        }
        [HttpPost]
        public IActionResult Edit(Student oStud)
        {
            oService.EditStudent(oStud);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Student oStud = oService.GetStudentById(id);
            return View(oStud);
        }
        [HttpPost]
        public IActionResult Delete(Student oStud)
        {
            oService.DeleteStudent(oStud);
            return RedirectToAction(nameof(Index));
        }
    }
}
