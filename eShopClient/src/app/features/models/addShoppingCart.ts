import { Price } from "./money";

export class AddShoppingCartModel{
    productId: string = "";
    price!: Price;
    quantity: number = 0;
    userId: string = "";
}