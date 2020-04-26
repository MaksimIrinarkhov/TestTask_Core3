using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestTask_Core3.Classes;
using TestTask_Core3.DTOs;
using TestTask_Core3.Entities;

namespace TestTask_Core3.Repository
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(TestDBContext context) : base(context)
        { }

        public Task<GroupDTO[]> GetGroups(GroupParametersFilter groupParametersFilter)
        {
            return FindAll().Where(x => groupParametersFilter == null ||
                (string.IsNullOrEmpty(groupParametersFilter.Name) || x.Name == groupParametersFilter.Name))
                .OrderBy(x => x.Name)
                .Skip((groupParametersFilter.PageNumber - 1) * groupParametersFilter.PageSize)
                .Take(groupParametersFilter.PageSize).Select(x => new GroupDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentsCount = x.StudentsGroups.Count
                }).ToArrayAsync();
        }

        public void CreateGroup(Group group)
        {
            Create(group);
        }

        public void UpdateGroup(Group dbGroup, Group group)
        {
            dbGroup.Name = group.Name;

            Update(dbGroup);

        }

        public void DeleteGroup(Group group)
        {
            Delete(group);
        }

        public Task<Group> GetGroupById(Guid groupId)
        {
            return FindByCondition(group => group.Id == groupId).Include(x => x.StudentsGroups)
                .FirstOrDefaultAsync();
        }

        public Task<GroupDTO> GetGroup(Guid groupId)
        {
            return FindByCondition(group => group.Id == groupId)
                .Select(x => new GroupDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    StudentsCount = x.StudentsGroups.Count
                }).FirstOrDefaultAsync();
        }

        public void AddStudentToGroup(Group group, Guid studentId)
        {
            if (!group.StudentsGroups.Select(x => x.StudentId).Contains(studentId))
            {
                group.StudentsGroups.Add(new StudentsGroup {Group = group, StudentId = studentId });
                Save();
            }
        }
        public void RemoveStudentFromGroup(Group group, Guid studentId)
        {
            var studentGroup = group.StudentsGroups.FirstOrDefault(x => x.StudentId == studentId);
            group.StudentsGroups.Remove(studentGroup);
            Save();
        }
    }
}
