using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Schools.Models;
using System.Data;
using static System.Collections.Specialized.BitVector32;
namespace Schools.Controllers
{
    public class StudentController : Controller
    {
        public IConfiguration Configuration { get; }
        public StudentController(IConfiguration configuration)
        {
            Configuration= configuration;
        }
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();
            try
            {
                using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
                {
                    con.Open();

                    using(SqlCommand cmd = new SqlCommand("sp_GetStudent",con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr= cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(dr);

                        foreach(DataRow row in dt.Rows)
                        {
                            students.Add(new Student
                            {
                                StudentId = Convert.ToInt32(row["StudentId"]),
                                SchoolsId = Convert.ToInt32(row["SchoolsId"]),
                                StudentNo = row["StudentNo"].ToString(),
                                FullName = row["FullName"].ToString(),
                                ClassId = Convert.ToInt32(row["ClassId"]),
                                Phone = row["Phone"].ToString(),
                                ParentName = row["ParentName"].ToString(),
                                ParentPhone= row["ParentPhone"].ToString()
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            
            return View(students);
        }
        public IActionResult CreateStudent()
        {
            ViewBag.School = GetSchool();
            ViewBag.Class = GetClass();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudent(Student students)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_InsertStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SchoolsId", students.SchoolsId);
                    cmd.Parameters.AddWithValue("@StudentNo", students.StudentNo);
                    cmd.Parameters.AddWithValue("@FullName", students.FullName);
                    cmd.Parameters.AddWithValue("@ClassId", students.ClassId);
                    cmd.Parameters.AddWithValue("@Phone", students.Phone);
                    cmd.Parameters.AddWithValue("@ParentName", students.ParentName);
                    cmd.Parameters.AddWithValue("@ParentPhone", students.ParentPhone);
                    cmd.ExecuteNonQuery();
                    return View();
                }
            }
        }

        public IActionResult EditStudent(int? id)
        {
            ViewBag.School = GetSchool();
            ViewBag.Class = GetClass();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStudent(Student students,int?id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteStudent(Student students, int? id)
        {
            return View();
        }
        private List<Class> GetClass()
        {
            var Class = new List<Class>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (var command = new SqlCommand("SELECT * FROM Class", con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Class.Add(new Class
                            {
                                ClassId = Convert.ToInt32(reader["ClassId"]),
                                ClassName = reader["ClassName"].ToString()
                            });
                        }
                    }
                }
            }
            return Class;
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
    }
}
