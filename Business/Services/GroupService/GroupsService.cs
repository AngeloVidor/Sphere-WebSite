using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebSite.Data.Interfaces.GroupRepository;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Business.Services.GroupService
{
    public class GroupsService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IS3Service _s3Service;

        public GroupsService(IGroupRepository groupRepository, IS3Service s3Service)
        {
            _groupRepository = groupRepository;
            _s3Service = s3Service;
        }

        public async Task<GroupModel> CreateGroup(GroupModel group, IFormFile groupImage)
        {
            if (groupImage == null || groupImage.Length == 0)
            {
                throw new ArgumentException("A imagem do grupo é obrigatória.");
            }

            group.GroupImageUrl = await _s3Service.UploadFileAsync(groupImage);

            if (string.IsNullOrEmpty(group.Name))
            {
                throw new ArgumentException("O nome do grupo é obrigatório");
            }

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

        public async Task JoinGroup(int groupId, int userId)
        {
            await _groupRepository.JoinGroup(groupId, userId);
        }
    }
}
