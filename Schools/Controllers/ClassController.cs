using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using Schools.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NuGet.Protocol.Plugins;
using System.Data.Common;

namespace Schools.Controllers
{
    public class ClassController : Controller
    {
        public IConfiguration Configuration { get; }
        public ClassController(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }
        public IActionResult Index()
        {
            List<Class> grade = new List<Class>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    foreach(DataRow row in dt.Rows)
                    {
                        grade.Add(new Class
                        {
                            ClassId = Convert.ToInt32(row["ClassId"]),
                            ClassName = row["ClassName"].ToString()
                        });
                       
                    }    

                }
            }
            return View(grade);
        }
        public IActionResult CreateClass()
        {
            ViewBag.Section = GetSection();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateClass(Class grade)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("sp_InsertClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", grade.SectionId);
                    cmd.Parameters.AddWithValue("@ClassName", grade.ClassName);
                    cmd.ExecuteNonQuery();
                }
            }
                return RedirectToAction("Index");
        }

        public IActionResult EditClass(int? id)
        {
            Class classname=new Class();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetClass", con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", id);

                    using (SqlDataReader dataReader =  cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            classname.ClassId = Convert.ToInt32(dataReader["ClassId"]);
                            classname.SectionId = Convert.ToInt32(dataReader["ClassId"]);
                            classname.ClassName = Convert.ToString(dataReader["ClassName"]);
                            con.Close();
                        }
                    }
                }
            }
               
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClass(Class grade, int? id)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdateClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", id);
                    cmd.Parameters.AddWithValue("@SectionId", grade.SectionId);
                    cmd.Parameters.AddWithValue("@ClassName", grade.ClassName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteClass(int id)
        {
        using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
        {
            con.Open();
                
                using (SqlCommand cmd= new SqlCommand("sp_DeleteClass", con))
                {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassId", id);
                cmd.ExecuteNonQuery();
                con.Close();
                }
            }

            return RedirectToAction("Index");
        }

        private List<Section> GetSection()
        {
            var section = new List<Section>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (var command = new SqlCommand("SELECT * FROM Section", con))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            section.Add(new Section
                            {
                                SectionId = Convert.ToInt32(reader["SectionId"]),
                                SectionName = reader["SectionName"].ToString()
                            });
                        }
                    }
                }
            }
            return section;
        }

        public IActionResult Section()
        {
            List<Section> section = new List<Section>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetSection", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    foreach (DataRow row in dt.Rows)
                    {
                        section.Add(new Section
                        {
                            SectionId = Convert.ToInt32(row["SectionId"]),
                            SectionName = row["SectionName"].ToString()

                        });
                    }

                }
            }
            return View(section);
        }
        public IActionResult CreateSection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSection(Section section)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertSection", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@SectionName", section.SectionName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Section");
        }

        public IActionResult EditSection(int? id)
        {
           Section section = new Section();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetSectionId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", id);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            section.SectionId = Convert.ToInt32(dataReader["SectionId"]);
                            section.SectionName = Convert.ToString(dataReader["SectionName"]);
                            
                        }
                    }
                    con.Close();
                }
            }

            return View(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSection(Section section, int? id)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_UpdateSection", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", id);
                    cmd.Parameters.AddWithValue("@SectionName", section.SectionName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Section");
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_DeleteClass", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SectionId", id);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return RedirectToAction("Section");
        }

        public IActionResult Subjects()
        {
            List<Subjects> subjects = new List<Subjects>();
            using(SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using(SqlCommand cmd = new SqlCommand("sp_GetSubjects",con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    SqlDataReader dr= cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    foreach(DataRow row in dt.Rows)
                    {
                        subjects.Add(new Subjects
                        {
                            SubjectsId = Convert.ToInt32(row["SubjectsId"]),
                            ClassId= Convert.ToInt32(row["ClassId"]),
                            SubjectsName = row["SubjectsName"].ToString()

                        });
                    }
                }
            }
            return View(subjects);
        }

        public IActionResult CreateSubjects()
        {
            ViewBag.Class = GetClass();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubjects(Subjects subjects)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_InsertSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClassId", subjects.ClassId);
                    cmd.Parameters.AddWithValue("@SubjectsName", subjects.SubjectsName);
                    cmd.ExecuteNonQuery();
                }
            }
                    return RedirectToAction("Subjects");
        }
        public IActionResult EditSubjects(int? id)
        {
            ViewBag.Class = GetClass();
            Subjects subjects = new Subjects();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_InsertSubjects", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectsId", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        subjects.SubjectsId = Convert.ToInt32(dr["Id"]);
                       subjects.SubjectsName = Convert.ToString(dr["Name"]);
                        
                    }
                    con.Close();
                }

            }                
            return View(subjects);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSubjects(Subjects subjects, int? id)
        {
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_UpdateSubjects", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubjectsId", id);
                    cmd.Parameters.AddWithValue("@ClassId", subjects.ClassId);
                    cmd.Parameters.AddWithValue("@SubjectsName", subjects.SubjectsName);
                    cmd.ExecuteNonQuery();
                }
            }
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
        public IActionResult DeleteSubjects(int? id)
        {
            return View();
        }
    }
}
