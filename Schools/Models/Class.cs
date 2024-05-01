namespace Schools.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public string ClassName { get; set; }
    }
}
