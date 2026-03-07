export class SearchFilters {
  searchTerm?: string = '';
  dateSearch?: Date;
  pageNumber: number = 1;      
  pageSize: number = 5;       
  sortBy: string = 'createdDate'; 
  orderBy: 'asc' | 'desc' = 'desc';
}
