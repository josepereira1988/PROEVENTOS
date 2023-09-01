import { PaginatedResult, Pagination } from './../../../models/Pagination';
import { Router } from '@angular/router';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';
import { environment } from 'src/environments/environment';
import { Subject, debounceTime } from 'rxjs';

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
  public eventoId: number = 0;
  public pagination = {} as Pagination;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged
        .pipe(debounceTime(1000))
        .subscribe((filtrarPor) => {
          this.spinner.show();
          this.eventoService
            .getEventos(
              this.pagination.currentPage,
              this.pagination.itemsPerPage,
              filtrarPor
            )
            .subscribe(
              (paginatedResult: PaginatedResult<Evento[]>) => {
                this.eventos = paginatedResult.result;
                this.pagination = paginatedResult.pagination;
              },
              (error: any) => {
                this.spinner.hide();
                this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
              }
            )
            .add(() => this.spinner.hide());
        });
    }
    this.termoBuscaChanged.next(evt.value);
  }
  constructor(private eventoService: EventoService, private modalService: BsModalService,
    private toastr: ToastrService, private spinner: NgxSpinnerService, private router: Router
  ) { }

  public ngOnInit(): void {
    //this.spinner.show();
    this.pagination = { currentPage: 1, itemsPerPage: 3, totalItems: 1 } as Pagination;
    this.carregarEventos();
    /** spinner starts on init */

  }

  public carregarEventos(): void {
    this.spinner.show();
    this.eventoService
      .getEventos(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (paginatedResult: PaginatedResult<Evento[]>) => {
          this.eventos = paginatedResult.result;
          this.pagination = paginatedResult.pagination;

        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
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
  pageChanged(event): void {

    this.pagination.currentPage = event.page;
    this.carregarEventos();

  }
}
