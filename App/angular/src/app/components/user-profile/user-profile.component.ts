import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../dtos/user.dto';
import { UpdateUserDTO } from '../../dtos/update-user.dto';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss',
})
export class UserProfileComponent implements OnInit {
  @ViewChild('updateForm') updateForm!: NgForm;

  profile: UserDTO | null = null;
  isEditing = false;
  isLoading = false;
  showPassword = false;
  showCurrentPasswordError = false;
  showFullNameError = false;
  showDateOfBirthError = false;
  showPasswordError = false;
  showAddressError = false;

  // Form data for editing
  editData = {
    fullname: '',
    dateOfBirth: '',
    address: '',
    password: '',
    confirmPassword: '',
  };

  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.userService.getUser().subscribe({
      next: (profile) => {
        this.profile = profile;
        // Initialize edit form with current data
        this.editData.fullname = profile.fullname;
        this.editData.dateOfBirth = this.formatDateForInput(
          profile.date_of_birth
        );
        this.editData.address = profile.address;
      },
      error: (error) => {
        console.error('Error fetching user profile:', error);
        this.notificationService.showError(
          'Không thể tải thông tin người dùng'
        );
      },
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      // Reset form data to current profile data
      if (this.profile) {
        this.editData.fullname = this.profile.fullname;
        this.editData.dateOfBirth = this.formatDateForInput(
          this.profile.date_of_birth
        );
        this.editData.address = this.profile.address;
        this.editData.password = '';
        this.editData.confirmPassword = '';
      }
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  validatePassword(): boolean {
    return !this.editData.password || this.editData.password.length >= 6;
  }

  validateConfirmPassword(): boolean {
    return (
      !this.editData.password ||
      this.editData.password === this.editData.confirmPassword
    );
  }

  validateAge(): boolean {
    if (!this.editData.dateOfBirth) return true; // Optional field

    const birthDate = new Date(this.editData.dateOfBirth);
    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < birthDate.getDate())
    ) {
      return age - 1 >= 18;
    }

    return age >= 18;
  }

  formatDateForInput(date: Date): string {
    if (!date) return '';
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  formatDateForDisplay(date: Date): string {
    if (!date) return 'Chưa cập nhật';
    return new Date(date).toLocaleDateString('vi-VN');
  }

  onSubmit(): void {
    if (
      !this.updateForm.valid ||
      !this.validatePassword() ||
      !this.validateConfirmPassword() ||
      !this.validateAge()
    ) {
      this.notificationService.showWarning('Vui lòng kiểm tra lại thông tin!');
      return;
    }

    this.isLoading = true;
    this.notificationService.showInfo('⏳ Đang cập nhật thông tin...');

    const updateDTO: UpdateUserDTO = {};

    // Only include changed fields
    if (this.editData.fullname !== this.profile?.fullname) {
      updateDTO.fullname = this.editData.fullname;
    }

    if (
      this.editData.dateOfBirth !==
      this.formatDateForInput(this.profile?.date_of_birth!)
    ) {
      updateDTO.date_of_birth = new Date(this.editData.dateOfBirth);
    }

    if (this.editData.address !== this.profile?.address) {
      updateDTO.address = this.editData.address;
    }

    if (this.editData.password) {
      updateDTO.password = this.editData.password;
    }

    // Check if there are any changes
    if (Object.keys(updateDTO).length === 0) {
      this.notificationService.showInfo('Không có thay đổi nào để cập nhật');
      this.isLoading = false;
      this.isEditing = false;
      return;
    }

    updateDTO.date_of_birth = new Date(this.editData.dateOfBirth);
    updateDTO.address = this.editData.address;
    updateDTO.fullname = this.editData.fullname;
    if (this.editData.password) {
      updateDTO.password = this.editData.password;
    }

    this.userService.updateUser(updateDTO).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.isEditing = false;
        this.notificationService.showSuccess(
          'Cập nhật thông tin thành công! Bạn sẽ được đăng xuất để đảm bảo bảo mật.'
        );

        // Auto logout after successful update
        setTimeout(() => {
          this.userService.logout();
          this.router.navigate(['/login']);
          this.notificationService.showInfo(
            'Vui lòng đăng nhập lại với thông tin mới.'
          );
        }, 2000); // Wait 2 seconds to show success message
      },
      error: (error) => {
        this.isLoading = false;
        let message = 'Cập nhật thông tin thất bại';
        if (error?.error?.message) {
          message = error.error.message;
        }
        this.notificationService.showError(message);
      },
    });
  }

  cancelEdit(): void {
    this.isEditing = false;
    // Reset form to original values
    if (this.profile) {
      this.editData.fullname = this.profile.fullname;
      this.editData.dateOfBirth = this.formatDateForInput(
        this.profile.date_of_birth
      );
      this.editData.address = this.profile.address;
      this.editData.password = '';
      this.editData.confirmPassword = '';
    }
  }
}
