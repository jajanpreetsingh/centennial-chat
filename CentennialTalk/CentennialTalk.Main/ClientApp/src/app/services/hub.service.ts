import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { MessageService } from './message.service';
import { DatePipe } from '@angular/common';
import { MemberService } from './member.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel } from '../../models/message.model';
import { UtilityService } from './utility.service';
import { CheckType } from '@angular/core/src/view';

@Injectable()
export class HubService {
  private hubConnection: HubConnection;

  private isConnected: boolean;
  private connectionId: string;

  private chatData: ChatModel;

  messages: MessageModel[] = [];

  constructor(private messageService: MessageService, private datePipe: DatePipe,
    private memberService: MemberService, private utilityService: UtilityService) { }

  initChatHub() {
    this.chatData = this.utilityService.getLocalChatData();

    console.log('chat data', this.chatData);

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
      'username': this.chatData.username,
      'chatCode': this.chatData.chatCode,
      'isModerator': this.chatData.username == this.chatData.moderator
    })).catch(e => {
      console.log(e);
    });

    this.memberService.updateStatus({
      username: this.chatData.username,
      chatCode: this.chatData.chatCode,
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
      username: this.chatData.username,
      chatCode: this.chatData.chatCode
    }));

    this.utilityService.navigateToPath('home');

    this.memberService.updateStatus({
      username: this.chatData.username,
      chatCode: this.chatData.chatCode,
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

    if (messageData.chatCode == this.chatData.chatCode) {
      this.messages.push(messageData);

      this.messages[this.messages.length - 1].isMine
        = this.messages[this.messages.length - 1].sender == this.chatData.username;
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
    //return this.messages.sort((a, b) => { a.sentDate - b.sentDate });
  }
}
