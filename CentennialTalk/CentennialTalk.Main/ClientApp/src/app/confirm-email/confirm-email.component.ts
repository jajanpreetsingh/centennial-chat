import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  constructor(private actRoute: ActivatedRoute, private accServ: AccountService) { }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      let userId: string = params['userId'];
      let code: string = params['code'];

      console.log(userId + "  :  " + code);

      this.accServ.verifyEmail(userId, code).subscribe(res => {
      });
    });
  }
}
