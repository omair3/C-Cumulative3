using Microsoft.AspNetCore.Mvc;
using C__Cumulative_Part_1.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Cumulative1.Model;

namespace C__Cumulative_Part_1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        // Dependency injection of the SchoolDbContext
        private readonly SchoolDbContext _context;

        // Constructor that initializes the database connection
        public TeacherAPIController()
        {
            _context = new SchoolDbContext();
        }

        /// <summary>
        /// Returns a list of teacher names in the system
        /// </summary>
        /// <returns>A list of strings</returns>
        /// <example>
        /// GET api/Teacher/ListTeacherNames -> ["John Smith", "Jane Doe"]
        /// </example>
        [HttpGet]
        [Route("ListTeacherNames")]
        public List<string> ListTeacherNames()
        {
            List<string> TeacherNames = new List<string>();

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                string query = "SELECT teacherfname, teacherlname FROM teachers";
                cmd.CommandText = query;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fname = reader["teacherfname"].ToString();
                        string lname = reader["teacherlname"].ToString();
                        string fullName = $"{fname} {lname}";
                        TeacherNames.Add(fullName);
                    }
                }
            }

            return TeacherNames;
        }

        /// <summary>
        /// Returns full information about a specific teacher by ID
        /// </summary>
        /// <param name="id">The teacher's ID</param>
        /// <returns>A Teacher object with full information</returns>
        /// <example>
        /// GET api/Teacher/FindTeacher/2
        /// </example>
        [HttpGet]
        [Route("FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Extra Iniative part  to return null if teacher is not found 
            Teacher SelectedTeacher = null;

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SelectedTeacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFirstName = reader["teacherfname"].ToString(),
                            TeacherLastName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDouble(reader["salary"])
                        };
                    }
                }
            }

            return SelectedTeacher;
        }

        /// <summary>
        /// Returns a list of all teachers with full information
        /// </summary>
        /// <returns>A list of Teacher objects</returns>
        /// <example>
        /// GET api/Teacher/ListTeacherRecords
        /// </example>
        [HttpGet]
        [Route("ListTeacherRecords")]
        public List<Teacher> ListTeacherRecords()
        {
            List<Teacher> Teachers = new List<Teacher>();

            using (MySqlConnection conn = _context.AccessDatabase())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM teachers";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher t = new Teacher
                        {
                            TeacherId = Convert.ToInt32(reader["teacherid"]),
                            TeacherFirstName = reader["teacherfname"].ToString(),
                            TeacherLastName = reader["teacherlname"].ToString(),
                            EmployeeNumber = reader["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(reader["hiredate"]),
                            Salary = Convert.ToDouble(reader["salary"])
                        };

                        Teachers.Add(t);
                    }
                }
            }

            return Teachers;
        }
        /// <summary>
        /// Updates a Teacher in the database. Data is sent as a Teacher object, and the request URL contains the Teacher ID.
        /// </summary>
        /// <param name="TeacherData">The updated Teacher object</param>
        /// <param name="id">The Teacher ID primary key</param>
        /// <example>
        /// PUT: api/Teacher/UpdateTeacher/4  
        /// Headers: Content-Type: application/json  
        /// Request Body:
        /// {
        ///     "TeacherFirstName": "Alex",
        ///     "TeacherLastName": "Johnson",
        ///     "EmployeeNumber": "T456",
        ///     "HireDate": "2020-01-15",
        ///     "Salary": 72000
        /// }
        /// -> 
        /// {
        ///     "TeacherId": 4,
        ///     "TeacherFirstName": "Alex",
        ///     "TeacherLastName": "Johnson",
        ///     "EmployeeNumber": "T456",
        ///     "HireDate": "2020-01-15T00:00:00",
        ///     "Salary": 72000
        /// }
        /// </example>
        /// <returns>
        /// The updated Teacher object
        /// </returns>

        [HttpPut(template: "UpdateTeacher/{TeacherId}")]
        public Teacher UpdateTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, hiredate=@hiredate, salary=@salary where teacherid=@id";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFirstName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLastName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);

                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.Parameters.AddWithValue("@id", TeacherId);


             

                Command.ExecuteNonQuery();



            }

            return FindTeacher(TeacherId);
        }
    }
}