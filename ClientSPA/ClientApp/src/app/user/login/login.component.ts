import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login: string = 'mark';
  password: string = '123';
  
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  public signIn(loginForm: any) {
  	this.authService.login(this.login, this.password);
  }

  public clearStorage(): void {
  	this.authService.logout();
  }

}
