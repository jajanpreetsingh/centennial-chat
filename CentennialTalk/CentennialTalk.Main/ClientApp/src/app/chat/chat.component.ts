import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';
import { MemberService } from '../services/member.service';
import { MessageService } from '../services/message.service';
import { v4 as uuid } from 'uuid';
import { DatePipe } from '@angular/common';

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
  connectionId: string;

  //members: string[] = [];

  private hubConnection: HubConnection;
  message = '';
  messages: any = [];

  constructor(private chatService: ChatService,
    private memberService: MemberService,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private router: Router, private datePipe: DatePipe) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      console.log(this.activatedRoute.snapshot.queryParams);

      this.username = this.activatedRoute.snapshot.queryParams['username'];
      this.chatCode = this.activatedRoute.snapshot.queryParams['chatCode'];
      this.moderator = this.activatedRoute.snapshot.queryParams['moderator'];
      this.title = this.activatedRoute.snapshot.queryParams['title'];

      this.amIModerator = this.moderator == this.username;

      console.log(this.username);
      console.log(this.chatCode);
      console.log(this.moderator);
      console.log(this.title);
    });

    this.initChatHub();
  }

  initChatHub() {
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

    this.connectionId = data;

    this.isConnected = true;

    this.hubConnection.invoke('JoinGroupChat', JSON.stringify({
      username: this.username,
      chatCode: this.chatCode,
      isModerator: this.amIModerator
    }));

    this.memberService.updateStatus({
      username: this.username,
      chatCode: this.chatCode,
      connectionId: this.connectionId,
      isConnected: true
    }).subscribe(res => {
      console.log(res);
    },
      err => {
        console.log(err);
      });
  }

  onConnectionAbort(data) {
    this.hubConnection.invoke('LeaveGroupChat', JSON.stringify({
      username: this.username,
      chatCode: this.chatCode
    }));

    this.memberService.updateStatus({
      username: this.username,
      chatCode: this.chatCode,
      connectionId: this.connectionId,
      isConnected: false
    }).subscribe(res => {
      console.log(res);
    },
      err => {
        console.log(err);
      });
  }

  onUserJoined(username) {
    console.log(username + ' joined');
  }

  onUserLeft(username) {
    console.log(username + ' left');
  }

  onMessageReceived(messageData) {
    if (messageData.chatCode == this.chatCode) {
      this.messages.push(messageData);
    }
  }

  sendMessage() {
    var content = this.message;

    this.hubConnection.invoke('Send', JSON.stringify({
      sender: this.username,
      chatCode: this.chatCode,
      content: content
    }));

    var mid = uuid();

    this.messageService.saveMessage({
      messageId: mid,
      content: content,
      chatCode: this.chatCode,
      sender: this.username,
      replyId: mid,
      sentDate: this.datePipe.transform(Date.now(), 'yyyy-MM-dd HH:mm:ss')
    }).subscribe(res => {
      console.log(res);
    },
      err => {
        console.log(err);
      });
  }
}
