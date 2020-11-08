import { Paging } from '../shared/Paging';
import { LogViewModel } from './LogViewModel';

export interface IPagedLogsListViewModel {
  Logs: LogViewModel[],
  Paging: Paging
}
