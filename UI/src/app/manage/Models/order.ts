export class Order {
    id: number;
    userId: string;
    orderDate: string;
    totalAmount: number;
    status: string;

    constructor(id: number, userId: string, orderDate: string, totalAmount: number, status: string) {
        this.id = id;
        this.userId = userId;
        this.orderDate = orderDate;
        this.totalAmount = totalAmount;
        this.status = status;
    }
}

