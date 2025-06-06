﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;
using ChatForLife.Repositories;

namespace ChatForLife.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMessageRepository _groupMessageRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<GroupMember> _groupMemberRepository;
        private readonly IRepository<Activity> _activityRepository;

        public GroupService(
            IGroupRepository groupRepository,
            IGroupMessageRepository groupMessageRepository,
            IRepository<User> userRepository,
            IRepository<GroupMember> groupMemberRepository,
            IRepository<Activity> activityRepository)
        {
            _groupRepository = groupRepository;
            _groupMessageRepository = groupMessageRepository;
            _groupMemberRepository = groupMemberRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _groupRepository.GetByIdAsync(groupId);
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(int userId)
        {
            return await _groupRepository.GetUserGroupsAsync(userId);
        }

        public async Task<Group> CreateGroupAsync(string name, string description, int privacy, int creatorUserId)
        {

            var user = await _userRepository.GetByIdAsync(creatorUserId);
            if (user == null)
            {
                throw new InvalidOperationException($"UserId {creatorUserId} ile ilişkili bir kullanıcı bulunamadı.");
            }
            var group = new Group
            {
                Name = name,
                Description = description,
                Privacy = privacy,
                CreatedAt = DateTime.Now
            };

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            // Grup oluşturan kullanıcıyı admin olarak ekle
            await AddMemberToGroupAsync(group.Id, creatorUserId, creatorUserId, true);

            // Grup oluşturma aktivitesi
            var activity = new Activity
            {
                UserId = creatorUserId,
                Type = "CreateGroup",
                Description = $"{name} grubunu oluşturdu",
                Icon = "👥",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();

            return group;
        }

        public async Task<GroupMessage> SendGroupMessageAsync(int groupId, int senderId, string content)
        {
            var message = new GroupMessage
            {
                GroupId = groupId,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.Now
            };

            await _groupMessageRepository.AddAsync(message);
            await _groupMessageRepository.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId)
        {
            return await _groupMessageRepository.GetGroupMessagesAsync(groupId);
        }

        public async Task AddMemberToGroupAsync(int groupId, int userId, int currentUserId, bool isAdmin = false)
        {
            // Admin yetkisi kontrolü
            //if (!await IsUserGroupAdminAsync(groupId, currentUserId))
            //{
            //    throw new UnauthorizedAccessException("Yalnızca admin kullanıcılar gruba üye ekleyebilir.");
            //}

            var groupMember = new GroupMember
            {
                GroupId = groupId,
                UserId = userId,
                IsAdmin = isAdmin,
                JoinedAt = DateTime.Now
            };

            await _groupMemberRepository.AddAsync(groupMember);
            await _groupMemberRepository.SaveChangesAsync();

            var group = await _groupRepository.GetByIdAsync(groupId);
            var activity = new Activity
            {
                UserId = userId,
                Type = "JoinGroup",
                Description = $"{group.Name} grubuna katıldı",
                Icon = "🚪",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();
        }

        public async Task RemoveMemberFromGroupAsync(int groupId, int userId, int currentUserId)
        {
            // Admin yetkisi kontrolü
            if (!await IsUserGroupAdminAsync(groupId, currentUserId))
            {
                throw new UnauthorizedAccessException("Yalnızca admin kullanıcılar gruptan üye çıkarabilir.");
            }

            var groupMembers = await _groupMemberRepository.FindAsync(
                gm => gm.GroupId == groupId && gm.UserId == userId);

            foreach (var member in groupMembers)
            {
                await _groupMemberRepository.RemoveAsync(member);
            }

            await _groupMemberRepository.SaveChangesAsync();

            var group = await _groupRepository.GetByIdAsync(groupId);
            var activity = new Activity
            {
                UserId = userId,
                Type = "LeaveGroup",
                Description = $"{group.Name} grubundan ayrıldı",
                Icon = "🚶",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();
        }


        public async Task<bool> IsUserInGroupAsync(int groupId, int userId)
        {
            return await _groupRepository.IsUserInGroupAsync(groupId, userId);
        }

        public async Task<bool> IsUserGroupAdminAsync(int groupId, int userId)
        {
            return await _groupRepository.IsUserGroupAdminAsync(groupId, userId);
        }

        public async Task<Group> GetGroupWithMembersAsync(int groupId)
        {
                return await _groupRepository.GetGroupWithMembersAsync(groupId);
        }

        public async Task UpdateGroupAsync(int groupId, string newName, string newDescription, int currentUserId)
        {
            // Admin yetkisi kontrolü
            if (!await IsUserGroupAdminAsync(groupId, currentUserId))
            {
                throw new UnauthorizedAccessException("Yalnızca admin kullanıcılar grup bilgilerini güncelleyebilir.");
            }

            var groupToUpdate = await _groupRepository.GetByIdAsync(groupId);
            if (groupToUpdate == null)
            {
                throw new ArgumentException($"Grup bulunamadı. GroupId: {groupId}");
            }

            groupToUpdate.Name = newName;
            groupToUpdate.Description = newDescription;
            // groupToUpdate.Privacy gibi diğer özellikleri de burada güncelleyebilirsiniz.

            await _groupRepository.UpdateAsync(groupToUpdate);
            await _groupRepository.SaveChangesAsync();

            // Grup güncelleme aktivitesi
            var activity = new Activity
            {
                UserId = currentUserId,
                Type = "UpdateGroup",
                Description = $"{groupToUpdate.Name} grubunun bilgilerini güncelledi",
                Icon = "✏️",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();
        }
    }
}
