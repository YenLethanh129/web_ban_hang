using System.ComponentModel;

namespace Dashboard.Winform.ViewModels
{
    public class ImageValidationViewModel : INotifyPropertyChanged
    {
        private string _imageUrl = string.Empty;
        private string _originalImageUrl = string.Empty; // Track original value
        private bool _isValidated = false;
        private bool _isValidating = false;
        private bool _isValid = false;
        private string _validationMessage = string.Empty;
        private bool _hasChanges = false;

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value ?? string.Empty;
                    OnPropertyChanged(nameof(ImageUrl));

                    // Check if there are changes from original
                    HasChanges = !string.Equals(_originalImageUrl, _imageUrl, StringComparison.OrdinalIgnoreCase);

                    // Reset validation state when URL changes
                    if (HasChanges)
                    {
                        IsValidated = false;
                        IsValid = false;
                        ValidationMessage = string.Empty;
                    }
                }
            }
        }

        public string OriginalImageUrl
        {
            get => _originalImageUrl;
            set
            {
                _originalImageUrl = value ?? string.Empty;
                HasChanges = !string.Equals(_originalImageUrl, _imageUrl, StringComparison.OrdinalIgnoreCase);
                OnPropertyChanged(nameof(OriginalImageUrl));
            }
        }

        public bool IsValidated
        {
            get => _isValidated;
            set
            {
                if (_isValidated != value)
                {
                    _isValidated = value;
                    OnPropertyChanged(nameof(IsValidated));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public bool IsValidating
        {
            get => _isValidating;
            set
            {
                if (_isValidating != value)
                {
                    _isValidating = value;
                    OnPropertyChanged(nameof(IsValidating));
                    OnPropertyChanged(nameof(CanValidate));
                }
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                if (_validationMessage != value)
                {
                    _validationMessage = value ?? string.Empty;
                    OnPropertyChanged(nameof(ValidationMessage));
                }
            }
        }

        public bool HasChanges
        {
            get => _hasChanges;
            private set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged(nameof(HasChanges));
                    OnPropertyChanged(nameof(CanValidate));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        // Computed properties for UI binding
        public bool CanValidate => !IsValidating && !string.IsNullOrWhiteSpace(ImageUrl) && HasChanges;

        public bool CanSave => !HasChanges || (IsValidated && IsValid);

        public bool RequiresValidation => HasChanges && !IsValidated;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetValidationResult(bool isValid, string message = "")
        {
            IsValid = isValid;
            ValidationMessage = message;
            IsValidated = true;
            IsValidating = false;
        }

        public void StartValidation()
        {
            IsValidating = true;
            ValidationMessage = "Đang kiểm tra URL...";
        }

        public void ResetValidation()
        {
            IsValidated = false;
            IsValid = false;
            IsValidating = false;
            ValidationMessage = string.Empty;
        }

        public void SaveChanges()
        {
            OriginalImageUrl = ImageUrl;
            HasChanges = false;
        }

        public void DiscardChanges()
        {
            ImageUrl = OriginalImageUrl;
            ResetValidation();
        }
    }
}