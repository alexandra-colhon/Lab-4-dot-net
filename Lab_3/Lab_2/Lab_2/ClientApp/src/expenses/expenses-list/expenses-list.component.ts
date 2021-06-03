import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Expenses } from '../expenses.model';
import { ExpensesService } from '../expenses.service';

@Component({
  selector: 'app-expenses-list',
  templateUrl: './expenses-list.component.html',
  styleUrls: ['./expenses-list.component.css']
})

export class ExpensesListComponent {

  public expenses: Expenses[];

  constructor(private expensesService: ExpensesService) {

  }

  getExpenses() {
    this.expensesService.getExpenses().subscribe(e => this.expenses = e);
  }

  ngOnInit() {
    this.getExpenses();
  }

}
