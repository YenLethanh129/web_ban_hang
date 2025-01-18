export interface ProductDTO {
  id: number;
  name: string;
  price: number;
  thumbnail: string;
  description: string;
  created_at: string;
  updated_at: string;
  category_id: number;
}

export interface ProductResponse {
  products: ProductDTO[];
  totalPage: number;
}
