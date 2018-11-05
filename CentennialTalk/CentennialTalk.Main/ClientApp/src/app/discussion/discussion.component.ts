import { Component } from '@angular/core';
import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';

@Component({
  selector: 'app-discussion',
  templateUrl: './discussion.component.html',
  styleUrls: ['./discussion.component.scss'],
  animations: [
    trigger('fiftyAnimation', [
      transition('small <=> large', animate('700ms ease-in', keyframes([
        style({ opacity: 0, transform: 'translateY(-75%)', offset: 0 }),
        style({ opacity: 1, transform: 'scale(1.2)', offset: 0.4 }),
        style({ opacity: 1, transform: 'translateY(35px)', offset: 0.5 }),
        style({ opacity: 1, transform: 'scale(1)', offset: 0.99 }),
        style({ opacity: 1, transform: 'translateY(0)', offset: 1.0 })
      ]))),
    ]),
    trigger('seventyFiveAnimation', [
      transition('fixed <=> shaked', animate('1000ms ease-in', keyframes([
        style({ opacity: 0, transform: 'translateX(-50%)', offset: 0.0 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #FFF, #744DA8)', transform: 'translateY(11%)', offset: 0.1 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #744DA8, #1FB3E0)',transform: 'translateX(30%)', offset: 0.2 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #1FB3E0, #49C219)',transform: 'translateY(-29%)', offset: 0.3 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #49C219, #F4DC2A)',transform: 'translateX(-25%)', offset: 0.4 }),
        style({ opacity: 0.6, background: 'linear-gradient(to right, #F4DC2A, #EEB417)',transform: 'translateY(44%)', offset: 0.5 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #EEB417, #D65129)',transform: 'scale(1.5)', offset: 0.7 }),
        style({ opacity: 0.2, background: 'linear-gradient(to right, #D65129, #D6CDCA)',transform: 'scale(1.2)', offset: 0.8 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #D6CDCA, #FFF)', transform: 'translateX(0)', offset: 0.9 }),
        style({ opacity: 1, transform: 'scale(1)', offset: 0.95 }),
        style({ opacity: 1, transform: 'translateY(0)', offset: 1.0  })
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


