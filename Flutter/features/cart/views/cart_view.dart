import 'package:flutter/material.dart';

import 'package:taambeit/widgets/custom_text_title.dart';

class CartView extends StatefulWidget {
  const CartView({super.key});
  @override
  State<CartView> createState() => _CartViewState();
}

class _CartViewState extends State<CartView> {
  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: CustomScrollView(
        slivers: [
          const SliverToBoxAdapter(
              child: Padding(
            padding: EdgeInsets.all(16),
            child: CustomTextTitle(
              title: 'Cart',
              fontWeight: FontWeight.w600,
            ),
          )),
          SliverList.builder(
            itemCount: 5,
            itemBuilder: (context, index) => Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 5),
              child: Container(
                //  padding: EdgeInsets.symmetric(vertical: 16),
                decoration: const BoxDecoration(
                  border: Border(
                    bottom: BorderSide(
                      color: Color.fromARGB(255, 232, 232, 232),
                      width: 1,
                    ),
                  ),
                ),

                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const Text(
                              'Pizza (Medium)',
                              style: TextStyle(
                                fontSize: 14,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                            buildText('Hoda Mahmoud'),
                            buildText('Fries (Medium), Coca-Cola.'),
                            buildText('Ketchup, Ranch (+10 EGP)'),
                          ],
                        ),
                        SizedBox(
                          height: 83,
                          width: 83,
                          child: Image.asset(
                            'images/meal.png',
                            fit: BoxFit.fill,
                          ),
                        ),
                      ],
                    ),
                    SizedBox(
                      width: 256,
                      height: 30,
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            '120.00 EGP',
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.w500),
                          ),
                          const Text(
                            'Quantity',
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.w400),
                          ),
                          Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              IconButton(
                                onPressed: () {},
                                icon: const Icon(
                                  Icons.maximize_rounded,
                                ),
                                color: const Color.fromARGB(255, 42, 145, 21),
                                padding: const EdgeInsets.only(top: 15),
                                iconSize: 20,
                              ),
                              const Text(
                                '1',
                                style: TextStyle(
                                    fontSize: 14, fontWeight: FontWeight.w500),
                              ),
                              IconButton(
                                padding: const EdgeInsets.only(top: 5),
                                onPressed: () {},
                                icon: const Icon(
                                  Icons.add,
                                  color: Color.fromARGB(255, 42, 145, 21),
                                ),
                                iconSize: 20,
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(
                      height: 15,
                    )
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Text buildText(String text) => Text(text,
      style: const TextStyle(
          fontSize: 12,
          fontWeight: FontWeight.w400,
          color: Color.fromARGB(255, 101, 101, 101)));
}
/*Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16),
              child: Container(
                padding: const EdgeInsets.symmetric(vertical: 10),
                decoration: BoxDecoration(
                  border: Border(
                    bottom: BorderSide(
                      color: Color.fromARGB(255, 232, 232, 232),
                      width: 1,
                    ),
                  ),
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    SizedBox(
                      height: 110,
                      width: 110,
                      child: Image.asset(
                        'images/meal.png',
                        fit: BoxFit.fill,
                      ),
                    ),
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                      'Pizza (Medium)',
                      style: TextStyle(
                        fontSize: 14,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                      buildText('Hoda Mahmoud'),
                        buildText('Fries (Medium), Coca-Cola.'),
                        buildText('Ketchup, Ranch (+10 EGP)'),
                        SizedBox(
                          width: 250,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                '120.00 EGP',
                                style: TextStyle(
                                    fontSize: 14, fontWeight: FontWeight.w500,color: Colors.black),
                              ),
                              Text(
                                'Quantity',
                                style: TextStyle(
                                    fontSize: 14, fontWeight: FontWeight.w400,color: Colors.black),
                              ),
                              Row(
                                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                  children: [
                                    IconButton(
                                      onPressed: () {},
                                      icon: Icon(
                                        Icons.maximize_rounded,
                                      ),
                                      color: Color.fromARGB(255, 42, 145, 21),
                                      padding: EdgeInsets.only(top: 15),
                                      iconSize: 20,
                                    ),
                                    Text(
                                      '1',
                                      style: TextStyle(
                                          fontSize: 14, fontWeight: FontWeight.w500),
                                    ),
                                    IconButton(
                                      padding: EdgeInsets.only(top: 3),
                                      onPressed: () {},
                                      icon: Icon(
                                        Icons.add,
                                        color: Color.fromARGB(255, 42, 145, 21),
                                      ),
                                      iconSize: 20,
                                    ),
                                  ],
                                ),
                            ],
                          ),
                        ),
                      ],
                    )
                  ],
                ),
              ),
            ),*/