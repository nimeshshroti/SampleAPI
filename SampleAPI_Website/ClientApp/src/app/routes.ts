import { Routes } from "@angular/router";
import { HomeeComponent } from './homee/homee.component';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from "./_guards/auth.guard";



export const appRoutes: Routes=[
  { path: '', component: HomeeComponent },
  {
    path: '',
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'messages', component: MessagesComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] }
    ]
  },
  { path: '**', redirectTo: '', pathMatch:'full' }
];
