using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask_Core3.Classes;
using TestTask_Core3.DTOs;
using TestTask_Core3.Entities;

namespace TestTask_Core3.Repository
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        Task<GroupDTO[]> GetGroups(GroupParametersFilter groupParametersFilter);
        Task<GroupDTO> GetGroup(Guid groupId);
        Task<Group> GetGroupById(Guid groupId);
        void CreateGroup(Group group);
        void UpdateGroup(Group dbGroup, Group group);
        void DeleteGroup(Group group);

        void AddStudentToGroup(Group group, Guid studentId);
        void RemoveStudentFromGroup(Group group, Guid studentId);
    }
}
