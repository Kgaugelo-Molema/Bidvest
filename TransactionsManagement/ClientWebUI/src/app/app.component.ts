import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'GPT - Transactions';
  @ViewChild('agGrid', { static: true }) agGrid: AgGridAngular;

  columnDefs = [
    { field: 'id', sortable: true, filter: true },
    { field: 'description', sortable: true, filter: true, editable: true },
    { field: 'amount', sortable: true, filter: true, editable: true, type: ['numberColumn'], headerName: 'Amount (ZAR)' },
  ];

  rowData = [];
  total = 0;
  messages = '';
  errors = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.getTransactions();
  }

  getTransactions() {
    const self = this;
    this.rowData = [];
    const service = this.http.get(`${this.baseUrl}api/transaction`);
    service.subscribe(data => {
      self.rowData = data as any[];
      self.calcTotal();
    }, error => console.error(error));
  }

  calcTotal() {
    this.rowData.forEach(record => {
      this.total += parseFloat(record.amount);
    })
  }

  sizeToFit() {
    this.agGrid.api.sizeColumnsToFit();
  }

  autoSizeAll(skipHeader) {
    const cols = this.agGrid.columnApi.getAllColumns().forEach(column => {
      this.agGrid.columnApi.autoSizeColumn(column.getColId());
      if (column.getColId() == 'status')
        column.setActualWidth(200);
    });
  }

  addTransaction() {
    const newTransaction = { id: 0, description: 'New Transaction', amount: 0 };
    this.agGrid.api.insertItemsAtIndex(0, [newTransaction]);
    this.rowData.push(newTransaction);
  }

  saveTransactions() {
    const self = this;
    this.messages = '';
    this.errors = '';
    const service = this.http.post(`${this.baseUrl}api/transaction/save`, this.rowData);
    service.subscribe(data => {
      const response = data as any[];
      response.forEach(m => {
        if (m != null) {
          self.messages += `\n${m.message}`;
          console.log(m.message);
        }
      });
      self.getTransactions();
    }, error => {
      self.errors += `\n${error.message}`;
      console.error(error.message);
    });
  }
}
