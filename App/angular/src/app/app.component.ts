import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { MaterialModule } from './material.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, HeaderComponent, FooterComponent, MaterialModule],
  templateUrl: './app.component.html',
  styles: '',
})
export class AppComponent {
  title = 'Chú bé VUI Coffee';
}
