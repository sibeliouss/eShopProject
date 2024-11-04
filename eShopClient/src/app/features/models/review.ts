import { UserModel } from "../../core/models/user";
import { ProductModel } from "./product";

export class ReviewModel {
	id:string= "";
	productId: string= "";
	product:ProductModel= new ProductModel();
	userId: string= "";
	user: UserModel = new UserModel();
	rating: number= 0;
	title: string= "";
	comment: string= "";
	createAt: string= "";
}