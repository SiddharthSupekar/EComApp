import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { loginInterceptor } from './Interceptors/LoginInterceptor/login.interceptor';
import { loaderInterceptor } from './Interceptors/LoaderInterceptor/loader.interceptor';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }),
              provideRouter(routes),
              provideHttpClient(withInterceptors([loaderInterceptor, loginInterceptor])),
              provideAnimations(),
              provideToastr(),
            ]
};
