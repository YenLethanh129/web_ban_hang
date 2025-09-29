import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  Observable,
  of,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  map,
  catchError,
} from 'rxjs';
import { AddressSearchResponse, AddressPrediction } from '../dtos/address.dto';
import * as OpenLocationCode from 'open-location-code';
import { WebEnvironment } from '../environments/WebEnvironment';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private readonly baseUrl = WebEnvironment.apiUrl;

  // Default location coordinates (Ho Chi Minh City center)
  private readonly defaultLocation = {
    lat: 10.8068205,
    lng: 106.6614606,
  };

  constructor(private http: HttpClient) {}

  /**
   * Search for address suggestions based on input text
   * @param input - The address text to search for
   * @param location - Optional location coordinates for proximity search
   * @param limit - Maximum number of results (default: 20)
   */
  searchAddresses(
    input: string,
    location?: { lat: number; lng: number },
    limit: number = 20
  ): Observable<AddressPrediction[]> {
    if (!input || input.trim().length < 2) {
      return of([]);
    }

    // Use provided location or default
    const searchLocation = location || this.defaultLocation;

    let params = new HttpParams()
      .set('input', input.trim())
      .set('location', `${searchLocation.lat},${searchLocation.lng}`)
      .set('limit', limit.toString());

    return this.http
      .get<AddressSearchResponse>(`${this.baseUrl}/location/search`, { params })
      .pipe(
        switchMap((response) => {
          if (response.status === 'OK' && response.predictions) {
            return of(response.predictions);
          }
          return of([]);
        })
      );
  }

  getAddressByPlaceId(placeId: string): Observable<AddressPrediction | null> {
    if (!placeId) {
      return of(null);
    }
    const params = new HttpParams().set('place_id', placeId);
    return this.http
      .get<AddressPrediction>(`${this.baseUrl}/location/details`, { params })
      .pipe(
        catchError((error) => {
          console.error('Error fetching address by place ID:', error);
          return of(null);
        })
      );
  }

  /**
   * Create an observable that debounces address searches
   * @param input$ - Observable of search input
   * @param location - Optional location coordinates
   * @param debounceMs - Debounce time in milliseconds (default: 300)
   */
  createDebouncedSearch(
    input$: Observable<string>,
    location?: { lat: number; lng: number },
    debounceMs: number = 300
  ): Observable<AddressPrediction[]> {
    return input$.pipe(
      debounceTime(debounceMs),
      distinctUntilChanged(),
      switchMap((input) => this.searchAddresses(input, location))
    );
  }

  /**
   * Get current user location (requires browser geolocation permission)
   */
  getCurrentLocation(): Promise<{ lat: number; lng: number }> {
    return new Promise((resolve, reject) => {
      if (!navigator.geolocation) {
        reject(new Error('Geolocation is not supported by this browser'));
        return;
      }

      navigator.geolocation.getCurrentPosition(
        (position) => {
          resolve({
            lat: position.coords.latitude,
            lng: position.coords.longitude,
          });
        },
        (error) => {
          console.warn('Could not get current location:', error);
          // Fallback to default location
          resolve(this.defaultLocation);
        },
        {
          enableHighAccuracy: true,
          timeout: 5000,
          maximumAge: 300000, // 5 minutes
        }
      );
    });
  }

  /**
   * Nếu khoảng cách nhỏ hơn 10km thì trả về true, ngược lại false
   * @param place_id Mã của địa chỉ cần kiểm tra
   * @param radiusKm Bán kính tối đa (km)
   */
  async isWithinDefaultRadius(
    place_id: string,
    radiusKm: number = 10
  ): Promise<boolean> {
    try {
      const addressCoords = await this.getAddressByPlaceId(
        place_id
      ).toPromise();
      if (!addressCoords) return false;

      // Extract lat/lng from addressCoords.result.geometry.location
      const coords = (addressCoords as any).result?.geometry?.location;

      if (
        !coords ||
        typeof coords.lat !== 'number' ||
        typeof coords.lng !== 'number'
      ) {
        return false;
      }

      const distance = this.distanceBetweenCoords(
        this.defaultLocation.lat,
        this.defaultLocation.lng,
        coords.lat,
        coords.lng
      );
      return distance <= radiusKm;
    } catch (error) {
      return false;
    }
  }

  private distanceCache = new Map<string, number>();

  distanceBetweenCoords(
    lat1: number,
    lng1: number,
    lat2: number,
    lng2: number
  ): number {
    const cacheKey = `${lat1},${lng1}-${lat2},${lng2}`;
    if (this.distanceCache.has(cacheKey)) {
      return this.distanceCache.get(cacheKey)!;
    }
    const toRad = (value: number) => (value * Math.PI) / 180;
    const R = 6371; // Bán kính Trái Đất trong km
    const dLat = toRad(lat2 - lat1);
    const dLng = toRad(lng2 - lng1);
    const a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(toRad(lat1)) *
        Math.cos(toRad(lat2)) *
        Math.sin(dLng / 2) *
        Math.sin(dLng / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    const distance = R * c; // Khoảng cách tính bằng km
    this.distanceCache.set(cacheKey, distance);
    console.log('Calculated distance:', distance, 'km');
    return distance;
  }
}
