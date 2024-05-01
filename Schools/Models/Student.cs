namespace Schools.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int SchoolsId { get; set; }
        public School Schools { get; set; }
        public string StudentNo { get; set; }
        public string FullName { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public string Phone { get; set; }
        public string ParentName { get; set; }
        public string ParentPhone { get; set; }
    }
}
