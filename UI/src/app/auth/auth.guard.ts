import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRole = route.data['role'];
    const userRole = this.authService.getRole();
    if (this.authService.isLoggedIn() && userRole === expectedRole) {
      return true;
    }
    this.router.navigate(['/']); // Điều hướng về login nếu chưa đăng nhập
    return false;
  }
}
