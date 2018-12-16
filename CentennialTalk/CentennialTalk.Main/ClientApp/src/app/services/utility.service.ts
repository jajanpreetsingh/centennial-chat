import { Injectable } from '@angular/core';
import { Router, Params } from '@angular/router';

@Injectable()
export class UtilityService {
  constructor(private router: Router) { }

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
}
