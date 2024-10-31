export class RequestModel{
    categoryId: string | null = null;
    pageNumber: number = 1;
    pageSize: number = 10;
    search: string = "";
    orderBy: string = "default";
}