import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../services/account.service';
import { Level } from '../../models/popup.model';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {
  constructor(private actRoute: ActivatedRoute, private accServ: AccountService, private util: UtilityService) { }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      let userId: string = params['userId'];
      let code: string = params['code'];

      this.accServ.verifyEmail(userId, code).subscribe(res => {
        if (res.code == 200) {
          this.util.addPageError("Success", "Email confirmed successfuully. Redirecting to login..", Level[Level.success]);

          this.util.navigateToPath('/general-login');
        }
        else {
          let errors: string[] = res.data;

          errors.forEach(x => {
            this.util.addPageError("Error", x, Level[Level.danger]);
          });
        }
      });
    });
  }
}
