using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using Schools.Models;
using Microsoft.Data.SqlClient;
using System.IO;
using NuGet.Packaging.Signing;


namespace Schools.Controllers
{
    public class SchoolController : Controller
    {
        
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Webhost { get; }
        public SchoolController (IConfiguration configuration, IWebHostEnvironment webHost)
        {
            Configuration = configuration;
            Webhost = webHost;
        }
        
                
        public IActionResult Index()
        {
            
           List<School> schools = new List<School>();
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetSchools", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    foreach(DataRow row in dt.Rows)
                    {
                        schools.Add(new School
                        {
                            SchoolsId = Convert.ToInt32(row["SchoolsId"]),
                            SchoolsName = row["SchoolsName"].ToString(),
                            Address = row["Address"].ToString(),
                            Phone = row["Phone"].ToString(),
                            Email = row["Email"].ToString(),
                            Logo = row["Logo"].ToString()
                           
                        });
                    }    

                }
            }
            return View(schools);
        }
        

        public IActionResult CreateSchool()
        {
            
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSchool(School school, IFormFile logo)
        {
            
            
            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("DevConnection")))
            {
                con.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertSchools", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                      
                        cmd.Parameters.AddWithValue("@SchoolsName", school.SchoolsName);
                        cmd.Parameters.AddWithValue("@Address", school.Address);
                        cmd.Parameters.AddWithValue("@Phone", school.Phone);
                        cmd.Parameters.AddWithValue("@Email", school.Email);
                        string wwwRootPath = Webhost.WebRootPath;
                        if (logo != null && logo.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString();
                            var uploads = Path.Combine(wwwRootPath, @"images\logo");
                            var extension = Path.GetExtension(logo.FileName);

                            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                            {
                                logo.CopyTo(fileStreams);
                            }
                            school.Logo = @"images\logo" + fileName + extension;
                        }
                        cmd.Parameters.AddWithValue("@Logo", school.Logo);
                        cmd.ExecuteNonQuery();

                    }
                   

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
             }
                return RedirectToAction("Index");
        }
        public IActionResult EditSchool(int? id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSchool(School school, int? id, IFormFile SchoolLogo)
        {
            return View();
        }
    }
}
