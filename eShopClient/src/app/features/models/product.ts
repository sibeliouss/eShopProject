import { Price } from "./money";
import { ProductDetailModel } from "./productDetail";


/* export interface ProductModel {
	id:string;
	name: string;
	brand: string;
	img: string;
	quantity: number;
	productDetail: ProductDetailModel;
	price: Price;
	isActive: boolean 
    isDeleted: boolean 
	createAt: Date;
	productCategories: ProductCategory[];
}



export interface ProductCategory {
	categoryId: string;
	categoryName: string;
} */

export class ProductModel {
	id:string='';
	name: string='';
	brand: string='';
	img: string='';
	quantity: number=0;
	productDetail!: ProductDetailModel;
	price!: Price;
	isActive: boolean  = true;
    isDeleted: boolean = false; 
	createAt!: Date;
	productCategories!: ProductCategory[];
}



export interface ProductCategory {
	categoryId: string;
	categoryName: string;
}

