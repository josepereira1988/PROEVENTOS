import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes, CanActivate } from '@angular/router';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { EventosComponent } from './Components/eventos/eventos.component';
import { ContatosComponent } from './Components/contatos/contatos.component';
import { PerfilComponent } from './Components/user/perfil/perfil.component';
import { PalestrantesComponent } from './Components/palestrantes/palestrantes.component';
import { EventoDetalheComponent } from './Components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './Components/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './Components/user/user.component';
import { LoginComponent } from './Components/user/login/login.component';
import { RegistrationComponent } from './Components/user/registration/registration.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './Components/home/home.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'user', redirectTo: 'user/perfil' },
      {
        path: 'user/perfil',
        component: PerfilComponent,
      },
      { path: 'eventos', redirectTo: 'eventos/lista' },
      {
        path: 'eventos',
        component: EventosComponent,
        children: [
          { path: 'detalhe/:id', component: EventoDetalheComponent },
          { path: 'detalhe', component: EventoDetalheComponent },
          { path: 'lista', component: EventoListaComponent },
        ],
      },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'palestrantes', component: PalestrantesComponent },
      { path: 'contatos', component: ContatosComponent },
    ],
  },
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
    ],
  },
  { path: 'home', component: HomeComponent },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
