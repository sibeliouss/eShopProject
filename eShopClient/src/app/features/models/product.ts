import { Price } from "./money";
import { ProductDetailModel } from "./productDetail";


export interface ProductModel {
	id:string;
	name: string;
	brand: string;
	img: string;
	quantity: number;
	productDetail: ProductDetailModel;
	price: Price;
	isFeatured: boolean;
    productDetailId:string;
	categoryIds: string[];
	createAt: Date;
}