using Dashboard.Winform.ViewModels.RBACModels;

namespace Dashboard.Winform.Events
{
    public class RolesLoadedEventArgs : EventArgs
    {
        public List<RoleViewModel> Roles { get; }

        public RolesLoadedEventArgs(List<RoleViewModel> roles)
        {
            Roles = roles ?? new List<RoleViewModel>();
        }
    }

    public class RoleUpdatedEventArgs : EventArgs
    {
        public RoleViewModel Role { get; }
        public bool IsNewRole { get; }

        public RoleUpdatedEventArgs(RoleViewModel role, bool isNewRole = false)
        {
            Role = role;
            IsNewRole = isNewRole;
        }
    }

    public class RoleDeletedEventArgs : EventArgs
    {
        public long RoleId { get; }
        public string RoleName { get; }

        public RoleDeletedEventArgs(long roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }
    }

    public class PermissionsLoadedEventArgs : EventArgs
    {
        public List<PermissionViewModel> Permissions { get; }

        public PermissionsLoadedEventArgs(List<PermissionViewModel> permissions)
        {
            Permissions = permissions ?? new List<PermissionViewModel>();
        }
    }

    public class PermissionUpdatedEventArgs : EventArgs
    {
        public PermissionViewModel Permission { get; }
        public bool IsNewPermission { get; }

        public PermissionUpdatedEventArgs(PermissionViewModel permission, bool isNewPermission = false)
        {
            Permission = permission;
            IsNewPermission = isNewPermission;
        }
    }

    public class PermissionDeletedEventArgs : EventArgs
    {
        public long PermissionId { get; }
        public string PermissionName { get; }

        public PermissionDeletedEventArgs(long permissionId, string permissionName)
        {
            PermissionId = permissionId;
            PermissionName = permissionName;
        }
    }

    public class RolePermissionsUpdatedEventArgs : EventArgs
    {
        public long RoleId { get; }
        public string RoleName { get; }
        public List<long> AssignedPermissionIds { get; }
        public List<long> RemovedPermissionIds { get; }

        public RolePermissionsUpdatedEventArgs(
            long roleId,
            string roleName,
            List<long> assignedPermissionIds,
            List<long> removedPermissionIds)
        {
            RoleId = roleId;
            RoleName = roleName;
            AssignedPermissionIds = assignedPermissionIds ?? new List<long>();
            RemovedPermissionIds = removedPermissionIds ?? new List<long>();
        }
    }

    public class DataLoadingEventArgs : EventArgs
    {
        public bool IsLoading { get; }
        public string? Message { get; }

        public DataLoadingEventArgs(bool isLoading, string? message = null)
        {
            IsLoading = isLoading;
            Message = message;
        }
    }

    public class OperationErrorEventArgs : EventArgs
    {
        public string Operation { get; }
        public Exception Exception { get; }
        public string UserMessage { get; }

        public OperationErrorEventArgs(string operation, Exception exception, string userMessage)
        {
            Operation = operation;
            Exception = exception;
            UserMessage = userMessage;
        }
    }

    public class ValidationErrorEventArgs : EventArgs
    {
        public string FieldName { get; }
        public string ErrorMessage { get; }

        public ValidationErrorEventArgs(string fieldName, string errorMessage)
        {
            FieldName = fieldName;
            ErrorMessage = errorMessage;
        }
    }
}