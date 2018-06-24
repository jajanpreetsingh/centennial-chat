import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-new-chat-login',
  templateUrl: './new-chat-login.component.html',
  styleUrls: ['./new-chat-login.component.css']
})

export class NewChatLoginComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  title: string
  amIModerator: boolean;

  constructor(private chatService: ChatService) {
  }

  ngOnInit() {
  }

  onChangeModerator() {
    this.username = this.moderator;
  }

  onSubmitNewChat() {

    this.chatService.createNewChat(
        {
          "moderator": this.moderator,
          "title": this.title
        }
    ).subscribe(data => {
      window.location.href = '/chat?data=' + data;
    });
  }
}
