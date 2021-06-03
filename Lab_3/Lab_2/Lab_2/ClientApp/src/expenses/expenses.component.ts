import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Expenses } from './expenses.model';

@Component({
  selector: 'app-expenses',
  templateUrl: './expenses.component.html',
  styleUrls: ['./expenses.component.css']
})
export class ExpensesComponent{

  public expenses: Expenses[];

  constructor(http: HttpClient, @Inject('API_URL') apiUrl: string) {
    http.get<Expenses[]>(apiUrl + 'expenses').subscribe(result => {
      this.expenses = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}
