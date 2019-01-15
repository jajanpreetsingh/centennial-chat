import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { AccountService, StorageKeys } from '../services/account.service';
import { UtilityService } from '../services/utility.service';
import { Popup, Level } from '../../models/popup.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  login: LoginModel = new LoginModel();

  utility: UtilityService;

  constructor(private accountService: AccountService, private util: UtilityService) {
    this.utility = util;
  }

  ngOnInit() {
  }

  onModeratorLogin() {
    console.log(this.login);

    this.accountService.tryModeratorLogin(this.login).subscribe(res => {
      console.log(res);
      if (res.code == 200) {
        this.accountService.setJwtToken(res.data);

        this.accountService.setLocalData(StorageKeys.LoginUsername, this.login.username);

        this.util.navigateToPath('/dashboard');

        this.utility.addPageError("Success", "Login successful", Level[Level.success]);
      }
      else if (res.code == 500) {

        let errors: string[] = res.data;

        errors.forEach(x => {

          this.utility.addPageError("Error", x, Level[Level.danger]);

        });
      }
      else {

        let errors: string[] = res.data;

        errors.forEach(x => {

          this.utility.addPageError("Warning", x, Level[Level.warning]);

        });
      }
    },
      err => {

        this.utility.addPageError("Error", err, Level[Level.danger]);
      });
  }

  goToForgotPassword() {
    this.util.navigateToPath('/forgot');
  }

  goToSignup() {
    this.util.navigateToPath('/signup');
  }
}
