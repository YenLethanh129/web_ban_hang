using Dashboard.Winform.ViewModels.RBACModels;
using System.ComponentModel;

namespace Dashboard.Winform.Helpers
{

    public class PermissionItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string DisplayText => $"{Name} ({Resource}:{Action})";

        public override string ToString()
        {
            return DisplayText;
        }

        public override bool Equals(object? obj)
        {
            return obj is PermissionItem other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static PermissionItem FromViewModel(PermissionViewModel viewModel)
        {
            return new PermissionItem
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Resource = viewModel.Resource,
                Action = viewModel.Action,
                Description = viewModel.Description
            };
        }
    }

    public static class RolePermissionUIHelper
    {
        public static void RestoreRoleSelection(DataGridView dgvRoles, long roleId)
        {
            try
            {
                for (int i = 0; i < dgvRoles.Rows.Count; i++)
                {
                    if (dgvRoles.Rows[i].DataBoundItem is RoleViewModel role && role.Id == roleId)
                    {
                        dgvRoles.ClearSelection();
                        dgvRoles.Rows[i].Selected = true;
                        dgvRoles.CurrentCell = dgvRoles.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static void ApplyDarkThemeToListBox(ListBox listBox)
        {
            listBox.BackColor = Color.FromArgb(42, 45, 86);
            listBox.ForeColor = Color.White;
            listBox.BorderStyle = BorderStyle.None;
        }

        public static BindingList<T> ToBindingList<T>(IEnumerable<T> source)
        {
            return new BindingList<T>(source.ToList());
        }

        public static void UpdateListBoxData<T>(ListBox listBox, IEnumerable<T> newData)
        {
            listBox.BeginUpdate();
            try
            {
                listBox.Items.Clear();
                foreach (var item in newData)
                {
                    if (item != null)
                    {
                        listBox.Items.Add(item);
                    }
                }
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        public static List<T> GetSelectedItems<T>(DataGridView dataGridView) where T : class
        {
            return dataGridView.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(row => row.DataBoundItem as T)
                .Where(item => item != null)
                .ToList()!;
        }
        public static void ShowAssignmentResult(int assignedCount, int removedCount, string roleName)
        {
            string message = "";
            if (assignedCount > 0 && removedCount > 0)
            {
                message = $"Đã gán {assignedCount} quyền và gỡ {removedCount} quyền cho role '{roleName}'";
            }
            else if (assignedCount > 0)
            {
                message = $"Đã gán {assignedCount} quyền cho role '{roleName}'";
            }
            else if (removedCount > 0)
            {
                message = $"Đã gỡ {removedCount} quyền khỏi role '{roleName}'";
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    public static class RolePermissionExtensions
    {
        public static PermissionItem ToPermissionItem(this PermissionViewModel permission)
        {
            return PermissionItem.FromViewModel(permission);
        }

        public static List<PermissionItem> ToPermissionItems(this IEnumerable<PermissionViewModel> permissions)
        {
            return permissions.Select(p => p.ToPermissionItem()).ToList();
        }

        public static List<long> GetIds(this IEnumerable<PermissionViewModel> permissions)
        {
            return permissions.Select(p => p.Id).ToList();
        }

        public static IEnumerable<PermissionViewModel> FilterBySearchTerm(
            this IEnumerable<PermissionViewModel> permissions,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return permissions;

            var term = searchTerm.ToLowerInvariant();
            return permissions.Where(p =>
                p.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase) ||
                p.Resource.Contains(term, StringComparison.InvariantCultureIgnoreCase) ||
                p.Action.Contains(term, StringComparison.InvariantCultureIgnoreCase) ||
                (p.Description?.ToLowerInvariant().Contains(term, StringComparison.InvariantCultureIgnoreCase) ?? false));
        }
    }
}