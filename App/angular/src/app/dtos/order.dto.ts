export interface OrderDTO {
  user_id: number;
  full_name: string;
  email: string;
  phone_number: string;
  address: string;
  note: string;
  total_money: number;
  shipping_method: string;
  shipping_address: string;
  payment_method: string;
  payment_status: string;
}

export interface OrderResponseDTO {
  order_id: number;
  note: string;
  status: string;
  total_money: number;
  order_date: string;
  shipping_method: string;
  payment_method: string;
  payment_status: string | null;
  created_at: string;
  last_modified: string;
}

// New interface for the updated API response
export interface NewOrderResponseDTO {
  created_at: string;
  last_modified: string;
  order_uuid: string;
  note: string;
  status: string;
  total_money: number;
  order_date: string;
  shipping_method: string;
  payment_method: string;
  payment_status: string;
  order_details: OrderDetailResponseDTO[];
}

export interface OrderDetailDTO {
  order_id: number;
  product_id: number;
  quantity: number;
  unit_price: number;
  total_money: number;
  size: string;
}

export interface OrderDetailResponseDTO {
  product_name: string;
  product_thumbnail: string;
  quantity: number;
  unit_price: number;
  total_amount: number;
  size: string;
}

export interface MomoInfoOrderDTO {
  order_id: number;
  amount: number;
}
