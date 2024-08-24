class GetMealReviewsRequest {
  String? customerName;
  String? customerImage;
  String? reviewText;
  int? rating;
  String? reviewDate;

  GetMealReviewsRequest(
      {this.customerName,
      this.customerImage,
      this.reviewText,
      this.rating,
      this.reviewDate});

  GetMealReviewsRequest.fromJson(Map<String, dynamic> json) {
    customerName = json['customerName'];
    customerImage = json['customerImage'];
    reviewText = json['reviewText'];
    rating = json['rating'];
    reviewDate = json['reviewDate'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['customerName'] = customerName;
    data['customerImage'] = customerImage;
    data['reviewText'] = reviewText;
    data['rating'] = rating;
    data['reviewDate'] = reviewDate;
    return data;
  }
}
