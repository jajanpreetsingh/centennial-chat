import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-general-login',
  templateUrl: './general-login.component.html',
  styleUrls: ['./general-login.component.scss']
})
export class GeneralLoginComponent implements OnInit {
  loggedIn: boolean = false;

  constructor(private accountService: AccountService) {
    this.loggedIn = this.accountService.isLoggedIn();
  }

  ngOnInit() {
    this.loggedIn = this.accountService.isLoggedIn();
  }
}
