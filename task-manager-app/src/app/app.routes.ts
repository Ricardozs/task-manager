import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'tasks',
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
  },
  {
    path: 'tasks',
    canActivate: [authGuard],
    loadChildren: () => import('./features/tasks/tasks.routes').then((m) => m.TASKS_ROUTES),
  },
  {
    path: '**',
    redirectTo: 'tasks',
  },
];
