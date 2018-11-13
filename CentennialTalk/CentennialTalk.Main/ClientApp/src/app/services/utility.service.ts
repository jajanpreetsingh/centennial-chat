import { Injectable } from '@angular/core';
import { Router, Params } from '@angular/router';

@Injectable()
export class UtilityService {
  constructor(private router: Router) { }

  navigateWithData(path, data) {
    this.router.navigate([path], data);
  }

  navigateToPath(path) {
    this.router.navigate([path]);
  }

  showErrorAlert(message) {
  }

  showSuccessAlert(message) {
  }
}
