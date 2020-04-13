import { Component, Input } from '@angular/core';
import { User } from '../../_models/user';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
    selector: 'app-member-card',
    templateUrl: './member-card.component.html',
    styleUrls: ['./member-card.component.css']
})
/** member-card component*/
export class MemberCardComponent {
  @Input() user: User
  /** member-card ctor */
  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  sendLikes(id: number) {
    this.userService.sendLikes(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You have liked successfully - ' + this.user.knownAs);
    }, error => {
      this.alertify.error(error);
    });
  }
}
