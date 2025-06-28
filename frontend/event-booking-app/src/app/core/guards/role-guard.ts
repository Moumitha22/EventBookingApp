import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    const expectedRoles: string[] = route.data['expectedRoles'] ?? [];
    const user = this.auth.currentUser;

    if (!user) {
      return this.router.createUrlTree(['/login'], {
        queryParams: { returnUrl: state.url }
      });
    }

    if (expectedRoles.length && !expectedRoles.includes(user.role)) {
      alert(`Access denied. Please login as: ${expectedRoles.join(', ')}`);
      return this.router.createUrlTree(['/login']);
    }

    return true;
  }
}
