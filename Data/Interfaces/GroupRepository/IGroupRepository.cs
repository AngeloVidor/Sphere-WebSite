using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Data.Interfaces.GroupRepository
{
    public interface IGroupRepository
    {
        Task<GroupModel> CreateGroup(GroupModel group);
        Task<GroupModel> GetGroupById(int groupId);
        Task<IEnumerable<GroupModel>> GetAllGroups();
        Task<GroupModel> UpdateGroup(GroupModel group);
        Task DeleteGroup(int groupId);
        Task JoinGroup(int groupId, int userId);
    }
}