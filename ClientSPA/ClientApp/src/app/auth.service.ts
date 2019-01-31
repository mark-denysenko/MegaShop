import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http"
import { tap } from 'rxjs/operators';
import { ApiResources } from './api-resources'

export const TOKEN_STORAGE: string = 'access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _httpClient: HttpClient) { }

  public get loggedIn(): boolean {
  	return localStorage.getItem(TOKEN_STORAGE) !== null;
  }

  login(login: string, password: string) {
  	console.log('login AuthServ ' + password +' ' + login);
  	this._httpClient.post<{userid: number, username: string, access_token: string}>(ApiResources.Login, {"login": login, "password": password}).pipe(tap(res => {
  		console.log('res ' + res);
  		localStorage.setItem(TOKEN_STORAGE, res.access_token);
  		console.log('token : ' + res.access_token);
  	})).subscribe(data => console.log(data));
  }

  register(login: string, username: string, password: string) {
  	return this._httpClient.post<{userid: number, login: string, username: string, access_token: string}>(ApiResources.Register, {login, username, password}).pipe(tap(res => {
      console.log("regsitered!");
      this.login(login, password);
  	}))
  }

  logout() {
  	localStorage.removeItem(TOKEN_STORAGE);
  }
}