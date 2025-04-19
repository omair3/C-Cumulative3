using C__Cumulative_Part_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace C__Cumulative_Part_1.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController()
        {
            _api = new TeacherAPIController();
        }

        /// <summary>
        /// Displays a list of all teachers on a dynamic page.
        /// </summary>
        /// <returns>A view with all teacher records.</returns>
        // GET: /TeacherPage/List
        public IActionResult List()
        {
            List<Teacher> Teachers = _api.ListTeacherRecords();
            return View(Teachers);
        }

        /// <summary>
        /// Displays detailed information for a specific teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to display.</param>
        /// <returns>A view with the teacher's details.</returns>
        // GET: /TeacherPage/Show/{id}
        public IActionResult Show(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        /// <summary>
        /// Loads the edit form with data for a specific teacher.
        /// </summary>
        /// <param name="id">The ID of the teacher to edit.</param>
        /// <returns>A view with a pre-filled form for editing.</returns>
        // GET: /TeacherPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        /// <summary>
        /// Updates a teacher's information in the database using submitted form data.
        /// </summary>
        /// <param name="id">The ID of the teacher to update.</param>
        /// <param name="TeacherFirstName">Updated first name.</param>
        /// <param name="TeacherLastName">Updated last name.</param>
        /// <param name="EmployeeNumber">Updated employee number.</param>
        /// <param name="HireDate">Updated hire date.</param>
        /// <param name="Salary">Updated salary.</param>
        /// <returns>Redirects to the teacher's details page after update.</returns>
        // POST: /TeacherPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string TeacherFirstName, string TeacherLastName, string EmployeeNumber, DateTime HireDate, double Salary)
        {
            Teacher UpdatedTeacher = new Teacher();
            UpdatedTeacher.TeacherFirstName = TeacherFirstName;
            UpdatedTeacher.TeacherLastName = TeacherLastName;
            UpdatedTeacher.EmployeeNumber = EmployeeNumber;
            UpdatedTeacher.HireDate = HireDate;
            UpdatedTeacher.Salary = Salary;

            _api.UpdateTeacher(id, UpdatedTeacher);

            return RedirectToAction("Show", new { id = id });
        }
    }
}
