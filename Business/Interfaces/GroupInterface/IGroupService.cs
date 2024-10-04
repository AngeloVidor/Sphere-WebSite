using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Business.Interfaces.GroupInterface
{
    public interface IGroupService
    {
        Task<GroupModel> CreateGroup(GroupModel group, IFormFile groupImage);
        Task<GroupModel> UpdateGroup(GroupModel group);
        Task DeleteGroup(int groupId);
        Task<GroupModel> GetGroupById(int groupId);
        Task<IEnumerable<GroupModel>> GetAllGroups();
    }
}