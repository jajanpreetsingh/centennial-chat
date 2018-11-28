import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-projector',
  templateUrl: './projector.component.html',
  styleUrls: ['./projector.component.scss']
})
export class ProjectorComponent {

  constructor(private router: Router) {
  }

  goToTranscript() {
    this.router.navigate(['/transcript']);
  }
}


