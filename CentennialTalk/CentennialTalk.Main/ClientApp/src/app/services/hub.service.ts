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
import { AccountService } from './account.service';
import { QuestionModel } from '../../models/question.model';
import { QuestionService } from './question.service';

@Injectable()
export class HubService {
  private hubConnection: HubConnection;

  private isConnected: boolean;
  private connectionId: string;

  private chatData: ChatModel;

  messages: MessageModel[] = [];

  members: MemberModel[] = [];

  publishedQuestion: QuestionModel;

  constructor(private messageService: MessageService, private datePipe: DatePipe,
    private memberService: MemberService, private utilityService: UtilityService,
    private accountService: AccountService, private questionService: QuestionService) { }

  initChatHub() {
    this.chatData = this.accountService.getLocalChatData();

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
    //console.log('connection id', data);

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

  onUserJoined(username: string) {
    //console.log(username + ' joined');

    let index = this.members.findIndex(x => x.username == username);

    if (index > -1)
      this.members[index].isConnected = true;
    else {
      let member = new MemberModel();

      member.username = username;
      member.isConnected = true;

      this.members.push(member);
    }
  }

  onUserLeft(username: string) {
    console.log(username + ' left');

    let index = this.members.findIndex(x => x.username == username);

    if (index > -1)
      this.members[index].isConnected = false;
    else {
      let member = new MemberModel();

      member.username = username;
      member.isConnected = false;

      this.members.push(member);
    }
  }

  onMessageReceived(messageData: MessageModel) {
    if (messageData.chatCode == this.chatData.chatCode) {
      messageData.isMine = messageData.sender == this.chatData.username;

      this.messages.push(messageData);
    }
  }

  publishQuestion(question: QuestionModel) {
    this.hubConnection.invoke('PublishQuestion', JSON.stringify(question)).then(() => {
      this.questionService.markPublished(question).subscribe(res => {

        if (res.code == 200) {

        }
        else {
          console.log(res.data);
        }
      });
    });
  }

  archiveQuestion(question: QuestionModel) {
    this.hubConnection.invoke('ArchiveQuestion', JSON.stringify(question)).then(() => {
      this.questionService.markArchived(question).subscribe(res => {
        if (res.code == 200) {

        }
        else {
          console.log(res.data);
        }
      });
    });
  }

  sendMessage(messageObj: MessageModel) {
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

    this.hubConnection.on('questionPublished', data => {
      this.onQuestionPublished(data);
    });

    this.hubConnection.on('questionArchived', data => {
      this.onQuestionArchived(data);
    });
  }

  onQuestionArchived(data: any): any {
    this.publishedQuestion = null;

    console.log("marking archived");

    let ques: QuestionModel = this.chatData.pollQuestions.find(x => x.id == data.id);

    if (ques == null) {
      ques = this.chatData.openQuestions.find(x => x.id == data.id);

      if (ques) {
        console.log("marking archived");
        this.chatData.openQuestions.find(x => x.id == data.id).archiveDate = data.archiveDate;
        this.chatData.openQuestions.find(x => x.id == data.id).isArchived = data.isArchived;
        console.log(this.chatData.openQuestions);
      }
    }
    else {
      console.log("marking archived");
      this.chatData.pollQuestions.find(x => x.id == data.id).archiveDate = data.archiveDate;
      this.chatData.pollQuestions.find(x => x.id == data.id).isArchived = data.isArchived;
      console.log(this.chatData.pollQuestions);
    }
  }

  onQuestionPublished(data: QuestionModel): any {
    this.publishedQuestion = data;

    let ques: QuestionModel = this.chatData.pollQuestions.find(x => x.id == data.id);

    if (ques == null) {
      ques = this.chatData.openQuestions.find(x => x.id == data.id);

      if (ques) {
        console.log("marking published");
        this.chatData.openQuestions.find(x => x.id == data.id).publishDate = data.publishDate;
        this.chatData.openQuestions.find(x => x.id == data.id).isPublished = data.isPublished;

        console.log(this.chatData.openQuestions);
      }
    }
    else {
      console.log("marking published");
      this.chatData.pollQuestions.find(x => x.id == data.id).publishDate = data.publishDate;
      this.chatData.pollQuestions.find(x => x.id == data.id).isPublished = data.isPublished;

      console.log(this.chatData.pollQuestions);
    }
  }

  fetchPreviousMessages() {
    console.log('chat code to get message', this.chatData.chatCode);
    this.messageService.getChatMessages(this.chatData.chatCode).subscribe(res => {
      let messageArray = res.data;

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
