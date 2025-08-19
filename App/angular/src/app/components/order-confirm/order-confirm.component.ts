import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-order-confirm',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './order-confirm.component.html',
  styleUrl: './order-confirm.component.scss',
})
export class OrderConfirmComponent implements OnInit{
  ngOnInit(): void {
      // this.route.params.subscribe(params) => {

      // }
  }
}
