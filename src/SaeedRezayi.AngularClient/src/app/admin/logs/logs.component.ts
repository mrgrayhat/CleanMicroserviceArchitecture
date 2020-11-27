import { Component, OnDestroy, OnInit } from '@angular/core';
import { IPagedLogsListViewModel, LogModuleService } from '@app/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css']
})
export class LogsComponent implements OnInit, OnDestroy {

  constructor(private logService: LogModuleService) {
  }

  destroy$: Subject<boolean> = new Subject<boolean>();
  pagedLogs: IPagedLogsListViewModel;
  logLevel: number = 1;

  ngOnInit(): void {
    this.getLogs();
  }


  ngOnDestroy() {
    this.destroy$.next(true);
    // unsubscribe
    this.destroy$.unsubscribe();
    console.log('logs component destroyed');
  }

  getLogs(page: number = 1, maxRecords: number = 10, loglevel: number = this.logLevel) {
    this.logService.getPagedLogsList(Math.round(page), maxRecords, loglevel)
      .toPromise().then(item => {
        this.pagedLogs = item;
      });
  }

}