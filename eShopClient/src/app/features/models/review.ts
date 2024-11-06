import { UserModel } from "../../core/models/user";
import { ProductModel } from "./product";

export interface ReviewModel {
	id:string;
	productId: string;
	product:ProductModel;
	userId: string;
	user: UserModel;
	rating: number;
	title: string;
	comment: string;
	createAt: string;
}