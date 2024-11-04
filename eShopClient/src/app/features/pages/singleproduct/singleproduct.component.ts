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

@Component({
  selector: 'app-singleproduct',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule],
  templateUrl: './singleproduct.component.html',
  styleUrls: ['./singleproduct.component.scss']
})
export class SingleproductComponent {

  product: ProductModel=new ProductModel;
  reviews: ReviewModel[] = [];
  requestCreateReview: CreateReviewModel=new CreateReviewModel;
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
    public wishList: WishListService
  ) {
    if (localStorage.getItem('response')) {
      this.isResponse = true;
      
    }

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
          this.swal.callToast('Ürün bilgileri alınırken bir hata oluştu.', 'error');
        }
      });
    });
  }

  setRating(rating: number) {
    this.starRating = rating;
  }

  createReview() {
     {
      this.requestCreateReview.userId = this.auth.token?.userId || ''; 
      this.requestCreateReview.productId = this.product.id || '';
      this.requestCreateReview.rating = this.starRating;
      this.requestCreateReview.title = this.title;
      this.requestCreateReview.comment = this.comment;
  
      this.http.post("https://localhost:7120/api/Reviews/CreateReview", this.requestCreateReview).subscribe({
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
    this.http.get(`https://localhost:7120/api/Reviews/AllowToComment/${this.product?.id}/${this.auth.token?.userId}`).subscribe({
      next: (res: any) => {
        this.allowToComment = res;
      },
      error: (err) => {
        console.error('Allow to comment error:', err);
       
      }
    });
  }

  getAllReview() {
    this.http.get("https://localhost:7120/api/Reviews/GetReviews/" + this.product?.id).subscribe({
      next: (res: any) => {
        this.reviews = res;
      
      },
      error: (err) => {
        console.error('All reviews fetch error:', err);
        
      }
    });
  }

  calculateStar() {
    this.http.get("https://localhost:7120/api/Reviews/CalculateStar/" + this.product?.id).subscribe({
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
    this.http.get("https://localhost:7120/api/Reviews/CalculateReviews/" + this.product?.id).subscribe({
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
  

  trackByReviewId(index: number, review: ReviewModel): string {
    return review.id; // ya da review'in bir benzersiz özelliği
  }
  
}
