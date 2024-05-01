namespace Schools.Models
{
    public class ExamViewModel
    {        
        public class ExamGridViewModel
        {
            public int ClassId { get; set; }
            public IEnumerable<Student> Students { get; set; }
            public IEnumerable<Class> Classes { get; set; }
            public IEnumerable<Subjects> Subjects { get; set; }
            public IList<Exams> Exams { get; set; }
        }
    }
}
