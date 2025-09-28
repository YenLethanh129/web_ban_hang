export interface UserDTO {
  fullname: string;
  phone_number: string;
  address: string;
  date_of_birth: Date;
}

export interface ProfileUserDTO {
  message: string;
  user: UserDTO;
}
