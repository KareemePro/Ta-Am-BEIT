import 'package:dio/dio.dart';
import 'package:taambeit/core/api/api_consumer.dart';
import 'package:taambeit/core/api/dio_consumer.dart';
import 'package:taambeit/features/chife/models/chife_meals_model.dart';

class ChifeMealsApi {
   ApiConsumer api = DioConsumer(dio: Dio());

  Future<ChifeMealsModel> getChifeMeals({required String id}) async {
    Map<String, dynamic> query = <String, dynamic>{
      'ChiefID': id,
    };
    var jsonData = await api.get('Chief/GetChiefMeals', queryParameters: query);
    ChifeMealsModel chifeMeals = ChifeMealsModel.fromJson(jsonData);
    return chifeMeals;
  }
}
