import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { RegisterService } from '../../Services/Register/register.service';
import { CartService } from '../../Services/Cart/cart.service';
import { SharedServiceService } from '../../Services/SharedService/shared-service.service';
import { share } from 'rxjs';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, CommonModule,RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {

  constructor(private router : Router, private registerService : RegisterService, private cartService : CartService, private sharedService : SharedServiceService){}

  userProfile:any;
  userRole:string | null = '' ;
  cartItems:any;
  itemsCount:number=0;
  image : any;


  ngOnInit():void{
    debugger
    this.userRole = sessionStorage.getItem("role");
    this.getProfile();
    this.getAllItems();
    
    

    this.sharedService.itemsCount$.subscribe((count)=>{
      this.itemsCount = count
  });

  }

  isAdmin():boolean {
    return this.userRole === 'Admin';
  }

  logout(){
    sessionStorage.clear();
    this.router.navigateByUrl('/login');
  }

  getProfile():void{
    this.registerService.getUserByEmail(sessionStorage.getItem("email")?? '').subscribe({
      next: (profile: any) => {

        this.userProfile = profile;
        console.log(this.userProfile)
        console.log(this.userProfile.profileImage)
        this.sharedService.setProfPicture('https://localhost:7040/' + this.userProfile.profileImage)
        this.sharedService.showProfilePicture$.subscribe((profPic)=>{
          this.image = profPic;
        })

      },
      error: (error: any) => {
        console.error('Error fetching profile:', error);
      },
    });
  }

  
  getAllItems() {

    const cartId = parseInt(sessionStorage.getItem("cartId") || "0", 10);
    this.cartService.getAllProducts(cartId).subscribe({
      next: (value: any) => {


      }
    });
  }
  
  

}
 