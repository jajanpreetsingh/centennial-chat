import { Component } from '@angular/core';
import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';

@Component({
  selector: 'app-discussion',
  templateUrl: './discussion.component.html',
  styleUrls: ['./discussion.component.scss'],
  animations: [
    trigger('fiftyAnimation', [
      transition('small <=> large', animate('1500ms ease-in', keyframes([
        style({ color: '#744DA8', offset: 0.05 }),
        style({ color: '#1FB3E0', offset: 0.20 }),
        style({ color: '#49C219', offset: 0.35 }),
        style({ color: '#F4DC2A', offset: 0.5 }),
        style({ color: '#EEB417', offset: 0.65 }),
        style({ color: '#D65129', offset: 0.8 }),
        style({ color: '#D6CDCA', offset: 0.95 })
        ]))),
    ]),
    trigger('seventyFiveAnimation', [
      transition('fixed <=> shaked', animate('1000ms ease-in', keyframes([
        style({transform: 'scale(1.2)', offset: 0.2 }),
        style({transform: 'scale(1.4)', offset: 0.5 }),
        style({transform: 'scale(1.6)', offset: 0.8 }),
        style({transform: 'scale(1)', offset: 1.0  })
      ])))
    ]),
  ]
})
export class DiscussionComponent {
  stateFifty: string = 'small';
  stateSeventyFive: string = 'fixed';

  animateMeFifty() {
    this.stateFifty = (this.stateFifty === 'small' ? 'large' : 'small');
  }

  animateMeSeventyFive() {
    this.stateSeventyFive = (this.stateSeventyFive === 'fixed' ? 'shaked' : 'fixed');
  }

}


