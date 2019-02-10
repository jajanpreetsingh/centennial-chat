import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../services/account.service';
import { UtilityService } from '../services/utility.service';
import { Level } from '../../models/popup.model';

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

  constructor(private actRoute: ActivatedRoute, private accServ: AccountService, private util: UtilityService) { }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.code = params['code'];
    });
  }

  resetPassword() {
    if (this.accServ.isValNull(this.password)
      || this.accServ.isValNull(this.confpassword)) {
      this.util.addPageError("Empty Password fields", "Please fill both the fields to confirm your new password", Level[Level.danger]);

      return;
    }

    if (this.password !== this.confpassword) {
      this.util.addPageError("Passwords dont match", "Both fields should have same value for new password", Level[Level.danger]);
      return;
    }

    this.accServ.resetPassword(this.userId, this.code, this.password).subscribe(res => {
      if (res.code == 200) {
        this.util.addPageError("Password reset successfull", "Your password was changed to the new one. Redirecting to login..", Level[Level.success]);

        this.util.navigateToPath('/general-login');
      }
      else {
        let errors: string[] = res.data;

        errors.forEach(x => {
          this.util.addPageError("Error", x, Level[Level.danger]);
        });
      }
    });
  }
}
