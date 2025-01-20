import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '../../Services/Loader/loader.service';
import { finalize } from 'rxjs/operators';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);

  loaderService.setLoading(true, req.url);

  return next(req).pipe(
    finalize(() => {
      loaderService.setLoading(false, req.url);
    })
  );
};
