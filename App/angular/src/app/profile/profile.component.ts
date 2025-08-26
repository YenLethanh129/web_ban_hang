import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from '../services/token.service';
import { UserDTO } from '../dtos/user.dto';

@Component({
  selector: 'app-profile',
  imports: [RouterModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  constructor(
    private router: Router,
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  profile: UserDTO | undefined;

  ngOnInit(): void {
    this.userService.getUser().subscribe(
      (profile) => {
        this.profile = profile;
      },
      (error) => {
        console.error('Error fetching user profile:', error);
      }
    );
  }
}
