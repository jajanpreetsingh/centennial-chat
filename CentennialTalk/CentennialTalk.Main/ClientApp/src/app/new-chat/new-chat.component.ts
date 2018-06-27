import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-new-chat',
  templateUrl: './new-chat.component.html',
  styleUrls: ['./new-chat.component.css']
})

export class NewChatComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  title: string

  constructor(private chatService: ChatService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
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
    ).subscribe(res => {
      if (res.code == 200) {
        console.log(res);

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
