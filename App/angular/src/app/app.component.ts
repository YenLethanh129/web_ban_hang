import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { MaterialModule } from './material.module';
import { DataLoadingService } from './services/data-loading.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, HeaderComponent, FooterComponent, MaterialModule],
  templateUrl: './app.component.html',
  styles: '',
})
export class AppComponent implements OnInit {
  title = 'Chú bé VUI Coffee';

  constructor(private dataLoadingService: DataLoadingService) {}

  ngOnInit() {
    // Initialize app data on startup
    this.initializeApp();
  }

  private async initializeApp() {
    try {
      console.log('🚀 Starting app initialization...');

      // Initialize core data
      const result = await this.dataLoadingService.initializeAppData();

      console.log('✅ App initialization completed:', result);

      // Check connectivity and adjust caching strategy
      this.dataLoadingService.checkConnectivityAndCache().subscribe({
        next: (isOnline) => {
          if (isOnline) {
            console.log('📶 App is online - full functionality available');
          } else {
            console.log('📵 App is offline - using cached data');
          }
        },
        error: (error) => {
          console.warn('Failed to check connectivity:', error);
        },
      });
    } catch (error) {
      console.error('❌ App initialization failed:', error);
      // App can still function with cached data or basic functionality
    }
  }
}
