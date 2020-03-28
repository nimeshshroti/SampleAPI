import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable(
  // we declare that this service should be created
  // by the root application injector.
 )
export class AuthService {

  baseURL = 'https://localhost:44351/api/auth/'
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) { }

  register(model: any) {
    return this.http.post(this.baseURL + 'register', model);
  }


  login(model: any) {
    return this.http.post(this.baseURL + 'login', model)
      .pipe(
      map((response: any) =>
      {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          console.log(this.decodedToken);
        }
      }
        )
      )
  }

  loggedin() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
      }
}
