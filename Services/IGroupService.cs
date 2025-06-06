﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Services
{
    public interface IGroupService
    {
        Task<Group> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);
        Task<Group> CreateGroupAsync(string name, string description, int privacy, int creatorUserId);
        Task<GroupMessage> SendGroupMessageAsync(int groupId, int senderId, string content);
        Task<IEnumerable<GroupMessage>> GetGroupMessagesAsync(int groupId);
        Task AddMemberToGroupAsync(int groupId, int userId, int currentUserId, bool isAdmin = false);
        Task RemoveMemberFromGroupAsync(int groupId, int userId, int currentUserId);
        Task<bool> IsUserInGroupAsync(int groupId, int userId);
        Task<bool> IsUserGroupAdminAsync(int groupId, int userId);

        Task<Group> GetGroupWithMembersAsync(int groupId);

        Task UpdateGroupAsync(int groupId, string newName, string newDescription, int currentUserId); // Yeni eklenecek
    
    }
}