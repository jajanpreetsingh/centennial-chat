import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { AccountService, StorageKeys } from '../services/account.service';

@Component({
  selector: 'app-join-chat',
  templateUrl: './join-chat.component.html',
  styleUrls: ['./join-chat.component.scss']
})
export class JoinChatComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  loggedIn: boolean = false;

  constructor(private chatService: ChatService,
    private utilityService: UtilityService, private accountService: AccountService) {
  }

  ngOnInit() {
    this.loggedIn = this.accountService.isLoggedIn();

    this.chatData = this.accountService.getLocalChatData();
  }

  goToNewChatPage() {
    this.utilityService.navigateToPath('/new');
  }

  updateInputChatCode() {
    this.accountService.setLocalData(StorageKeys.ChatCode, this.chatData.chatCode);
}

  goToIconPage() {
    this.updateInputChatCode();
    this.utilityService.navigateToPath('/icon');
  }

  onSubmitJoinChat() {

    let user = this.chatData.username;
    let code = this.chatData.chatCode;

    console.log(this.chatData.username + " : " + this.chatData.chatCode);

    if (this.accountService.isValNull(user) || this.accountService.isValNull(code)) {
      //handle errors
      console.log("joining interrupted");
      return;
    }

    this.chatService.joinChat(
      {
        "username": this.chatData.username,
        "chatCode": this.chatData.chatCode
      }
    ).subscribe(res => {
      if (res.code == 200) {
        this.chatData = res.data;

        console.log(this.chatData);

        this.accountService.setLocalData(StorageKeys.ChatCode, this.chatData.chatCode);
        this.accountService.setLocalData(StorageKeys.ChatTitle, this.chatData.title);
        this.accountService.setLocalData(StorageKeys.ChatUsername, this.chatData.username);
        this.accountService.setLocalData(StorageKeys.ChatModerator, this.chatData.moderator);

        this.accountService.setLocalData(StorageKeys.OpenQuestions, JSON.stringify(this.chatData.openQuestions));

        this.accountService.setLocalData(StorageKeys.PollingQuestions, JSON.stringify(this.chatData.pollQuestions));

        this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(this.chatData.members));

        if (this.accountService.isLoggedIn() || this.accountService.amIModerator()) {
          console.log("going to projector....");
          this.utilityService.navigateToPath('/projector');
        }
        else {
          console.log("going to discussion....");
          this.utilityService.navigateToPath('/discussion');
        }

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
}
