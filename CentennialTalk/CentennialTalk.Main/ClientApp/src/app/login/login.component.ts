import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { AccountService } from '../services/account.service';
import { UtilityService } from '../services/utility.service';

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

        this.accountService.setLocalCredentials(this.login);

        this.utility.navigateToPath('');
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
    this.utility.navigateToPath(['/signup']);
  }
}
