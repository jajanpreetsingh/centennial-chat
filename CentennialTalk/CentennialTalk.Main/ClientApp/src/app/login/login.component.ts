import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { AccountService, StorageKeys } from '../services/account.service';
import { UtilityService } from '../services/utility.service';
//import { detectChanges } from '@angular/core/src/render3';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  login: LoginModel = new LoginModel();

  constructor(private accountService: AccountService, private utility: UtilityService) { }

  ngOnInit() {
  }

  onModeratorLogin() {

    console.log(this.login);

    this.accountService.tryModeratorLogin(this.login).subscribe(res => {

      console.log(res);
      if (res.code == 200) {

        this.accountService.setJwtToken(res.data);

        this.accountService.setLocalData(StorageKeys.LoginUsername, this.login.username);

        window.location.reload();
      }
      else {
        console.log("Login failed");
      }
    },
      err => {
        console.log(err);
      });
  }

  goToSignup() {
    this.utility.navigateToPath('/signup');
  }
}
