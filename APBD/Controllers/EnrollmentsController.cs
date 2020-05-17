using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD.DAL;
using APBD.DTO;
using APBD.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class EnrollmentsController : Controller
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService service)
        {
            _dbService = service;
        }

        [HttpPost("/add")]
        public IActionResult AddStudent(StudentDTO student)
        {

            if (_dbService.addStudent(student))
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("/promotions")]
        public IActionResult Promotions(PromotionDTO promo)
        {

            if (_dbService.promo(promo))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}