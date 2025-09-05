import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  Observable,
  of,
  debounceTime,
  distinctUntilChanged,
  switchMap,
} from 'rxjs';
import { AddressSearchResponse, AddressPrediction } from '../dtos/address.dto';
import { WebEnvironment } from '../environments/WebEnvironment';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private readonly baseUrl = WebEnvironment.apiUrl;

  // Default location coordinates (Ho Chi Minh City center)
  private readonly defaultLocation = {
    lat: 10.8033079,
    lng: 106.6409201,
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
}
