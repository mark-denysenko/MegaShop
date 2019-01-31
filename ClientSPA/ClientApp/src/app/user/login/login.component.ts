import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../auth.service';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login: string = 'test';
  password: string = '123';
  
  constructor(private authService: AuthService, private _httpClient: HttpClient) { }

  ngOnInit() {
  }

  public signIn(loginForm: any){
  	this.authService.login(this.login, this.password);
  }

  public clearStorage(): void {
  	this.authService.logout();
  }

}
