import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  constructor(public router: Router) {}
  authRoutes = ['/login', '/register'];

  isAuthRoute(): boolean {
    return this.authRoutes.some((route) => this.router.url.includes(route));
  }
}
