import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { ChatModel } from '../../models/chat.model';
import { TranscriptRequestModel } from '../../models/transcriptrequest.model';

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

  downloadTranscript(trm: TranscriptRequestModel) {
    return this.http.post("/api/chat/transcript", trm)
      .map(res => res.json());
  }

  getChatList(chatCode: string) {
    return this.http.post("/api/chat/list", { 'value': chatCode })
      .map(res => res.json());
  }

  download(data: any) {
    let json = atob(data.data);
    let blob = this.base64toBlob(json);

    let url = window.URL.createObjectURL(blob);
    let link = document.createElement("a");
    link.setAttribute("href", url);
    link.setAttribute("download", data.name);
    link.style.display = "none";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  base64toBlob(byteString) {
    var ia = new Uint8Array(byteString.length);
    for (var i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ia], { type: 'application/vnd.ms-word' });
  }
}
