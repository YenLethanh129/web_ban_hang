using Dashboard.Winform.ViewModels.RBACModels;

namespace Dashboard.Winform.Events
{
    public class RolesLoadedEventArgs : EventArgs
    {
        public List<RoleViewModel> Roles { get; }

        public int TotalCount { get; set; }

        public RolesLoadedEventArgs(List<RoleViewModel> roles)
        {
            Roles = roles;
        }
    }

    public class PermissionsLoadedEventArgs : EventArgs
    {
        public List<PermissionViewModel> Permissions { get; }

        public int TotalCount { get; set; }

        public PermissionsLoadedEventArgs(List<PermissionViewModel> permissions)
        {
            Permissions = permissions;
        }
    }

    public class RoleSavedEventArgs : EventArgs
    {
        public RoleViewModel Role { get; }
        public bool IsNewRole { get; }

        public RoleSavedEventArgs(RoleViewModel role, bool isNewRole)
        {
            Role = role;
            IsNewRole = isNewRole;
        }
    }

    public class PermissionSavedEventArgs : EventArgs
    {
        public PermissionViewModel Permission { get; }
        public bool IsNewPermission { get; }

        public PermissionSavedEventArgs(PermissionViewModel permission, bool isNewPermission)
        {
            Permission = permission;
            IsNewPermission = isNewPermission;
        }
    }

    public class RolePermissionsAssignedEventArgs : EventArgs
    {
        public long RoleId { get; }
        public List<long> PermissionIds { get; }

        public RolePermissionsAssignedEventArgs(long roleId, List<long> permissionIds)
        {
            RoleId = roleId;
            PermissionIds = permissionIds;
        }
    }
}