import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-join-chat',
  templateUrl: './join-chat.component.html',
  styleUrls: ['./join-chat.component.css']
})
export class JoinChatComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  isLoggedIn: boolean;

  constructor(private chatService: ChatService,
    private utilityService: UtilityService) {
  }

  ngOnInit() {
    if (this.utilityService.isJwtValid()) {
      let cred = this.utilityService.getLocalCredentials();

      if (cred != null)
        this.chatData.username = cred.username;
    }
  }

  onSubmitJoinChat() {
    this.chatService.joinChat(
      {
        "username": this.chatData.username,
        "chatCode": this.chatData.chatCode
      }
    ).subscribe(res => {
      if (res.code == 200) {
        this.chatData.moderator = res.data.moderator;
        this.chatData.title = res.data.title;

        this.utilityService.setLocalChatData(this.chatData);

        this.utilityService.navigateToPath('/chat');
      }
      else {
        this.chatData = new ChatModel();
        console.log(res.data);
      }
    },
      error => {
        this.chatData = new ChatModel();
        console.log(error);
      });
  }
}
