import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class ChatService {

  constructor(private http: Http) {
  }

  createNewChat(newChat) {

    console.log("newChat", newChat);

    return this.http.post("/api/chat/new", newChat)
      .map(res => res.json());
  }

  joinChat(joinChat) {

    return this.http.post("/api/chat/join", joinChat)
      .map(res => res.json());
  }
}
