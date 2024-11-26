import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ProductModel } from '../../models/product';
import { ReviewModel } from '../../models/review';
import { CreateReviewModel } from '../../models/createReview';
import { Star } from '../../models/star';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { SwalService } from '../../../core/services/swal.service';
import { WishListService } from '../../services/wish-list.service';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { BaseInputErrorsComponent } from '../../../core/components/base-input-errors/base-input-errors.component';
import { ReviewService } from '../../services/review.service';


@Component({
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, BaseInputErrorsComponent],
  templateUrl: './singleproduct.component.html',
  styleUrls: ['./singleproduct.component.scss']
})
export class SingleproductComponent {

  product: ProductModel | null= null;
  products: ProductModel[]=[];
  reviews: ReviewModel[] = [];
  requestCreateReview: CreateReviewModel =new CreateReviewModel;
  comment: string = "";
  title: string = "";
  isResponse: boolean | undefined = undefined;
  allowToComment: boolean = false;
  starRating: number = 0;
  rating: number = 0;
  starData: Star = new Star;
  

  constructor(
    private http: HttpClient,
    private activated: ActivatedRoute,
    public auth: AuthService,
    private swal: SwalService,
    private translate: TranslateService,
    public wishList: WishListService,
    public shopping: ShoppingCartService,
    private reviewService: ReviewService,
    
  ) {
    if (localStorage.getItem('response')) {
      this.isResponse = true;
      
    }}

    ngOnInit(): void {
      this.activated.params.subscribe(res => {
        this.http.get<ProductModel[]>('https://localhost:7120/api/Products/GetProductDetailById/' + res["value"]).subscribe({
          next: (res: any) => {
            this.product = res;
            this.getAllReview();
            this.AllowToComment();
            this.calculateReviews();
            this.calculateStar();
          },
          error: (err) => {
            console.error('Product fetch error:', err);
          
          }
        });
      });
      
    }

  setRating(rating: number) {
    this.starRating = rating;
  }

  createReview() {
     {
      
      this.requestCreateReview.userId = this.auth.token?.userId ?? ''; 
      this.requestCreateReview.productId = this.product?.id ?? '';
      this.requestCreateReview.rating = this.starRating;
      this.requestCreateReview.title = this.title;
      this.requestCreateReview.comment = this.comment;
  
      this.http.post(this.reviewService.createReviewUrl(), this.requestCreateReview).subscribe({
        next: (res: any) => {
          this.getAllReview();
          this.clearReviews();
          this.calculateReviews();
          this.calculateStar();
          this.translate.get("commentSuccessfullySaved").subscribe(res => {
            this.swal.callToast(res, 'success');
          });
        },
        error: (err) => {
          console.error('Review creation error:', err);
        }
      });
     
      console.error('requestCreateReview, auth token veya product null durumda');
    }
  }
  

  
  AllowToComment() {
    if (this.product && this.auth.token) {
      this.http.get<boolean>(this.reviewService.allowToCommentUrl(this.product.id, this.auth.token.userId!)).subscribe({
        next: (res) => {
          this.allowToComment = res;
        },
        error: (err) => {
          console.error('Allow to comment error:', err);
        }
      });
    }
  }

  getAllReview() {
    this.http.get(this.reviewService.getReviewsUrl(this.product?.id!)).subscribe({
      next: (res: any) => {
        this.reviews = res;
      
      },
      error: (err) => {
        console.error('All reviews fetch error:', err);
        
      }
    });
  }

  calculateStar() {
    this.http.get(this.reviewService.calculateStarUrl(this.product?.id!)).subscribe({
      next: (res: any) => {
        this.starData = res;
        console.log(this.starData);
      },
      error: (err) => {
        console.error('Calculate star error:', err);
        
      }
    });
  }

  calculateReviews() {
    this.http.get(this.reviewService.calculateReviewsUrl(this.product!.id)).subscribe({
      next: (res: any) => {
        this.rating = res;
      },
      error: (err) => {
        console.error('Calculate reviews error:', err);
        
      }
    });
  }

  clearReviews() {
    this.title = "";
    this.comment = "";
    this.starRating = 0;
  }
  getStars(rating: number) {
    const stars = [];
    for (let i = 0; i < 5; i++) {
      if (rating > i) {
        if (rating > i + 0.5) {
          stars.push({ class: 'fas fa-star' }); // Tam yıldız
        } else {
          stars.push({ class: 'fa-solid fa-star-half-stroke' }); // Yarım yıldız
        }
      } else {
        stars.push({ class: 'far fa-star' }); // Boş yıldız
      }
    }
    return stars;
  }
  

  trackReview(index: number, review: ReviewModel): string {
    return review.id; // ya da review'in bir benzersiz özelliği
  }

  
   addToCart(product: ProductModel): void {
    if (product) {
        this.shopping.addShoppingCart(product);
    }
}
  
}
