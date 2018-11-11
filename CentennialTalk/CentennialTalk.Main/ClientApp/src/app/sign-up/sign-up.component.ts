import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { SignupModel } from '../../models/signup.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  signup: SignupModel = new SignupModel();

  constructor(private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
  }

  onModeratorSignup() {
    this.accountService.tryModeratorSignup(this.signup).subscribe(res => {
      if (res.code == 200) {
        this.router.navigate(['/home']);
      }
      else {
        console.log("Signup failed");
      }
    },
      err => {
        console.log(err);
      });
  }
}
