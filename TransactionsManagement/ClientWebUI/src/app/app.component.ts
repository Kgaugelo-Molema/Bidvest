import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'acme';
  @ViewChild('agGrid', { static: true }) agGrid: AgGridAngular;

  columnDefs = [
    { field: 'id', sortable: true, filter: true },
    { field: 'description', sortable: true, filter: true },
    { field: 'amount', sortable: true, filter: true },
  ];

  rowData = [];
  total = 0;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    const self = this;
    console.log('OnInit()');
    let service = this.http.get(`${this.baseUrl}api/transaction`);
    service.subscribe(data => {
      self.rowData = data as any[];
    }, error => console.error(error));
  }

  getSelectedRows() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const selectedData = selectedNodes.map(node => node.data );
    let messages = 'Message Log\n';
    this.agGrid.api.forEachNode(node => {
      let status = '';
      const amount = Number(node.data.amount);
      if (amount == NaN || node.data.amount == undefined || node.data.amount == '') {
        node.setDataValue('status', 'Invalid Amount!');
      }
      else {
        const balance = node.data.balance - amount;
        if (node.data.account_type == 'savings' && balance < 0) {
          status = 'Declined';
          messages += `\nAccount Number ${node.data.account_number} is overdrawn!`
        }
        else if (node.data.account_type == 'cheque' && balance < -500) {
          status = 'Declined';
          messages += `\nAccount Number ${node.data.account_number} has reached the maximum over draft limit!`
        }
        else {
          status = 'Approved';
          node.setDataValue('balance', balance.toFixed(2));  
        }
        node.setDataValue('status', status);  
      }       
    });
    if (messages != 'Message Log\n')
      alert(messages); 
    this.getTotal();
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

  getTotal() {
    const self = this;
    this.total = 0;
    this.agGrid.api.forEachNode(node => {
      self.total += parseFloat(node.data.balance);
    });
  }

}
