import { Price } from "./money";


export interface ProductModel {
	id:string;
	name: string;
	brand: string;
	img: string;
	quantity: number;
	price: Price;
	isFeatured: boolean;
    productDetailId:string;
	categoryIds: string[];
}