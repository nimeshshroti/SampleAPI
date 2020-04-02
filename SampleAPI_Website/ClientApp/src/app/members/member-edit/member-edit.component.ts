import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';

@Component({
    selector: 'app-member-edit',
    templateUrl: './member-edit.component.html',
    styleUrls: ['./member-edit.component.css']
})
/** member-edit component*/
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  @HostListener('window: beforeunload', ['$event'])
  unloadEvent($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  /** member-edit ctor */
  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
    private authService: AuthService, private userService: UserService) { }

    ngOnInit() {
      this.route.data.subscribe(data => {
        this.user = data['user'];
      });
    }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('Profile updated successfully.');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
      });

    }
}
