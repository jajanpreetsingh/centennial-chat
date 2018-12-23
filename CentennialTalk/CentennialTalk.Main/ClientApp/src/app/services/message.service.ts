import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { MemberReaction, MessageModel } from '../../models/message.model';

@Injectable()
export class MessageService {
  constructor(private http: Http) {
  }

  getChatMessages(chatCode: string) {
    return this.http.post("/api/message/messages", { 'value': chatCode })
      .map(res => res.json());
  }

  saveMessage(messageData: MessageModel) {
    console.log('sending', messageData);
    return this.http.post("/api/message/send", messageData)
      .map(res => res.json());
  }

  getReaction(reaction: MemberReaction) {
    return this.http.post("/api/message/reaction", reaction)
      .map(res => res.json());
  }
}
