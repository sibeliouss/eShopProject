import { Price } from "./money";

export interface ProductDiscountModel {
	id: string;
	productId: string;
	name: string;
	brand: string;
	img: string;
	price: Price;
	discountPercentage: number;
	startDate: string;
	endDate: string;
	discountedPrice: number;
	quantity:number
}

