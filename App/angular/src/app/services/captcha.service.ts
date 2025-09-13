import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

export interface CaptchaChallenge {
  question: string;
  answer: number;
  userAnswer?: string;
}

@Injectable({
  providedIn: 'root',
})
export class CaptchaService {
  private failedAttempts = 0;
  private maxAttempts = 3;
  private showCaptchaSubject = new BehaviorSubject<boolean>(false);

  showCaptcha$ = this.showCaptchaSubject.asObservable();

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    // Load failed attempts from localStorage only in browser
    if (isPlatformBrowser(this.platformId)) {
      const savedAttempts = localStorage.getItem('loginFailedAttempts');
      if (savedAttempts) {
        this.failedAttempts = parseInt(savedAttempts, 10);
        if (this.failedAttempts >= this.maxAttempts) {
          this.showCaptchaSubject.next(true);
        }
      }
    }
  }

  generateCaptcha(): CaptchaChallenge {
    const num1 = Math.floor(Math.random() * 10) + 1;
    const num2 = Math.floor(Math.random() * 10) + 1;
    const operations = ['+', '-', '*'];
    const operation = operations[Math.floor(Math.random() * operations.length)];

    let answer: number;
    let question: string;

    switch (operation) {
      case '+':
        answer = num1 + num2;
        question = `${num1} + ${num2} = ?`;
        break;
      case '-':
        answer = Math.max(num1, num2) - Math.min(num1, num2);
        question = `${Math.max(num1, num2)} - ${Math.min(num1, num2)} = ?`;
        break;
      case '*':
        answer = num1 * num2;
        question = `${num1} × ${num2} = ?`;
        break;
      default:
        answer = num1 + num2;
        question = `${num1} + ${num2} = ?`;
    }

    return { question, answer };
  }

  validateCaptcha(challenge: CaptchaChallenge): boolean {
    return parseInt(challenge.userAnswer || '0', 10) === challenge.answer;
  }

  recordFailedAttempt(): void {
    this.failedAttempts++;
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(
        'loginFailedAttempts',
        this.failedAttempts.toString()
      );
    }

    if (this.failedAttempts >= this.maxAttempts) {
      this.showCaptchaSubject.next(true);
    }
  }

  recordSuccessfulLogin(): void {
    this.failedAttempts = 0;
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('loginFailedAttempts');
    }
    this.showCaptchaSubject.next(false);
  }

  shouldShowCaptcha(): boolean {
    return this.failedAttempts >= this.maxAttempts;
  }

  getFailedAttempts(): number {
    return this.failedAttempts;
  }

  getRemainingAttempts(): number {
    return Math.max(0, this.maxAttempts - this.failedAttempts);
  }

  // Reset captcha requirement (for admin or after timeout)
  resetCaptchaRequirement(): void {
    this.failedAttempts = 0;
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('loginFailedAttempts');
    }
    this.showCaptchaSubject.next(false);
  }

  // Check if account should be temporarily locked
  isAccountLocked(): boolean {
    if (!isPlatformBrowser(this.platformId)) return false;

    const lockTime = localStorage.getItem('accountLockTime');
    if (!lockTime) return false;

    const lockTimestamp = parseInt(lockTime, 10);
    const currentTime = Date.now();
    const lockDuration = 15 * 60 * 1000; // 15 minutes

    if (currentTime - lockTimestamp > lockDuration) {
      // Lock expired
      if (isPlatformBrowser(this.platformId)) {
        localStorage.removeItem('accountLockTime');
      }
      this.resetCaptchaRequirement();
      return false;
    }

    return true;
  }

  lockAccount(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('accountLockTime', Date.now().toString());
    }
  }

  getLockRemainingTime(): number {
    if (!isPlatformBrowser(this.platformId)) return 0;

    const lockTime = localStorage.getItem('accountLockTime');
    if (!lockTime) return 0;

    const lockTimestamp = parseInt(lockTime, 10);
    const currentTime = Date.now();
    const lockDuration = 60 * 1000; // 15 minutes
    const elapsed = currentTime - lockTimestamp;

    return Math.max(0, lockDuration - elapsed);
  }
}
