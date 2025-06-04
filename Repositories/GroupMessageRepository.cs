using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatForLife.Models;
using ChatForLife.Models.Entities;

namespace ChatForLife.Repositories
{
    public class GroupMessageRepository : Repository<GroupMessage>, IGroupMessageRepository
    {
        public GroupMessageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId)
        {
            return await _context.GroupMessages
                .Include(gm => gm.Sender)
                .Where(gm => gm.GroupId == groupId)
                .OrderBy(gm => gm.SentAt)
                .ToListAsync();
        }
    }
}