import 'package:taambeit/features/meal/models/get_meal_option_request.dart';
import 'package:taambeit/features/meal/models/get_meal_reviews_request.dart';


class MealModel {
  String? mealID;
  String chiefID ='';
  String? chiefName;
  String? chiefImage;
  String? createdDate;
  int? mealSpiceLevel;
  int? mealCategory;
  int? mealStyle;
  String? title;
  String? description;
  String? rating;
  int? reviewCount;
  List<int>? mealTags;
  List<GetMealOptionsRequest>? getMealOptionsRequest;
  List<GetMealReviewsRequest>? getMealReviewsRequest;

  MealModel(
      {this.mealID,
      required this.chiefID,
      this.chiefName,
      this.chiefImage,
      this.createdDate,
      this.mealSpiceLevel,
      this.mealCategory,
      this.mealStyle,
      this.title,
      this.description,
      this.rating,
      this.reviewCount,
      this.mealTags,
      this.getMealOptionsRequest,
      this.getMealReviewsRequest});

  MealModel.fromJson(Map<String, dynamic> json) {
    mealID = json['mealID'];
    chiefID = json['chiefID'];
    chiefName = json['chiefName'];
    chiefImage = json['chiefImage'];
    createdDate = json['createdDate'];
    mealSpiceLevel = json['mealSpiceLevel'];
    mealCategory = json['mealCategory'];
    mealStyle = json['mealStyle'];
    title = json['title'];
    description = json['description'];
    rating = json['rating'].toString();
    reviewCount = json['reviewCount'];
    mealTags = json['mealTags'].cast<int>();
    if (json['getMealOptionsRequest'] != null) {
      getMealOptionsRequest = <GetMealOptionsRequest>[];
      json['getMealOptionsRequest'].forEach((v) {
        getMealOptionsRequest!.add( GetMealOptionsRequest.fromJson(v));
      });
    }
    if (json['getMealReviewsRequest'] != null) {
      getMealReviewsRequest = <GetMealReviewsRequest>[];
      json['getMealReviewsRequest'].forEach((v) {
        getMealReviewsRequest!.add( GetMealReviewsRequest.fromJson(v));
      });
    }
  }

  
}

