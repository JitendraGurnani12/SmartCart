import { OrderItem } from './orderItem.model';

export interface Order {
  id: number;
  createdAt: Date;
  orderItems: OrderItem[];
}