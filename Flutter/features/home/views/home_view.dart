import 'package:flutter/material.dart';

import 'package:taambeit/features/Account/views/account_view.dart';
import 'package:taambeit/features/cart/views/cart_view.dart';
import 'package:taambeit/features/home/views/home.dart';

class HomeView extends StatefulWidget {
  static String id = 'NavBar';
  const HomeView({super.key});

  @override
  State<HomeView> createState() => _HomeViewState();
}

class _HomeViewState extends State<HomeView> {
  int selectIndex = 0;
  List<Widget> bage = [
    const Home(),
    const CartView(),
    const AccountView(),
  ];

  @override
  Widget build(BuildContext context) {
    //double height = MediaQuery.of(context).size.height;
    //double width = MediaQuery.of(context).size.width;
    return DefaultTabController(
      length: 3,
      child: Scaffold(
        backgroundColor: const Color.fromARGB(255, 255, 255, 255),
        bottomNavigationBar: BottomAppBar(
          color: const Color.fromARGB(255, 248, 249, 248),
          height: 62,
          padding: const EdgeInsets.symmetric(horizontal: 0),
          child: TabBar(
            onTap: (value) {
              setState(() {
                selectIndex = value;
              });
            },
            dividerHeight: 0,
            unselectedLabelStyle: const TextStyle(
              color: Color.fromARGB(255, 101, 101, 101),
              fontSize: 14,
            ),
            labelColor: const Color.fromARGB(255, 42, 145, 21),
            indicatorSize: TabBarIndicatorSize.tab,
            indicator: const UnderlineTabIndicator(
              insets: EdgeInsets.only(bottom: 59),
              borderSide: BorderSide(
                color: Color.fromARGB(255, 42, 145, 21),
                width: 4,
              ),
              borderRadius: BorderRadius.all(Radius.circular(10)),
            ),
            tabs: const [
              Tab(
                icon: Icon(
                  Icons.home_filled,
                  size: 25,
                ),
                text: 'Home',
                iconMargin: EdgeInsets.only(top: 3),
              ),
              Tab(
                icon: Icon(
                  Icons.shopping_cart_outlined,
                ),
                iconMargin: EdgeInsets.only(top: 3),
                text: 'My Cart',
              ),
              Tab(
                icon: Icon(
                  Icons.person_outline_outlined,
                  size: 26,
                ),
                iconMargin: EdgeInsets.only(top: 3),
                text: 'Account',
              ),
            ],
          ),
        ),
        body: bage.elementAt(selectIndex),
      ),
    );
  }
}
/*
 BottomNavigationBar(
        backgroundColor: const Color.fromARGB(255, 255, 255, 255),
        selectedItemColor: const Color.fromARGB(255, 42, 145, 21),
        iconSize: 28,
        unselectedFontSize: 14,
        selectedFontSize: 14,
        currentIndex: selectIndex,
        
        //fixedColor: Colors.amber,
        onTap: (value) {
          setState(() {
            selectIndex = value;
          });
        },
        //  landscapeLayout:BottomNavBarLandscapeLayout.centered ,

        items: const [
          BottomNavigationBarItem(
              label: 'Home',
              icon: Icon(Icons.home_rounded),
              tooltip: AutofillHints.birthdayDay),
          BottomNavigationBarItem(
              label: 'My Cart', icon: Icon(Icons.shopping_cart_outlined)),
          BottomNavigationBarItem(
              label: 'Account', icon: Icon(Icons.person_outline_rounded))
        ],
      ),*/