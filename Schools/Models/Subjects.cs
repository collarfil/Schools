namespace Schools.Models
{
    public class Subjects
    {
        public int SubjectsId { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public string SubjectsName { get; set; }
    }
}
