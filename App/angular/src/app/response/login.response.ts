export interface LoginResponse {
  token: string;
  user: {
    id: number;
    fullname: string;
    phone_number: string;
    address: string;
    date_of_birth: string;
    role_id: number;
  }
} 