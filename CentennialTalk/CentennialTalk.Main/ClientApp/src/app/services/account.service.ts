import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { JwtHelper } from 'angular2-jwt';
import 'rxjs/add/operator/map';
import { Globals } from '../../models/globals';
import { Response } from '../../models/response.model';
import { SignupModel } from '../../models/signup.model';
import { LoginModel } from '../../models/login.model';
import { ChatModel } from '../../models/chat.model';

@Injectable()
export class AccountService {
  constructor(private http: Http, private globals: Globals) {
  }

  tryModeratorLogin(login: LoginModel) {
    return this.http.post("/api/auth/login", login)
      .map(res => res.json());
  }

  tryModeratorSignup(signup: SignupModel) {
    return this.http.post("/api/auth/signup", signup)
      .map(res => res.json());
  }

  tryLogout() {
    return this.http.get("/api/auth/logout")
      .map(res => res.json());
  }

  setLocalChatData(chatData: ChatModel) {
    localStorage.setItem(StorageKeys[StorageKeys.Chat], JSON.stringify(chatData));
  }

  getLocalChatData() {
    var chatString = localStorage.getItem(StorageKeys[StorageKeys.Chat]);

    if (chatString != null && chatString != '')
      return JSON.parse(chatString);
    else
      return {};
  }

  setJwtToken(token: string) {
    localStorage.setItem(StorageKeys[StorageKeys.JwtToken], token);
  }

  getJwtToken() {
    return localStorage.getItem(StorageKeys[StorageKeys.JwtToken]);
  }

  setLocalCredentials(login: LoginModel) {
    if (login != null) {
      localStorage.setItem(StorageKeys[StorageKeys.Login], JSON.stringify(login));

      this.globals.loginData = login;
    }
    else {
      localStorage.setItem(StorageKeys[StorageKeys.Login], '');

      this.globals.loginData = null;
    }
  }

  isJwtValid() {
    var token = this.getJwtToken();

    return token != null
      && token != ''
      && !(new JwtHelper().isTokenExpired(token));
  }

  getLocalCredentials() {
    var loginString = localStorage.getItem(StorageKeys[StorageKeys.Login]);

    if (loginString != null && loginString != '')
      return JSON.parse(loginString);
    else
      return {};
  }

  setIcon(name: string) {
    localStorage.setItem(StorageKeys[StorageKeys.IconName], name);

    this.globals.iconName = name;
  }

  getIcon() {
    return localStorage.getItem(StorageKeys[StorageKeys.IconName]);
  }

  getGlobals() {
    return this.globals;
  }

  getJwtData() {
    let token = this.getJwtToken();

    if (token == null)
      return;

    console.log(new JwtHelper().urlBase64Decode(token));
  }
}

export enum StorageKeys {
  Login,
  Chat,
  JwtToken,
  IconName
}
