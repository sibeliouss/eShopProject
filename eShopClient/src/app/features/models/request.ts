export class RequestModel {
  categoryId?: string | null = null; // Boş string yerine `null` kullan
  pageNumber: number = 1;
  pageSize: number = 10;
  search: string = '';
  orderBy: string = "default";
}
