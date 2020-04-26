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
    //[Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ILogger<GroupsController> logger;
        private readonly IGroupRepository groupRepository;
        public GroupsController(IGroupRepository groupRepository, ILogger<GroupsController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<IActionResult> GetGroups([FromQuery] GroupParametersFilter groupParametersFilter)
        {
            try
            {
                var groups = await groupRepository.GetGroups(groupParametersFilter);
                logger.LogInformation($"Returned {groups.Length} groups from database", "GetGroups");
                return Ok(groups);
            }
            catch
            {
                var message = "Error has happened during receiving groups from the database";
                logger.LogError(message, "GetGroups");
                return BadRequest(message);
            }

        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            try
            {
                var group = await groupRepository.GetGroup(id);
                if (group != null)
                {
                    logger.LogInformation($"Returned group with {id} from database", "GetGroup");
                    return Ok(group);
                }
                else
                {
                    logger.LogError($"Null object returned for {id}", "GetGroup");
                    return NotFound();
                }
            }
            catch
            {
                var message = "Error has happened during receiving group data from the database";
                logger.LogError(message, "GetGroup", id);
                return BadRequest(message);
            }
        }

        // POST: api/Groups
        [HttpPost]
        public IActionResult CreateGroup([FromBody] Group group)
        {
            var name = string.Empty;
            if (group == null)
            {
                var message = "Group object couldn't be null";
                logger.LogError(message, "CreateGroup");
                return BadRequest(message);
            }
            else
            {
                name = group.Name;
            }

            try
            {
                var message = "New group has been created";
                groupRepository.CreateGroup(group);
                logger.LogInformation(message, "CreateGroup", group?.Id ?? Guid.Empty, name);
                /////////////???????????????????????????????????????
                return CreatedAtRoute("", new { id = group.Id }, group);
            }
            catch
            {
                var message = "Error has happened during creating group";
                logger.LogError(message, "CreateGroup", group?.Id ?? Guid.Empty, name);
                return BadRequest(message);
            }
        }

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] Group group)
        {
            var name = string.Empty;
            if (group == null)
            {
                var message = "Group object couldn't be null";
                logger.LogError(message, "UpdateGroup");
                return BadRequest(message);
            }
            else
            {
                name = group.Name;
            }

            if (id == Guid.Empty)
            {
                var message = "Group Id couldn't be empty";
                logger.LogError(message, "UpdateGroup");
                return BadRequest(message);
            }

            try
            {
                var dbGroup = await groupRepository.GetGroupById(id);
                if (dbGroup == null)
                {
                    logger.LogError($"Group with id {id}, hasn't been found in the database.", "UpdateGroup");
                    return NotFound();
                }
                groupRepository.UpdateGroup(dbGroup, group);
                logger.LogInformation($"Group with id {id} was successufully updated", "UpdateGroup", id, group?.Id ?? Guid.Empty, name);
                return NoContent();
            }
            catch
            {
                var message = "Error has happened during updating group";
                logger.LogError(message, "UpdateGroup", id, group?.Id ?? Guid.Empty, name);
                return BadRequest(message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            try
            {
                var dbGroup = await groupRepository.GetGroupById(id);
                if (dbGroup == null)
                {
                    logger.LogError($"Group with id {id} hasn't been found in the database.", "DeleteGroup");
                    return NotFound();
                }
                groupRepository.DeleteGroup(dbGroup);
                logger.LogInformation($"Group with id {id} was successfully deleted", "DeleteGroup", id);
                return NoContent();
            }
            catch
            {
                var message = "Error has happened during removing group";
                logger.LogError(message, "DeleteGroup", id);
                return BadRequest(message);
            }
        }

        [HttpPost("AddStudent/{groupId}/{studentId}")]
        public async Task<IActionResult> AddStudentToGroup(Guid groupId, Guid studentId)
        {
            if (groupId == Guid.Empty)
            {
                var message = "Group Id couldn't be empty";
                logger.LogError(message, "AddStudentToGroup");
                return BadRequest(message);
            }
            if (studentId == Guid.Empty)
            {
                var message = "Student Id couldn't be empty";
                logger.LogError(message, "AddStudentToGroup");
                return BadRequest(message);
            }

            try
            {
                var dbGroup = await groupRepository.GetGroupById(groupId);

                if (dbGroup == null)
                {
                    var message = $"Group with id: {groupId} wasn't be found in the database";
                    logger.LogError(message, "AddStudentToGroup");
                    return BadRequest(message);
                }

                groupRepository.AddStudentToGroup(dbGroup, studentId);

                logger.LogInformation($"Student with id {studentId} was successfully added to group {groupId}", "AddStudentToGroup", $"groupId: {groupId}", $"studentId: {studentId}");
                return NoContent();
            }
            catch
            {
                var message = $"Error has happened during adding student with id: {studentId} to group {groupId}";
                logger.LogError(message, "AddStudentToGroup", $"groupId: {groupId}", $"studentId: {studentId}");
                return BadRequest(message);
            }
        }

        [HttpDelete("RemoveStudent/{groupId}/{studentId}")]
        public async Task<IActionResult> RemoveStudentFromGroup(Guid groupId, Guid studentId)
        {
            if (groupId == Guid.Empty)
            {
                var message = "Group Id couldn't be empty";
                logger.LogError(message, "RemoveStudentFromGroup");
                return BadRequest(message);
            }
            if (studentId == Guid.Empty)
            {
                var message = "Student Id couldn't be empty";
                logger.LogError(message, "RemoveStudentFromGroup");
                return BadRequest(message);
            }

            try
            {
                var dbGroup = await groupRepository.GetGroupById(groupId);
                groupRepository.RemoveStudentFromGroup(dbGroup, studentId);

                logger.LogInformation($"Student with id {studentId} was successfully removed from group {groupId}", "RemoveStudentFromGroup", 
                    $"groupId: {groupId}", $"studentId: {studentId}");
                return NoContent();
            }
            catch
            {
                var message = $"Error has happened during removing student with id: {studentId} to group {groupId}";
                logger.LogError(message, "RemoveStudentFromGroup", $"groupId: {groupId}", $"studentId: {studentId}");
                return BadRequest(message);
            }
        }
    }
}
