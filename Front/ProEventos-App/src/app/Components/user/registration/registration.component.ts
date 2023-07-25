import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from 'src/app/helpers/validatorField';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  form!: FormGroup;

  constructor(private fb: FormBuilder) { }
  get f(): any {
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
  }
  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha','confirmeSenha')
    }
    this.form = this.fb.group({
      primeiroNome:['',Validators.required],
      ultimoNome:['',Validators.required] ,
      email:['',[Validators.required,Validators.email]] ,
      userName:['',Validators.required] ,
      senha:['',[Validators.required,Validators.minLength(6)]] ,
      confirmeSenha:['',[Validators.required,Validators.minLength(6)]]
    },formOptions)
  }
}
