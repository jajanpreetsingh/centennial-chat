import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { MessageService } from './message.service';
import { DatePipe } from '@angular/common';
import { MemberService } from './member.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel, MemberReaction } from '../../models/message.model';
import { UtilityService } from './utility.service';
import { MemberModel } from '../../models/member.model';
import { AccountService, StorageKeys } from './account.service';
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

  openQuestions: QuestionModel[] = [];

  pollQuestions: QuestionModel[] = [];

  publishedQuestion: QuestionModel;

  answeredQuestions: string[] = [];

  constructor(private messageService: MessageService, private datePipe: DatePipe,
    private memberService: MemberService, private utilityService: UtilityService,
    private accountService: AccountService, private questionService: QuestionService) { }

  initChatHub() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl('/discussion')
      .build();

    this.initEventCallbacks();

    this.hubConnection.start();
  }

  onConnectionStart(data) {
    if (this.chatData == null)
      this.chatData = new ChatModel();

    this.chatData = this.accountService.getLocalChatData();

    if (this.chatData == null || this.chatData.chatCode == '') {
      this.utilityService.navigateToPath('home');
    }

    this.connectionId = data;

    this.accountService.setLocalData(StorageKeys.ChatConnectionId, this.connectionId);

    this.fetchPreviousMessages();

    this.openQuestions = JSON.parse(this.accountService.getLocalData(StorageKeys.OpenQuestions));
    this.pollQuestions = JSON.parse(this.accountService.getLocalData(StorageKeys.PollingQuestions));

    console.log("p questions from hubinstance", this.pollQuestions);
    console.log("o questions from hubinstance", this.openQuestions);

    this.members = JSON.parse(this.accountService.getLocalData(StorageKeys.ChatMembers));

    console.log("members from hubinstance", this.members);

    this.isConnected = true;

    this.hubConnection.invoke('JoinGroupChat', JSON.stringify({
      'username': this.chatData.username,
      'chatCode': this.chatData.chatCode,
      'isModerator': this.accountService.amIModerator()
    })).catch(e => {
      console.log(e);
    });

    this.memberService.updateStatus({
      username: this.chatData.username,
      chatCode: this.chatData.chatCode,
      connectionId: this.connectionId,
      isConnected: this.isConnected
    }).subscribe(res => {
      //console.log(res);
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

    this.accountService.setLocalData(StorageKeys.ChatConnectionId, "");

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

    this.chatData = new ChatModel();
    this.members = [];
    this.pollQuestions = [];
    this.openQuestions = [];
    this.publishedQuestion = null;
    this.messages = [];
    this.answeredQuestions = [];

    this.utilityService.navigateToPath('/home');
  }

  onUserJoined(member: MemberModel) {
    this.members = this.members.filter(x => x.username != member.username);

    member.isConnected = true;

    this.members.push(member);

    this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(this.members));

    console.log("members from hubinstance", this.members);
  }

  onUserLeft(member: MemberModel) {
    this.members = this.members.filter(x => x.username != member.username);

    member.isConnected = false;

    this.members.push(member);

    this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(this.members));

    console.log("members from hubinstance", this.members);
  }

  onMessageReceived(messageData: MessageModel) {
    if (messageData.chatCode == this.chatData.chatCode) {
      messageData.isMine = messageData.sender == this.chatData.username;

      messageData.likeCount = messageData.reactions == null ? 0 : messageData.reactions.filter(x => x.reaction == 1).length;
      messageData.dislikeCount = messageData.reactions == null ? 0 : messageData.reactions.filter(x => x.reaction == -1).length;

      console.log("pushing messages : ", messageData);

      let index: number = this.messages.findIndex(x => x.messageId == messageData.messageId);

      if (index >= 0)
        this.messages.splice(index, 1, messageData);
      else
        this.messages.push(messageData);
    }
  }

  publishQuestion(question: QuestionModel) {
    this.hubConnection.invoke('PublishQuestion', JSON.stringify(question)).then(val => {
      this.questionService.markPublished(question).subscribe(res => {
        if (res.code == 200) {
          let ques: QuestionModel = res.data;

          if (ques.isPollingQuestion) {
            let index = this.pollQuestions.findIndex(x => x.id == ques.id);

            if (index >= 0)
              this.pollQuestions.splice(index, 1, ques);
          }
          else {
            let index = this.openQuestions.findIndex(x => x.id == ques.id);

            if (index >= 0)
              this.openQuestions.splice(index, 1, ques);
          }
        }
        else {
          console.log(res.data);
        }
      });
    });
  }

  archiveQuestion(question: QuestionModel) {
    this.hubConnection.invoke('ArchiveQuestion', JSON.stringify(question)).then(val => {
      this.questionService.markArchived(question).subscribe(res => {
        if (res.code == 200) {
          let ques: QuestionModel = res.data;

          if (ques.isPollingQuestion) {
            let index = this.pollQuestions.findIndex(x => x.id == ques.id);

            if (index >= 0)
              this.pollQuestions.splice(index, 1, ques);
          }
          else {
            let index = this.openQuestions.findIndex(x => x.id == ques.id);

            if (index >= 0)
              this.openQuestions.splice(index, 1, ques);
          }
        }
        else {
          console.log(res.data);
        }
      });
    });
  }

  sendMessage(messageObj: MessageModel) {
    console.log("sending : " + messageObj);

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

    this.hubConnection.on('messageReacted', data => {
      this.onMessageReacted(data);
    });
  }

  sendReact(data: MemberReaction) {
    this.hubConnection.invoke("ReactToMessage", JSON.stringify(data));
  }

  onMessageReacted(data: MemberReaction): any {
    this.messageService.getReaction(data).subscribe(res => {
      if (res.code == 200) {
        console.log(res);
        this.onMessageReceived(res.data);
      }
    })
  }

  onQuestionArchived(data: QuestionModel): any {
    this.publishedQuestion = null;

    data.isArchived = true;

    let quesIndex: number = this.chatData.pollQuestions.findIndex(x => x.id == data.id);

    if (quesIndex < 0) {
      quesIndex = this.chatData.openQuestions.findIndex(x => x.id == data.id);

      if (quesIndex >= 0) {
        console.log("marking archived");

        this.chatData.openQuestions.splice(quesIndex, 1);

        this.chatData.openQuestions.push(data);

        console.log(this.chatData.openQuestions);
      }
    }
    else {
      console.log("marking archived");

      this.chatData.pollQuestions.splice(quesIndex, 1);

      this.chatData.pollQuestions.push(data);

      console.log(this.chatData.pollQuestions);
    }
  }

  onQuestionPublished(data: QuestionModel): any {
    if (this.answeredQuestions.findIndex(x => x == data.id) >= 0)
      return;

    this.publishedQuestion = data;

    data.isPublished = true;

    let quesIndex: number = this.chatData.pollQuestions.findIndex(x => x.id == data.id);

    if (quesIndex < 0) {
      quesIndex = this.chatData.openQuestions.findIndex(x => x.id == data.id);

      if (quesIndex >= 0) {
        console.log("marking published");

        this.chatData.openQuestions.splice(quesIndex, 1);

        this.chatData.openQuestions.push(data);

        console.log(this.chatData.openQuestions);
      }
    }
    else {
      console.log("marking published");

      this.chatData.pollQuestions.splice(quesIndex, 1);

      this.chatData.pollQuestions.push(data);

      console.log(this.chatData.pollQuestions);
    }
  }

  fetchPreviousMessages() {
    this.messages = [];

    console.log('chat code to get message', this.chatData.chatCode);
    this.messageService.getChatMessages(this.chatData.chatCode).subscribe(res => {
      let messageArray = res.data;

      if (messageArray != null && messageArray.length > 0)
        for (var i = 0; i < messageArray.length; i++) {
          this.onMessageReceived(messageArray[i]);
        }
    },
      err => {
        console.log(err);
      });
  }
}
