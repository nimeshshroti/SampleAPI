import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
  model: any = {};
  

  constructor(private authService: AuthService) { }
  ngOnInit() {
    
  };

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('logged in successfully');
    }, error => {
      console.log('failed to login');
    })
  }

  loggedin() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');


  }
  
}

