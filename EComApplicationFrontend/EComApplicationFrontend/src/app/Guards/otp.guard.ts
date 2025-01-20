import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const otpGuard: CanActivateFn = (route, state) => {

  const verifyOtp = sessionStorage.getItem("verifyOtp");
  const token = sessionStorage.getItem("token")
  const router = inject(Router);

  if(verifyOtp && token){
    return true
  }
  // else if()
  else{
    router.navigateByUrl("/verify")
    return false
  }
};
