import { Component, Input } from '@angular/core';
import { User } from '../../_models/user';

@Component({
    selector: 'app-member-card',
    templateUrl: './member-card.component.html',
    styleUrls: ['./member-card.component.css']
})
/** member-card component*/
export class MemberCardComponent {
  @Input() user: User
    /** member-card ctor */
    constructor() {

    }
}
