using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        [Display(Name = "ID de l'inscription")]
        public int EnrollmentID { get; set; }

        [Display(Name = "ID du cours")]
        public int CourseID { get; set; }

        [Display(Name = "ID de l'étudiant")]
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "Aucune note")]
        [Display(Name = "Note")]
        public Grade? Grade { get; set; }

        [Display(Name = "Cours")]
        public Course? Course { get; set; }

        [Display(Name = "Étudiant")]
        public Student? Student { get; set; }
    }
}