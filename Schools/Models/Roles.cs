namespace Schools.Models
{
    public class Roles
    {
        public int RolesId { get; set; }
        public string RolesName { get; set; }
        public int UsersId { get; set; }
        public Users Users { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
