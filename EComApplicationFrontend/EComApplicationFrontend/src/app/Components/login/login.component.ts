import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { LoginService } from '../../Services/Login/login.service';
import { CommonModule, NgClass } from '@angular/common';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule, RouterLink, NgClass, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private loginService: LoginService, private router: Router) {}

  // Define FormGroup
  loginForm: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  logFormData: any;
  loggedAgent: any;

  // Verifying function
  forVerifying(): void {
    this.logFormData = this.loginForm.value;

    // Service call for login
    this.loginService.postLogin(this.logFormData).subscribe({
      next: (value: any) => {
        this.loggedAgent = value;

        // Store data in session storage
        sessionStorage.setItem('username', this.loggedAgent.username);
        sessionStorage.setItem('email', this.loggedAgent.email);
        sessionStorage.setItem('role', this.loggedAgent.roleName);
        sessionStorage.setItem('token', this.loggedAgent.token);
        sessionStorage.setItem('userId', this.loggedAgent.userId);
        sessionStorage.setItem('cartId', this.loggedAgent.cartId);

        Swal.fire({
          icon: "success",
          timer: 1500 ,
          showConfirmButton:false
        });
        this.router.navigateByUrl('/verify');
      },
      error: (value: any) => {
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Incorrect username or password",
          showConfirmButton:false
        });
        this.resetForm();
      }
    });
  }

  // Reset form
  resetForm(): void {
    this.loginForm.reset();
  }
}
