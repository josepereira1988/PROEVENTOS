import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LoteService } from './services/lote.service';
import { EventoService } from './services/evento.service';

import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxCurrencyModule } from 'ngx-currency';
import { TabsModule } from 'ngx-bootstrap/tabs';



import { EventosComponent } from './Components/eventos/eventos.component';
import { PalestrantesComponent } from './Components/palestrantes/palestrantes.component';
import { ContatosComponent } from './Components/contatos/contatos.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { PerfilComponent } from './Components/user/perfil/perfil.component';
import { EventoDetalheComponent } from './Components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './Components/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './Components/user/user.component';
import { RegistrationComponent } from './Components/user/registration/registration.component';
import { LoginComponent } from './Components/user/login/login.component';

import { NavComponent } from './shared/nav/nav.component';
import { TituloComponent } from './shared/titulo/titulo.component';
import { AccountService } from './services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { HomeComponent } from './Components/home/home.component';
import { PerfilDetalheComponent } from './Components/user/perfil/perfil-detalhe/perfil-detalhe.component';
import { RedesSociaisComponent } from './Components/redesSociais/redesSociais.component';
import { PalestranteListaComponent } from './Components/palestrantes/palestrante-lista/palestrante-lista.component';
import { PalestranteDetalheComponent } from './Components/palestrantes/palestrante-detalhe/palestrante-detalhe.component';

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
      EventosComponent,
      PalestrantesComponent,
      NavComponent,
      DateTimeFormatPipe,
      ContatosComponent,
      DashboardComponent,
      PerfilComponent,
      TituloComponent,
      EventoDetalheComponent,
      EventoListaComponent,
      UserComponent,
      RegistrationComponent,
      LoginComponent,
      HomeComponent,
      PerfilDetalheComponent,
      RedesSociaisComponent,
      PalestranteListaComponent,
      PalestranteDetalheComponent

   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    NgxCurrencyModule,
    PaginationModule.forRoot(),
    ToastrModule.forRoot(),
    TabsModule.forRoot()
  ],
  providers: [EventoService,LoteService,AccountService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
