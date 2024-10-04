using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebSite.Data.Interfaces.GroupRepository;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Data.Repositories.GroupRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupModel> CreateGroup(GroupModel group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task DeleteGroup(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Grupo n encontrado");
            }
        }

        public async Task<IEnumerable<GroupModel>> GetAllGroups()
        {
            return await _context.Groups.Include(g => g.Users).ToListAsync();
        }

        public async Task<GroupModel> GetGroupById(int groupId)
        {
            var group = await _context
                .Groups.Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.GroupID == groupId);

            if (group == null)
            {
                throw new KeyNotFoundException("grupo n encontrado");
            }
            return group;
        }

        public async Task<GroupModel> UpdateGroup(GroupModel group)
        {
            var existingGroup = await _context.Groups.FindAsync(group.GroupID);
            if (existingGroup == null)
            {
                throw new KeyNotFoundException("Grupo não encontrado.");
            }
            existingGroup.Name = group.Name;
            existingGroup.Description = group.Description;
            existingGroup.Tags = group.Tags;
            existingGroup.Users = group.Users;

            _context.Groups.Update(existingGroup);
            await _context.SaveChangesAsync();
            return existingGroup;
        }
    }
}
