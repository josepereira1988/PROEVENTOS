import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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

const routes: Routes = [
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
    ]
  },
  { path: 'eventos', redirectTo: 'eventos/lista' },
  { path: "dashboard", component: DashboardComponent },
  { path: "user/perfil", component: PerfilComponent },
  {
    path: "eventos", component: EventosComponent,
    children: [
      { path: 'detalhe/:id', component: EventoDetalheComponent },
      { path: 'detalhe', component: EventoDetalheComponent },
      { path: 'lista', component: EventoListaComponent }
    ]
  },
  { path: "contatos", component: ContatosComponent },
  { path: "palestrantes", component: PalestrantesComponent },
  { path: "", redirectTo: 'dashboard', pathMatch: 'full' },
  { path: "**", redirectTo: 'dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
