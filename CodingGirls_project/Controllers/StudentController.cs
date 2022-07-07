using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodingGirls_project.Contexts;
using CodingGirls_project.Models;
using System.Text.Json;


namespace CodingGirls_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            if (_context.Student == null || _context.Student.Count() == 0)
            {
                return NotFound("Não há nenhum aluno cadastrado.");
            }
            return await _context.Student
                .Include(s => s.Course)
                .ToListAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            if (_context.Student == null || _context.Student.Count() == 0)
            {
                return NotFound("Não há nenhum estudante cadastrado.");
            }
            var student =  _context.Student
                .Include(s => s.Course)
                .FirstOrDefault(s => s.Id == id);


            return student ;
          
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (student.Id <= 0)
            {
                return BadRequest("Nenhum id informado para alteração");
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id))
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

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var listErrors = new List<string>();
            string response = "";

            try
            {       
                
                if (_context.Student == null)
                {
                    if (student.StudentName == string.Empty)
                        listErrors.Add("O campo nome está ausente!");
                    
                    response = JsonSerializer.Serialize(listErrors);
                }

                else if (!StudentExists(student.Id))
                {
                    _context.Student.Add(student);
                    await _context.SaveChangesAsync();
                    response = JsonSerializer.Serialize(student);
                }
                else
                {
                    response = "Este aluno já possui cadastro!";
                }

            }
            catch (Exception ex)
            {
                response = JsonSerializer.Serialize(ex.Message);
            }

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, response);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Student == null)
            {
                return NotFound("Não há nenhum aluno cadastrado");
            }
            var contato = await _context.Student.FindAsync(id);

            if (contato == null)
            {
                return NotFound($"O id: {id} não existe na base de dados.");
            }

            _context.Student.Remove(contato);
            await _context.SaveChangesAsync();

            return Ok("O registro foi removido com sucesso!");
        }

        private bool StudentExists(int id)
        {
            return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
