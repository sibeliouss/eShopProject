import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  private baseUrl: string = 'https://localhost:7120/api/Reviews';

  getProductDetailUrl(productId: string): string {
    return `https://localhost:7120/api/Products/GetProductDetailById/${productId}`;
  }

  getReviewsUrl(productId: string): string {
    return `${this.baseUrl}/GetReviews/${productId}`;
  }

  createReviewUrl(): string {
    return `${this.baseUrl}/CreateReview`;
  }

  allowToCommentUrl(productId: string, userId: string): string {
    return `${this.baseUrl}/AllowToComment/${productId}/${userId}`;
  }

  calculateStarUrl(productId: string): string {
    return `${this.baseUrl}/CalculateStar/${productId}`;
  }

  calculateReviewsUrl(productId: string): string {
    return `${this.baseUrl}/CalculateReviews/${productId}`;
  }
}
