import { Headers, Http, BaseRequestOptions, RequestMethod } from '@angular/http';
import { TOKEN_STORAGE } from './auth.service';

const AUTH_HEADER_KEY = 'Authorization';
const AUTH_PREFIX = 'Bearer';

export class AuthRequestOptions extends BaseRequestOptions {
  
  constructor() {
    super();
    
    const token = localStorage.getItem(TOKEN_STORAGE);
    if(token) {
      this.headers.append(AUTH_HEADER_KEY, `${AUTH_PREFIX} ${token}`);
    }

    if(this.method == RequestMethod.Post) {
    	this.headers.append('Content-Type', 'application/json');
    }
  }
}