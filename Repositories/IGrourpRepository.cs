using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetUserGroupsAsync(int userId);
        Task<Group> GetGroupWithMembersAsync(int groupId);
        Task<bool> IsUserInGroupAsync(int groupId, int userId);
        Task<bool> IsUserGroupAdminAsync(int groupId, int userId);
    }
}