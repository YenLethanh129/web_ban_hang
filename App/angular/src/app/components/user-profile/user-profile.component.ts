import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../dtos/user.dto';
import { UpdateUserDTO } from '../../dtos/update-user.dto';
import { NotificationService } from '../../services/notification.service';
import { AddressAutocompleteComponent } from '../shared/address-autocomplete/address-autocomplete.component';
import { AddressPrediction } from '../../dtos/address.dto';
import { Subject, takeUntil } from 'rxjs';
import { ValidateDTO } from '../../dtos/validate.dto';
import { ValidateService } from '../../services/validate.service';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule,
    CommonModule,
    AddressAutocompleteComponent,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss',
})
export class UserProfileComponent implements OnInit, OnDestroy {
  @ViewChild('updateForm') updateForm!: NgForm;

  profile: UserDTO | null = null;
  isEditing = false;
  isLoading = false;
  showFullNameError = false;
  showDateOfBirthError = false;
  showAddressError = false;

  // Validation DTOs
  validateFullNameDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };
  validateDateOfBirthDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };
  validateAddressDTO: ValidateDTO = {
    isValid: true,
    errors: [],
  };

  // Form data for editing
  editData = {
    fullname: '',
    dateOfBirth: '',
    address: '',
  };

  private destroy$ = new Subject<void>();

  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadUserProfile();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadUserProfile(): void {
    const currentUser = this.userService.getCurrentUser();
    if (currentUser) {
      this.setProfileData(currentUser);
      return;
    }

    this.userService.user$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (user) => {
        if (user) {
          this.setProfileData(user);
        }
      },
      error: (error) => {
        console.error('Error in user subscription:', error);
      },
    });

    // Load from server if not available
    this.userService.getUser().subscribe({
      next: (profile) => {
        
        this.setProfileData(profile);
      },
      error: (error) => {
        console.error('Error fetching user profile:', error);
        this.notificationService.showError(
          'Không thể tải thông tin người dùng'
        );
        // Redirect to login if unable to load profile
        this.router.navigate(['/login']);
      },
    });
  }

  private setProfileData(profile: UserDTO): void {
    this.profile = profile;
    // Initialize edit form with current data
    this.editData.fullname = profile.fullname;
    this.editData.dateOfBirth = this.formatDateForInput(profile.date_of_birth);
    this.editData.address = profile.address;

    // Validate initial data
    this.validateFullName();
    this.validateDateOfBirth();
    this.validateAddress();
  }

  /**
   *
   * VALIDATE METHODS
   *
   */

  validateFullName(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.editData.fullname = input.value;
    }
    this.validateFullNameDTO = ValidateService.validateFullName(
      this.editData.fullname
    );
  }

  validateDateOfBirth(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.editData.dateOfBirth = input.value;
    }
    if (
      this.editData.dateOfBirth &&
      this.editData.dateOfBirth.trim().length > 0
    ) {
      this.validateDateOfBirthDTO = ValidateService.validateDateOfBirth(
        this.editData.dateOfBirth
      );
    } else {
      this.validateDateOfBirthDTO = { isValid: true, errors: [] };
    }
  }

  validateAddress(event?: Event): void {
    if (event) {
      const input = event.target as HTMLInputElement;
      this.editData.address = input.value;
    }
    this.validateAddressDTO = ValidateService.validateAddress(
      this.editData.address
    );
  }

  validateForm(): boolean {
    // Trigger all validations
    this.validateFullName();
    this.validateDateOfBirth();
    this.validateAddress();

    const isValid =
      this.validateFullNameDTO.isValid &&
      this.validateDateOfBirthDTO.isValid &&
      this.validateAddressDTO.isValid;

    if (!isValid) {
      this.notificationService.showWarning(
        '⚠️ Vui lòng kiểm tra lại thông tin!'
      );
    }

    return isValid;
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

        // Validate after resetting data
        this.validateFullName();
        this.validateDateOfBirth();
        this.validateAddress();
      }
    }
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
    // Xử lý cả trường hợp date là string ISO hoặc Date object
    let d: Date;
    if (typeof date === 'string') {
      d = new Date(date);
    } else {
      d = date;
    }
    // Nếu date không hợp lệ, trả về 'Chưa cập nhật'
    if (isNaN(d.getTime())) return 'Chưa cập nhật';
    // Hiển thị dạng dd/MM/yyyy
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${day}/${month}/${year}`;
  }

  onAddressSelected(prediction: AddressPrediction): void {
    this.editData.address = prediction.description;
    this.validateAddress();
    
  }

  onSubmit(): void {
    if (!this.validateForm()) {
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

    this.userService.updateUser(updateDTO).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.isEditing = false;
        this.notificationService.showSuccess('Cập nhật thông tin thành công!');

        this.userService.refreshUserSync();
        this.loadUserProfile();

        // Reload website to reflect changes in header
        window.location.reload();
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

      // Validate after resetting data
      this.validateFullName();
      this.validateDateOfBirth();
      this.validateAddress();
    }
  }
}
