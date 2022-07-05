using System.ComponentModel.DataAnnotations.Schema;

namespace CodingGirls_project.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Column("student_name")]
        public string StudentName { get; set; }

        public DateTime BirthDate { get; set; }

        public char Gender { get; set; }

        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("non_attendance")]
        public int NonAttendance { get; set; }

        public virtual Course? Course { get; set; }
    }
}
