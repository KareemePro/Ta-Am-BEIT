import 'package:dartz/dartz.dart';
import 'package:dio/dio.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:taambeit/core/api/api_consumer.dart';
import 'package:taambeit/core/api/dio_consumer.dart';
import 'package:taambeit/core/cache/cache_helper.dart';

import 'package:taambeit/core/errors/exceptions.dart';
import 'package:taambeit/features/auth/models/user_token.dart';

class LoginApi {
  ApiConsumer api = DioConsumer(dio: Dio());

  Future<Either<List<dynamic>?, UserToken>> logIn(
      {required String email, required String password}) async {
    try {
      final respons = await api.post(
        'Auth/Login',
        data: {
          'email': email,
          'password': password,
        },
      );
      UserToken user = UserToken.fromJson(respons);
      final decodedToken = JwtDecoder.decode(user.jwt);
      CacheData().setData(key: 'jwt', value: user.jwt);
      CacheData().setData(key: 'id', value: decodedToken['uid']);

      return Right(user);
    } on ServerException catch (e) {
      return left(e.errorModel.errorMesage);
    }
  }
}
