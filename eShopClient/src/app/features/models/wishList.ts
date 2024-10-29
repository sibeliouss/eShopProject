import { Price } from "./money";

export interface WishListModel {
	productId: string;
	userId: string;
	price: Price;
}