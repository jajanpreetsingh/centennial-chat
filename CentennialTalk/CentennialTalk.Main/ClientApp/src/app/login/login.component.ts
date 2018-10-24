import { Component, OnInit } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { AccountService } from '../services/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  login: LoginModel = new LoginModel();

  constructor(private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    private router: Router, private utilityService: UtilityService) { }

  ngOnInit() {
  }

  onModeratorLogin() {
    this.accountService.tryModeratorLogin(this.login).subscribe(res => {
      if (res.code == 200) {
        this.utilityService.setJwtToken(res.data);

        this.utilityService.setLocalCredentials(this.login);

        //this.utilityService.getJwtData();


        this.router.navigate(['/new']);
      }
      else {
        console.log("Login failed");
      }
    },
      err => {
        console.log(err);
      });
  }

}
