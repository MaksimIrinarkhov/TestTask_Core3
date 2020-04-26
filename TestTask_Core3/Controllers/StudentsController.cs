using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using TestTask_Core3.Classes;
using TestTask_Core3.DTOs;
using TestTask_Core3.Entities;
using TestTask_Core3.Repository;

namespace TestTask_Core3.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> logger;
        private readonly IStudentRepository studentRepository;
        public StudentsController(IStudentRepository studentRepository, ILogger<StudentsController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        // GET: api/Students
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] StudentParametersFilter studentParametersFilter)
        {
            try
            {
                var students = await studentRepository.GetStudents(studentParametersFilter);
                logger.LogInformation($"Returned {students.Length} students from database", "GetStudents");
                return Ok(students);
            }
            catch
            {
                var message = "Error has happened during receiving students data from the database";
                logger.LogError(message, "GetStudents");
                return BadRequest(message);
            }

        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(Guid id)
        {
            try
            {
                var student = await studentRepository.GetStudent(id);
                if (student != null)
                {
                    logger.LogInformation($"Returned student with {id} from database", "GetStudent");
                    return Ok(student);
                }
                else
                {
                    logger.LogError($"Null object returned for {id}", "GetStudent");
                    return NotFound();
                }
            }
            catch
            {
                var message = "Error has happened during receiving student data from the database";
                logger.LogError(message, "GetStudent", id);
                return BadRequest(message);
            }
        }

        // POST: api/Students
        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student student)
        {
            var FIO = string.Empty;
            var sex = string.Empty;
            var uniqueId = string.Empty;
            if (student == null)
            {
                var message = "Student object couldn't be null";
                logger.LogError(message, "CreateStudent");
                return BadRequest(message);
            }
            else
            {
                FIO = $"{student.LastName} {student.FirstName} {student.MiddleName}";
                sex = student.SexStr;
                uniqueId = student.UniqueId;
            }

            try
            {
                var message = "New student has been created";
                studentRepository.CreateStudent(student);
                logger.LogInformation(message, "CreateStudent", student?.Id ?? Guid.Empty, FIO, sex, uniqueId);
                /////////////???????????????????????????????????????
                return CreatedAtRoute("", new { id = student.Id }, student);
            }
            catch
            {
                var message = "Error has happened during creating student";
                logger.LogError(message, "CreateStudent", student?.Id ?? Guid.Empty, FIO, sex, uniqueId);
                return BadRequest(message);
            }
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] Student student)
        {
            var FIO = string.Empty;
            var sex = string.Empty;
            var uniqueId = string.Empty;
            if (student == null)
            {
                var message = "Student object couldn't be null";
                logger.LogError(message, "UpdateStudent");
                return BadRequest(message);
            }
            else
            {
                FIO = $"{student.LastName} {student.FirstName} {student.MiddleName}";
                sex = student.SexStr;
                uniqueId = student.UniqueId;
            }

            if (id == Guid.Empty)
            {
                var message = "Student Id couldn't be empty";
                logger.LogError(message, "UpdateStudent");
                return BadRequest(message);
            }

            try
            {
                var dbStudent = await studentRepository.GetStudentById(id);
                if (dbStudent == null)
                {
                    logger.LogError($"Student with id {id}, hasn't been found in the database.", "UpdateStudent");
                    return NotFound();
                }
                studentRepository.UpdateStudent(dbStudent, student);
                logger.LogInformation($"Student with id {id} was successufully updated", "UpdateStudent", id, student?.Id ?? Guid.Empty, FIO, sex, uniqueId);
                return NoContent();
            }
            catch
            {
                var message = "Error has happened during updating student";
                logger.LogError(message, "UpdateStudent", id, student?.Id ?? Guid.Empty, FIO, sex, uniqueId);
                return BadRequest(message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var dbStudent = await studentRepository.GetStudentById(id);
                if (dbStudent == null)
                {
                    logger.LogError($"Student with id {id} hasn't been found in the database.", "DeleteStudent");
                    return NotFound();
                }
                studentRepository.DeleteStudent(dbStudent);
                logger.LogInformation($"Student with id {id} was successfully deleted", "DeleteStudent", id);
                return NoContent();
            }
            catch
            {
                var message = "Error has happened during removing student";
                logger.LogError(message, "DeleteStudent", id);
                return BadRequest(message);
            }
        }
    }
}
