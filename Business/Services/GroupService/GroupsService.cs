using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebSite.Data.Interfaces.GroupRepository;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Business.Services.GroupService
{
    public class GroupsService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupsService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupModel> CreateGroup(GroupModel group)
        {
            if (string.IsNullOrEmpty(group.Name))
            {
                throw new ArgumentException("O noem do grupo é obrigatório");
            }
            /*if (group.Users == null || group.Users.Count == 0)
            {
                throw new ArgumentException("O grupo deve ter ao menos um usuário");
            }*/
            return await _groupRepository.CreateGroup(group);
        }

        public async Task DeleteGroup(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("ID do grupo inválido");
            }
            await _groupRepository.DeleteGroup(groupId);
        }

        public async Task<IEnumerable<GroupModel>> GetAllGroups()
        {
            return await _groupRepository.GetAllGroups();
        }

        public async Task<GroupModel> GetGroupById(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("ID do grupo inválido");
            }
            return await _groupRepository.GetGroupById(groupId);
        }

        public async Task<GroupModel> UpdateGroup(GroupModel group)
        {
            if (group.GroupID <= 0)
            {
                throw new ArgumentException("ID do grupo inválido");
            }
            return await _groupRepository.UpdateGroup(group);
        }
    }
}
