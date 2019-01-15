import { Component, OnInit } from '@angular/core';
import { AccountService, StorageKeys } from '../services/account.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isIn: boolean = false;
  loggedIn: boolean = false;

  utility: UtilityService;

  constructor(private accountService: AccountService, private utilityService: UtilityService) {
    this.loggedIn = this.accountService.isLoggedIn();
    this.utility = utilityService;
  }

  ngOnInit() {
    this.loggedIn = this.accountService.isLoggedIn();
  }

  goToJoinChat() {
    this.utilityService.navigateToPath('/join');
  }

  goToSignup() {
    this.utilityService.navigateToPath('/signup');
  }

  goToGeneralLogin() {
    this.utilityService.navigateToPath('/general-login');
  }

  goToDashboard() {
    this.utilityService.navigateToPath('/dashboard');
  }

  logoutToHome() {
    this.accountService.tryLogout().subscribe(res => {
      console.log(res);

      if (res.code == 200) {
        this.accountService.clearAllLocalData();

        this.utilityService.navigateToPath('/home');
      }
      else {
        console.log("logout failed");
      }
    });
  }

  hideError(index: number) {
    this.utility.errors.splice(index, 1);
  }

  toggleState() {
    this.isIn = !this.isIn;
  }
}
