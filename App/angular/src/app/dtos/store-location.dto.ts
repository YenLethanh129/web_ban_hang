export interface StoreLocationDTO {
  id: number;
  name: string;
  address: string;
  phone: string;
  hours: string;
  image: string;
  mapUrl: string;
  status: 'open' | 'closed' | 'coming-soon';
  statusText: string;
  latitude?: number;
  longitude?: number;
}

// Mock data for store locations
export const MOCK_STORE_LOCATIONS: StoreLocationDTO[] = [
  {
    id: 1,
    name: 'Funny Boy Coffee - Quận 1',
    address: '123 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP.HCM',
    phone: '028 3822 1234',
    hours: '6:00 - 22:00 (Thứ 2 - Chủ nhật)',
    image:
      'https://images.unsplash.com/photo-1554118811-1e0d58224f24?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=123+Nguyen+Hue+Street,+District+1,+Ho+Chi+Minh+City',
    status: 'open',
    statusText: 'Đang mở cửa',
    latitude: 10.7769,
    longitude: 106.7009,
  },
  {
    id: 2,
    name: 'Funny Boy Coffee - Quận 3',
    address: '456 Đường Võ Văn Tần, Phường 6, Quận 3, TP.HCM',
    phone: '028 3930 5678',
    hours: '6:30 - 21:30 (Thứ 2 - Chủ nhật)',
    image:
      'https://images.unsplash.com/photo-1501339847302-ac426a4a7cbb?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=456+Vo+Van+Tan+Street,+District+3,+Ho+Chi+Minh+City',
    status: 'open',
    statusText: 'Đang mở cửa',
    latitude: 10.7859,
    longitude: 106.6926,
  },
  {
    id: 3,
    name: 'Funny Boy Coffee - Quận 7',
    address: '789 Đường Nguyễn Thị Thập, Phường Tân Phú, Quận 7, TP.HCM',
    phone: '028 5413 9012',
    hours: '7:00 - 22:30 (Thứ 2 - Chủ nhật)',
    image:
      'https://images.unsplash.com/photo-1522992319-0365e5f11656?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=789+Nguyen+Thi+Thap+Street,+District+7,+Ho+Chi+Minh+City',
    status: 'open',
    statusText: 'Đang mở cửa',
    latitude: 10.7411,
    longitude: 106.7222,
  },
  {
    id: 4,
    name: 'Funny Boy Coffee - Thủ Đức',
    address: '321 Đường Võ Văn Ngân, Phường Linh Chiểu, TP. Thủ Đức, TP.HCM',
    phone: '028 7106 3456',
    hours: '6:00 - 23:00 (Thứ 2 - Chủ nhật)',
    image:
      'https://images.unsplash.com/photo-1559925393-8be0ec4767c8?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=321+Vo+Van+Ngan+Street,+Thu+Duc+City,+Ho+Chi+Minh+City',
    status: 'open',
    statusText: 'Đang mở cửa',
    latitude: 10.8505,
    longitude: 106.7717,
  },
  {
    id: 5,
    name: 'Funny Boy Coffee - Bình Thạnh',
    address: '654 Đường Xô Viết Nghệ Tĩnh, Phường 25, Quận Bình Thạnh, TP.HCM',
    phone: '028 3844 7890',
    hours: '6:30 - 22:00 (Thứ 2 - Chủ nhật)',
    image:
      'https://images.unsplash.com/photo-1442512595331-e89e73853f31?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=654+Xo+Viet+Nghe+Tinh+Street,+Binh+Thanh+District,+Ho+Chi+Minh+City',
    status: 'closed',
    statusText: 'Đã đóng cửa',
    latitude: 10.8012,
    longitude: 106.7148,
  },
  {
    id: 6,
    name: 'Funny Boy Coffee - Hà Nội (Sắp mở)',
    address: '888 Phố Hoàn Kiếm, Quận Hoàn Kiếm, Hà Nội',
    phone: '024 3926 1111',
    hours: 'Sắp mở cửa - Dự kiến tháng 10/2025',
    image:
      'https://images.unsplash.com/photo-1509042239860-f550ce710b93?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80',
    mapUrl:
      'https://maps.google.com/?q=888+Hoan+Kiem+Street,+Hoan+Kiem+District,+Hanoi',
    status: 'coming-soon',
    statusText: 'Sắp mở cửa',
    latitude: 21.0285,
    longitude: 105.8542,
  },
];
