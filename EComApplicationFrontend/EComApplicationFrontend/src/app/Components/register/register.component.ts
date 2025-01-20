import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule, formatDate } from '@angular/common';
import { RegisterService } from '../../Services/Register/register.service';
import { Router, RouterLink } from '@angular/router';
import { CountrystateService } from '../../Services/CountryState/countrystate.service';
import Swal from 'sweetalert2'
import { PhoneDirective } from '../../Directives/PhoneDirective/phone.directive';
import { customEmailValidator } from './emailValidator';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink,PhoneDirective],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  private router = inject(Router);

  selectedFile: File | null = null; // Holds the selected file
  isSubmitting = false; // Loading state
  submissionMessage: string | null = null; // Success/Error message
  countryData: any;
  filteredStateData: any;
  roles: any;
  image:Blob | null = null
  todayDate:any = formatDate(new Date(), "yyyy-MM-dd", "en")


  constructor(
    private fb: FormBuilder, 
    private registerService: RegisterService, 
    private countrystateService: CountrystateService
  ) {}

  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required,Validators.maxLength(20)]),
    lastName: new FormControl('',  [Validators.required,Validators.maxLength(20)]),
    email: new FormControl('', [Validators.required, customEmailValidator()]),
    mobile: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$')]),
    roleId: new FormControl('', Validators.required),
    dob: new FormControl('', Validators.required),
    address: new FormControl(''),
    countryId: new FormControl('', Validators.required),
    stateId: new FormControl('', Validators.required),
    zipcode: new FormControl('', Validators.required)
  });

  ngOnInit(): void {
    this.getCountries();
    this.getRoles();
  }

  // Handle file selection
  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  getCountries(): void {
    this.countrystateService.getCountries().subscribe({
      next: (value: any) => {
        this.countryData = value;
        console.log(this.countryData);
      },
      error: (error: any) => {
        console.error(error);
      }
    });
  }

  // Submit the form data
  onSubmit(): void {
    debugger
    if (this.registerForm.invalid) {
      this.submissionMessage = 'Please fill all required fields.';
      this.registerForm.markAllAsTouched();
    }

    if(this.registerForm.valid){
      this.isSubmitting = true;

    // Create FormData to handle file upload and user data
    const formData = new FormData();
    Object.keys(this.registerForm.controls).forEach((key) => {
      formData.append(key, this.registerForm.get(key)?.value as string);
    });

    console.log("Checking data",formData.get("countryId"),formData.get("stateId"))

    // Add the image file if selected
    if (this.selectedFile) {
      formData.append('profileImage', this.selectedFile, this.selectedFile.name);
    }

    console.log('FormData to be submitted:', formData);

    // Call the backend API
    this.registerService.postUser(formData).subscribe({
      next: (value: any) => {
        debugger;
        Swal.fire({
          icon: "success",
          title: "Registered Successfully",
          text: "Your login details will be sent to your registered email address",
          showConfirmButton: false,
          timer: 2000
        });
        console.log(value);
        const cartId = value;
        sessionStorage.setItem("cartId", cartId)
        this.router.navigateByUrl('/login');
      },
      error: (error: any) => {
        console.error(error);
        Swal.fire({
          icon: "error",
          title: "Error",
          text:"Entered email address already exists",
          showConfirmButton: false,
          timer: 1000
        });
        this.submissionMessage = 'An error occurred while submitting the form.';
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
    }
  }

  getRoles(): void {
    this.registerService.getRoles().subscribe({
      next: (value: any) => {
        this.roles = value;

      },
      error: (error: any) => {
        console.error(error);
      }
    });
  }

  onCountryChange(): void {
    debugger
    const selectedCountry = this.registerForm.get('countryId')?.value;
    if (selectedCountry) {
      this.countrystateService.getStateByCountryId(selectedCountry).subscribe({
        next: (value: any) => {
          this.filteredStateData = value;
        },
        error: (error: any) => {
          console.error(error);
        }
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    debugger
    const control = this.registerForm.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }
}
