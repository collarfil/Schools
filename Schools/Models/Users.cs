namespace Schools.Models
{
    public class Users
    {
        public int UsersId { get; set; }
        public int SchoolsId { get; set; }
        public School Schools { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
