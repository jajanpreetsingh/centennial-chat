import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Injectable()
export class UtilityService {
  constructor(private router: Router) { }

  navigate(path, data) {
    //this.router.navigate([path], {
    //  queryParams: btoa(data)
    //});
  }
}
