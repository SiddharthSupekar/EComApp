import { Component } from '@angular/core';
import { LoginService } from '../../Services/Login/login.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2'
import { customEmailValidator } from '../register/emailValidator';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {

  constructor(private loginService : LoginService, private router : Router){}

  forgotPassword : FormGroup=new FormGroup({
    email : new FormControl('',[Validators.required, customEmailValidator()])
  });

  formData:any;

  onSubmit(){
    debugger
    this.formData= this.forgotPassword.value;
    const email:string = this.formData.email;
    this.loginService.forgotPassword(email).subscribe({
      next:(value:any)=>{
        console.log(value);
        Swal.fire({
          icon: "success",
          title: "Sent",
          text: "Your new login credentials have been sent to your registered email address",
          showConfirmButton:false,
          timer:1000
        });
        this.router.navigateByUrl('/login')
      },
      error:(error:any)=>{
        Swal.fire({
          icon: "error",
          title: "Wrong email",
          text: "Could not the entered any email in our database ",
          showConfirmButton:false,
          timer:1000
        });
      }
    })
  }
  isFieldInvalid(fieldName: string): boolean {
    const control = this.forgotPassword.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

}
