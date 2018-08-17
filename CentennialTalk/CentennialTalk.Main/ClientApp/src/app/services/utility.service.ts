import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt'

@Injectable()
export class UtilityService {
  constructor(private activatedRoute: ActivatedRoute,
    private router: Router) { }

  setLocalChatData(chatData) {
    localStorage.setItem("chatData", JSON.stringify(chatData));
  }

  getLocalChatData() {
    var chatString = localStorage.getItem("chatData");

    if (chatString != null && chatString != '')
      return JSON.parse(chatString);
    else
      return {};
  }

  setJwtToken(token) {
    localStorage.setItem("jwt", token);
  }

  getJwtToken() {
    return localStorage.getItem("jwt");
  }

  setLocalCredentials(login) {
    localStorage.setItem("login", JSON.stringify(login));
  }

  isJwtValid() {
    var token = this.getJwtToken();

    return token != null
      && token != ''
      && !(new JwtHelperService().isTokenExpired(token));
  }

  getLocalCredentials() {
    var loginString = localStorage.getItem("login");

    if (loginString != null && loginString != '')
      return JSON.parse(loginString);
    else
      return {};
  }

  navigateWithData(path, data) {
    this.router.navigate([path], data);
  }

  navigateToPath(path) {
    this.router.navigate([path]);
  }
}
