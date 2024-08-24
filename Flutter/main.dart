import 'dart:io';

import 'package:flutter/material.dart';
import 'package:taambeit/core/cache/cache_helper.dart';
import 'package:taambeit/features/Account/views/account_view.dart';
import 'package:taambeit/features/Account/views/favourite_view.dart';

import 'package:taambeit/features/auth/view/customer_sign_up_view.dart';
import 'package:taambeit/features/auth/view/login_view.dart';
import 'package:taambeit/features/home/views/home_view.dart';
import 'package:taambeit/features/meal/views/meal_view.dart';
import 'package:taambeit/features/onboarding/views/onbording.dart';

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}

int? initScreen;
String? r;
Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  HttpOverrides.global = MyHttpOverrides();
  await CacheData().cacheInitialization();
  initScreen = CacheData().getData(key: 'initScreen') as int?;
  CacheData().setData(key: 'initScreen', value: 1);

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(fontFamily: "Poppins"),
      initialRoute: initScreen == 0 || initScreen == null
          ? OnboardingView.id
          : HomeView.id,
      routes: {
        OnboardingView.id: (context) => const OnboardingView(),
        LoginView.id: (context) => LoginView(),
        CustomerSignUpView.id: (context) => CustomerSignUpView(),
        HomeView.id: (context) => const HomeView(),
        //ChefView.id: (context) => const ChefView(),
        AccountView.id: (context) => const AccountView(),
        FavouriteView.id: (context) => const FavouriteView(),
        MealView.id: (context) => const MealView(),
      },
    );
  }
}
