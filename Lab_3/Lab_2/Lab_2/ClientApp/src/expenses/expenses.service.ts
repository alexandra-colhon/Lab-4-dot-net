import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Expenses } from './expenses.model';

@Injectable({
  providedIn: 'root'
})
export class ExpensesService {

  private apiUrl: string;

  constructor(private httpClient: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.apiUrl = apiUrl;
  }

  getExpenses(): Observable<Expenses[]>
  {
    return this.httpClient.get<Expenses[]>(this.apiUrl + 'expenses');
  }
}
