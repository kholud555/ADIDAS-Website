import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import * as L from 'leaflet';

import { DeliverymanService } from '../services/deliveryman.service';
import { CustomValidators } from '../services/validators.service';
import { DeliveryManRegistration } from '../models/deliveryman.model';

@Component({
  selector: 'app-deliveryman-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
  template: `
    <div class="container">
      <div class="registration-wrapper">
        <div class="card">
          <div class="header">
            <h1 class="title">Join Our Delivery Team</h1>
            <p class="subtitle">Register as a delivery partner and start earning today!</p>
          </div>

          <form [formGroup]="registrationForm" (ngSubmit)="onSubmit()" class="registration-form">
            
            <!-- Username Field -->
            <div class="form-group">
              <label class="form-label" for="userName">Username *</label>
              <input
                type="text"
                id="userName"
                formControlName="userName"
                class="form-input"
                [class.error]="isFieldInvalid('userName')"
                [class.success]="isFieldValid('userName')"
                placeholder="Enter your username"
              />
              <span *ngIf="isFieldInvalid('userName')" class="error-message">
                <span *ngIf="registrationForm.get('userName')?.errors?.['required']">Username is required</span>
                <span *ngIf="registrationForm.get('userName')?.errors?.['minlength']">Username must be at least 3 characters</span>
              </span>
            </div>

            <!-- Email Field -->
            <div class="form-group">
              <label class="form-label" for="email">Email Address *</label>
              <input
                type="email"
                id="email"
                formControlName="email"
                class="form-input"
                [class.error]="isFieldInvalid('email')"
                [class.success]="isFieldValid('email')"
                placeholder="your@email.com"
              />
              <span *ngIf="isFieldInvalid('email')" class="error-message">
                <span *ngIf="registrationForm.get('email')?.errors?.['required']">Email is required</span>
                <span *ngIf="registrationForm.get('email')?.errors?.['invalidEmail']">Please enter a valid email address</span>
              </span>
            </div>

            <!-- Phone Field -->
            <div class="form-group">
              <label class="form-label" for="phoneNumber">Phone Number *</label>
              <input
                type="tel"
                id="phoneNumber"
                formControlName="phoneNumber"
                class="form-input"
                [class.error]="isFieldInvalid('phoneNumber')"
                [class.success]="isFieldValid('phoneNumber')"
                placeholder="01xxxxxxxxx"
              />
              <span *ngIf="isFieldInvalid('phoneNumber')" class="error-message">
                <span *ngIf="registrationForm.get('phoneNumber')?.errors?.['required']">Phone number is required</span>
                <span *ngIf="registrationForm.get('phoneNumber')?.errors?.['invalidPhone']">Phone must start with 01 and be 11 digits</span>
              </span>
            </div>

            <!-- Password Field -->
            <div class="form-group">
              <label class="form-label" for="password">Password *</label>
              <div class="password-input-wrapper">
                <input
                  [type]="showPassword ? 'text' : 'password'"
                  id="password"
                  formControlName="password"
                  class="form-input"
                  [class.error]="isFieldInvalid('password')"
                  [class.success]="isFieldValid('password')"
                  placeholder="Enter secure password"
                />
                <button
                  type="button"
                  class="password-toggle"
                  (click)="togglePasswordVisibility()"
                >
                  {{ showPassword ? '🙈' : '👁️' }}
                </button>
              </div>
              <div class="password-requirements">
                <small class="requirement" [class.met]="hasUpperCase">✓ At least one uppercase letter</small>
                <small class="requirement" [class.met]="hasSpecialChar">✓ At least one special character</small>
                <small class="requirement" [class.met]="hasMinLength">✓ At least 8 characters</small>
              </div>
              <span *ngIf="isFieldInvalid('password')" class="error-message">
                <span *ngIf="registrationForm.get('password')?.errors?.['required']">Password is required</span>
              </span>
            </div>

            <!-- Location Section -->
            <div class="form-group">
              <label class="form-label">Your Location *</label>
              <p class="location-info">Click on the map to set your delivery location</p>
              <div id="map" class="map-container"></div>
              <div class="location-display" *ngIf="selectedLocation">
                <p><strong>Selected Location:</strong></p>
                <p>Latitude: {{ selectedLocation.lat | number:'1.4-4' }}</p>
                <p>Longitude: {{ selectedLocation.lng | number:'1.4-4' }}</p>
              </div>
              <span *ngIf="!selectedLocation && formSubmitted" class="error-message">
                Please select your location on the map
              </span>
            </div>

            <!-- Terms Agreement -->
            <div class="checkbox-container">
              <input
                type="checkbox"
                id="agreeTerms"
                formControlName="agreeTerms"
                class="checkbox-input"
              />
              <label for="agreeTerms" class="checkbox-label">
                I agree to the <a href="#" class="terms-link">Terms and Conditions</a> and 
                <a href="#" class="terms-link">Privacy Policy</a> *
              </label>
            </div>
            <span *ngIf="isFieldInvalid('agreeTerms')" class="error-message">
              You must agree to the terms and conditions
            </span>

            <!-- Submit Button -->
            <button
              type="submit"
              class="btn btn-primary submit-btn"
              [disabled]="registrationForm.invalid || !selectedLocation || isSubmitting"
            >
              <span *ngIf="isSubmitting" class="loading-spinner"></span>
              {{ isSubmitting ? 'Registering...' : 'Register as Deliveryman' }}
            </button>

            <!-- Success/Error Messages -->
            <div *ngIf="submitMessage" class="submit-message" [class.success]="submitSuccess" [class.error]="!submitSuccess">
              {{ submitMessage }}
            </div>

          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .registration-wrapper {
      min-height: 100vh;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: var(--spacing-lg) 0;
    }

    .header {
      text-align: center;
      margin-bottom: var(--spacing-xl);
    }

    .title {
      font-size: 2.5rem;
      font-weight: 700;
      color: var(--title-color);
      margin-bottom: var(--spacing-sm);
      background: linear-gradient(135deg, var(--first-color), var(--first-color-alt));
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
    }

    .subtitle {
      font-size: 1.1rem;
      color: var(--text-color-light);
      margin-bottom: 0;
    }

    .registration-form {
      max-width: 600px;
      margin: 0 auto;
    }

    .password-input-wrapper {
      position: relative;
    }

    .password-toggle {
      position: absolute;
      right: 12px;
      top: 50%;
      transform: translateY(-50%);
      background: none;
      border: none;
      cursor: pointer;
      font-size: 1.2rem;
      color: var(--text-color-light);
      padding: 4px;
    }

    .password-requirements {
      margin-top: var(--spacing-xs);
      display: flex;
      flex-direction: column;
      gap: 2px;
    }

    .requirement {
      font-size: 0.8rem;
      color: var(--error-color);
      transition: color 0.3s ease;
    }

    .requirement.met {
      color: var(--success-color);
    }

    .location-info {
      font-size: 0.9rem;
      color: var(--text-color-light);
      margin-bottom: var(--spacing-sm);
      font-style: italic;
    }

    .location-display {
      margin-top: var(--spacing-sm);
      padding: var(--spacing-sm);
      background-color: var(--container-color);
      border-radius: var(--border-radius-sm);
      font-size: 0.9rem;
    }

    .location-display p {
      margin: 2px 0;
    }

    .terms-link {
      color: var(--first-color);
      text-decoration: none;
      font-weight: 500;
    }

    .terms-link:hover {
      text-decoration: underline;
    }

    .submit-btn {
      width: 100%;
      margin-top: var(--spacing-lg);
      font-size: 1.1rem;
      padding: var(--spacing-md) var(--spacing-lg);
    }

    .submit-message {
      margin-top: var(--spacing-md);
      padding: var(--spacing-sm);
      border-radius: var(--border-radius-sm);
      text-align: center;
      font-weight: 500;
    }

    .submit-message.success {
      background-color: rgba(134, 142, 150, 0.1);
      color: var(--success-color);
      border: 1px solid var(--success-color);
    }

    .submit-message.error {
      background-color: rgba(231, 76, 60, 0.1);
      color: var(--error-color);
      border: 1px solid var(--error-color);
    }

    @media (max-width: 768px) {
      .title {
        font-size: 2rem;
      }
      
      .registration-form {
        max-width: 100%;
      }
    }
  `]
})
export class DeliverymanRegistrationComponent implements OnInit, AfterViewInit, OnDestroy {
  registrationForm!: FormGroup;
  showPassword = false;
  selectedLocation: { lat: number; lng: number } | null = null;
  map!: L.Map;
  marker!: L.Marker;
  isSubmitting = false;
  formSubmitted = false;
  submitMessage = '';
  submitSuccess = false;

