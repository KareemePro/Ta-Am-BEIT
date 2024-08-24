import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:taambeit/features/auth/cubit/customer_sign_up_state.dart';
import 'package:taambeit/features/auth/services/sign_up_api.dart';

class CustomerSignUpCubit extends Cubit<CustomerSignUpState> {
  CustomerSignUpCubit(this.signUpApi) : super(CustomerSignUpInitial());
  final CustomerSignUpApi signUpApi;
  late String firstName;
  late String lastName;
  late String email;
  late String password;

  signUp() async {
    emit(SignUpLoading());
    final response = await signUpApi.signUp(
        firstName: firstName,
        lastName: lastName,
        email: email,
        password: password);

    response.fold(
      (errorMessage) => emit(SignUpFailure(errorMessage: errorMessage)),
      (success) => emit(SignUpSuccess()),
    );
  }
}
