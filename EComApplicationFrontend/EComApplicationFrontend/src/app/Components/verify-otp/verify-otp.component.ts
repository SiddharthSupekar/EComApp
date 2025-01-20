import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../../Services/Login/login.service';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './verify-otp.component.html',
  styleUrls: ['./verify-otp.component.css']
})
export class VerifyOtpComponent {
  constructor(private loginService: LoginService, private router: Router) {}

  otpForm: FormGroup = new FormGroup({
    otp: new FormControl('', [Validators.required, Validators.pattern(/^\d{6}$/)]) // Validate for exactly 6 digits
  });

  otpData: any;
  username: string = '';

  onSubmit(): void {
    this.username = sessionStorage.getItem('username') || '';
    this.otpData = this.otpForm.value;

    if (this.otpForm.valid && this.username) {
      const otpDetails = {
        username: this.username,
        otp: this.otpData.otp
      };

      console.log(otpDetails);

      this.loginService.verifyOtp(otpDetails).subscribe({
        next: (value: any) => {
          Swal.fire({
            icon: "success",
            title: "Otp Verified",
            showConfirmButton: true,
            timer: 1000
          });
          sessionStorage.setItem('verifyOtp', 'true');
          const role = sessionStorage.getItem('role');
          if (role === 'Admin') {
            this.router.navigateByUrl('/layout/admin-dashboard');
          } else if (role === 'Customer') {
            this.router.navigateByUrl('/layout/customer-marketplace');
          } else {
            this.router.navigateByUrl('/layout');
          }
        },
        error: (error: any) => {
          alert('Invalid OTP');
          console.error(error);
        }
      });
    }
  }
}
