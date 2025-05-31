using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;
using ChatForLife.Repositories;

namespace ChatForLife.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRepository<Activity> _activityRepository;

        public MessageService(IMessageRepository messageRepository, IRepository<Activity> activityRepository)
        {
            _messageRepository = messageRepository;
            _activityRepository = activityRepository;
        }

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
        {
            return await _messageRepository.GetMessagesBetweenUsersAsync(user1Id, user2Id);
        }

        public async Task<IEnumerable<Message>> GetReceivedMessagesAsync(int userId)
        {
            return await _messageRepository.GetReceivedMessagesAsync(userId);
        }

        public async Task<Message> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.Now,
                IsRead = false
            };

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            // Mesaj gönderme aktivitesi
            var activity = new Activity
            {
                UserId = senderId,
                Type = "Message",
                Description = "Özel mesaj gönderdi",
                Icon = "💬",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();

            return message;
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _messageRepository.UpdateAsync(message);
                await _messageRepository.SaveChangesAsync();
            }
        }
    }
}