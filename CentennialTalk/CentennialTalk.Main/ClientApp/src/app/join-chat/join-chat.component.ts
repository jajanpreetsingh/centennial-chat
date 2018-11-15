import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { ChatModel } from '../../models/chat.model';
import { UtilityService } from '../services/utility.service';
import { AccountService } from '../services/account.service';
import { Globals } from '../../models/globals';

@Component({
  selector: 'app-join-chat',
  templateUrl: './join-chat.component.html',
  styleUrls: ['./join-chat.component.scss']
})
export class JoinChatComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  isLoggedIn: boolean = false;

  constructor(private chatService: ChatService,
    private utilityService: UtilityService, private accountService: AccountService, private globals: Globals) {

    console.log("from join chat : " + this.globals.isLoggedIn);

    this.isLoggedIn = this.globals.isLoggedIn;
  }

  ngOnInit() {
    if (this.accountService.isJwtValid()) {
      let cred = this.accountService.getLocalCredentials();

      if (cred != null)
        this.chatData.username = cred.username;

      this.isLoggedIn = this.globals.isLoggedIn;
    }
  }

  goToNewChatPage() {
    this.utilityService.navigateToPath('/new');
  }

  onSubmitJoinChat() {
    this.chatService.joinChat(
      {
        "username": this.chatData.username,
        "chatCode": this.chatData.chatCode
      }
    ).subscribe(res => {
      if (res.code == 200) {
        this.chatData.moderator = res.data.moderator;
        this.chatData.title = res.data.title;

        this.accountService.setLocalChatData(this.chatData);

        this.utilityService.navigateToPath('/chat');
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
