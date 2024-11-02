export interface ResponseDto<T> {
    Data: T;
    TotalPageCount: number;
    PageNumber: number;
    PageSize: number;
    IsFirstPage: boolean;
    IsLastPage: boolean;
    OrderBy: string;
  }