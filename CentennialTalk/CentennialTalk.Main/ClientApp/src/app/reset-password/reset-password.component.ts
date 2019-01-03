import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  password: string;
  confpassword: string;

  userId: string;
  code: string;

  constructor(private actRoute: ActivatedRoute, private accServ: AccountService) { }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.code = params['code'];
    });
  }

  resetPassword() {
    if (this.accServ.isValNull(this.password)
      || this.accServ.isValNull(this.confpassword)
      || this.accServ.isValNull(this.userId)
      || this.accServ.isValNull(this.code)
      || this.password !== this.confpassword)
      return;

    console.log("prd : " + this.userId);
    console.log("prd : " + this.code)

    this.accServ.resetPassword(this.userId, this.code, this.password).subscribe(res => { console.log(res) });
  }
}
