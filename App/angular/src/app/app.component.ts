import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from "./components/footer/footer.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, FooterComponent],
  templateUrl: './app.component.html',
  styles: '',
})
export class AppComponent {
  title = 'Chú bé VUI Coffee';
}
