import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output()  cancelRegister = new EventEmitter();
  model: any={};
  registerForm: FormGroup;
  maxDate: Date;
  

  constructor(private accountService : AccountService, private toastr: ToastrService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18); // para que solo acepte mayores de 17
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      
      password: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
    // si cambio el password una vez ya validado, con esto forzamos que vuelva a comprar la validez   
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
    
  }
  //validez password-confirmPassword
  matchValues(matchTo: string) : ValidatorFn{
    return(control: AbstractControl)=>{
      return control?.value === control?.parent?.controls[matchTo].value ? null: {isMatching: true}
    }
  }
  register(){
    console.log(this.registerForm.value);

    /*
    this.accountService.register(this.model).subscribe(response=>{
      console.log(response);
      this.cancel();
    },error=>{
      console.log(error)
      this.toastr.error(error.error);
    })
    */
  }

  cancel(){
    //console.log("cancelled")
    //emitimos un false cuando hace click en cancel
    this.cancelRegister.emit(false)
  }
}
