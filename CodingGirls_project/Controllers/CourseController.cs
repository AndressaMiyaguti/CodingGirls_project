using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodingGirls_project.Contexts;
using CodingGirls_project.Models;
using System.Text.Json;

namespace CodingGirls_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly SchoolContext _context;

        public CourseController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Course
        [HttpGet]
       public async Task<JsonResult> GetCourse()
        {
            if (_context.Course == null || _context.Course.Count() == 0)
                return new JsonResult("Não há nenhum curso cadastrado.");

            var courses = await _context.Course
                .Where(c => c.Active)
                .Include(c => c.Students)
                .ToListAsync();

            return new JsonResult(new
            {
                total = courses.Count,
                courses = courses
            });
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
          if (_context.Course == null)
          {
              return NotFound();
          }
            // var course = await _context.Course.FindAsync(id);
            var course = await _context.Course
                .FindAsync(id);
                //.Include(c => c.Students);               
              
            /*
            if (course == null)
            {
                return NotFound();
            }*/

            return course;
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
           
            if (course.Id <= 0)
            {
                return BadRequest("Nenhuma referecia de curso informado para alteração");
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Alteração realizada com sucesso!");
        }

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      /*  [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
          if (_context.Course == null)
          {
              return Problem("Entity set 'SchoolContext.Course'  is null.");
          }
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }*/
        [HttpPost("add")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            var listErrors = new List<string>();
            string response = "";

            if (_context.Course == null)
                return Ok("Nenhum curso foi informado!");

            try
            {
                if (_context.Course != null)
                {
                    if (course.CourseName == string.Empty)
                        listErrors.Add("O nome para o curso está ausente!");


                    if (_context.Course.Any(c => c.CourseName == course.CourseName))
                        listErrors.Add("O curso informado já está em uso.");


                    if (listErrors.Count <= 0)
                    {
                        if (!CourseExists(course.Id))
                        {
                            _context.Course.Add(course);
                            await _context.SaveChangesAsync();
                            response = JsonSerializer.Serialize(course);
                        }
                        else
                        {
                            response = "Referencia do curso já está em uso!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = JsonSerializer.Serialize(ex.Message);
            }

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, response);
        }


        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Course == null)
            {
                return NotFound("Não há nenhum curso cadastrado");
            }
            var contato = await _context.Course.FindAsync(id);

            if (contato == null)
            {
                return NotFound($"O id: {id} não existe na base de dados.");
            }

            _context.Course.Remove(contato);
            await _context.SaveChangesAsync();

            return Ok("O registro foi removido com sucesso!");
        }

        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
