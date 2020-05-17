using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD.DAL;
using APBD.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD.Controllers
{
    [ApiController]
    [Route("api/students")]

    public class StudentsController : ControllerBase

    {
        private readonly IDbService _dbService;
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("semestr/{id}")]
        public IActionResult GetStudentsSemestr(string index)
        {
            return Ok(_dbService.GetStudentsSemestr(index));
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentDetails(int id)
        {
            if (id == 1)
            {
                return Ok("Jan Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Andrzej Malewski");
            }
            return NotFound("Nie znaleziono studenta");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 10000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            var s1 = new Student { IdStudent = 1, FirstName = "Jan", LastName = "Kowalski", IndexNumber = "s20001" };

            s1.FirstName = student.FirstName;
            s1.LastName = student.LastName;
            s1.IndexNumber = student.IndexNumber;

            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveStudent(int id)
        {
            var s1 = new Student { IdStudent = 1, FirstName = "Jan", LastName = "Kowalski", IndexNumber = "s20001" };

            if (s1.IdStudent == id)
            {
                s1 = null;
            }

            return Ok("Usuwanie ukończone");
        }
    }
}