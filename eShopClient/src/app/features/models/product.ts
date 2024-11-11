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
	isActive: boolean 
    isDeleted: boolean 
	createAt: Date;
	productCategories: ProductCategory[];
	cartId:string;
    wishListId: string;
	isFavorite?: boolean;

}



export interface ProductCategory {
	categoryId: string;
	categoryName: string;
} 


