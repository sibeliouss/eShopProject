export interface Price {
	value: number;
	currency: string;
}

export interface ProductModel {
	name: string;
	brand: string;
	img: string;
	quantity: number;
	price: Price;
	isFeatured: boolean;
    productDetailId:string;
	categoryIds: string[];
}