import { Component, OnInit } from '@angular/core';
;

import { OrderService } from '../../services/order.sesrvice';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html'
})
export class OrderListComponent implements OnInit {

  orders: Order[] = [];

  constructor(
    private orderService: OrderService
  ) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {

    this.orderService.getOrders().subscribe({
      next: (response) => {
        this.orders = response.data;
      },
      error: (error) => {
        console.error(error);
      }

    });

  }

  getTotalItems(order: any): number {

    if (!order.orderItems) {
      return 0;
    }
    return order.orderItems.reduce(
      (sum: number, item: any) => sum + item.quantity,
      0
    );
  }

}