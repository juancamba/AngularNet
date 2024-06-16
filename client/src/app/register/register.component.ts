import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any={};
  registerForm: FormGroup;
  
  @Output()  cancelRegister = new EventEmitter();
  constructor(private accountService : AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
      confirmPassword: new FormControl()
    })
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
