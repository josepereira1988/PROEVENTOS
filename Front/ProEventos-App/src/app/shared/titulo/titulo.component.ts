import { Component, Input, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  @Input() titulo:string = "";
  @Input() subtitulo:string = "Desde 2021";
  @Input() iconClass:string = "fa fa-user";
  @Input() botaoLitar:boolean = false;
  constructor(private router: Router) { }

  ngOnInit() {
  }
  listar(): void{
    this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`]);
  }

}
