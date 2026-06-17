import { Component } from '@angular/core';
import { OrderService } from '../../services/order.sesrvice';
import { Order } from '../../models/order.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css']
})
export class OrderDetailComponent {

  order!: Order;
  totalAmount = 0;
  constructor(private orderService: OrderService,
    private route:ActivatedRoute
  )
  {

  }
  ngOnInit(){
    this.getOrderDetail();
  }
  getOrderDetail(){
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.orderService.getOrderDetailById(id).subscribe(res=>{
      this.order = res.data;
      this.totalAmount = this.order.orderItems.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0
      );
    })
  }
}
