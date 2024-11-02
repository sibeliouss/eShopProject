import { UserModel } from "../../core/models/user";

export class CreateReviewModel{
    productId: string="";
    userId: string="";
    user!: UserModel;
    raiting: number = 0;
    title: string = "";
    comment: string = "";
}