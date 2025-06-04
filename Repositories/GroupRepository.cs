using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatForLife.Models;
using ChatForLife.Models.Entities;

namespace ChatForLife.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            return await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<Group> GetGroupWithMembersAsync(int groupId)
        {
            return await _context.Groups
                .Include(g => g.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task<bool> IsUserInGroupAsync(int groupId, int userId)
        {
            return await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
        }

        public async Task<bool> IsUserGroupAdminAsync(int groupId, int userId)
        {
            return await _context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId && gm.IsAdmin);
        }
    }
}