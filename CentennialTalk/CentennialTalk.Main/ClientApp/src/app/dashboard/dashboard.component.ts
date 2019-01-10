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
}
