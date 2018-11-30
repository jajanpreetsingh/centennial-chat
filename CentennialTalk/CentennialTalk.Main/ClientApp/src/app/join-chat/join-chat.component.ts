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

  loggedin: boolean = false;

  constructor(private chatService: ChatService,
    private utilityService: UtilityService, private accountService: AccountService, private globals: Globals) {

    console.log("from join chat : " + this.globals.isLoggedIn);

    this.loggedin = this.globals.isLoggedIn;
  }

  ngOnInit() {
    if (this.accountService.isJwtValid()) {
      let cred = this.accountService.getLocalCredentials();

      if (cred != null)
        this.chatData.username = cred.username;

      this.loggedin = this.globals.isLoggedIn;
    }
  }

  goToNewChatPage() {
    this.utilityService.navigateToPath('/new');
  }

  onSubmitJoinChat() {

    console.log(this.chatData.username + " : " + this.chatData.chatCode);

    this.chatService.joinChat(
      {
        "username": this.chatData.username,
        "chatCode": this.chatData.chatCode
      }
    ).subscribe(res => {
      if (res.code == 200) {
        this.chatData = res.data;

        console.log(this.chatData)

        this.accountService.setLocalChatData(this.chatData);

        if (this.globals.isLoggedIn) {
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
