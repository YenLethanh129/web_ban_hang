import { ValidateDTO } from '../dtos/validate.dto';

export class ValidateService {
  static validateEmail(email: string): ValidateDTO {
    if (!email || email.trim().length === 0) {
      return { isValid: false, errors: ['Email không được để trống'] };
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const isValid = emailRegex.test(email);
    return {
      isValid,
      errors: isValid ? [] : ['Email không hợp lệ'],
    };
  }

  static validatePhoneNumber(phoneNumber: string): ValidateDTO {
    if (!phoneNumber || phoneNumber.trim().length === 0) {
      return { isValid: false, errors: ['Số điện thoại không được để trống'] };
    }
    const phoneRegex = /^\d{10}$/;
    const isValid = phoneRegex.test(phoneNumber);
    return {
      isValid,
      errors: isValid ? [] : ['Số điện thoại phải có đúng 10 chữ số'],
    };
  }

  // Mật khẩu tối thiểu 12 ký tự
  // Chứa ít một chữ hoa, một chữ thường, một số và một ký tự đặc biệt
  static validatePassword(password: string): ValidateDTO {
    const validate: ValidateDTO = { isValid: true, errors: [] };
    if (!password || password.trim().length === 0) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu không được để trống');
      return validate;
    }

    const lengthValid = password.length >= 12;
    const uppercaseValid = /[A-Z]/.test(password);
    const lowercaseValid = /[a-z]/.test(password);
    const numberValid = /\d/.test(password);
    const specialCharValid = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    if (!lengthValid) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu phải có ít nhất 12 ký tự');
    }
    if (!uppercaseValid) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu phải chứa ít nhất một chữ hoa');
    }
    if (!lowercaseValid) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu phải chứa ít nhất một chữ thường');
    }
    if (!numberValid) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu phải chứa ít nhất một số');
    }
    if (!specialCharValid) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu phải chứa ít nhất một ký tự đặc biệt');
    }
    return validate;
  }

  static validateFullName(fullName: string): ValidateDTO {
    const validate: ValidateDTO = { isValid: true, errors: [] };

    // Kiểm tra không được để trống hoặc null
    if (!fullName || fullName.trim().length === 0) {
      validate.isValid = false;
      validate.errors.push('Họ và tên không được để trống');
      return validate;
    }

    const trimmedName = fullName.trim();

    // Regex cho phép chỉ chữ cái tiếng Việt (có dấu) và khoảng trắng
    // Không cho phép ký tự đặc biệt như @, :, số, v.v.
    const nameRegex =
      /^[a-zA-ZàáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđĐÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴ\s]+$/;

    if (!nameRegex.test(trimmedName)) {
      validate.isValid = false;
      validate.errors.push(
        'Họ và tên chỉ được chứa chữ cái tiếng Việt và khoảng trắng'
      );
    }

    // Kiểm tra phải có ít nhất 2 từ (họ và tên)
    const nameParts = trimmedName
      .split(/\s+/)
      .filter((part) => part.length > 0);
    if (nameParts.length < 2) {
      validate.isValid = false;
      validate.errors.push('Họ và tên phải có ít nhất 2 từ (họ và tên)');
    }

    // Kiểm tra độ dài hợp lý (2-50 ký tự)
    if (trimmedName.length < 2) {
      validate.isValid = false;
      validate.errors.push('Họ và tên quá ngắn (tối thiểu 2 ký tự)');
    } else if (trimmedName.length > 50) {
      validate.isValid = false;
      validate.errors.push('Họ và tên quá dài (tối đa 50 ký tự)');
    }

    // Kiểm tra không được có nhiều khoảng trắng liên tiếp
    if (/\s{2,}/.test(trimmedName)) {
      validate.isValid = false;
      validate.errors.push(
        'Họ và tên không được có nhiều khoảng trắng liên tiếp'
      );
    }

    // Kiểm tra không được bắt đầu hoặc kết thúc bằng khoảng trắng (đã trim nhưng validate thêm)
    if (fullName !== trimmedName) {
      validate.isValid = false;
      validate.errors.push(
        'Họ và tên không được có khoảng trắng ở đầu hoặc cuối'
      );
    }

    return validate;
  }

  static validateConfirmPassword(
    password: string,
    confirmPassword: string
  ): ValidateDTO {
    const validate: ValidateDTO = { isValid: true, errors: [] };

    if (!confirmPassword || confirmPassword.trim().length === 0) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu xác nhận không được để trống');
      return validate;
    }

    if (password !== confirmPassword) {
      validate.isValid = false;
      validate.errors.push('Mật khẩu xác nhận không khớp');
    }
    return validate;
  }

  static validateAddress(address: string): ValidateDTO {
    const validate: ValidateDTO = { isValid: true, errors: [] };
    if (!address || address.trim().length === 0) {
      validate.isValid = false;
      validate.errors.push('Địa chỉ không được để trống');
    }
    return validate;
  }

  // Ngày sinh phải là ngày hợp lệ và không được lớn hơn ngày hiện tại, đủ 18 tuổi
  static validateDateOfBirth(dateOfBirth: string): ValidateDTO {
    const validate: ValidateDTO = { isValid: true, errors: [] };
    const dob = new Date(dateOfBirth);
    const today = new Date();
    if (isNaN(dob.getTime())) {
      validate.isValid = false;
      validate.errors.push('Ngày sinh không hợp lệ');
      return validate;
    }
    if (dob > today) {
      validate.isValid = false;
      validate.errors.push('Ngày sinh không được lớn hơn ngày hiện tại');
    }
    const age = today.getFullYear() - dob.getFullYear();
    const monthDiff = today.getMonth() - dob.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < dob.getDate())) {
      validate.errors.push('Bạn phải đủ 18 tuổi');
      validate.isValid = false;
    }

    return validate;
  }
}
