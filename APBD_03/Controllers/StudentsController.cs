using APBD_03.Models;
using APBD_03.Helpers;
using APBD_03.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace APBD_03.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly string csvPath = @"Data/data.csv";
        public List<Student> studentsList = new List<Student>();

    [HttpGet]
    public IActionResult GetStudents()
        {
            try
            {
                studentsList = DataHelper.GetStudentList(csvPath);
                return Ok(studentsList);
            }catch (Exception ex)
            {
                return BadRequest("Nie znaleziono pliku");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetStudents(string id)
        {
            try
            {
                studentsList = DataHelper.GetStudentList(csvPath);
                foreach (var student in studentsList)
                {
                    if(student.IndexNumber == id)
                    {
                        return Ok(student);
                    }
                }
                return NotFound("Nie znaleziono studenta");
            }
            catch (Exception ex)
            {
                return BadRequest("Nie znaleziono pliku");
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudents(string id, Student updStudent)
        {
            if(updStudent.FirstName == null || updStudent.LastName == null || updStudent.IndexNumber == null 
                || updStudent.BirthDate == null || updStudent.Studies == null || updStudent.Mode == null 
                || updStudent.Email == null || updStudent.FathersName == null || updStudent.MothersName == null)
            {
                return BadRequest("Niekompletne dane");
            }
            try
            {
                studentsList = DataHelper.GetStudentList(csvPath);
                bool change = false;
                foreach (Student stud in studentsList)
                {
                    if (Regex.IsMatch(stud.IndexNumber, id))
                    {
                        if (!Regex.IsMatch(updStudent.IndexNumber, id))
                        {
                            return BadRequest("B³êdna operacja!");
                        }
                        DataHelper.UpdateStudent(stud, updStudent);
                    }
                    change = true;
                    break;
                }
                if (change == false)
                {
                    return NotFound("Nieistniej¹ce ID");
                }
                DataHelper.UpdateFile(csvPath, studentsList);
                return Ok(updStudent);
            }
            catch (Exception ex)
            {
                return BadRequest("Nie znaleziono pliku");
            }
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] Student updStudent)
        {
            if (updStudent.FirstName == null || updStudent.LastName == null || updStudent.IndexNumber == null
                || updStudent.BirthDate == null || updStudent.Studies == null || updStudent.Mode == null
                || updStudent.Email == null || updStudent.FathersName == null || updStudent.MothersName == null)
            {
                return BadRequest("Niekompletne dane");
            }
            if (!Regex.Match(updStudent.IndexNumber, @"^s\d+$").Success)
            {
                return BadRequest("Podano niepoprawny indeks");
            }
            try
            {
                studentsList = DataHelper.GetStudentList(csvPath);
                foreach (var student in studentsList)
                {
                    if (Regex.IsMatch(student.IndexNumber, updStudent.IndexNumber)) {
                        return BadRequest("Ten indeks ju¿ istnieje w bazie");
                    }
                    
                }
                studentsList.Add(updStudent);
                DataHelper.UpdateFile(csvPath, studentsList);
                return Ok(updStudent);
            }

            catch (Exception ex)
            {
                return BadRequest("Nie znaleziono pliku");
            }
        }
        
        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber) {
            studentsList = DataHelper.GetStudentList(csvPath);
            try
            {
                foreach (var student in studentsList)
                {
                    if (Regex.IsMatch(student.IndexNumber, indexNumber))
                    {
                        if (student == null)
                        {
                            return NotFound("Nie znaleziono studenta");
                        }
                        studentsList.Remove(student);
                        return Ok(indexNumber);
                    }
                    
                }
                return BadRequest("Podany numer nie istnieje");
            }
            catch (Exception ex)
            {
                return BadRequest("Nie znaleziono pliku");
            }

        }
    }

}
