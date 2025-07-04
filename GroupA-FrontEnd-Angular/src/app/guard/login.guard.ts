import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';

export const LoginGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  // let service = inject(LoginComponent);
  const router: Router = inject(Router);
  let token = window.sessionStorage.getItem('token');
  if (token != undefined) return true;
  else {
    router.navigateByUrl('/home');
    return false;
  }
};