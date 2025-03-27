using ArtAttack.Domain;
using System.Collections.Generic;

namespace ArtAttack.Services
{
    public interface IWaitListViewModel
    {
        void AddUserToWaitlist(int userId, int productWaitListId);
        void RemoveUserFromWaitlist(int userId, int productWaitListId);
        List<UserWaitList> GetUsersInWaitlist(int waitListProductId);
        List<UserWaitList> GetUserWaitlists(int userId);
        int GetWaitlistSize(int productWaitListId);
        bool IsUserInWaitlist(int userId, int productWaitListId);
    }
}