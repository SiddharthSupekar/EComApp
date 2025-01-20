import { Component } from '@angular/core';
import { RegisterService } from '../../Services/Register/register.service';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {

  constructor(private registerService : RegisterService, private router : Router){}

  userData:string | null = '';
  passwordsData:any;
  passwordMatchValidator:ValidatorFn = (formGroup:AbstractControl)=>{
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  };
  passwords:FormGroup = new FormGroup({
    oldPassword: new FormControl('',Validators.required),
    newPassword: new FormControl('',Validators.required),
    confirmPassword : new FormControl('', Validators.required)
  }, this.passwordMatchValidator
)

  onSubmit():void{
    this.userData = sessionStorage.getItem("username");
    this.passwordsData = this.passwords.value;
    const details = {
      username : this.userData,
      oldPassword : this.passwordsData.oldPassword,
      newPassword : this.passwordsData.newPassword
    }

    this.registerService.changePassword(details).subscribe({
      next:(value:any)=>{
        Swal.fire({
          icon: "success",
          title: "Product deleted successfully",
          showConfirmButton: true,
          timer: 800
        });
        this.router.navigateByUrl('/layout')
      }
    })

  }
  isFieldInvalid(fieldName: string): boolean {
    debugger
    const control = this.passwords.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

}
