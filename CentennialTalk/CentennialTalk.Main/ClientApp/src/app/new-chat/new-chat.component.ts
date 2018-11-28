import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { AccountService } from '../services/account.service';
import { QuestionModel } from '../../models/question.model';

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

  constructor(private chatService: ChatService, private utilityService: UtilityService, private accountService: AccountService) {
  }

  ngOnInit() {
    if (this.accountService.isJwtValid()) {
      let login = this.accountService.getLocalCredentials();

      if (login == null)
        this.utilityService.navigateToPath('/home');
      else {
        this.chatData.username = login.username;
        this.chatData.moderator = login.username;
      }
    }
  }

  maintainFocus(index: number, obj: any): number {
    return index;
  }

  refreshData() {
    console.log(this.chatData);
  }

  addOpenQuestion() {
    if (this.chatData.openQuestions == null || this.chatData.openQuestions.length <= 0)
      this.chatData.openQuestions = [];

    let q = new QuestionModel();
    q.content = this.openQuestion;
    q.isPollingQuestion = false;

    this.chatData.openQuestions.push(q);
  }

  addPollQuestion() {
    if (this.chatData.pollQuestions == null || this.chatData.pollQuestions.length <= 0)
      this.chatData.pollQuestions = [];

    let q = new QuestionModel();
    q.selectMultiple = this.allowMultiple;
    q.content = this.pollQuestion;

    q.options = [];

    console.log(this.pollOptions);

    if (this.pollOptions != null && this.pollOptions.length > 0)
      this.pollOptions.forEach(x => q.options.push(x));

    q.isPollingQuestion = true;

    this.chatData.pollQuestions.push(q);

    this.pollOptions = [];
  }

  addOption() {
    if (this.pollOptions == null)
      this.pollOptions = [];

    this.pollOptions.push('');
  }

  removeOption() {
    if (this.pollOptions == null)
      this.pollOptions = [];

    if (this.pollOptions.length > 0)
      this.pollOptions.pop();
  }

  onChangeModerator() {
    this.chatData.username = this.chatData.moderator;
  }

  onSubmitNewChat() {
    this.chatService.createNewChat(this.chatData).subscribe(res => {
      if (res.code == 200) {
        this.chatData.moderator = res.data.moderator;
        this.chatData.title = res.data.title;
        this.chatData.chatCode = res.data.chatCode;
        this.chatData.expirationDate = res.data.expirationDate;
        this.chatData.activationDate = res.data.activationDate;

        this.accountService.setLocalChatData(this.chatData);

        //this.utilityService.navigateToPath('/projector');

        this.utilityService.navigateToPath('/discussion');
      }
      else {
        this.chatData = new ChatModel();
        console.log(res.data);
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
