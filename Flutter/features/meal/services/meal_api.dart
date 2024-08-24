
import 'package:dio/dio.dart';
import 'package:taambeit/core/api/api_consumer.dart';
import 'package:taambeit/core/api/dio_consumer.dart';

import 'package:taambeit/features/meal/models/meal.dart';

class MealApi {
  final ApiConsumer api = DioConsumer(dio: Dio());

    Future<List<MealModel>> getAllMeal({
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
    
      List<MealModel> mealsList = [];
      Map<String, dynamic> query = {};

      if (tagFilter != null) query['TagFilter'] = tagFilter;
      if (sizeFilter != null) query['SizeFilter'] = sizeFilter;
      if (sortBy != null) query['SortBy'] = sortBy;
      if (mealSpiceLevel != null) query['MealSpiceLevel'] = mealSpiceLevel;
      if (mealCategory != null) query['MealCategory'] = mealCategory;
      if (mealStyle != null) query['MealStyle'] = mealStyle;
      if (chiefFilter != null) query['ChiefFilter'] = chiefFilter;
      if (startPrice != null) query['StartPrice'] = startPrice;
      if (endPrice != null) query['EndPrice'] = endPrice;
      if (pageSize != null) query['PageSize'] = pageSize;
      if (pageNumber != null) query['PageNumber'] = pageNumber;

      List<dynamic> meals = await api.get('Meals', queryParameters: query);
      for (var meal in meals) {
        MealModel mealModel = MealModel.fromJson(meal);
        mealsList.add(mealModel);
      }
      return mealsList;
  
  }
}
