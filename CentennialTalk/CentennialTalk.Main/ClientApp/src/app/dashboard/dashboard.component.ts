import { Component, OnInit } from '@angular/core';
import { ChatModel } from '../../models/chat.model';
import { ChatService } from '../services/chat.service';
import { UtilityService } from '../services/utility.service';
import { StorageKeys, AccountService } from '../services/account.service';
import { Level } from '../../models/popup.model';

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

  joinThisChat(user: string, code: string) {

    if (this.accountService.isValNull(user)) {
      this.utilityService.addPageError("Missing Identity",
        "Select a psudonym/icon to join the session", Level[Level.danger]);
      return;
    }

    if (this.accountService.isValNull(code)) {
      this.utilityService.addPageError("No chat code",
        "Enter a valid chat code to join the session", Level[Level.danger]);
      return;
    }

    this.chatServ.joinChat(
      {
        "username": user,
        "chatCode": code
      }
    ).subscribe(res => {
      if (res.code == 200) {
        let chatData = res.data;

        this.accountService.setLocalData(StorageKeys.ChatCode, chatData.chatCode);
        this.accountService.setLocalData(StorageKeys.ChatTitle, chatData.title);
        this.accountService.setLocalData(StorageKeys.ChatUsername, chatData.username);
        this.accountService.setLocalData(StorageKeys.ChatModerator, chatData.moderator);

        this.accountService.setLocalData(StorageKeys.OpenQuestions, JSON.stringify(chatData.openQuestions));

        this.accountService.setLocalData(StorageKeys.PollingQuestions, JSON.stringify(chatData.pollQuestions));

        this.accountService.setLocalData(StorageKeys.ChatMembers, JSON.stringify(chatData.members));

        if (this.accountService.isLoggedIn() || this.accountService.amIModerator()) {
          this.utilityService.addPageError("Success", "Redirecting to Moderator session page", Level[Level.success]);
          this.utilityService.navigateToPath('/projector');

        }
        else {
          this.utilityService.addPageError("Success", "Redirecting to session page", Level[Level.success]);
          this.utilityService.navigateToPath('/discussion');
        }
      }
      else {
        let errors: string[] = res.data;

        errors.forEach(x => {
          this.utilityService.addPageError("Error while joining", x, Level[Level.danger]);
        });
      }
    },
      error => {
        this.utilityService.addPageError("Response error",
          error, Level[Level.danger]);
      });
  }
}
