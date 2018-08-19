import { Component, OnInit, OnDestroy } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-new-chat',
  templateUrl: './new-chat.component.html',
  styleUrls: ['./new-chat.component.css']
})

export class NewChatComponent implements OnInit, OnDestroy {
  chatData: ChatModel = new ChatModel();

  constructor(private chatService: ChatService, private utilityService: UtilityService) {
  }

  ngOnInit() {
    if (this.utilityService.isJwtValid()) {

      let login = this.utilityService.getLocalCredentials();

      if (login == null)
        this.utilityService.navigateToPath('/home');
      else {
        this.chatData.username = login.username;
        this.chatData.moderator = login.username;
      }
    }
  }

  onChangeModerator() {
    this.chatData.username = this.chatData.moderator;
  }

  onSubmitNewChat() {
    this.chatService.createNewChat(
      {
        "moderator": this.chatData.moderator,
        "title": this.chatData.title
      }
    ).subscribe(res => {
      if (res.code == 200) {

        this.chatData.moderator = res.data.moderator;
        this.chatData.title = res.data.title;
        this.chatData.chatCode = res.data.chatCode;

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

  ngOnDestroy() {

  }
}
