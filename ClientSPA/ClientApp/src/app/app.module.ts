import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { RequestOptions, HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthRequestOptions } from './auth-request-options';
import  {AuthErrorHandler } from './auth-error-handler';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    LoginComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    //HttpModule
  ],
  providers: [
  {
  	provide: RequestOptions,
  	useClass: AuthRequestOptions
  },
  {
  	provide: ErrorHandler,
  	useClass: AuthErrorHandler
  }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
