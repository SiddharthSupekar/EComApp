import { Component, OnInit } from '@angular/core';
import { RegisterService } from '../../Services/Register/register.service';
import { CommonModule, DatePipe, formatDate } from '@angular/common';
import { CountrystateService } from '../../Services/CountryState/countrystate.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import Swal from 'sweetalert2'
import { ReadVarExpr } from '@angular/compiler';
import { read } from '@popperjs/core';
import { SharedServiceService } from '../../Services/SharedService/shared-service.service';
import { customEmailValidator } from '../register/emailValidator';
import { PhoneDirective } from '../../Directives/PhoneDirective/phone.directive';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe, CommonModule, PhoneDirective],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  userProfile: any = null;
  countryData: any[] = [];
  stateData: any[] = [];
  filteredStateData : any[] = []
  selectedFile: File | null = null;
  roles:any;
  isView:boolean = true
  loggedInEmail : string | null = ''
  image:any
  todayDate:any = formatDate(new Date(), "yyyy-MM-dd","en")

  constructor(
    private registerService: RegisterService,
    private countrystateService: CountrystateService,
    private sharedService : SharedServiceService
  ) {}

  updateForm:FormGroup = new FormGroup({
    
    firstName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    lastName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    email: new FormControl('', [Validators.required, customEmailValidator()]),
    mobile: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$')]),
    dob: new FormControl('', Validators.required),
    address: new FormControl(''),
    countryId: new FormControl('', Validators.required),
    stateId: new FormControl('', Validators.required),
    zipcode: new FormControl('', Validators.required)
  })

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.getProfile();
    this.getCountries();
  }

  onSubmit(){
    debugger
    // const updateData = this.updateForm.value;
    // updateData.append
    if(this.updateForm.valid){
      const formData = new FormData();
      Object.keys(this.updateForm.controls).forEach((key) => {
        formData.append(key, this.updateForm.get(key)?.value as string);
      });

      if(this.selectedFile){
        formData.append('profileImage', this.selectedFile, this.selectedFile.name)
      }


      this.registerService.updateUser(sessionStorage.getItem("username")?? '',formData).subscribe({
        next : (value:any)=>{
          debugger
          Swal.fire({
            icon: "success",
            title: "Profile updated",
            showConfirmButton: true,
            timer: 1000
          });
          this.isView = true;
          this.getProfile();
        },
        error :(error : any)=>{
          Swal.fire({
            icon: "error",
            title: "Error!!",
            text: "Either null value or the image profile picture is not selected",
          });
        }
      });
  
    }
    else{
      this.updateForm.markAllAsTouched();
    }
    // else{
    //   formData.append('profileImage',)
    // }

  }

  forUpdate(){
    this.isView = false
    this.updateForm.get('firstName')?.setValue(this.userProfile.firstName);
    this.updateForm.get('lastName')?.setValue(this.userProfile.lastName);
    this.updateForm.get('email')?.setValue(this.userProfile.email);
    this.updateForm.get('mobile')?.setValue(this.userProfile.mobile);
    this.updateForm.get('dob')?.setValue(formatDate(this.userProfile.dob,"yyyy-MM-dd","en"));
    this.updateForm.get('address')?.setValue(this.userProfile.address);
    this.updateForm.get('countryId')?.setValue(this.userProfile.countryId);
    this.onCountryChange();
    this.updateForm.get('stateId')?.setValue(this.userProfile.stateId);
    this.updateForm.get('zipcode')?.setValue(this.userProfile.zipcode);
  }

  onFileSelected(event: any): void {
    debugger
    this.selectedFile = event.target.files[0];
    const imagefromhtml = event.target as HTMLInputElement
    if(imagefromhtml.files && imagefromhtml.files[0]){
      const file = imagefromhtml.files[0]
      var reader = new FileReader();
    reader.onload=()=>{
      this.image = reader.result
    };
    reader.readAsDataURL(file)
    }

    
  }

  getRoles(){
    this.registerService.getRoles().subscribe({
      next:(value:any)=>{
        this.roles = value
      }
    })
  }

  getCountries(): void {
    this.countrystateService.getCountries().subscribe({
      next: (countries: any[]) => {
        
        this.countryData = countries;
        this.tryMapCountryState();
      },
      error: (error: any) => {
        console.error('Error fetching countries:', error);
      },
    });
  }


  getProfile(): void {
    debugger
    this.loggedInEmail = sessionStorage.getItem("email")?? '';
    this.registerService.getUserByEmail(this.loggedInEmail).subscribe({
      next: (profile: any) => {
        debugger
        this.userProfile = profile;
        this.tryMapCountryState();
        this.sharedService.setProfPicture('https://localhost:7040/'+this.userProfile.profileImage);
      },
      error: (error: any) => {
      },
    });
  }

  getState(countryId: number): void {
    this.countrystateService.getStateByCountryId(countryId).subscribe({
      next: (states: any[]) => {
        this.stateData = states;
        this.mapState();
      },
      error: (error: any) => {
      },
    });
  }


  tryMapCountryState(): void {
    if (this.userProfile && this.countryData.length) {
      const country = this.countryData.find(
        (c: any) => c.id === this.userProfile.countryId
      );
      this.userProfile.countryName = country ? country.name : 'Unknown';

      this.getState(this.userProfile.countryId);
    }
  }


  mapState(): void {
    if (this.userProfile && this.stateData.length) {
      const state = this.stateData.find(
        (s: any) => s.id === this.userProfile.stateId
      );
      this.userProfile.stateName = state ? state.name : 'Unknown';
    }
  }

  onCountryChange(): void {
    debugger
    const selectedCountry = this.updateForm.get('countryId')?.value;
    if (selectedCountry) {
      this.countrystateService.getStateByCountryId(selectedCountry).subscribe({
        next: (value: any) => {
          this.filteredStateData = value;
          console.log(this.filteredStateData);
        },
        error: (error: any) => {
          console.error(error);
        }
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const control = this.updateForm.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }
}
