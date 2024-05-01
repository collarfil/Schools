namespace Schools.Models
{
    public class AssignClass
    {
        public int AssignClassId { get; set; }
        public int UsersId { get; set; }
        public Users Users { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
