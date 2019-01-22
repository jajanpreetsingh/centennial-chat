import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  email: string;

  constructor(private accServ: AccountService) { }

  ngOnInit() {
  }

  sendResetLink() {
    if (this.accServ.isValNull(this.email))
      return;

    this.accServ.sendResetLink(this.email).subscribe(res => {
    });
  }
}
