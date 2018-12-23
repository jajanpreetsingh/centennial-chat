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

  constructor(private accountService: AccountService, private utilityService: UtilityService) {
    this.loggedIn = this.accountService.isLoggedIn();
  }

  ngOnInit() {

    this.loggedIn = this.accountService.isLoggedIn();
  }

  goToSignup() {
    this.utilityService.navigateToPath('/signup');
  }

  goToGeneralLogin() {
    this.utilityService.navigateToPath('/general-login');
  }

  logoutToHome() {
    console.log("logging out");

    this.accountService.tryLogout().subscribe(res => {
      console.log(res);


      if (res.code == 200) {
        this.accountService.clearAllLocalData();

        this.accountService.isLoggedIn();

        this.utilityService.navigateToPath('/home');
      }
      else {
        console.log("Login failed");
      }
    });
  }

  toggleState() {
    this.isIn = !this.isIn;
  }
}
