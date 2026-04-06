import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';

import { Injectable } from '@angular/core';
import { Observable, throwError, switchMap, catchError } from 'rxjs';
import { TokenService } from '../services/hiremind/token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private tokenService: TokenService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const token = this.tokenService.getAccessToken();

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req).pipe(

      catchError((error: HttpErrorResponse) => {

        if (error.status === 401) {

          return this.tokenService.refreshToken().pipe(

            switchMap((res: any) => {

              const newAccessToken = res.accessToken;
              const newRefreshToken = res.refreshToken;

              this.tokenService.saveTokens(newAccessToken, newRefreshToken);

              const clonedRequest = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${newAccessToken}`
                }
              });

              return next.handle(clonedRequest);
            }),

            catchError(err => {

              this.tokenService.logout();

              return throwError(() => err);
            })

          );

        }

        return throwError(() => error);

      })
    );
  }
}
