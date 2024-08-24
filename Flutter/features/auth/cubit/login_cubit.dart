import 'package:dio/dio.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:taambeit/core/api/api_consumer.dart';
import 'package:taambeit/core/api/dio_consumer.dart';
import 'package:taambeit/features/auth/cubit/login_state.dart';
import 'package:taambeit/features/auth/services/login_api.dart';


class LoginCubit extends Cubit<LoginState> {
  LoginCubit(this.loginApi) : super(LoginInitial());
  LoginApi loginApi;
  ApiConsumer api = DioConsumer(dio: Dio());
  late String email;
  late String password;

  logIn() async {
    emit(LoginLoading());
    final response = await loginApi.logIn(email: email, password: password);
    response.fold(
      (errorMessage) => emit(LoginFailure(errorMessage: errorMessage)),
      (token) => emit(LoginSuccess()),
    );
  }
}
