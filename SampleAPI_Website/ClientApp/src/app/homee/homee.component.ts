import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-homee',
    templateUrl: './homee.component.html'
})
/** homee component*/
export class HomeeComponent implements OnInit {

  registerMode = false;
  values: any;
    /** homee ctor */
  constructor(private http: HttpClient) { }
  ngOnInit() {
    this.getValues()
  };

  registerToggle() {
    this.registerMode = true;
  }

  getValues() {
    this.http.get('https://localhost:44351/api/values/').subscribe(response => {
      this.values = response;
    }, error => {
        console.log(error);
      })
  };


  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}
