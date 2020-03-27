import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { _throw as throwError } from 'rxjs/observable/throw';

@Injectable(
  // we declare that this service should be created
  // by the root application injector.
)
export class ErrorInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    return next.handle(req).pipe(
      catchError(
        error => {
          if (error instanceof HttpErrorResponse) {
            if (error.status === 401) {
              return throwError(error.message);
            }
            const applicationError = error.headers.get('Application-Error');
            if (applicationError) {
              console.error(applicationError);
              return throwError(applicationError);
            }
            const serverError = error.error;
            let modelStateErrors = '';
            if (serverError && typeof serverError === 'object') {
              for (const key in serverError) {
                if (serverError[key]) {
                  modelStateErrors += serverError[key] + '\n';
                }
              }
            }
            return throwError(modelStateErrors || serverError || 'Server Error');

          }
        }
      ));
    }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
}


