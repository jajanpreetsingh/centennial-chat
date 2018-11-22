import { Component, OnInit } from '@angular/core';
import { Globals } from '../../models/globals';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-general-login',
  templateUrl: './general-login.component.html',
  styleUrls: ['./general-login.component.scss']
})
export class GeneralLoginComponent implements OnInit {
  loggedIn: boolean = false;

  constructor(private accountService: AccountService, private globals: Globals) {
    globals.loginData = this.accountService.getLocalCredentials();

    this.loggedIn = this.globals.isLoggedIn;

    console.log("from general login : " + this.loggedIn);
  }

  ngOnInit() {
    this.loggedIn = this.globals.isLoggedIn;
  }
}
