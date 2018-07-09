import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-join-chat',
  templateUrl: './join-chat.component.html',
  styleUrls: ['./join-chat.component.css']
})
export class JoinChatComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  title: string;

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
    ).subscribe(res => {
      if (res.code == 200) {
        this.moderator = res.data.moderator;
        this.title = res.data.title;

        this.router.navigate(['/chat'], {
          queryParams: {
            username: this.username,
            moderator: this.moderator,
            title: this.title,
            chatCode: this.chatCode
          }
        });
      }
      else {
        this.username =
          this.moderator =
          this.chatCode =
          this.title = '';
        console.log(res.data);
      }
    },
      error => {
        this.username =
          this.moderator =
          this.chatCode =
          this.title = '';
        console.log(error);
      });
  }
}
