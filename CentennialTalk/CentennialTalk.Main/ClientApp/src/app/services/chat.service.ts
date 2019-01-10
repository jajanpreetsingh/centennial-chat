import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';
import { ChatModel } from '../../models/chat.model';

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
      .subscribe(res => {

        //let url = window.URL.createObjectURL(new Blob(res, { type: 'application/vnd.ms-word' }));
        //var link = document.createElement("a");
        //link.setAttribute("href", url);
        //link.setAttribute("download", "test.docx");
        //link.style.display = "none";
        //document.body.appendChild(link);
        //link.click();
        //document.body.removeChild(link);

      });
  }

  getChatList(chatCode: string) {
    return this.http.post("/api/chat/list", { 'value': chatCode })
      .map(res => res.json());
  }
}
