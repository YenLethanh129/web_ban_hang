export interface ProductDTO {
  id: number;
  name: string;
  price: number;
  thumbnail: string;
  description: string;
  created_at: string;
  updated_at: string;
  category_id: number;
  images: string[];
}

export interface ProductResponse {
  products: ProductDTO[];
  totalPage: number;
  totalItem: number;
}
