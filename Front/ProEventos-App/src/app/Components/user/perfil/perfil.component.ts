import { UserUpdate } from './../../../models/Identity/UserUpdate';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Route, Router } from '@angular/router';
import { AccountService } from './../../../services/account.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from 'src/app/helpers/validatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form!: FormGroup;
  get f(): any { return this.form.controls; }

  constructor(private fb: FormBuilder,
              private accountService:AccountService,
              private toastr:ToastrService,
              private spinner:NgxSpinnerService,
              private router: Router,
               ) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (result: UserUpdate) => {this.userUpdate = result;
                              this.spinner.hide();
                              this.form.patchValue(this.userUpdate);
                               this.toastr.success('Usuário Carregado', 'Sucesso');
                              },
      (error:any) => {this.toastr.error("erro ao carregar perfil"), console.log(error),this.spinner.hide()},
      () => {},
    );
  }
  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      userName: [''],
        titulo: ['NaoInformado', Validators.required],
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', [Validators.required]],
        descricao: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        password: ['', [Validators.minLength(4), Validators.nullValidator]],
        confirmePassword: ['', Validators.nullValidator],
    }, formOptions);
  }
  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }
  onSubmit(): void {
    // Vai parar aqui se o form estiver inválido
    this.atualizarUsuario();
  }
  public atualizarUsuario() {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();
    this.accountService
      .updateUser(this.userUpdate)
      .subscribe(
        () => this.toastr.success('Usuário atualizado!', 'Sucesso'),
        (error) => {
          this.toastr.error(error.error);
          console.error(error);
        }
      )
      .add(() => this.spinner.hide());

  }
}
