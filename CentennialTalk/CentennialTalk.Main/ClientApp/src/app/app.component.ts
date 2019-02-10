import { Component, OnInit } from '@angular/core';
import { UtilityService } from './services/utility.service';
import { AccountService } from './services/account.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'VoiceIt';

  constructor(private actRoute: ActivatedRoute, private accountService: AccountService, private utilityService: UtilityService) {
  }

  ngOnInit() {
    //this.actRoute.queryParams.subscribe(params => {
    //  let userId = params['userId'];
    //  let code = params['code'];

    //  console.log("userid", userId);
    //  console.log("code", code);

    //  if (this.accountService.isValNull(userId) || this.accountService.isValNull(code)) {
    //    this.logoutToHome();
    //  }
    //  else {
    //    console.log("redirecting");
    //    this.utilityService.navigateWithData("/reset", { queryParams: { "userId": userId, "code": code } });
    //  }

    //});

    this.logoutToHome();
  }

  logoutToHome() {
    this.accountService.tryLogout().subscribe(res => {
      if (res.code == 200) {
        this.accountService.clearAllLocalData();

        //this.utilityService.navigateToPath('/home');
      }
    });
  }
}
