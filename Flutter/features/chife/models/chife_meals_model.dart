import 'package:taambeit/features/meal/models/meal.dart';

class ChifeMealsModel {
  String? chiefID;
  String? chiefName;
  String? prfileImage;
  String? coverImage;
  bool? isOnline;
  String? description;
  int? reviewCount;
  String? rating;
  int? ordersDone;
  List<MealModel>? meals;

  ChifeMealsModel(
      {this.chiefID,
      this.chiefName,
      this.prfileImage,
      this.coverImage,
      this.isOnline,
      this.description,
      this.reviewCount,
      this.rating,
      this.ordersDone,
      this.meals});

  ChifeMealsModel.fromJson(Map<String, dynamic> json) {
    chiefID = json['chiefID'];
    chiefName = json['chiefName'];
    prfileImage = json['prfileImage'];
    coverImage = json['coverImage'];
    isOnline = json['isOnline'];
    description = json['description'];
    reviewCount = json['reviewCount'];
    rating = json['rating'].toStringAsFixed(1);
    ordersDone = json['ordersDone'];
    if (json['meals'] != null) {
      meals = <MealModel>[];
      json['meals'].forEach((v) {
        meals!.add( MealModel.fromJson(v));
      });
    }
  }
}
