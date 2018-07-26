import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { MessageService } from './message.service';
import { DatePipe } from '@angular/common';
import { MemberService } from './member.service';

@Injectable()
export class HubService {
  private hubConnection: HubConnection;

  private isConnected: boolean;
  private connectionId: string;

  private joinGroupReq: any;

  messages: any = [];

  constructor(private messageService: MessageService, private datePipe: DatePipe,
    private memberService: MemberService) { }

  initChatHub(joinGroupReq) {
    this.joinGroupReq = joinGroupReq;

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

    this.hubConnection.invoke('JoinGroupChat', this.joinGroupReq);

    this.memberService.updateStatus({
      username: this.joinGroupReq.username,
      chatCode: this.joinGroupReq.chatCode,
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
      username: this.joinGroupReq.username,
      chatCode: this.joinGroupReq.chatCode
    }));

    this.memberService.updateStatus({
      username: this.joinGroupReq.username,
      chatCode: this.joinGroupReq.chatCode,
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
    if (messageData.chatCode == this.joinGroupReq.chatCode) {
      this.messages.push(messageData);
    }
  }

  sendMessage(messageObj) {
    messageObj.sentDate = this.datePipe.transform(Date.now(), 'yyyy-MM-dd HH:mm:ss');

    this.hubConnection.invoke('Send', JSON.stringify(messageObj));

    this.messageService.saveMessage(messageObj).subscribe(res => {
      console.log(res);
    },
      err => {
        console.log(err);
      });
  }

  getMessages() {
    return this.messages.sort((a, b) => { a.sentDate - b.sentDate });
  }
}
