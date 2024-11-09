export interface ResponseDto<T> {
    data: T;
    totalPageCount: number;
    pageNumber: number;
    pageSize: number;
    isFirstPage: boolean;
    isLastPage: boolean;
    orderBy: string;
  }