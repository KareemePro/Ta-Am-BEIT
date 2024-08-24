import 'package:dio/dio.dart';
import 'package:taambeit/core/cache/cache_helper.dart';

class ApiInterceptor extends Interceptor{
  @override
  void onRequest(RequestOptions options, RequestInterceptorHandler handler) {
    options.headers['token'] =CacheData().getData(key: 'jwt') != null ? "Bearer ${CacheData().getData(key: 'jwt')}":null;
    super.onRequest(options, handler);
  }
}