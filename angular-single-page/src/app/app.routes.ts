import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { TodoComponent } from './todo/todo.component';
import { AuthGuard } from './auth.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'todo', component: TodoComponent, canActivate: [AuthGuard] },
    { path: '', component: LoginComponent }
];
