using Dashboard.Winform.ViewModels.RBACModels;

namespace Dashboard.Winform.Events
{
    public class UsersLoadedEventArgs : EventArgs
    {
        public List<UserViewModel> Users { get; }
        public int TotalCount { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }

        public UsersLoadedEventArgs(List<UserViewModel> users, int totalCount = 0, int currentPage = 1, int pageSize = 10)
        {
            Users = users ?? [];
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }

    public class UserSavedEventArgs : EventArgs
    {
        public UserDetailViewModel User { get; }
        public bool IsNewUser { get; }
        public string Message { get; }

        public UserSavedEventArgs(UserDetailViewModel user, bool isNewUser, string message = "")
        {
            User = user;
            IsNewUser = isNewUser;
            Message = message;
        }
    }

    public class UserDeletedEventArgs : EventArgs
    {
        public long UserId { get; }
        public string UserName { get; }
        public string Message { get; }

        public UserDeletedEventArgs(long userId, string userName, string message = "")
        {
            UserId = userId;
            UserName = userName;
            Message = message;
        }
    }

    public class RoleAssignedEventArgs : EventArgs
    {
        public long UserId { get; }
        public long RoleId { get; }
        public string UserName { get; }
        public string RoleName { get; }
        public string Message { get; }

        public RoleAssignedEventArgs(long userId, long roleId, string userName, string roleName, string message = "")
        {
            UserId = userId;
            RoleId = roleId;
            UserName = userName;
            RoleName = roleName;
            Message = message;
        }
    }

    public class RoleRemovedEventArgs : EventArgs
    {
        public long UserId { get; }
        public long RoleId { get; }
        public string UserName { get; }
        public string RoleName { get; }
        public string Message { get; }

        public RoleRemovedEventArgs(long userId, long roleId, string userName, string roleName, string message = "")
        {
            UserId = userId;
            RoleId = roleId;
            UserName = userName;
            RoleName = roleName;
            Message = message;
        }
    }

    public class UserFilterChangedEventArgs : EventArgs
    {
        public string? SearchText { get; }
        public long? RoleId { get; }
        public string? Status { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }

        public UserFilterChangedEventArgs(string? searchText = null, long? roleId = null,
            string? status = null, int pageSize = 10, int currentPage = 1)
        {
            SearchText = searchText;
            RoleId = roleId;
            Status = status;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }
    }
}