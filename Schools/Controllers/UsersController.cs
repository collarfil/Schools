using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Schools.Models;
using System.Data;

namespace Schools.Controllers
{
    public class UsersController : Controller
    {
        public IConfiguration Configuration { get; }
        public UsersController(IConfiguration configuration)
        {
            Configuration= configuration;
        }
        public IActionResult Index()
        {
            List<Users> users = new List<Users>();
            using(SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();   
                using (SqlCommand cmd = new SqlCommand("sp_GetUsers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    foreach(DataRow row in dt.Rows)
                    {
                        users.Add(new Users
                        {
                            UsersId = Convert.ToInt32(row["UsersId"]),
                            SchoolsId = Convert.ToInt32(row["SchoolsId"]),
                            FullName = row["FullName"].ToString(),
                            Address = row["Address"].ToString(),
                            Phone = row["Phone"].ToString()
                        });
                    }
                    
                }
               
            }
            return View(users);
        }

        public IActionResult CreateUsers()
        {
            ViewBag.School = GetSchool();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUsers(Users users)
        {
            if (!ModelState.IsValid)
            {
                using(SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertUsers",con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SchoolsId", users.SchoolsId);
                        cmd.Parameters.AddWithValue("@FullName", users.FullName);
                        cmd.Parameters.AddWithValue("@Address", users.Address);
                        cmd.Parameters.AddWithValue("@Phone", users.Phone);
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult EditUsers(int? id)
        {
            ViewBag.School = GetSchool();
            Users users = new Users();

            using(SqlConnection con=new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using(SqlCommand cmd =new SqlCommand("sp_GetUsersId",con))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsersId", id);

                    using(SqlDataReader dr= cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            users.SchoolsId = Convert.ToInt32(dr["SchoolsId"]);
                            users.FullName = Convert.ToString(dr["FullName"]);
                            users.Address = Convert.ToString(dr["Address"]);
                            users.Phone = Convert.ToString(dr["Phone"]);
                           
                        }
                    }
                    con.Close();
                }
            }
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUsers(Users users, int? id)
        {
            if (!ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUsers", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UsersId", id);
                        cmd.Parameters.AddWithValue("@SchoolsId", users.SchoolsId);
                        cmd.Parameters.AddWithValue("@FullName", users.FullName);
                        cmd.Parameters.AddWithValue("@Address", users.Address);
                        cmd.Parameters.AddWithValue("@Phone", users.Phone);
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteUsers()
        {
            return View();
        }
        public IActionResult Roles()
        {
            List<Roles> roles = new List<Roles>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("sp_GetRoles",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    foreach(DataRow row in dt.Rows)
                    {
                        roles.Add(new Roles
                        {
                            RolesId = Convert.ToInt32(row["RolesId"]),
                            RolesName = row["RolesName"].ToString(),
                            Username = row["Username"].ToString(),
                            Password = row["Password"].ToString(),
                            UsersId = Convert.ToInt32(row["UsersId"])
                        });
                    }

                }
            }
                return View(roles);
        }

        public IActionResult CreateRoles()
        {
            ViewBag.Users = GetUsers();
;            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRoles(Roles roles)
        {
            if (!ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertRoles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RolesName", roles.RolesName);
                        cmd.Parameters.AddWithValue("@Username", roles.Username);
                        cmd.Parameters.AddWithValue("@Password", roles.Password);
                        cmd.Parameters.AddWithValue("@UsersId", roles.UsersId);
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            return RedirectToAction("Roles");
        }
        public IActionResult EditRoles(int? id)
        {
            ViewBag.Users = GetUsers();
            Roles roles = new Roles();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetRolesId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RolesId", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            roles.RolesId = Convert.ToInt32(dr["RolesId"]);
                            roles.RolesName = Convert.ToString(dr["RolesName"]);
                            roles.Username = Convert.ToString(dr["Username"]);
                            roles.Password = Convert.ToString(dr["Password"]);
                            roles.UsersId = Convert.ToInt32(dr["UsersId"]);
                            con.Close();
                        }
                    }

                }
            }
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoles(Roles roles, int? id)
        {
            if (!ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateRoles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RolesId", id);
                        cmd.Parameters.AddWithValue("@RolesName", roles.RolesName);
                        cmd.Parameters.AddWithValue("@Username", roles.Username);
                        cmd.Parameters.AddWithValue("@Password", roles.Password);
                        cmd.Parameters.AddWithValue("@UsersId", roles.UsersId);
                        con.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            return View();
        }
        public IActionResult DeleteRoles(Roles roles, int? id)
        {
            return View();
        }

        public IActionResult AssignClass()
        {
            return View();
        }

        public IActionResult CreateAssignClass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAssignClass(AssignClass roles)
        {
            return View();
        }
        public IActionResult EditAssignClass(int? id)
        {
            return View();
        }
        public IActionResult EditAssignClass(AssignClass roles, int? id)
        {
            return View();
        }
        public IActionResult DeleteAssignClass(int? id)
        {
            return View();
        }

        private List<School> GetSchool()
        {
            var schools = new List<School>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (var command = new SqlCommand("SELECT * FROM Schools", con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schools.Add(new School
                            {
                                SchoolsId = Convert.ToInt32(reader["SchoolsId"]),
                                SchoolsName = reader["SchoolsName"].ToString()
                            });
                        }
                    }
                }
            }
            return schools;
        }

        private List<Users> GetUsers()
        {
            var users = new List<Users>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (var command = new SqlCommand("SELECT * FROM Users", con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new Users
                            {
                                UsersId = Convert.ToInt32(reader["UsersId"]),
                                FullName = reader["FullName"].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}
