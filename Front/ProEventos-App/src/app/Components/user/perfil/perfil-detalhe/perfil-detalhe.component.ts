import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ValidatorField } from 'src/app/helpers/validatorField';
import { UserUpdate } from 'src/app/models/Identity/UserUpdate';
import { PalestranteService } from 'src/app/services/Palestrante.service';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter();

  usuario = {} as UserUpdate;
  form!: FormGroup;

  constructor( private fb: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService
  ) { }
  public get ehPalestrante(): boolean {
    return this.usuario.funcao === 'Palestrante';
  }
  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }

  private verificaForm(): void {
    this.form.valueChanges
      .subscribe(() => this.changeFormValue.emit({...this.form.value}))
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService
      .getUser()
      .subscribe(
        (userRetorno: UserUpdate) => {
          console.log(userRetorno);
          this.usuario = userRetorno;
          this.form.patchValue(this.usuario);
          this.toaster.success('Usuário Carregado', 'Sucesso');
        },
        (error) => {
          console.error(error);
          this.toaster.error('Usuário não Carregado', 'Erro');
          this.router.navigate(['/dashboard']);
        }
      )
      .add(() => this.spinner.hide());
  }

  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword'),
    };

    this.form = this.fb.group(
      {
        userName: [''],
        imagemURL: [''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', [Validators.required]],
        descricao: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        password: ['', [Validators.minLength(4), Validators.nullValidator]],
        confirmePassword: ['', Validators.nullValidator],
      },
      formOptions
    );
  }

  // Conveniente para pegar um FormField apenas com a letra F
  get f(): any {
    return this.form.controls;
  }

  onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.usuario = { ...this.form.value };
    this.spinner.show();

    if (this.f.funcao.value == 'Palestrante') {
      this.palestranteService.post().subscribe(
        () => this.toaster.success('Função palestrante Ativada!', 'Sucesso!'),
        (error) => {
          this.toaster.error('A função palestrante não pode ser Ativada', 'Error');
          console.error(error);
        }
      )
    }

    this.accountService
      .updateUser(this.usuario)
      .subscribe(
        () => this.toaster.success('Usuário atualizado!', 'Sucesso'),
        (error) => {
          this.toaster.error(error.error);
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }
}
