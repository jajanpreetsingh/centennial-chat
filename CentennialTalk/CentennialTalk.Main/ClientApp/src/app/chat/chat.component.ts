import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';
import { MemberService } from '../services/member.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  amIModerator: boolean;
  title: string;

  isConnected: boolean;

  //members: string[] = [];

  private hubConnection: HubConnection;
  message = '';
  messages: string[] = [];

  constructor(private chatService: ChatService,
    private memberService: MemberService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      console.log(this.activatedRoute.snapshot.queryParams);

      this.username = this.activatedRoute.snapshot.queryParams['username'];
      this.chatCode = this.activatedRoute.snapshot.queryParams['chatCode'];
      this.moderator = this.activatedRoute.snapshot.queryParams['moderator'];
      this.title = this.activatedRoute.snapshot.queryParams['title'];
    });

    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl('/discussion')
      .build();

    this.hubConnection.start();

    this.hubConnection.on('connectionStarted', data => {
      this.onConnectionStart(data);      
    });

    this.hubConnection.on('connectionAborted', data => {
      this.onConnectionAbort(data);
    });

    this.hubConnection.on('userJoined', data => {
      this.onUserJoined(data);
    });

    this.hubConnection.on('userLeft', data => {
      this.onUserLeft(data);
    });

    this.hubConnection.on('messageReceived', data => {
      this.onMessageReceived(data);
    });
  }

  onConnectionStart(data) {
    console.log('connection id', data);
    this.isConnected = true;

    this.hubConnection.invoke('JoinGroupChat', JSON.stringify({
      username: this.username,
      chatCode: this.chatCode
    }));
  }

  onConnectionAbort(data) {
    this.hubConnection.invoke('LeaveGroupChat', JSON.stringify({
      username: this.username,
      chatCode: this.chatCode
    }));
  }

  onUserJoined(data) {
    console.log(data + ' joined');
  }

  onUserLeft(data) {
    console.log(data + ' left');
  }

  onMessageReceived(data) {
    console.log(data);
    if (data.chatCode == this.chatCode) {
      this.messages.push(data.content);
    }
  }

  sendMessage() {
    this.hubConnection.invoke('Send', JSON.stringify({
      sender: this.username,
      chatCode: this.chatCode,
      content: this.message
    }));
  }
}
