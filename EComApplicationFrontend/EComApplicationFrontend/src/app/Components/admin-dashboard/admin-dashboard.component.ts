import { Component } from '@angular/core';
import { ProductService } from '../../Services/Product/product.service';
import { CommonModule, formatDate } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule, FormsModule, NgModel } from '@angular/forms';
import Swal from 'sweetalert2'
import { PhoneDirective } from '../../Directives/PhoneDirective/phone.directive';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})

export class AdminDashboardComponent {


  constructor(private productService : ProductService){}
  products:any;
  showForm:boolean=false
  isView:boolean = false
  selectedFile:File | null = null;
  currentProdId:any
  todayDate:any = formatDate(new Date(),"yyyy-MM-dd","en") ;
  sellingPrice:any;
  purchasePrice:any;
  image : any;
  sellingPriceSmall:boolean = false;
  prodPic:any;

  ngOnInit():void{
    this.getAllProducts();
  }
  productDetails:FormGroup = new FormGroup({
    productName : new FormControl('',[Validators.required, Validators.maxLength(20)]),
    productCode : new FormControl('',[Validators.required, Validators.maxLength(6)]),
    category : new FormControl('',[Validators.required, Validators.maxLength(20)]),
    brand : new FormControl('',[Validators.required, Validators.maxLength(20)]),
    sellingPrice : new FormControl('', Validators.required),
    purchasePrice : new FormControl(0,Validators.required),
    purchaseDate : new FormControl(0,[Validators.required,Validators.max((this.todayDate))]),
    stock : new FormControl('', [Validators.required] ),
    // productImage:new FormControl()
  });

  getAllProducts(){
    const userId = parseInt(sessionStorage.getItem("userId") || '0' , 10)
      this.productService.getAllProductsByUser(userId).subscribe({
        next:(value:any)=>{
          debugger
          this.products = value
          console.log(this.products);
        },
        error:(error:any)=>{
          console.log(error);
          
        }
      });
  }

  onUpdate(product:any){
    debugger
    this.isView = false;
    this.showForm = true;
    this.productDetails.enable();
    this.currentProdId = product.id;
    this.prodPic = product.productImage as string;
    this.productDetails.get('productName')?.setValue(product.productName);
    this.productDetails.get('productCode')?.setValue(product.productCode);
    this.productDetails.get('category')?.setValue(product.category);
    this.productDetails.get('brand')?.setValue(product.brand);
    this.productDetails.get('sellingPrice')?.setValue(product.sellingPrice);
    this.productDetails.get('purchasePrice')?.setValue(product.purchasePrice);
    this.productDetails.get('purchaseDate')?.setValue(formatDate(product.purchaseDate,"yyyy-MM-dd", "en"));
    this.productDetails.get('stock')?.setValue(product.stock);
    this.productDetails.get('productName')?.setValue(product.productName);
    // this.productDetails.get('productImage')?.setValue((product.productImage as string));
    // this.productDetails.patchValue(product)
    
  }
  forUpdate(){
    debugger
    // if(this.sellingPrice <= this.purchasePrice){
    //   // this.productDetails.setErrors({'invalid':true});
    //   this.sellingPriceSmall = true
    //   }
    const details = this.productDetails.value
    if(details.sellingPrice < details.purchasePrice)
    {
      this.productDetails.setErrors({'invalid':true});
      this.sellingPriceSmall = true

    }
    if(this.productDetails.invalid){
      this.productDetails.markAllAsTouched()
    }
    if(this.productDetails.valid){

      const formData = new FormData()

    Object.keys(this.productDetails.controls).forEach((key)=>{
      formData.append(key,this.productDetails.get(key)?.value as string);
    });

    if(this.selectedFile){
      formData.append('productImage',this.selectedFile,this.selectedFile.name);
    }
    const userId = parseInt(sessionStorage.getItem("userId") || '0',10)
    this.productService.updateProduct(userId, this.currentProdId , formData).subscribe({
      next:(value:any)=>{
        Swal.fire({
          icon: "success",
          title: "Your work has been saved",
          showConfirmButton: false,
          timer: 1500
        });
        this.getAllProducts();
        this.resetForm();
        
      }
    })
    }


  }
  onView(product:any){
    debugger
    this.isView = true;
    this.showForm = true;
    this.productDetails.disable()
    this.prodPic = product.productImage
    this.currentProdId = this.productDetails.get('id')?.value;
    this.productDetails.get('productName')?.setValue(product.productName);
    this.productDetails.get('productCode')?.setValue(product.productCode);
    this.productDetails.get('category')?.setValue(product.category);
    this.productDetails.get('brand')?.setValue(product.brand);
    this.productDetails.get('sellingPrice')?.setValue(product.sellingPrice);
    this.productDetails.get('purchasePrice')?.setValue(product.purchasePrice);
    this.productDetails.get('purchaseDate')?.setValue(formatDate(product.purchaseDate, "yyyy-MM-dd","en"));
    this.productDetails.get('stock')?.setValue(product.stock);
    this.productDetails.get('productName')?.setValue(product.productName);
    // this.productDetails.get('productImage')?.setValue(product.productImage as string);


    
    
  }
  onDelete(productId:number){
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.productService.deleteProduct(productId).subscribe({
          next:(value:any)=>{
            Swal.fire({
              icon: "success",
              title: "Product deleted successfully",
              showConfirmButton: false,
              timer: 1200
            });
            this.getAllProducts();
          }
        })
        Swal.fire({
          title: "Deleted!",
          text: "Your file has been deleted.",
          icon: "success"
        });
      }
    });
    
  }
  hideForm(){
    this.showForm = false
  }
  onFileSelected(event:any){
    this.selectedFile = event.target.files[0]
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

  isFieldInvalid(fieldName: string): boolean {
    const control = this.productDetails.get(fieldName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

  resetForm(){
    this.productDetails.reset();
    this.isView= false;
    this.showForm = false;
  }

}
