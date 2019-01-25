import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { JwtHelper } from 'angular2-jwt';
import 'rxjs/add/operator/map';
import { SignupModel } from '../../models/signup.model';
import { LoginModel } from '../../models/login.model';
import { ChatModel } from '../../models/chat.model';

@Injectable()
export class AccountService {
  constructor(private http: Http) {
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

  verifyEmail(userId: string, code: string) {
    return this.http.post("/api/auth/verify", { 'userId': userId, 'token': code })
      .map(res => res.json());
  }

  sendResetLink(email: string) {
    return this.http.post("/api/auth/resetlink", { 'value': email })
      .map(res => res.json());
  }

  resetPassword(userId: string, code: string, newpassword: string) {
    return this.http.post("/api/auth/reset", { 'userId': userId, 'token': code, 'password': newpassword })
      .map(res => res.json());
  }

  setLocalData(key: StorageKeys, value: string): void {
    localStorage.setItem(StorageKeys[key], JSON.stringify(value));
  }

  getLocalData(key: StorageKeys): string {
    let val = localStorage.getItem(StorageKeys[key]);

    if (JSON.parse(val) != null)
      return JSON.parse(val).toString();
    return "";
  }

  setJwtToken(token: string) {
    this.setLocalData(StorageKeys.JwtToken, token);
  }

  getJwtToken() {
    return this.getLocalData(StorageKeys.JwtToken);
  }

  isJwtValid() {
    var token = this.getJwtToken();

    return token != null
      && token != ''
      && !(new JwtHelper().isTokenExpired(token));
  }

  clearAllLocalData() {
    this.setLocalData(StorageKeys.LoginUsername, '');
    this.setLocalData(StorageKeys.SignupUsername, '');
    this.setLocalData(StorageKeys.SignUpEmail, '');
    this.setLocalData(StorageKeys.JwtToken, '');

    this.clearChatRelatedData();
  }

  clearChatRelatedData() {
    this.setLocalData(StorageKeys.ChatTitle, '');
    this.setLocalData(StorageKeys.ChatCode, '');
    this.setLocalData(StorageKeys.ChatUsername, '');
    this.setLocalData(StorageKeys.ChatModerator, '');
    this.setLocalData(StorageKeys.ChatConnectionId, '');
    this.setLocalData(StorageKeys.PollingQuestions, '');
    this.setLocalData(StorageKeys.OpenQuestions, '');
    this.setLocalData(StorageKeys.ChatMembers, '');
    this.setLocalData(StorageKeys.PublishedQuestion, '');
    this.setLocalData(StorageKeys.ReturnUrl, '');
  }

  isLoggedIn(): boolean {
    let logUsr: string = this.getLocalData(StorageKeys.LoginUsername);
    let jwt: string = this.getLocalData(StorageKeys.JwtToken);

    let loggedin: boolean = !this.isValNull(logUsr) && !this.isValNull(jwt);

    return loggedin;
  }

  getUserId(): string {
    let token: string = this.getJwtToken();

    return new JwtHelper().decodeToken(token).jti;
  }

  getLocalChatData(): ChatModel {
    let chat: ChatModel = new ChatModel();

    chat.chatCode = this.getLocalData(StorageKeys.ChatCode);
    chat.title = this.getLocalData(StorageKeys.ChatTitle);
    chat.moderator = this.getLocalData(StorageKeys.ChatModerator);
    chat.username = this.getLocalData(StorageKeys.ChatUsername);
    chat.connectionId = this.getLocalData(StorageKeys.ChatConnectionId);

    let val = this.getLocalData(StorageKeys.OpenQuestions);

    if (!this.isValNull(val))
      chat.openQuestions = JSON.parse(val);

    let val2 = this.getLocalData(StorageKeys.PollingQuestions);

    if (!this.isValNull(val2))
      chat.pollQuestions = JSON.parse(val2);

    let val3 = this.getLocalData(StorageKeys.ChatMembers);

    if (!this.isValNull(val3))
      chat.members = JSON.parse(val3);

    return chat;
  }

  getJwtData() {
    let token = this.getJwtToken();

    if (token == null)
      return;

    console.log(new JwtHelper().urlBase64Decode(token));
  }

  amIModerator(): boolean {
    let usr = this.getLocalData(StorageKeys.ChatUsername);
    let mod = this.getLocalData(StorageKeys.ChatModerator);

    let amI = ((!this.isValNull(usr)) && (!this.isValNull(mod)) && mod === usr);

    console.log(usr + "  :  " + mod + "  :  " + amI);

    return amI;
  }

  isValNull(val): boolean {
    return val === undefined || val === "" || val === '' || val === null;
  }
}

export enum StorageKeys {
  JwtToken,

  LoginUsername,

  SignupUsername,
  SignUpEmail,

  ChatTitle,
  ChatCode,
  ChatUsername,
  ChatModerator,
  ChatConnectionId,
  PollingQuestions,
  OpenQuestions,
  ChatMembers,

  PublishedQuestion,

  ReturnUrl
}
