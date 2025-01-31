import { AddressModel } from "./address";
import { ProductModel } from "./product";

export interface Price {
	value: number;
	currency: string;
}

export class BuyerModel {
    id: string = "";
    name: string = "Sibel";
    surname: string = "Öztürk";
    identityNumber: string = "11111111111";
    email: string = "sibelmozturk@hotmail.com";
    gsmNumber: string = "05555000000";
    registrationDate: string = "2013-04-21 15:12:09";
    lastLoginDate: string = "2015-10-05 12:43:35";
    registrationAddress: string = "test";
    city: string = "test";
    country: string = "test";
    zipCode: string = "test";
    ip: string = "test";
}


export class PaymentCardModel {
    cardHolderName: string = "Sibel Öztürk ";
    cardNumber: string = "5400360000000003";
    expireYear: string = "2032";
    expireMonth: string = "11";
    cvc: string = "123";
}


export class PaymentModel {
	userId: string="";
	products: ProductModel[]=[];
	buyer: BuyerModel = new BuyerModel;
    paymentCard: PaymentCardModel = new PaymentCardModel;
	address!: AddressModel;
	billingAddress!: AddressModel;
	shippingAndCartTotal: number=0;
	currency: string="";
}