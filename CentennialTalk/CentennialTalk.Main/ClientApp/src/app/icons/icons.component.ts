import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-icons',
  templateUrl: './icons.component.html',
  styleUrls: ['./icons.component.scss']
})
export class IconsComponent {
  constructor(private accountService: AccountService, private utilityService: UtilityService) {

  }

  setIconName(name: string) {
    console.log("icons: " + name);
    this.accountService.setIcon(name);

    this.utilityService.navigateToPath('general-login');
  }
}
