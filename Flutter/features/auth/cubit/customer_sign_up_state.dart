class CustomerSignUpState {}

final class CustomerSignUpInitial extends CustomerSignUpState {}
final class SignUpSuccess extends CustomerSignUpState {}

final class SignUpLoading extends CustomerSignUpState {}

final class SignUpFailure extends CustomerSignUpState {
  final List<dynamic>? errorMessage;

  SignUpFailure({required this.errorMessage});
}