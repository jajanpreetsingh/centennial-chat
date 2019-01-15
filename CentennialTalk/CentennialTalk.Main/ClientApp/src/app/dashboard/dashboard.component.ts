import { Component, OnInit } from '@angular/core';
import { ChatModel } from '../../models/chat.model';
import { ChatService } from '../services/chat.service';
import { UtilityService } from '../services/utility.service';
import { StorageKeys, AccountService } from '../services/account.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  chats: ChatModel[] = [];

  constructor(private chatServ: ChatService, private utilityService: UtilityService,
    private accountService: AccountService) { }

  ngOnInit() {
    this.getList();
  }

  goToNewChatPage() {
    this.utilityService.navigateToPath('/new');
  }

  goToIconPage() {
    this.accountService.setLocalData(StorageKeys.ReturnUrl, '/dashboard');
    this.utilityService.navigateToPath('/icon');
  }

  getList() {
    this.chatServ.getChatList(this.accountService.getUserId()).subscribe(res => {
      if (res.code == 200) {
        this.chats = res.data;
      }
    });
  }

  onSubmitJoinChat(user: string, code: string) {
    if (this.accountService.isValNull(user) || this.accountService.isValNull(code)) {
      //handle errors
      console.log("joining interrupted");
      return;
    }

    this.chatServ.joinChat(
      {
        "username": user,
        "chatCode": code
      }
    ).subscribe(res => {
      if (res.code == 200) {
        //this.chatData = res.data;

        //console.log(this.chatData);

        //this.accountService.setLocalData(StorageKeys.ChatCode, this.chatData.chatCode);
        //this.accountService.setLocalData(StorageKeys.ChatTitle, this.chatData.title);
        //this.accountService.setLocalData(StorageKeys.ChatUsername, this.chatData.username);
        //this.accountService.setLocalData(StorageKeys.ChatModerator, this.chatData.moderator);

        //this.accountService.setLocalData(StorageKeys.OpenQuestions, JSON.stringify(this.chatData.openQuestions));

        //this.accountService.setLocalData(StorageKeys.PollingQuestions, JSON.stringify(this.chatData.pollQuestions));

        //this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(this.chatData.members));

        //if (this.accountService.isLoggedIn() || this.accountService.amIModerator()) {
        //  console.log("going to projector....");
        //  this.utilityService.navigateToPath('/projector');
        //}
        //else {
        //  console.log("going to discussion....");
        //  this.utilityService.navigateToPath('/discussion');
        //}
      }
      else {
        //this.chatData = new ChatModel();
        //console.log(res.data);
      }
    },
      error => {
        //this.chatData = new ChatModel();
        //console.log(error);
      });
  }
}
