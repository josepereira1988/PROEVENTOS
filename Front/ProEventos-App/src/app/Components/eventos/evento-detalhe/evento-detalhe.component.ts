import { LoteService } from './../../../services/lote.service';
import { Lote } from './../../../models/Lote';
import { EventoService } from 'src/app/services/evento.service';
import { Evento } from 'src/app/models/Evento';

import { variable } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar = 'post';
  eventoId!: number;
  loteAtual = { id: 0, nome: '', index: 0 };
  imagemURL = 'assets/upload.png';
  file: File;


  get modoEditar(): boolean {
    return this.estadoSalvar == 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }
  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private loteService: LoteService,
    private toaster: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {
    this.localeService.use('pt-br')
  }
  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');
    if (this.eventoId != null && this.eventoId != 0) {
      this.estadoSalvar = 'put';
      this.spinner.show();
      this.eventoService.getEventoById(this.eventoId).subscribe(
        {
          next: (evento: Evento) => {
            this.evento = { ...evento };
            this.form.patchValue(this.evento);
            if (this.evento.imagemURL !== '') {
              this.imagemURL = environment.apiURL +'Resources/Images/'+this.evento.imagemURL;
            }
            //this.carregarLotes();
            evento.lotes.forEach(lote => {
              this.lotes.push(this.criarLote(lote));
            });
          },
          error: (error: any) => {
            this.spinner.hide(),
              this.toaster.error("Erro ao tentar carregar o evento"),
              console.log(error)
          },
          complete: () => {
            this.spinner.hide()
          }
        }
      )
    }
  }
  public carregarLotes(): void {
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote));
        });
      },
      (error: any) => {
        this.toaster.error('Erro ao tentar carregar lotes', 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }
  public validation(): void {
    this.form = this.fb.group(
      {
        tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', Validators.required],
        dataEvento: ['', Validators.required],
        qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        imagemURL: [''],
        lotes: this.fb.array([])
      }
    )
  }
  adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote))

  }

  criarLote(lote: Lote): FormGroup {

    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required]
    })
  }


  public retornaTitulo(nome: string): string {
    return nome == null || nome == '' ? 'Nome do lote' : nome
  }
  public RESETfORM(): void {
    this.form.reset();
  }
  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }
  public salvarAlteracao(): void {
    this.spinner.show();
    const verbhttp = this.estadoSalvar;

    if (this.form.valid) {
      this.evento = (this.estadoSalvar === 'post')
        ? { ...this.form.value }
        : { id: this.evento.id, ...this.form.value };

      let service = {} as Observable<Evento>;

      if (this.estadoSalvar == 'post') {
        service = this.eventoService.post(this.evento);
      }
      else {
        service = this.eventoService.put(this.evento);
      }
      service.subscribe(
        (result: Evento) => {
          this.toaster.success('Evento salvo com sucesso')
          this.router.navigate([`eventos/detalhe/${result.id}`])
        },
        (error: any) => { console.error(error), this.spinner.hide(), this.toaster.error('Erro ao sabar evento', 'Erro') },
        () => { this.spinner.hide() }
      )
    }
  }
  public salvarLote(): void {
    if (this.form.controls['lotes'].valid) {
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toaster.success('Lotes salvo com Sucesso!', 'Sucesso');
          //this.lotes.reset();
        },
        (error: any) => { this.toaster.error('Erro ao gravar lotes', 'Erro'); console.error(error) },
        () => { }
      ).add(() => this.spinner.hide())
    }
  }
  public removerLotes(template: TemplateRef<any>, index: number): void {
    this.loteAtual.id = this.lotes.get(index + '.id').value;
    this.loteAtual.nome = this.lotes.get(index + '.nome').value;
    this.loteAtual.index = index;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
  public confirmDeletarLote(): void {
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toaster.success('Lote deletado com sucesso', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.index);
      },
      (error: any) => { this.toaster.error(`Erro ao deletar o lote ${this.loteAtual.id}`, 'Erro'); console.error(error) },
      () => { }
    )

  }
  public declineDeletarLote(): void {
    this.modalRef.hide();

  }
  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagem();
  }
  uploadImagem(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toaster.success('Imagem atualizada com Sucesso', 'Sucesso!');
      },
      (error: any) => {
        this.toaster.error('Erro ao fazer upload de imagem', 'Erro!');
        console.log(error);
      }
    ).add(() => this.spinner.hide());
  }

}
