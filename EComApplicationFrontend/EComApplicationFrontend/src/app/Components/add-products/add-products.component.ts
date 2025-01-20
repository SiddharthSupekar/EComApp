import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ProductService } from '../../Services/Product/product.service';
import { CommonModule, formatDate, NgClass } from '@angular/common';
import Swal from 'sweetalert2'
import { PhoneDirective } from '../../Directives/PhoneDirective/phone.directive';

@Component({
  selector: 'app-add-products',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, NgClass, CommonModule],
  templateUrl: './add-products.component.html',
  styleUrl: './add-products.component.css'
})
export class AddProductsComponent {

  constructor(private productService : ProductService){}
  todayDate:any = formatDate(new Date(),"yyyy-MM-dd","en") 
  sellingPrice: any;
  purchasePrice:any;
// sellingPriceValidator:ValidatorFn = (formGroup:AbstractControl)=>{
//   const sellingPrice = formGroup.get('sellingPrice')?.value;
//   const purchasePrice = formGroup.get('purchasePrice')?.value;
//   return sellingPrice > purchasePrice ? null: {'invalid' : true};
// };
  productDetails:FormGroup = new FormGroup({
    productName : new FormControl('',[Validators.required,Validators.maxLength(20)]),
    productCode : new FormControl('',[Validators.required,Validators.maxLength(6), Validators.minLength(6)]),
    category : new FormControl('',[Validators.required,Validators.maxLength(20)]),
    brand : new FormControl('',[Validators.required,Validators.maxLength(20)]),
    sellingPrice : new FormControl(0, [Validators.required]),
    purchasePrice : new FormControl(0,[Validators.required]),
    purchaseDate : new FormControl('',[Validators.required,Validators.max((this.todayDate))]),
    stock : new FormControl('', Validators.required )
  }
);

  selectedFile: File | null = null; 

  onAdd(){
    debugger
    if(this.sellingPrice <= this.purchasePrice){
      this.productDetails.setErrors({'invalid':true});
      
    }
    if(this.productDetails.invalid){
      this.productDetails.markAllAsTouched()
    }
    if(this.productDetails.valid)
    {
      const formData = new FormData();

      Object.keys(this.productDetails.controls).forEach((key)=>{
        formData.append(key,this.productDetails.get(key)?.value as string)
      });

      if(this.selectedFile){
        formData.append("productImage",this.selectedFile,this.selectedFile.name);
      }
      const userId = parseInt(sessionStorage.getItem("userId") || '0', 10)
      this.productService.addProduct(userId, formData).subscribe({
        next:(value:any)=>{
          Swal.fire({
            icon: "success",
            title: "Your work has been saved",
            showConfirmButton: false,
            timer: 800
          });
          this.productDetails.reset();
        },
        error:(error:any)=>{
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Either Null entry or the entered product code already exists ",
            showConfirmButton:false,
            timer : 1200
          });
        }
      });
    }

    

  }
  onFileSelected(event:any){
    this.selectedFile = event.target.files[0]
  }

  isFieldInvalid(fieldName: string): boolean {
    const control = this.productDetails.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }


}
