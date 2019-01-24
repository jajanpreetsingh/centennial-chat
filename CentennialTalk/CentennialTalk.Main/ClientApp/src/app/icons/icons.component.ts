import { Component, OnInit } from '@angular/core';
import { AccountService, StorageKeys } from '../services/account.service';
import { UtilityService } from '../services/utility.service';
import { MemberService } from '../services/member.service';
import { Level } from '../../models/popup.model';

@Component({
  selector: 'app-icons',
  templateUrl: './icons.component.html',
  styleUrls: ['./icons.component.scss']
})
export class IconsComponent implements OnInit {
  chatCode: string;

  usedIcons: string[] = [];

  iconPool: string[] = ["groot", "iron-man", "spider-man", "captain-america",
    "jessica-jones", "catwoman", "storm-marvel", "mystique", "rogue", "wolverine",
    "jean-grey", "deadpool", "batman", "superman", "isis", "harley-quinn",
    "sailor-moon", "joker", "avatar", "rick-sanchez", "morty-smith", "finn",
    "jake", "princess-bubblegum", "thor", "pennywise", "martian", "the-flash-head", "olive-oyl", "harry-potter"];

  column: number = 6;

  iconGrid: string[][] = [];

  constructor(private accountService: AccountService, private utilityService: UtilityService,
    private memberService: MemberService) {
  }

  ngOnInit() {
    this.chatCode = this.accountService.getLocalData(StorageKeys.ChatCode);

    this.memberService.getMembers(this.chatCode).subscribe(res => {
      if (res.code != 500) {
        this.usedIcons = res.data; //icons already used by chat members

        this.iconPool = this.iconPool.filter(x => this.usedIcons.indexOf(x) < 0);//get all unused icons

        if (this.iconPool.length <= 0) {
          let rurl = this.accountService.getLocalData(StorageKeys.ReturnUrl);
          this.accountService.setLocalData(StorageKeys.ReturnUrl, '');

          this.utilityService.navigateToPath(rurl);
          this.utilityService.addPageError("Cannot assign identity", "Icon pool is empty. Reach out to moderator", Level[Level.danger]);
        }

        let i = 0;

        let cc = 0;//current column

        let rowArray: string[] = [];

        while (i < this.iconPool.length) {
          rowArray.push(this.iconPool[i]);

          ++i;
          ++cc;

          if (cc == this.column) {
            this.iconGrid.push(rowArray);
            rowArray = [];
            cc = 0;
          }
        }
      }
    });
  }

  setIconName(name: string) {
    this.accountService.setLocalData(StorageKeys.ChatUsername, name);

    let rurl = this.accountService.getLocalData(StorageKeys.ReturnUrl);
    this.accountService.setLocalData(StorageKeys.ReturnUrl, '');

    this.utilityService.navigateToPath(rurl);
  }
}
