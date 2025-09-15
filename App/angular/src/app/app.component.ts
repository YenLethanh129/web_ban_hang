import { Component, OnInit, AfterViewInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { MaterialModule } from './material.module';
import { DataLoadingService } from './services/data-loading.service';
import { AuthInitService } from './services/auth-init.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, HeaderComponent, FooterComponent, MaterialModule],
  templateUrl: './app.component.html',
  styles: '',
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'Chú bé VUI Coffee';

  constructor(
    private dataLoadingService: DataLoadingService,
    private authInitService: AuthInitService
  ) {}

  ngOnInit() {
    // Initialize app data on startup
    this.initializeApp();
  }

  ngAfterViewInit() {
    // Không tự động chạy auth check nữa - để cho guard xử lý
    // Auth check sẽ được xử lý bởi AuthGuard khi cần thiết
    console.log(
      '✅ App view initialized - Auth check sẽ được xử lý bởi guards'
    );
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
