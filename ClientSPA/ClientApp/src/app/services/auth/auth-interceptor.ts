import { Injectable } from '@angular/core';
import { Router } from "@angular/router";
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse
} from '@angular/common/http';
import { tap, map, catchError } from 'rxjs/operators';
import { AuthService, NEW_ACCESS_TOKEN } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
 
  constructor(private router: Router, private authService: AuthService) {}
 
  intercept(req: HttpRequest<any>, next: HttpHandler) { 
    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const jwtToken = this.authService.getJwtToken();

    if(jwtToken != null) {
	    req = req.clone({
	      headers: req.headers.set('Authorization', 'Bearer ' + jwtToken)
	    });
	}

	req = req.clone({ headers: req.headers.set('Content-Type', 'application/json') });
 
    // send cloned request with header to the next handler.
    return next.handle(req)
    .pipe(map((event: HttpEvent<any>) => {
    	if(event instanceof HttpResponse) {
    		
    		let newToken = event.headers.get(NEW_ACCESS_TOKEN);
    		if(newToken != null) {
    			this.authService.setJwtToken(event.headers.get(NEW_ACCESS_TOKEN));
    		}

    		if(event.status === 401 || event.status === 403) {
    			this.authService.logout();
                this.router.navigate(['/login']);
                // location.reload(true);
    		}
    	}
    	return event;
    }));
  }
}