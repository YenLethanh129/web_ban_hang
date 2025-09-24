export interface LoginRequestDTO {
  phone_number: string;
  password: string;
}

export interface LoginResponse {
  message: string;
  status: number;
}
