import { Price } from "./money";
export interface ProductDiscount {
	id: string;
	productId: string;
	discountedPrice: number;
	discountPercentage: number;
	startDate: Date;
	endDate: Date;
  }

export interface OrderDetailModel {
	productId: string;
	name: string;
	brand: string;
	quantity: number;
	price: Price;
	productDiscount?: ProductDiscount | null; 
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

