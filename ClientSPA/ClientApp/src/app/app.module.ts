import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

// @auth0/angular-jwt

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './services/auth/auth-interceptor';
import { TOKEN_STORAGE, NEW_ACCESS_TOKEN } from './services/auth/auth.service';
import { ProductComponent } from './products/product/product.component';
import { ProductsListComponent } from './products/products-list/products-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    LoginComponent,
    HomeComponent,
    ProductComponent,
    ProductsListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    // HttpClientXsrfModule.withOptions({
    // 	headerName: NEW_ACCESS_TOKEN,
    // }),
    // JwtModule.forRoot({
    // 	config: {
    // 		tokenGetter: function tokenGetter() {
    // 			return localStorage.getItem(TOKEN_STORAGE);
    // 		},
    // 		whitelistedDomains: ['localhost:44388'],
    // 		blacklistedRoutes: []
    // 	}
    // })
  ],
  providers: [
  { 
    provide: HTTP_INTERCEPTORS, 
    useClass: AuthInterceptor, 
    multi: true 
  }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
