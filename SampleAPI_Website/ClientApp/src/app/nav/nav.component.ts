import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
  model: any = {};
  

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }
  ngOnInit() {
    
  };

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, error => {
      this.alertify.error(error);
      }, () => {
        this.router.navigate(['/members']);
    });
  }

  loggedin() {
    return this.authService.loggedin();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('logged out');
    this.router.navigate(['/homee']);


  }
  
}

