
import 'package:taambeit/features/meal/models/meal.dart';

import 'package:taambeit/features/meal/services/meal_api.dart';

class MealViewModel {
  List<MealModel> meals = [];

   Future<List<MealModel>> fetchMeals ({
    int? tagFilter,
    int? sizeFilter,
    int? sortBy,
    int? mealSpiceLevel,
    int? mealCategory,
    int? mealStyle,
    String? chiefFilter,
    num? startPrice,
    num? endPrice,
    int? pageSize,
    int? pageNumber,
  }) async {
    return meals = await MealApi().getAllMeal(
      tagFilter: tagFilter,
      sizeFilter: sizeFilter,
      sortBy: sortBy,
      mealSpiceLevel: mealSpiceLevel,
      mealCategory: mealCategory,
      chiefFilter: chiefFilter,
      endPrice: endPrice,
      mealStyle: mealStyle,
      pageNumber: pageNumber,
      pageSize: pageSize,
      startPrice: startPrice,
    );
  }
}
