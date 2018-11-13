import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { Globals } from '../../models/globals';
import { AccountService } from '../services/account.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  loggedIn: boolean = false;
  login: LoginModel = new LoginModel();
  isIn: boolean = false;

  constructor(private globals: Globals,
    private accountService: AccountService, private utilityService: UtilityService) {
    this.loggedIn = this.globals.isLoggedIn;
  }

  ngOnInit() {
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

        this.accountService.setJwtToken('');
        this.accountService.setLocalCredentials(null);

        this.globals.loginData = null;

        this.utilityService.navigateToPath('/home');
      }
      else {
        console.log("Login failed");
      }
    });
  }

  toggleState() {
    let bool = this.isIn;
    this.isIn = bool === false ? true : false;
  }
}
