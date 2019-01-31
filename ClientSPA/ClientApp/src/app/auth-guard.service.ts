import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private router: Router, private authService: AuthService) { }

  canActivate() {
  	if(!this.authService.loggedIn) {
  		return true;
  	}

  	this.router.navigate(['/login']);
  	return false;
  }
}
