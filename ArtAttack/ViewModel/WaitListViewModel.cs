using ArtAttack.Domain;
using ArtAttack.Model;
using System.Collections.Generic;

namespace ArtAttack.Services
{
    public class WaitListViewModel : IWaitListViewModel
    {
        private readonly WaitListModel _waitListModel;

        public WaitListViewModel(string connectionString)
        {
            _waitListModel = new WaitListModel(connectionString);
        }

        public void AddUserToWaitlist(int userId, int productWaitListId)
        {
            _waitListModel.AddUserToWaitlist(userId, productWaitListId);
        }

        public void RemoveUserFromWaitlist(int userId, int productWaitListId)
        {
            _waitListModel.RemoveUserFromWaitlist(userId, productWaitListId);
        }

        public List<UserWaitList> GetUsersInWaitlist(int waitListProductId)
        {
            return _waitListModel.GetUsersInWaitlist(waitListProductId);
        }

        public List<UserWaitList> GetUserWaitlists(int userId)
        {
            return _waitListModel.GetUserWaitlists(userId);
        }

        public int GetWaitlistSize(int productWaitListId)
        {
            return _waitListModel.GetWaitlistSize(productWaitListId);
        }

        public bool IsUserInWaitlist(int userId, int productWaitListId)
        {
            return _waitListModel.IsUserInWaitlist(userId, productWaitListId);
        }
    }
}