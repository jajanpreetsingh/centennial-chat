import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class MessageService {
  constructor(private http: Http) {
  }

  getChatMessages(chatCode) {
    return this.http.post("/api/message/messages", { 'value':chatCode })
      .map(res => res.json());
  }

  saveMessage(messageData) {
    console.log('sending');
    return this.http.post("/api/message/send", messageData)
      .map(res => res.json());
  }
}
