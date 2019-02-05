import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http"
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiResources } from '../../api-resources'
import { User } from '../../models/user';

export const TOKEN_STORAGE: string = 'access_token';
export const NEW_ACCESS_TOKEN: string = 'new_access_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private _httpClient: HttpClient) {
    //this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    //this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  public get loggedIn(): boolean {
    // parse and check jwt time !
  	return localStorage.getItem(TOKEN_STORAGE) !== null;
  }

  login(login: string, password: string) {
  	return this._httpClient.post<{userid: number, username: string, access_token: string}>(ApiResources.Login, {"login": login, "password": password}).pipe(tap(res => {
  		localStorage.setItem(TOKEN_STORAGE, res.access_token);
  	})).subscribe();
  }

  register(login: string, username: string, password: string) {
  	return this._httpClient.post<{id: number, login: string, name: string}>(ApiResources.Register, {"login": login, "username": username, "password": password}).pipe(tap(res => {
  		if(res.login === login) 
  			this.login(login, password);
  	})).subscribe();
  }

  logout() {
  	localStorage.removeItem(TOKEN_STORAGE);
    this.currentUserSubject.next(null);
  }

  getJwtToken(): string {
    return localStorage.getItem(TOKEN_STORAGE);
  }

  setJwtToken(value: string): void {
    localStorage.setItem(TOKEN_STORAGE, value);
  }
}