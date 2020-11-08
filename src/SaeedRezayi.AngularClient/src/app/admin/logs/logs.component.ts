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

  ngOnInit(): void {
    this.getLogs();
  }


  ngOnDestroy() {
    this.destroy$.next(true);
    // unsubscribe
    this.destroy$.unsubscribe();
    console.log("logs component destroyed");
  }

  getLogs() {
    this.logService.getPagedLogsList()
      .toPromise().then(item => {
        this.pagedLogs = item;
      });
  }

}