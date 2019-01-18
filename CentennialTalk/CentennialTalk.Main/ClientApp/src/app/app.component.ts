import { Component, OnInit } from '@angular/core';
import { UtilityService } from './services/utility.service';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'VoiceIt';

  constructor(private accountService: AccountService, private utilityService: UtilityService) {
  }

  ngOnInit() {
    //this.logoutToHome();
  }

  logoutToHome() {
    this.accountService.tryLogout().subscribe(res => {
      console.log(res);

      if (res.code == 200) {
        this.accountService.clearAllLocalData();

        this.utilityService.navigateToPath('/home');
      }
      else {
        console.log("logout failed");
      }
    });
  }
}
