import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isIn: boolean = false;
  constructor(private router: Router) { }

  ngOnInit() {
  }

  goToSignup() {
    this.router.navigate(['/signup']);
  }

  goToGeneralLogin() {
    this.router.navigate(['/general-login']);
  }

  toggleState() {
    let bool = this.isIn;
    this.isIn = bool === false ? true : false;
  }
}
