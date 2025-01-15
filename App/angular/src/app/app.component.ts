import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  templateUrl: './app.component.html',
  styles: '',
})
export class AppComponent {
  title = 'Nông sản Thủy Bình';
}
