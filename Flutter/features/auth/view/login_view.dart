import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:taambeit/constants.dart';
import 'package:taambeit/features/auth/cubit/login_cubit.dart';
import 'package:taambeit/features/auth/cubit/login_state.dart';
import 'package:taambeit/features/auth/services/login_api.dart';
import 'package:taambeit/features/auth/view/customer_sign_up_view.dart';
import 'package:taambeit/features/home/views/home_view.dart';
import 'package:taambeit/widgets/custom_button.dart';

import 'package:taambeit/widgets/custom_text_form_field.dart';

// ignore: must_be_immutable
class LoginView extends StatelessWidget {
  static String id = 'Login';
  LoginView({super.key});

  bool rememberMy = false;
  List<String> lunguge = ["English", "Arabic"];
  String selectedItem = "English";
  GlobalKey<FormState> formState = GlobalKey();
  

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => LoginCubit(LoginApi()),
      child: Scaffold(
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
        backgroundColor: backgroundWhite,
        body: BlocConsumer<LoginCubit, LoginState>(
          listener: (context, state) {
            if (state is LoginSuccess) {
              ScaffoldMessenger.of(context)
                  .showSnackBar(const SnackBar(content: Text('Success')));
              Navigator.pushReplacementNamed(context, HomeView.id);
            } else if (state is LoginFailure) {
              ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(state.errorMessage.toString())));
            }
          },
          builder: (context, state) {
            return ListView(
              children: [
                const Padding(
                  padding: EdgeInsets.only(top: 3, left: 25),
                  child: Row(
                    children: [
                      Row(
                        children: [
                          Text(
                            'Login Account',
                            style: TextStyle(
                                fontFamily: 'Roboto',
                                fontWeight: FontWeight.w400,
                                fontSize: 22,
                                color: Color.fromARGB(255, 2, 2, 2)),
                          ),
                          Icon(
                            Icons.person_outline_rounded,
                            color: black,
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 12),
                  child: Image.asset(
                    "images/logo.png",
                    height: 120,
                  ),
                ),
                Form(
                  key: formState,
                  child: Column(
                    children: [
                      Padding(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 25, vertical: 6),
                        child: TextFormFieldLogin(
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
                            context.read<LoginCubit>().email = valu!;
                          },
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 25, vertical: 6),
                        child: TextFormFieldLogin(
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
                            context.read<LoginCubit>().password = valu!;
                          },
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(left: 13, right: 25),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Row(
                              children: [
                                Checkbox(
                                  value: rememberMy,
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
                                  'Remember My',
                                  style: TextStyle(
                                    fontSize: 15.5,
                                    fontFamily: 'Poppins',
                                    color: Color.fromARGB(255, 139, 139, 139),
                                  ),
                                ),
                              ],
                            ),
                            const Text(
                              'Forget Password',
                              style: TextStyle(
                                decoration: TextDecoration.underline,
                                fontSize: 15.5,
                                fontFamily: 'Poppins',
                                color: Color.fromARGB(255, 139, 139, 139),
                              ),
                            ),
                          ],
                        ),
                      ),
                      Padding(
                        padding:
                            const EdgeInsets.only(top: 10, left: 25, right: 25),
                        child: state is LoginLoading
                            ? const CircularProgressIndicator(
                                color: Color.fromARGB(255, 42, 145, 21),
                              )
                            : CustomButton(
                                textName: "Log in",
                                onPressed: () {
                                  if (formState.currentState!.validate()) {
                                    formState.currentState!.save();
                                    context.read<LoginCubit>().logIn();
                                  }
                                },
                              ),
                      ),
                      Container(
                        padding: const EdgeInsets.only(top: 25),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            const Text('Don\'t Have an Account? ',
                                style: TextStyle(
                                    fontSize: 15,
                                    color: black,
                                    fontWeight: FontWeight.w400)),
                            GestureDetector(
                              onTap: () {
                                Navigator.pushNamed(
                                    context, CustomerSignUpView.id);
                              },
                              child: const Text('Sign up',
                                  style: TextStyle(
                                      fontSize: 15,
                                      fontWeight: FontWeight.w400,
                                      color: Color.fromARGB(255, 49, 172, 24))),
                            )
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            );
          },
        ),
      ),
    );
  }
}
