import 'package:flutter/material.dart';
import 'package:taambeit/features/auth/view/customer_sign_up_view.dart';
import 'package:taambeit/features/auth/view/login_view.dart';
import 'package:taambeit/features/home/views/home_view.dart';
import 'package:taambeit/widgets/custom_button.dart';
import 'package:taambeit/widgets/custom_lite_button.dart';

class OnboardingView extends StatefulWidget {
  static String id = 'onboarding';

  const OnboardingView({super.key});
  @override
  State<OnboardingView> createState() => _OnboardingView();
}

class _OnboardingView extends State<OnboardingView> {
  Widget dotPageView() {
    return Builder(builder: ((context) {
      return Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          for (int i = 0; i < 2; i++)
            Container(
              margin: const EdgeInsets.only(right: 5),
              height: 6,
              width: 6,
              decoration: BoxDecoration(
                  color: i == pageNumber
                      ? const Color.fromARGB(255, 158, 227, 146)
                      : const Color.fromARGB(255, 42, 145, 21),
                  borderRadius: const BorderRadius.all(Radius.circular(3))),
            ),
        ],
      );
    }));
  }

  PageController nextPage = PageController();
  int pageNumber = 0;
  @override
  Widget build(BuildContext context) {
    double height = MediaQuery.of(context).size.height;
    double width = MediaQuery.of(context).size.width;
    return Scaffold(
      body: SizedBox(
        height: height,
        child: PageView(
          controller: nextPage,
          onPageChanged: (value) => pageNumber = value,
          children: [
            Stack(
              clipBehavior: Clip.none,
              children: [
                SizedBox(
                  width: width,
                  height: height * .77,
                  child: Image.asset(
                    'images/onboarding1.png',
                    fit: BoxFit.cover,
                  ),
                ),
                Positioned(
                  top: height * .75,
                  child: Container(
                    alignment: Alignment.center,
                    width: width,
                    height: height * .25,
                    decoration: const BoxDecoration(
                      borderRadius: BorderRadius.only(
                        topLeft: Radius.circular(20),
                        topRight: Radius.circular(20),
                      ),
                      color: Color.fromARGB(255, 252, 255, 251),
                    ),
                    child: Column(
                      children: [
                        SizedBox(
                          height: height * .03,
                        ),
                        const Text(
                          'Ta’am beit',
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        const Text(
                          'Enjoy a variety of fresh Homecooked meals\nfrom local chefs Delivered to your door',
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              color: Color.fromARGB(255, 101, 101, 101),
                              fontSize: 15),
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        dotPageView(),
                        SizedBox(
                          height: height * .02,
                        ),
                        SizedBox(
                          width: width * .92,
                          height: height * .05,
                          child: CustomButton(
                            textName: 'Next',
                            onPressed: () {
                              nextPage.animateToPage(1,
                                  duration: const Duration(milliseconds: 700),
                                  curve: Curves.easeIn);
                            },
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
            Stack(
              clipBehavior: Clip.none,
              children: [
                SizedBox(
                  width: width,
                  height: height * .60,
                  child: Image.asset(
                    'images/onboarding2.png',
                    fit: BoxFit.cover,
                  ),
                ),
                Positioned(
                  top: height * .58,
                  child: Container(
                    alignment: Alignment.center,
                    width: width,
                    height: height * .42,
                    decoration: const BoxDecoration(
                      borderRadius: BorderRadius.only(
                        topLeft: Radius.circular(20),
                        topRight: Radius.circular(20),
                      ),
                      color: Colors.white,
                    ),
                    child: Column(
                      children: [
                        SizedBox(
                          height: height * .03,
                        ),
                        const Text(
                          'Ta’am beit',
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        const Text(
                          "Enjoy the perfect balance of taste and health \n with our menu selections - join us and savor \n every bite!",
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              color: Color.fromARGB(255, 101, 101, 101),
                              fontSize: 15),
                        ),
                        SizedBox(
                          height: height * .03,
                        ),
                        dotPageView(),
                        SizedBox(
                          height: height * .03,
                        ),
                        SizedBox(
                          width: width * .92,
                          height: height * .05,
                          child: CustomButton(
                            textName: 'Log in',
                            onPressed: () {
                              Navigator.pushNamed(context, LoginView.id);
                            },
                          ),
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        SizedBox(
                          width: width * .92,
                          height: height * .05,
                          child: CustomLiteButton(
                            textName: 'Register',
                            onPressed: () {
                              Navigator.pushNamed(context, CustomerSignUpView.id);
                            },
                          ),
                        ),
                        SizedBox(
                          height: height * .02,
                        ),
                        GestureDetector(
                          onTap: () {
                            Navigator.restorablePushReplacementNamed(context, HomeView.id);
                          },
                          child: const Text(
                            'Join as guest',
                            style: TextStyle(
                              decoration: TextDecoration.underline,
                              textBaseline: TextBaseline.alphabetic,
                              fontWeight: FontWeight.w500,
                              fontSize: 17,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
