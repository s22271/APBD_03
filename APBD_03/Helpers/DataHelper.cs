namespace APBD_03.Helpers;
using APBD_03.Models;
public class DataHelper
{
    public static List<Student> GetStudentList(string srcPath)
    {
        var studentList = new List<Student>();

        FileInfo fi = new FileInfo(srcPath);
        StreamReader sr = new StreamReader(fi.OpenRead());
        string line;

        while ((line = sr.ReadLine()) != null)
        {
            string[] record = line.Split(",");

            Student student = new Student
            {
                FirstName = record[0],
                LastName = record[1],
                IndexNumber = record[2],
                BirthDate = record[3],
                Studies = record[4],
                Mode = record[5],
                Email = record[6],
                FathersName = record[7],
                MothersName = record[8]
            };

            studentList.Add(student);
        }

        return studentList;
    }
    public static void UpdateFile(string expPath, List<Student> newList)
    {
        StreamWriter sw = File.CreateText(expPath);

        foreach (Student s in newList)
        {
            sw.WriteLine();
        }
    }
    public static void UpdateStudent(Student stud, Student updStudent)
    {
        stud.FirstName = updStudent.FirstName;
        stud.LastName = updStudent.LastName;
        stud.BirthDate = updStudent.BirthDate;
        stud.Studies = updStudent.Studies;
        stud.Mode = updStudent.Mode;
        stud.Email = updStudent.Email;
        stud.FathersName = updStudent.FathersName;
        stud.MothersName = updStudent.MothersName;
    }
}