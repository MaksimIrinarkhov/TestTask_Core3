using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask_Core3.Classes;
using TestTask_Core3.DTOs;
using TestTask_Core3.Entities;

namespace TestTask_Core3.Repository
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(TestDBContext context) : base(context)
        {
        }

        public Task<StudentDTO[]> GetStudents(StudentParametersFilter studentParametersFilter)
        {
            return FindAll().Where(x => studentParametersFilter == null ||
            (
                (string.IsNullOrEmpty(studentParametersFilter.LastName) || x.LastName == studentParametersFilter.LastName) &&
                (string.IsNullOrEmpty(studentParametersFilter.FirstName) || x.FirstName == studentParametersFilter.FirstName) &&
                (string.IsNullOrEmpty(studentParametersFilter.MiddleName) || x.MiddleName == studentParametersFilter.MiddleName) && 
                (!studentParametersFilter.Sex.HasValue || x.Sex == studentParametersFilter.Sex.Value) &&
                (studentParametersFilter.UniqueId == string.Empty || x.UniqueId == studentParametersFilter.UniqueId) &&
                (string.IsNullOrEmpty(studentParametersFilter.GroupName)|| x.StudentsGroups.Where(y => y.Group.Name == studentParametersFilter.GroupName).Any())
            )).OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Skip((studentParametersFilter.PageNumber - 1) * studentParametersFilter.PageSize)
                .Take(studentParametersFilter.PageSize).Select(x => new StudentDTO
                {
                    Id = x.Id,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    Sex = x.Sex,
                    UniqueId = x.UniqueId,
                    Groups = x.StudentsGroups.Select(y => y.Group.Name)
                }).ToArrayAsync();
        }

        public void CreateStudent(Student student)
        {
            Create(student);
        }

        public void UpdateStudent(Student dbStudent, Student student)
        {
            dbStudent.LastName = student.LastName;
            dbStudent.FirstName = student.FirstName;
            dbStudent.MiddleName = student.MiddleName;
            dbStudent.Sex = student.Sex;
            dbStudent.UniqueId = student.UniqueId;

            Update(dbStudent);

        }

        public void DeleteStudent(Student student)
        {
            Delete(student);
        }

        public Task<Student> GetStudentById(Guid studentId)
        {
            return FindByCondition(student => student.Id == studentId)
                .FirstOrDefaultAsync();
        }

        public Task<StudentDTO> GetStudent(Guid studentId)
        {
            return FindByCondition(student => student.Id == studentId)
                .Select(x => new StudentDTO
                {
                    Id = x.Id,
                    LastName = x.LastName,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    Sex = x.Sex,
                    UniqueId = x.UniqueId,
                    Groups = x.StudentsGroups.Select(y => y.Group.Name)
                }).FirstOrDefaultAsync();
        }
    }
}
