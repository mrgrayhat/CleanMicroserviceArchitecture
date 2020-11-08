import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { IPagedLogsListViewModel } from '@app/core/models/admin/IPagedLogsListViewModel';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { APP_CONFIG, IAppConfig } from '../api/app.config';


@Injectable({
  providedIn: 'root'
})
export class LogModuleService {

  constructor(
    private injector: Injector,
    @Inject(APP_CONFIG) private appConfig: IAppConfig,
  ) { }

  // logsResponse: IPagedResponse<ILogViewModel> | undefined;

  getPagedLogsList(): Observable<IPagedLogsListViewModel> {
    const url = `${this.appConfig.apiEndpoint}/Logs`;
    const http = this.injector.get<HttpClient>(HttpClient);
    return http.get<IPagedLogsListViewModel>(url)
      .pipe(tap(c => {
        return c;
      }));
  }


}