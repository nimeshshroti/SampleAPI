import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';



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
    constructor(private authService: AuthService) {

  }

  ngOnInit() {
    
  };

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log("registration successful.");
    }, error => {
      console.log(error);
    }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log("cancelled");
  }
}
