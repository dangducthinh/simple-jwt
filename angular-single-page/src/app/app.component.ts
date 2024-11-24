import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './login/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(private authService: AuthService, private router: Router){}
  title = 'angular-single-page';

  isAuthenticated () {
    return this.authService.isAuthenticated();
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login'])
  }
}
