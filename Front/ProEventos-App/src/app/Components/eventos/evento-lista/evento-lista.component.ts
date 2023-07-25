import { Router } from '@angular/router';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImg: number = 150;
  public marginImg: number = 2;
  public isCollapsed = true;
  private filtroListado: string = '';
  public eventoId: number = 0;
  public get filtroLista() {
    return this.filtroListado;
  }
  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroListado) : this.eventos
  }
  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }
  constructor(private eventoService: EventoService, private modalService: BsModalService,
    private toastr: ToastrService, private spinner: NgxSpinnerService, private router: Router
  ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.carregarEventos();
    /** spinner starts on init */

  }

  public carregarEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp
        this.eventosFiltrados = this.eventos
      },
      error: (error: any) => {
        this.spinner.hide()
        this.toastr.error("Erro ao carregar os eventos", 'Erro!')
      },
      complete: () => this.spinner.hide()
    }
    )

  }
  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        console.log(result)
        this.toastr.success('O Evento deletado com sucesso', 'Deletado!');
        this.spinner.hide();
        this.carregarEventos();

      },
      (erro: any) => {
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId} ${erro}`);
        this.spinner.hide();
       },
      () => this.spinner.hide(),
    )
  }
  decline(): void {
    this.modalRef?.hide();
  }
  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }
  mostraImagem(imagemURL: string): string {

    return (imagemURL !== '') ? environment.apiURL + 'Resources/Images/' + imagemURL : 'assets/upload.png';

  }
}
