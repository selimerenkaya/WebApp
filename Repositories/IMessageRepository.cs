using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
        Task<IEnumerable<Message>> GetReceivedMessagesAsync(int userId);
        Task<IEnumerable<Message>> GetSentMessagesAsync(int userId);
    }
}