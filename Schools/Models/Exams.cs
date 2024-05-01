namespace Schools.Models
{
    public class Exams
    {
        public int ExamsId { get; set; }
        public int SchoolsId { get; set; }
        public School Schools { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int ClassId { get; set; }
        public int SubjectsId { get; set; }
        public Subjects Subjects { get; set; }
        public Class Class { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public int Score3 { get; set; }
        public string Term { get; set; }
        public string Session { get; set; }
    }
}
