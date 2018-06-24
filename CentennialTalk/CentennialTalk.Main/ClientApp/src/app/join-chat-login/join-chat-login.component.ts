import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-join-chat-login',
  templateUrl: './join-chat-login.component.html',
  styleUrls: ['./join-chat-login.component.css']
})
export class JoinChatLoginComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  amIModerator: boolean;

  constructor(private chatService: ChatService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
  }

  onSubmitJoinChat() {
    this.chatService.joinChat(
      {
        "username": this.username,
        "chatCode": this.chatCode
      }
    ).subscribe(data => {
      console.log('routing..');
      this.router.navigate(['/chat']);
    });
  }
}
