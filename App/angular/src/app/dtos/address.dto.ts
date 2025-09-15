export interface AddressPrediction {
  description: string;
  reference: string;
  score: number | null;
  place_id: string;
  structured_formatting: {
    main_text: string;
    secondary_text: string;
  };
  plus_code: {
    compound_code: string;
    global_code: string;
  };
  has_children: boolean;
  display_type: string | null;
}

export interface AddressSearchResponse {
  predictions: AddressPrediction[];
  status: string;
  executed_time: number | null;
  executed_time_all: number | null;
}
