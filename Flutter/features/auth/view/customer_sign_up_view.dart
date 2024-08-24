import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:taambeit/constants.dart';
import 'package:taambeit/features/auth/cubit/customer_sign_up_cubit.dart';
import 'package:taambeit/features/auth/cubit/customer_sign_up_state.dart';
import 'package:taambeit/features/auth/services/sign_up_api.dart';
import 'package:taambeit/features/auth/view/login_view.dart';
import 'package:taambeit/widgets/custom_button.dart';
import 'package:taambeit/widgets/custom_text_form_field.dart';

class CustomerSignUpView extends StatelessWidget {
  static String id = 'SignUp';
  CustomerSignUpView({super.key});
  bool? agree = false;
  GlobalKey<FormState> formState = GlobalKey();
  final CustomerSignUpApi customerSignUpApi = CustomerSignUpApi();
  @override
  Widget build(BuildContext context) {
    double height = MediaQuery.of(context).size.height;
    double width = MediaQuery.of(context).size.width;

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          onPressed: () {
            Navigator.maybePop(context);
          },
          padding: const EdgeInsets.only(left: 10),
          icon: const Icon(
            Icons.arrow_back_ios_new_outlined,
            color: black,
          ),
        ),
        backgroundColor: Colors.white,
      ),
      backgroundColor: Colors.white,
      body: BlocProvider(
        create: (context) => CustomerSignUpCubit(CustomerSignUpApi()),
        child: ListView(
          children: <Widget>[
            Padding(
              padding: EdgeInsets.symmetric(horizontal: width * .05),
              child: const Text(
                'Create New Account',
                style: TextStyle(
                    fontFamily: 'Roboto',
                    fontWeight: FontWeight.w400,
                    fontSize: 22,
                    color: Color.fromARGB(255, 2, 2, 2)),
              ),
            ),
            SizedBox(
              height: height * .05,
            ),
            Image.asset('images/logo.png'),
            SizedBox(
              height: height * .05,
            ),
            BlocConsumer<CustomerSignUpCubit, CustomerSignUpState>(
              listener: (context, state) {
                if (state is SignUpSuccess) {
                  ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
                    content: Text('success'),
                  ));
                } else if (state is SignUpFailure) {
                  ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text(state.errorMessage.toString())));
                }
              },
              builder: (context, state) {
                return Form(
                  key: formState,
                  child: Padding(
                    padding: EdgeInsets.symmetric(horizontal: width * .05),
                    child: Column(
                      children: [
                        TextFormFieldLogin(
                          labelText: "Email",
                          validator: (value) {
                            if (value.isEmpty) return 'Email is requierd';
                            if (RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$')
                                .hasMatch(value)) {
                              return null;
                            }
                            return "Please a Valid Email";
                          },
                          onSaved: (valu) {
                            context.read<CustomerSignUpCubit>().email = valu!;
                          },
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            SizedBox(
                              width: width * .44,
                              child: TextFormFieldLogin(
                                labelText: "First Name",
                                validator: (value) {
                                  if (value.isEmpty) {
                                    return 'First Name is required';
                                  }
                                  return null;
                                },
                                onSaved: (valu) {
                                  context
                                      .read<CustomerSignUpCubit>()
                                      .firstName = valu!;
                                },
                              ),
                            ),
                            SizedBox(
                              width: width * .44,
                              child: TextFormFieldLogin(
                                labelText: "Last Name",
                                validator: (vlaue) {
                                  if (vlaue.isEmpty) {
                                    return "Last Name is required";
                                  }
                                  return null;
                                },
                                onSaved: (valu) {
                                  context.read<CustomerSignUpCubit>().lastName =
                                      valu!;
                                },
                              ),
                            ),
                          ],
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        TextFormFieldLogin(
                          labelText: "Password",
                          icon: Icons.visibility_off_outlined,
                          iconColor: const Color.fromARGB(255, 139, 139, 139),
                          iconSize: 30,
                          scureText: true,
                          validator: (value) {
                            if (value.isEmpty) return 'Paswword is required';
                            return null;
                          },
                          onSaved: (valu) {
                            context.read<CustomerSignUpCubit>().password =
                                valu!;
                          },
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.start,
                          children: [
                            Checkbox(
                              value: agree,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(4),
                              ),
                              side: const BorderSide(
                                color: Color.fromARGB(255, 139, 139, 139),
                                width: 2,
                                //style: ,
                              ),
                              onChanged: (vul) {},
                            ),
                            const Text(
                              'you agree to [Platform]',
                            ),
                          ],
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        state is SignUpLoading
                            ? const CircularProgressIndicator(
                                color: Color.fromARGB(255, 42, 145, 21),
                              )
                            : CustomButton(
                                textName: 'Sign up',
                                onPressed: () {
                                  if (formState.currentState!.validate()) {
                                    formState.currentState!.save();
                                    context
                                        .read<CustomerSignUpCubit>()
                                        .signUp();
                                  }
                                },
                              ),
                        SizedBox(
                          height: height * .02,
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            const Text(
                              'Already Have an Account? ',
                              style: TextStyle(
                                  fontSize: 15,
                                  color: black,
                                  fontWeight: FontWeight.w400),
                            ),
                            GestureDetector(
                              onTap: () {
                                Navigator.pushNamed(context, LoginView.id);
                              },
                              child: const Text(
                                'Log in',
                                style: TextStyle(
                                  fontSize: 15,
                                  fontWeight: FontWeight.w400,
                                  color: Color.fromARGB(255, 49, 172, 24),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
