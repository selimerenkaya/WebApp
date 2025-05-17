using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Repositories
{
    public interface IGroupMessageRepository : IRepository<GroupMessage>
    {
        Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId);
    }
}