import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { ChatModel } from '../../models/chat.model';
import { saveAs as importedSaveAs } from "file-saver";

@Injectable()
export class ChatService {
  constructor(private http: Http) {
  }

  createNewChat(newChat: ChatModel) {
    return this.http.post("/api/chat/new", newChat)
      .map(res => res.json());
  }

  joinChat(joinChat) {
    return this.http.post("/api/chat/join", joinChat)
      .map(res => res.json());
  }

  downloadTranscript(chatCode: string) {
    return this.http.post("/api/chat/transcript", { 'value': chatCode })
      .map(res => res.json());
  }

  getChatList(chatCode: string) {
    return this.http.post("/api/chat/list", { 'value': chatCode })
      .map(res => res.json());
  }
}
