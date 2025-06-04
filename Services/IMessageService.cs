using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
        Task<IEnumerable<Message>> GetReceivedMessagesAsync(int userId);
        Task<Message> SendMessageAsync(int senderId, int receiverId, string content);
        Task MarkMessageAsReadAsync(int messageId);
    }
}