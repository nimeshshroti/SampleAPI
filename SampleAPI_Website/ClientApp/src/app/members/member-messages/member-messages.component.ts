import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { error } from 'protractor';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-member-messages',
    templateUrl: './member-messages.component.html',
    styleUrls: ['./member-messages.component.css']
})
/** member-messages component*/
export class MemberMessagesComponent implements OnInit {
  @Input() receiverId: number;
  messages: Message[];
  newMessage: any = {};
  /** member-messages ctor */
  constructor(private userService: UserService, private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  this.loadMessages();
}

  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.receiverId).
      pipe(tap(messages => {
        for (let i = 0; i < messages.length; i++) {
          if (messages[i].isRead === false && messages[i].receiverId === currentUserId) {
            this.userService.markAsRead(currentUserId, messages[i].id);
          }
        }
      }))
      .subscribe(messages => {
        this.messages = messages;
      }, error => {
        this.alertify(error);
      });
  }

  sendMessage() {
    this.newMessage.receiverId = this.receiverId;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage).
      subscribe((message: Message) => {
        this.messages.unshift(message);
        this.newMessage.content = '';
      }, error => {
        this.alertify.error(error);
      });
  }
}
