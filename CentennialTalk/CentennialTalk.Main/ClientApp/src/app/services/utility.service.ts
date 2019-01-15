import { Injectable } from '@angular/core';
import { Router, Params } from '@angular/router';
import { Popup } from '../../models/popup.model';
import { Observable } from 'rxjs/Observable';
import { delay } from 'q';
import { error } from 'util';
import { setTimeout } from 'timers';

@Injectable()
export class UtilityService {
  constructor(private router: Router) { }

  public errors: Popup[] = [];

  navigateWithData(path: string, data: any) {
    this.router.navigate([path], data);
  }

  navigateToPath(path: string) {
    this.router.navigate([path]);
  }

  showErrorAlert(message: string) {
  }

  showSuccessAlert(message: string) {
  }

  addPageError(heading:string,message: string, level: string) {
    let pop = new Popup();
    pop.level = level;
    pop.message = message;
    pop.heading = heading;

    let ind = this.errors.push(pop)-1;

    setTimeout(() => { this.errors.splice(ind, 1); }, 3500);
  }
}
