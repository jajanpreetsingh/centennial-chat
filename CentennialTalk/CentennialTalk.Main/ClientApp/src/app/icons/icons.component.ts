import { Component, OnInit } from '@angular/core';
import { AccountService, StorageKeys } from '../services/account.service';
import { UtilityService } from '../services/utility.service';
import { MemberService } from '../services/member.service';

@Component({
  selector: 'app-icons',
  templateUrl: './icons.component.html',
  styleUrls: ['./icons.component.scss']
})
export class IconsComponent implements OnInit {

  chatCode: string;

  usedIcons: string[] = [];

  constructor(private accountService: AccountService, private utilityService: UtilityService,
    private memberService: MemberService) {
  }

  ngOnInit() {

    this.chatCode = this.accountService.getLocalData(StorageKeys.ChatCode);

    this.memberService.getMembers(this.chatCode).subscribe(res => {
      if (res.code != 500) {
        this.usedIcons = res.data;

        console.log("used icons", this.usedIcons);
      }
    })
  }

  setIconName(name: string) {
    console.log("icons: " + name);
    this.accountService.setLocalData(StorageKeys.ChatUsername, name);

    this.utilityService.navigateToPath('general-login');
  }
}
