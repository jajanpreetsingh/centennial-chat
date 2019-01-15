import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { SignupModel } from '../../models/signup.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Popup, Level } from '../../models/popup.model';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  signup: SignupModel = new SignupModel();

  utility: UtilityService;

  pageMessages: Popup[] = [];

  constructor(private accountService: AccountService,
    private util: UtilityService,
    private router: Router) {

    this.utility = util;
  }

  ngOnInit() {
  }

  onModeratorSignup() {
    this.accountService.tryModeratorSignup(this.signup).subscribe(res => {
      if (res.code == 200) {
        this.router.navigate(['/home']);

        this.utility.addPageError("Success", "Login successful", Level[Level.success]);
      }
      else if (res.code == 500) {

        let errors: string[] = res.data;

        errors.forEach(x => {

          this.utility.addPageError("Error", x, Level[Level.danger]);

        });

        console.log(this.utility.errors);
      }
      else {

        let errors: string[] = res.data;

        errors.forEach(x => {

          this.utility.addPageError("Warning", x, Level[Level.warning]);

        });
      }
    },
      err => {

        this.utility.addPageError("Error", err, Level[Level.danger]);
      });

  }
}
