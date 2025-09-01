import { ApplicationConfig, PLATFORM_ID } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { StorageService } from './services/storage.service';
import { NotificationService } from './services/notification.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch()),
    provideAnimations(),
    StorageService,
    NotificationService,
    // {
    //   provide: PLATFORM_ID,
    //   useValue: typeof window === 'undefined' ? 'server' : 'browser',
    // },
  ],
};
