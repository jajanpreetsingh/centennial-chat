import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { Router, ActivatedRoute, Params } from '@angular/router';
import 'rxjs/add/operator/map';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AccountService {
  constructor(private http: Http,
    private activatedRoute: ActivatedRoute, private router: Router) {
  }

  tryModeratorLogin(login) {
    return this.http.post("/api/auth/login", login)
      .map(res => res.json());
  }

  tryModeratorSignup(signup) {
    return this.http.post("/api/auth/signup", signup)
      .map(res => res.json());
  }

  isUserLoggedIn() {
    var token = localStorage.getItem("jwt");
    var isExpired = token != null
      && token != ''
      && new JwtHelperService().isTokenExpired(token);

    return isExpired;
  }
}
