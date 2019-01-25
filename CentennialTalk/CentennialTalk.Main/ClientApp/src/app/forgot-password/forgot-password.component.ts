import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Level } from '../../models/popup.model';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  email: string;

  constructor(private accServ: AccountService, private util: UtilityService) { }

  ngOnInit() {
  }

  sendResetLink() {
    if (this.accServ.isValNull(this.email)) {
      this.util.addPageError("No email information", "We dont have your email information with us. Please reach out to App support", Level[Level.danger]);

      return;
    }

    this.accServ.sendResetLink(this.email).subscribe(res => {
      if (res.code == 200) {

        this.util.addPageError("Reset link sent successfully", "Your password reset link was sent to your registered email. Click the link to reset your password", Level[Level.success]);

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
