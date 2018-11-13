import { Injectable } from '@angular/core';
import { LoginModel } from './login.model';
import { ChatModel } from './chat.model';

@Injectable()
export class Globals {
  get isLoggedIn(): boolean {
    return this.loginData != null;
  };

  loginData: LoginModel;

  chatData: ChatModel;
}
