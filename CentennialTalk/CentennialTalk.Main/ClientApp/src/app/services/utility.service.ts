import { Injectable } from '@angular/core';
import { Router, Params } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt'
import { AccountService } from './account.service';
import * as jwtDecode from 'jwt-decode';

@Injectable()
export class UtilityService {
  constructor(private router: Router, private accountService: AccountService) { }

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

  showErrorAlert(message) {
  }

  showSuccessAlert(message) {
  }

  getJwtData() {
    let token = this.getJwtToken();

    if (token == null)
      return;

    console.log(new JwtHelperService().urlBase64Decode(token));

    console.log(jwtDecode(token));
  }

  logout() {
    this.accountService.tryLogout().subscribe(res => {
      this.setJwtToken('');
      this.setLocalCredentials('');

      this.navigateToPath('home');
    });
  }
}
