import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnDestroy,
  forwardRef,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { AddressService } from '../../../services/address.service';
import { AddressPrediction } from '../../../dtos/address.dto';

@Component({
  selector: 'app-address-autocomplete',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './address-autocomplete.component.html',
  styleUrl: './address-autocomplete.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AddressAutocompleteComponent),
      multi: true,
    },
  ],
})
export class AddressAutocompleteComponent
  implements OnInit, OnDestroy, ControlValueAccessor
{
  @Input() placeholder: string = 'Nhập địa chỉ của bạn';
  @Input() required: boolean = false;
  @Input() disabled: boolean = false;
  @Input() icon: string = 'fas fa-map-marker-alt';
  @Input() label: string = 'Địa chỉ';
  @Input() showLabel: boolean = true;
  @Input() useCurrentLocation: boolean = false;

  @Output() addressSelected = new EventEmitter<AddressPrediction>();
  @Output() inputFocus = new EventEmitter<void>();
  @Output() inputBlur = new EventEmitter<void>();

  @ViewChild('addressInput', { static: true })
  addressInput!: ElementRef<HTMLInputElement>;

  value: string = '';
  predictions: AddressPrediction[] = [];
  isLoading: boolean = false;
  showSuggestions: boolean = false;
  selectedIndex: number = -1;
  currentLocation: { lat: number; lng: number } | null = null;

  private destroy$ = new Subject<void>();
  private searchSubject = new Subject<string>();
  private onChange = (value: string) => {};
  private onTouched = () => {};

  constructor(private addressService: AddressService) {}

  ngOnInit(): void {
    this.initializeSearch();
    this.initializeLocation();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeSearch(): void {
    this.addressService
      .createDebouncedSearch(
        this.searchSubject.asObservable(),
        this.currentLocation || undefined
      )
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (predictions) => {
          this.predictions = predictions;
          this.isLoading = false;
          this.showSuggestions = predictions.length > 0;
        },
        error: (error) => {
          console.error('Address search error:', error);
          this.predictions = [];
          this.isLoading = false;
          this.showSuggestions = false;
        },
      });
  }

  private async initializeLocation(): Promise<void> {
    if (this.useCurrentLocation) {
      try {
        this.currentLocation = await this.addressService.getCurrentLocation();
      } catch (error) {
        console.warn('Could not get current location, using default');
      }
    }
  }

  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    const inputValue = target.value;

    this.value = inputValue;
    this.onChange(inputValue);
    this.selectedIndex = -1;

    if (inputValue.trim().length >= 2) {
      this.isLoading = true;
      this.searchSubject.next(inputValue);
    } else {
      this.predictions = [];
      this.showSuggestions = false;
      this.isLoading = false;
    }
  }

  onFocus(): void {
    this.inputFocus.emit();
    if (this.predictions.length > 0) {
      this.showSuggestions = true;
    }
  }

  onBlur(): void {
    this.inputBlur.emit();
    this.onTouched();

    // Delay hiding suggestions to allow click events on suggestions
    setTimeout(() => {
      this.showSuggestions = false;
      this.selectedIndex = -1;
    }, 200);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (!this.showSuggestions || this.predictions.length === 0) return;

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.selectedIndex = Math.min(
          this.selectedIndex + 1,
          this.predictions.length - 1
        );
        break;

      case 'ArrowUp':
        event.preventDefault();
        this.selectedIndex = Math.max(this.selectedIndex - 1, -1);
        break;

      case 'Enter':
        event.preventDefault();
        if (
          this.selectedIndex >= 0 &&
          this.selectedIndex < this.predictions.length
        ) {
          this.selectPrediction(this.predictions[this.selectedIndex]);
        }
        break;

      case 'Escape':
        this.showSuggestions = false;
        this.selectedIndex = -1;
        this.addressInput.nativeElement.blur();
        break;
    }
  }

  selectPrediction(prediction: AddressPrediction): void {
    this.value = prediction.description;
    this.onChange(this.value);
    this.showSuggestions = false;
    this.selectedIndex = -1;
    this.predictions = [];

    // Emit the selected address
    this.addressSelected.emit(prediction);
  }

  clearInput(): void {
    this.value = '';
    this.onChange('');
    this.predictions = [];
    this.showSuggestions = false;
    this.selectedIndex = -1;
    this.addressInput.nativeElement.focus();
  }

  // ControlValueAccessor implementation
  writeValue(value: string): void {
    this.value = value || '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
