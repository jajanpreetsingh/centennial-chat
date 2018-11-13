import { Component, OnInit } from '@angular/core';
import { Globals } from '../../models/globals';

@Component({
  selector: 'app-general-login',
  templateUrl: './general-login.component.html',
  styleUrls: ['./general-login.component.scss']
})
export class GeneralLoginComponent implements OnInit {
  loggedIn: boolean = false;

  constructor(private globals: Globals) {
    this.loggedIn = this.globals.isLoggedIn;
  }

  ngOnInit() {
    this.loggedIn = this.globals.isLoggedIn;
  }
}
