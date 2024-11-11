import { AddressModel } from "./address";
import { ProductModel } from "./product";

export interface Price {
	value: number;
	currency: string;
}

export interface Buyer {
	id: string;
	name: string;
	surname: string;
	identityNumber: string;
	email: string;
	gsmNumber: string;
	registrationDate: string;
	lastLoginDate: string;
	registrationAddress: string;
	city: string;
	country: string;
	zipCode: string;
	ip: string;
}



export interface PaymentCard {
	cardHolderName: string;
	cardNumber: string;
	expireYear: string;
	expireMonth: string;
	cvc: string;

}

export interface PaymentModel {
	userId: string;
	products: ProductModel[];
	buyer: Buyer;
	address: AddressModel;
	billingAddress: AddressModel;
	paymentCard: PaymentCard;
	shippingAndBasketTotal: number;
	currency: string;
}