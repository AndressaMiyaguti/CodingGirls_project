using CodingGirls_project.Contexts;
using CodingGirls_project.Controllers;
using CodingGirls_project.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestSchool1
{
    public class Tests
    {
        private IEnumerable<Student> GetStudent()
        {
            return new List<Student>()
            {
                new Student { Id = 4, StudentName = "Teste 1", BirthDate = DateTime.Today, },
                new Student { Id = 5, StudentName = "Teste 1", BirthDate = DateTime.Today, },
                new Student { Id = 6, StudentName = "Teste 1", BirthDate = DateTime.Today, },
                new Student { Id = 7, StudentName = "Teste 1", BirthDate = DateTime.Today, },
              
            };
        }

        [Theory]//Anotação que indica um método que executará vários testes. Um TDD único deve usar [Fact]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void TestGetSuccess(int Id)
        {
            var data = GetStudent().AsQueryable();
            var mockSet = new Mock<DbSet<Student>>();

            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "MockedDataBase")
                .Options;

            var mockContext = new Mock<SchoolContext>(options);

            mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.Student).Returns(mockSet.Object);

            var controller = new StudentController(mockContext.Object);

            var contato = controller.GetStudent(Id);

            Assert.NotNull(contato);//Valida ou não o teste
        }
    }
    /* private Course GetCourse()
     {
         Course course = new Course()
         {
             Id = 1,
             CourseName = "Full Stack",
             Active = true,

         };
          return course;
     }

 private IEnumerable<Course> GetAllCourse()
 {
     IEnumerable<Course> courses = new List<Course>();
     return courses;
 }
    [Fact]
    public void TestGetCourse()
    {
           // var contextTest = new CourseContext().mockContext;


    }*/


}
