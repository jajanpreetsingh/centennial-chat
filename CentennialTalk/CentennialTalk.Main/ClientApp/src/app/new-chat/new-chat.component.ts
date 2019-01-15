import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { AccountService, StorageKeys } from '../services/account.service';
import { QuestionModel } from '../../models/question.model';
import { Response } from '../../models/response.model';
import { Level } from '../../models/popup.model';

@Component({
  selector: 'app-new-chat',
  templateUrl: './new-chat.component.html',
  styleUrls: ['./new-chat.component.scss']
})

export class NewChatComponent implements OnInit, OnDestroy {
  chatData: ChatModel = new ChatModel();

  openQuestion: string = '';
  pollQuestion: string = '';

  allowMultiple: boolean = false;

  pollOptions: string[] = [];

  loggedIn: boolean = false;

  constructor(private chatService: ChatService, private utilityService: UtilityService, private accountService: AccountService) {
  }

  ngOnInit() {
    this.loggedIn = this.accountService.isLoggedIn();

    if (!this.loggedIn) {
      this.utilityService.navigateToPath('/home');
      this.accountService.clearAllLocalData();
    }
    else {
      this.chatData.username =
        this.chatData.moderator = this.accountService.getLocalData(StorageKeys.ChatUsername);
    }

    this.chatData.activationDate = new Date();

    this.chatData.expirationDate = new Date();
  }

  maintainFocus(index: number, obj: any): number {
    console.log("maintain focus : " + obj);
    return index;
  }

  refreshData() {
    console.log(this.chatData);
  }

  addOpenQuestion() {

    if (this.chatData.openQuestions != null && this.chatData.openQuestions.findIndex(x => x.content == this.openQuestion) >= 0) {
      this.utilityService.addPageError("Duplicate Question", "Cannot add question with same content", Level[Level.danger]);
      return;
    }

    if (this.chatData.openQuestions == null || this.chatData.openQuestions.length <= 0)
      this.chatData.openQuestions = [];

    let q = new QuestionModel();
    q.content = this.openQuestion;
    q.isPollingQuestion = false;
    q.chatCode = this.chatData.chatCode;

    this.chatData.openQuestions.push(q);

    this.openQuestion = '';
  }

  removeOpenQuestion(ques: QuestionModel) {
    if (this.chatData.openQuestions == null || this.chatData.openQuestions.length == 0)
      return;

    let ind = this.chatData.openQuestions.findIndex(x => x.content == ques.content);

    if (ind >= 0) {
      this.chatData.openQuestions.splice(ind, 1);
    }
  }

  addPollQuestion() {

    if (this.chatData.pollQuestions != null && this.chatData.pollQuestions.findIndex(x => x.content == this.pollQuestion) >= 0) {
      this.utilityService.addPageError("Duplicate Question", "Cannot add question with same content", Level[Level.danger]);
      return;
    }

    if (this.pollOptions == null || this.pollOptions.length <= 0) {
      this.utilityService.addPageError("Empty options", "Cannot add polls without options", Level[Level.danger]);
      return;
    }

    if (this.chatData.pollQuestions == null || this.chatData.pollQuestions.length <= 0)
      this.chatData.pollQuestions = [];

    let q = new QuestionModel();
    q.selectMultiple = this.allowMultiple;
    q.content = this.pollQuestion;
    q.chatCode = this.chatData.chatCode;

    q.options = [];

    console.log(this.pollOptions);

      this.pollOptions.forEach(x => q.options.push(x));

    q.isPollingQuestion = true;

    this.chatData.pollQuestions.push(q);

    this.pollOptions = [];
    this.pollQuestion = '';
  }

  removePollQuestion(ques: QuestionModel) {
    if (this.chatData.pollQuestions == null || this.chatData.pollQuestions.length == 0)
      return;

    let ind = this.chatData.pollQuestions.findIndex(x => x.content == ques.content);

    if (ind >= 0) {
      this.chatData.pollQuestions.splice(ind, 1);
    }
  }

  addOption() {
    if (this.pollOptions == null && this.pollOptions.length <= 0)
      this.pollOptions = [];

    this.pollOptions.push("");
  }

  removeOption() {
    if (this.pollOptions == null)
      this.pollOptions = [];

    if (this.pollOptions.length > 0)
      this.pollOptions.pop();
  }

  onChangeModerator() {
    this.chatData.username =
      this.chatData.moderator = this.accountService.getLocalData(StorageKeys.ChatUsername);
  }

  onSubmitNewChat() {
    let user = this.chatData.username;
    let mod = this.chatData.moderator;

    console.log(user + " : " + mod);

    if (this.accountService.isValNull(user) || this.accountService.isValNull(mod)) {
      //handle errors
      console.log("joining interrupted");
      return;
    }

    this.chatData.creatorId = this.accountService.getUserId();

    this.chatService.createNewChat(this.chatData).subscribe(res => {
      if (res.code == 200) {
        this.chatData = res.data;

        this.accountService.setLocalData(StorageKeys.ChatCode, this.chatData.chatCode);
        this.accountService.setLocalData(StorageKeys.ChatTitle, this.chatData.title);
        this.accountService.setLocalData(StorageKeys.ChatUsername, this.chatData.username);
        this.accountService.setLocalData(StorageKeys.ChatModerator, this.chatData.moderator);

        this.accountService.setLocalData(StorageKeys.OpenQuestions, JSON.stringify(this.chatData.openQuestions));

        this.accountService.setLocalData(StorageKeys.PollingQuestions, JSON.stringify(this.chatData.pollQuestions));

        this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(this.chatData.members));

        this.utilityService.navigateToPath('/projector');
      }
      else {
        this.chatData = new ChatModel();
      }
    },
      error => {
        this.chatData = new ChatModel();
        console.log(error);
      });
  }

  ngOnDestroy() {
  }
}
