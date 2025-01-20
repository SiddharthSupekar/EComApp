import { Routes } from '@angular/router';
import { RegisterComponent } from './Components/register/register.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { LoginComponent } from './Components/login/login.component';
import { VerifyOtpComponent } from './Components/verify-otp/verify-otp.component';
import { ForgotPasswordComponent } from './Components/forgot-password/forgot-password.component';
import { LayoutComponent } from './Components/layout/layout.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { UserMarketplaceComponent } from './Components/user-marketplace/user-marketplace.component';
import { AddProductsComponent } from './Components/add-products/add-products.component';
import { AdminDashboardComponent } from './Components/admin-dashboard/admin-dashboard.component';
import { loginGuard } from './Guards/login.guard';
import { otpGuard } from './Guards/otp.guard';
import { CartComponent } from './Components/cart/cart.component';
import { InvoiceComponent } from './Components/invoice/invoice.component';

export const routes: Routes = [
    {
        path:'',
        component:LoginComponent
    },
    {
        path:"register",
        component:RegisterComponent
    },
    {
        path:'login',
        component:LoginComponent,
        
    },
    {
        path:'verify',
        component:VerifyOtpComponent,
        canActivate:[loginGuard]
    },
    
    {
        path:'layout',
        component:LayoutComponent,
        canActivate:[loginGuard,otpGuard],
    
        children:[
            {
                path:'profile',
                component:ProfileComponent
            },
            {
                path:'change-password',
                component:ChangePasswordComponent
            },
            {
                path:'customer-marketplace',
                component:UserMarketplaceComponent
            },
            {
                path:'add-product',
                component:AddProductsComponent
            },
            {
                path:'forgot-password',
                component:ForgotPasswordComponent
            },
            {
                path:'admin-dashboard',
                component:AdminDashboardComponent
            },
            {
                path:'cart',
                component:CartComponent,
            },
            {
                path:'invoice',
                component:InvoiceComponent
            }
            
        
        ]
    },

    {
        path:'forgot-password',
        component:ForgotPasswordComponent
    },
    
    // {
    //     path:'change-password',
    //     component:ChangePasswordComponent
    // },
    // {
    //     path:'customer-marketplace',
    //     component:UserMarketplaceComponent
    // },
    // {
    //     path:'add-product',
    //     component:AddProductsComponent
    // },
    // {
    //     path:'forgot-password',
    //     component:ForgotPasswordComponent
    // },
    // {
    //     path:'admin-dashboard',
    //     component:AdminDashboardComponent
    // }

];
