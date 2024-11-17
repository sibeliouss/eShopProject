import { Price } from "./money";


export interface OrderDetailModel {
	productId: string;
	name: string;
	brand: string;
	quantity: number;
	price: Price;
}

export interface OrderModel {
	id: string;
	orderNumber: string;
	productQuantity: number;
	createAt: string;
	paymentMethod: string;
	status: string;
	paymentNumber: string;
	paymentCurrency: string;
	products: OrderDetailModel[];
}