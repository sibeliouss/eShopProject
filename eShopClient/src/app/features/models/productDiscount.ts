export interface ProductDiscountModel {
	id: string;
	productId: string;
	productName: string;
	productBrand: string;
	productImg: string;
	productPrice: number;
	discountPercentage: number;
	startDate: string;
	endDate: string;
	discountedPrice: number;
}