  // Password validation flags
  hasUpperCase = false;
  hasSpecialChar = false;
  hasMinLength = false;

  constructor(
    private fb: FormBuilder,
    private deliverymanService: DeliverymanService
  ) {}

  ngOnInit() {
    this.initializeForm();
    this.setupPasswordValidation();
  }

  ngAfterViewInit() {
    this.initializeMap();
  }

  ngOnDestroy() {
    if (this.map) {
      this.map.remove();
    }
  }

  initializeForm() {
    this.registrationForm = this.fb.group({
      userName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, CustomValidators.emailValidator()]],
      phoneNumber: ['', [Validators.required, CustomValidators.phoneValidator()]],
      password: ['', [Validators.required, CustomValidators.passwordValidator()]],
      agreeTerms: [false, [Validators.requiredTrue]]
    });
  }

  setupPasswordValidation() {
    this.registrationForm.get('password')?.valueChanges.subscribe(value => {
      if (value) {
        this.hasUpperCase = /[A-Z]/.test(value);
        this.hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(value);
        this.hasMinLength = value.length >= 8;
      } else {
        this.hasUpperCase = false;
        this.hasSpecialChar = false;
        this.hasMinLength = false;
      }
    });
  }

  initializeMap() {
    // Initialize map centered on Egypt
    this.map = L.map('map').setView([30.0444, 31.2357], 10);

    // Add OpenStreetMap tiles (free)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    // Try to get user's current location
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const lat = position.coords.latitude;
          const lng = position.coords.longitude;
          this.map.setView([lat, lng], 15);
          this.setLocationMarker(lat, lng);
        },
        (error) => {
          console.log('Geolocation error:', error);
        }
      );
    }

    // Add click event to map
    this.map.on('click', (e: L.LeafletMouseEvent) => {
      this.setLocationMarker(e.latlng.lat, e.latlng.lng);
    });
  }

  setLocationMarker(lat: number, lng: number) {
    // Remove existing marker
    if (this.marker) {
      this.map.removeLayer(this.marker);
    }

    // Add new marker
    this.marker = L.marker([lat, lng]).addTo(this.map);
    this.selectedLocation = { lat, lng };
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registrationForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched || this.formSubmitted));
  }

  isFieldValid(fieldName: string): boolean {
    const field = this.registrationForm.get(fieldName);
    return !!(field && field.valid && (field.dirty || field.touched));
  }

  onSubmit() {
    this.formSubmitted = true;
    
    if (this.registrationForm.valid && this.selectedLocation) {
      this.isSubmitting = true;
      this.submitMessage = '';

      const registrationData: DeliveryManRegistration = {
        ...this.registrationForm.value,
        latitude: this.selectedLocation.lat,
        longitude: this.selectedLocation.lng
      };

      this.deliverymanService.registerDeliveryman(registrationData).subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.submitSuccess = true;
          this.submitMessage = 'Registration successful! Welcome to our delivery team.';
          this.registrationForm.reset();
          this.selectedLocation = null;
          if (this.marker) {
            this.map.removeLayer(this.marker);
          }
        },
        error: (error) => {
          this.isSubmitting = false;
          this.submitSuccess = false;
          this.submitMessage = error.error?.message || 'Registration failed. Please try again.';
        }
      });
    } else {
      this.submitMessage = 'Please fill all required fields and select your location.';
      this.submitSuccess = false;
    }
  }
}