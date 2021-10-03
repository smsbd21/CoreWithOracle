using OraCoreCrud.Models;
using System.Collections.Generic;

namespace OraCoreCrud.Interface
{
    public interface IStudentService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentById(int id);
        void AddStudent(Student stud);
        void EditStudent(Student stud);
        void DeleteStudent(Student stud);
        Student GetStudentDataById(int id);
    }
}
