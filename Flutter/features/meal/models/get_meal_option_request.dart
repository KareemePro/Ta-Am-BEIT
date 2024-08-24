import 'package:taambeit/features/meal/models/get_meal_side_dishes_request.dart';

class GetMealOptionsRequest {
  String? mealOptionID;
  int? mealSizeOption;
  bool? isAvailable;
  bool? saveQuantity;
  int? quantity;
  int? price;
  String thumbnailImage = '';
  String? fullScreenImage;
  List<GetMealSideDishesRequest>? getMealSideDishesRequest;
  //List<UsedIngredients>? usedIngredients;

  GetMealOptionsRequest({
    this.mealOptionID,
    this.mealSizeOption,
    this.isAvailable,
    this.saveQuantity,
    this.quantity,
    this.price,
    required this.thumbnailImage,
    required this.fullScreenImage,
    //  this.getMealSideDishesRequest,
    //this.usedIngredients
  });

  GetMealOptionsRequest.fromJson(Map<String, dynamic> json) {
    mealOptionID = json['mealOptionID'];
    mealSizeOption = json['mealSizeOption'];
    isAvailable = json['isAvailable'];
    saveQuantity = json['saveQuantity'];
    quantity = json['quantity'];
    price = json['price'];
    thumbnailImage = json['thumbnailImage'];
    fullScreenImage = json['fullScreenImage'];
    if (json['getMealSideDishesRequest'] != null) {
      getMealSideDishesRequest = <GetMealSideDishesRequest>[];
      json['getMealSideDishesRequest'].forEach((v) {
        getMealSideDishesRequest!.add(GetMealSideDishesRequest.fromJson(v));
      });
      //}
      //if (json['usedIngredients'] != null) {
      //  usedIngredients = <UsedIngredients>[];
      //  json['usedIngredients'].forEach((v) {
      //  usedIngredients!.add(new UsedIngredients.fromJson(v));
      //});
    }
  }
}
