import 'package:dio/dio.dart';
import 'package:taambeit/core/errors/error_model.dart';


class ServerException implements Exception {
  final ErrorModel errorModel;

  ServerException({required this.errorModel});
}

void handelDioException(DioException exception) {
  switch (exception.type) {
    case DioExceptionType.connectionTimeout:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.sendTimeout:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.receiveTimeout:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.badCertificate:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.cancel:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.connectionError:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.unknown:
      throw ServerException(
          errorModel: ErrorModel(errorMesage: exception.response!.data));
    case DioExceptionType.badResponse:
      switch (exception.response?.statusCode) {
        case 400: //Bad request
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
        case 401:
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
        case 403:
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
        case 404:
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
        case 409:
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
        case 504:
          throw ServerException(
              errorModel: ErrorModel(errorMesage: exception.response!.data));
      }
  }
}
