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
  product_id: number;
  number_of_product: number;
  price: number;
  total_money: number;
  size: string;
  note: string;
  status: string;
  totalMoney: number;
  orderDate: string;
  shippingMethod: string;
  paymentMethod: string;
  paymentStatus: string | null;
  created_at: string;
  updated_at: string;
  orderId: string;
  address: string;
}

export interface OrderDetailDTO {
  order_id: number;
  product_id: number;
  quantity: number;
  unit_price: number;
  total_money: number;
  size: string;
}

export interface MomoInfoOrderDTO {
  order_id: number;
  amount: number;
}
