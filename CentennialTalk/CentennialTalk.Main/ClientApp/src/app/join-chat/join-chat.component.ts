import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { AccountService, StorageKeys } from '../services/account.service';
import { Level } from '../../models/popup.model';

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

    this.accountService.setLocalData(StorageKeys.ReturnUrl, '/join');

    this.utilityService.navigateToPath('/icon');
  }

  onSubmitJoinChat() {
    let user = this.chatData.username;
    let code = this.chatData.chatCode;

    if (this.accountService.isValNull(user)) {
      this.utilityService.addPageError("Missing Identity",
        "Select a psudonym/icon to join the session", Level[Level.danger]);
      return;
    }

    if(this.accountService.isValNull(code)) {
      this.utilityService.addPageError("No chat code",
        "Enter a valid chat code to join the session", Level[Level.danger]);
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
          this.utilityService.addPageError("Success", "Redirecting to Moderator session page", Level[Level.success]);
          this.utilityService.navigateToPath('/projector');

        }
        else {
          this.utilityService.addPageError("Success", "Redirecting to session page", Level[Level.success]);
          this.utilityService.navigateToPath('/discussion');
        }
      }
      else {
        this.chatData = new ChatModel();
        let errors: string[] = res.data;

        errors.forEach(x => {
          this.utilityService.addPageError("Error while joining", x, Level[Level.danger]);
        });
      }
    },
      error => {
        this.chatData = new ChatModel();
        this.utilityService.addPageError("Response error",
          error, Level[Level.danger]);
      });
  }
}
