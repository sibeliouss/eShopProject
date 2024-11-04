import { UserModel } from "../../core/models/user";

export class CreateReviewModel{
    productId: string="";
    userId: string="";
    user: UserModel=new UserModel;
    rating: number=0;
    title: string="" ;
    comment: string="";
}