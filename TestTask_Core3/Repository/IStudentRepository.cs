using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask_Core3.Classes;
using TestTask_Core3.DTOs;
using TestTask_Core3.Entities;

namespace TestTask_Core3.Repository
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
        Task<StudentDTO[]> GetStudents(StudentParametersFilter studentParametersFilter);
        Task<StudentDTO> GetStudent(Guid studentId);
        Task<Student> GetStudentById(Guid studentId);
        void CreateStudent(Student student);
        void UpdateStudent(Student dbStudent, Student student);
        void DeleteStudent(Student student);
    }
}
