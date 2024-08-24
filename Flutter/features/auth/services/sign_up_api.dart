import 'package:dartz/dartz.dart';
import 'package:dio/dio.dart';
import 'package:taambeit/core/api/api_consumer.dart';
import 'package:taambeit/core/api/dio_consumer.dart';
import 'package:taambeit/core/api/end_point.dart';
import 'package:taambeit/core/errors/exceptions.dart';

class CustomerSignUpApi {
  final ApiConsumer api = DioConsumer(dio: Dio());

  Future<Either<List<dynamic>?, dynamic>> signUp(
      {required String firstName,
      required String lastName,
      required String email,
      required String password}) async {
    try {
      final response = await api.post(EndPoint.customerSignUp, data: {
        "email": email,
        "firstName": firstName,
        "lastName": lastName,
        "password": password
      });
      return right(response);
    } on ServerException catch (e) {
      return left(e.errorModel.errorMesage);
    }
  }
}
