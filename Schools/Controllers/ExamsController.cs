using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using Schools.Models;

using System.Configuration;
using static Schools.Models.ExamViewModel;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Schools.Controllers
{
    public class ExamsController : Controller
    {
        public IConfiguration Configuration { get; }
        public ExamsController(IConfiguration connfiguration)
        {
            Configuration = connfiguration;
        }
        public IActionResult Index()
        {
            List<Exams> exams= new List<Exams>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetSchools", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    foreach (DataRow row in dt.Rows)
                    {
                        exams.Add(new Exams
                        {
                            ExamsId = Convert.ToInt32(row["ExamsId"]),
                            SchoolsId = Convert.ToInt32(row["SchoolsId"]),
                            StudentId = Convert.ToInt32(row["StudentId"]),
                            ClassId = Convert.ToInt32(row["ClassId"]),
                            SubjectsId = Convert.ToInt32(row["SubjectsId"]),
                            Score1 = Convert.ToInt32(row["Score1"]),
                            Score2 = Convert.ToInt32(row["Score2"]),
                            Score3 = Convert.ToInt32(row["Score3"]),
                            Term = row["Term"].ToString(),
                            Session = row["Session"].ToString()



                        });
                    }

                }
            }
            return View(exams);
        }

        public IActionResult CreateExams(int? classId)
        {
            ViewBag.ClassId = classId;
            var students = GetStudent() ?? Enumerable.Empty<Student>();
            var classes = GetClasses() ?? Enumerable.Empty<Class>();
            var subjects = GetSubjects() ?? Enumerable.Empty<Subjects>();
            return View(new ExamGridViewModel
            {
                Students = students,
                Classes = classes,
                Subjects = subjects
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateExams(ExamGridViewModel model)
        {
            using(SqlConnection con =new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                foreach(var exams in model.Exams)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertExams", con))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
                        cmd.Parameters.AddWithValue("@SchoolsId", exams.SchoolsId);
                        cmd.Parameters.AddWithValue("@StudentId", exams.StudentId);
                        cmd.Parameters.AddWithValue("@ClassId", exams.ClassId);
                        cmd.Parameters.AddWithValue("@SubjectsId", exams.SubjectsId);
                        cmd.Parameters.AddWithValue("@Score1", exams.Score2);
                        cmd.Parameters.AddWithValue("@Score2", exams.Score2);
                        cmd.Parameters.AddWithValue("@Score3", exams.Score3);
                        cmd.Parameters.AddWithValue("@Term", exams.Term);
                        cmd.Parameters.AddWithValue("@Session", exams.Session);
                        cmd.ExecuteNonQuery();
                    }
                }
                    
                          
                                
            }    
            return Json(new { success = true }); 
        }

        public IActionResult EditExams(int? id)
        {
            return View();
        }

        public IActionResult EditExams(Exams exams, int? id)
        {
            return View();
        }

        public IActionResult DeleteExams(int? id)
        {
            return View();
        }
        private IEnumerable<Student> GetStudent()
        {
           
            var students = new List<Student>();

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                string query = "SELECT StudentId, FullName FROM Student";
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = (int)reader["StudentId"],
                                FullName = reader["FullName"].ToString()
                            });
                        }
                    }
                }
            }

            return students;
        }

        private IEnumerable<Class> GetClasses()
        {
           
            var classes = new List<Class>();

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                string query = "SELECT ClassId, ClassName FROM Class";
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(new Class
                            {
                                ClassId = (int)reader["ClassId"],
                                ClassName = reader["ClassName"].ToString()
                            });
                        }
                    }
                }
            }

            return classes;
        }

        private IEnumerable<Subjects> GetSubjects()
        {
           
            var subjects = new List<Subjects>();

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                string query = "SELECT SubjectsId, SubjectsName FROM Subjects";
                using (var command = new SqlCommand(query, con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subjects
                            {
                                SubjectsId = (int)reader["SubjectsId"],
                                SubjectsName = reader["SubjectsName"].ToString()
                            });
                        }
                    }
                }
            }

            return subjects;
        }
    }
   
    
}
