import 'package:flutter/material.dart';
import 'package:taambeit/core/cache/cache_helper.dart';
import 'package:taambeit/features/Account/views/favourite_view.dart';
import 'package:taambeit/features/auth/view/customer_sign_up_view.dart';
import 'package:taambeit/features/auth/view/login_view.dart';



class AccountView extends StatelessWidget {
  static String id = 'myAccountView';
  const AccountView({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: ListView(
        children: [
          const SizedBox(
            height: 64,
          ),
          buildTitleBageAndAccountContainer(),
          const SizedBox(
            height: 40,
          ),
          buildListTail(
            const Icon(
              Icons.receipt_long_rounded,
              size: 18,
            ),
            null,
            'My Orders',
            () {},
          ),
          buildListTail(
            const Icon(
              Icons.favorite_border,
              size: 18,
            ),
            null,
            'Favourite',
            () {
              Navigator.pushNamed(context, FavouriteView.id);
            },
          ),
          buildListTail(
            const Icon(
              Icons.receipt_long_rounded,
              size: 18,
            ),
            null,
            'Vouchers',
            () {},
          ),
          buildListTail(
            null,
            'images/group.png',
            'Refer a friend',
            () {},
          ),
          buildListTail(
            null,
            'icons/chef-hat.png',
            'Join as a Chef',
            () {},
          ),
          buildListTail(
            const Icon(
              Icons.language,
              size: 18,
            ),
            null,
            'Change The Language',
            () {},
          ),
          buildListTail(
            null,
            'icons/ask-question.png',
            'Help',
            () {},
          ),
          buildListTail(
            const Icon(
              Icons.info_outline,
              size: 18,
            ),
            null,
            'About App',
            () {},
          ),
          if(CacheData().getData(key: 'jwt') == null)
          buildListTail(
            const Icon(
              Icons.account_box_outlined,
              size: 18,
            ),
            null,
            'Sign up',
            () {
              Navigator.pushNamed(context, CustomerSignUpView.id);
            },
          ),
          if(CacheData().getData(key: 'jwt') == null)
          buildListTail(
            const Icon(
              Icons.login,
              size: 18,
            ),
            null,
            'Log in',
            () {
              Navigator.pushNamed(context, LoginView.id);
            },
          ),
          if(CacheData().getData(key: 'jwt') != null)
          buildListTail(
            const Icon(
              Icons.logout_outlined,
              size: 18,
            ),
            null,
            'Log out',
            () {
              CacheData().remove(key: 'id');
              CacheData().remove(key: 'jwt');
              Navigator.pushNamed(context, LoginView.id);
            },
          ),
        ],
      ),
    );
  }

  InkWell buildListTail(
      Icon? icon, String? imageUrl, String text, void Function() onTap) {
    return InkWell(
      onTap: onTap,
      child: Container(
        alignment: Alignment.centerLeft,
        height: 55,
        decoration: const BoxDecoration(
          border: Border(
            bottom: BorderSide(
              color: Color.fromARGB(255, 232, 232, 232),
              width: 1,
            ),
          ),
        ),
        child: Row(
          children: [
            Padding(
              padding: const EdgeInsets.only(right: 10),
              child: (icon == null)
                  ? SizedBox(width: 20, child: Image.asset('$imageUrl'))
                  : icon,
            ),
            Text(
              text,
              style: const TextStyle(fontSize: 16, fontWeight: FontWeight.w400),
            ),
          ],
        ),
      ),
    );
  }

  Widget buildTitleBageAndAccountContainer() {
    return SizedBox(
      height: 98,
      width: 390,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Padding(
            padding: EdgeInsets.only(left: 5),
            child: Text(
              'My Account',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.w600,
                color: Color.fromARGB(255, 50, 47, 47),
              ),
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const SizedBox(
                width: 240,
                child: Row(
                  mainAxisSize: MainAxisSize.max,
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Icon(
                      Icons.account_circle,
                      color: Color.fromARGB(255, 160, 217, 149),
                      size: 50,
                    ),
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'Ahmed Ibrahim',
                          style: TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                        Text(
                          'Ahmedelshafei2361@gmail.com',
                          style: TextStyle(
                              fontSize: 12,
                              fontWeight: FontWeight.w400,
                              color: Color.fromARGB(255, 101, 101, 101)),
                        ),
                      ],
                    )
                  ],
                ),
              ),
              InkWell(
                child: Column(
                  children: [
                    Image.asset('icons/edit.png'),
                    const Text(
                      'edit',
                      style: TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.w400,
                        color: Color.fromARGB(255, 101, 101, 101),
                      ),
                    ),
                  ],
                ),
              )
            ],
          ),
        ],
      ),
    );
  }
}
