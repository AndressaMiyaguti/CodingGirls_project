using System.ComponentModel.DataAnnotations.Schema;

namespace CodingGirls_project.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Column("course_name")]
        public string CourseName { get; set; }

        public bool Active { get; set; }

      //  public virtual Student? Students { get; set; }

        #region Navigation Properties
        public virtual List<Student>? Students { get; set; }
        #endregion


    }
}
