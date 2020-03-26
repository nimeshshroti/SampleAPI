import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeeComponent } from './homee/homee.component';
import { RegisterComponent } from './register/register.component';
import { AuthService } from './_services/auth.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HomeeComponent,
    RegisterComponent,
    CounterComponent,
    ValueComponent,
    NavComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'value', component: ValueComponent },
      { path: 'nav', component: NavComponent },
      { path: 'homee', component: HomeeComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
