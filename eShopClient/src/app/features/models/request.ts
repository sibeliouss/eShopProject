export class RequestModel {
    categoryId: string | null = null; // Başlangıç değeri olarak null kullanmak daha anlamlı olabilir
    pageNumber: number = 1;
    pageSize: number = 10;
    search: string = '';
    orderBy: string = "default";
  }
  