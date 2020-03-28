import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';



@Component({
    selector: 'app-register',
    templateUrl: './register.component.html'
})
/** register component*/
export class RegisterComponent implements OnInit {
  @Input() valuesFromHomee: any;
  @Output() cancelRegister = new EventEmitter();

  model:any ={ };
  /** register ctor */
  constructor(private authService: AuthService, private alertify: AlertifyService) {

  }

  ngOnInit() {
    
  };

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success("registration successful.");
    }, error => {
      this.alertify.error(error);
    }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log("cancelled");
  }
}
