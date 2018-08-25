import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { MessageService } from './message.service';
import { DatePipe } from '@angular/common';
import { MemberService } from './member.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel } from '../../models/message.model';
import { UtilityService } from './utility.service';
import { MemberModel } from '../../models/member.model';

@Injectable()
export class HubService {
  private hubConnection: HubConnection;

  private isConnected: boolean;
  private connectionId: string;

  private chatData: ChatModel;

  messages: MessageModel[] = [];

  members: MemberModel[] = [];

  constructor(private messageService: MessageService, private datePipe: DatePipe,
    private memberService: MemberService, private utilityService: UtilityService) { }

  initChatHub() {
    this.chatData = this.utilityService.getLocalChatData();

    if (this.chatData == null || this.chatData.chatCode == '') {
      this.utilityService.navigateToPath('home');
      return;
    }

    this.fetchPreviousMessages();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl('/discussion')
      .build();

    this.hubConnection.start();

    this.initEventCallbacks();
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
      isConnected: this.isConnected
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

    this.isConnected = false;

    this.memberService.updateStatus({
      username: this.chatData.username,
      chatCode: this.chatData.chatCode,
      connectionId: this.connectionId,
      isConnected: this.isConnected
    }).subscribe(res => {
      console.log(res);
    },
      err => {
        console.log(err);
      });
  }

  onUserJoined(username) {
    console.log(username + ' joined');

    let index = this.members.indexOf(username);

    if (index > -1)
      this.members[index].isConnected = true;
    else {
      let member = new MemberModel();

      member.username = username;
      member.isConnected = true;

      this.members.push(member);
    }
  }

  onUserLeft(username) {
    console.log(username + ' left');

    let index = this.members.indexOf(username);

    if (index > -1)
      this.members[index].isConnected = false;
    else {
      let member = new MemberModel();

      member.username = username;
      member.isConnected = false;

      this.members.push(member);
    }
  }

  onMessageReceived(messageData) {
    if (messageData.chatCode == this.chatData.chatCode) {
      messageData.isMine = messageData.sender == this.chatData.username;

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

  initEventCallbacks() {
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

  fetchPreviousMessages() {
    console.log('chat code to get message',this.chatData.chatCode);
    this.messageService.getChatMessages(this.chatData.chatCode).subscribe(res => {
      let messageArray = res.data;

      console.log('prev messages', res.data);

      if (messageArray != null && messageArray.length > 0)
        for (var i = 0; i < messageArray.length; i++) {
          this.onMessageReceived(messageArray[i])
        }
    },
      err => {
        console.log(err);
      });
  }

  getMessages() {
    //return this.messages.sort((a, b) => { a.sentDate - b.sentDate });
  }
}
