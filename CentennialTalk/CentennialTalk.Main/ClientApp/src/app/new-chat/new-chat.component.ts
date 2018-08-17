import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { LoginModel } from '../../models/login.model';

@Component({
  selector: 'app-new-chat',
  templateUrl: './new-chat.component.html',
  styleUrls: ['./new-chat.component.css']
})

export class NewChatComponent implements OnInit {
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
        console.log(res);

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
