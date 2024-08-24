import 'package:taambeit/features/meal/models/get_meal_side_dish_options_request.dart';


class GetMealSideDishesRequest {
  String? mealSideDishID;
  bool? isFree;
  bool? isTopping;
  List<GetMealSideDishOptionsRequest>? getMealSideDishOptionsRequest;

  GetMealSideDishesRequest(
      {this.mealSideDishID,
      this.isFree,
      this.isTopping,
      this.getMealSideDishOptionsRequest});

  GetMealSideDishesRequest.fromJson(Map<String, dynamic> json) {
    mealSideDishID = json['mealSideDishID'];
    isFree = json['isFree'];
    isTopping = json['isTopping'];
    if (json['getMealSideDishOptionsRequest'] != null) {
      getMealSideDishOptionsRequest = <GetMealSideDishOptionsRequest>[];
      json['getMealSideDishOptionsRequest'].forEach((v) {
        getMealSideDishOptionsRequest!
            .add( GetMealSideDishOptionsRequest.fromJson(v));
      });
    }
  }
}
