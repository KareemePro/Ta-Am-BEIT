import 'package:flutter/material.dart';

import 'package:flutter/services.dart';

import 'package:taambeit/features/meal/models/get_meal_side_dish_options_request.dart';

import 'package:taambeit/features/meal/models/meal.dart';
import 'package:taambeit/features/meal/views/widgets/custom_read_more_text.dart';
import 'package:taambeit/widgets/custom_icon_background.dart';

import 'package:taambeit/widgets/custom_icon_rating.dart';

class MealView extends StatefulWidget {
  const MealView({
    super.key,
  });
  static String id = 'mealView';

  @override
  State<MealView> createState() => _MealViewState();
}

class _MealViewState extends State<MealView> {
  String? toppingsGroupValue;
  String? sideDishOptionsGroupValue;
  int quantity = 1;
  String selectedMealSizeOption = '';
  String? image;
  int? price;
  List<String> sideDishOptionSize = ['s', 'M', 'L'];
  var s = [];
  var sideDishOptions = <GetMealSideDishOptionsRequest?>[];
  var toppings = <GetMealSideDishOptionsRequest?>[];
  @override
  Widget build(BuildContext context) {
    MealModel meal = ModalRoute.of(context)?.settings.arguments as MealModel;
    SystemChrome.setSystemUIOverlayStyle(const SystemUiOverlayStyle(
      statusBarColor: Color.fromARGB(0, 0, 0, 0),
      statusBarIconBrightness: Brightness.light,
    ));
    return Scaffold(
      backgroundColor: Colors.white,
      body: CustomScrollView(
        slivers: [
          SliverToBoxAdapter(
            child: Container(
              decoration: const BoxDecoration(
                border: Border(
                  bottom: BorderSide(
                      color: Color.fromARGB(255, 244, 244, 244), width: 6),
                ),
              ),
              child: Column(
                children: [
                  Stack(
                    children: [
                      Container(
                        clipBehavior: Clip.antiAlias,
                        decoration: const BoxDecoration(
                          borderRadius: BorderRadius.only(
                            bottomLeft: Radius.circular(10),
                            bottomRight: Radius.circular(10),
                          ),
                        ),
                        width: double.infinity,
                        height: 200,
                        child: Image.network(
                          image ??
                              meal.getMealOptionsRequest!.first
                                  .fullScreenImage!,
                          fit: BoxFit.fill,
                        ),
                      ),
                      Positioned(
                        top: 40,
                        left: 20,
                        child: SizedBox(
                          width: 370,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              CustomIconBackground(
                                icon: const Icon(Icons.keyboard_arrow_left),
                                onTap: () {
                                  Navigator.maybePop(context);
                                },
                              ),
                              SizedBox(
                                width: 60,
                                child: Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    CustomIconBackground(
                                      icon: const Icon(
                                        Icons.favorite_border,
                                        color:
                                            Color.fromARGB(255, 101, 101, 101),
                                        size: 18,
                                      ),
                                      onTap: () {},
                                    ),
                                    CustomIconBackground(
                                      icon: const Icon(
                                        Icons.more_horiz_outlined,
                                        size: 18,
                                      ),
                                      onTap: () {},
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
                  ),
                  Padding(
                    padding: const EdgeInsets.only(left: 16, right: 18),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const SizedBox(
                          height: 15,
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            buildTextMediumBold(meal.title!),
                            SizedBox(
                              width: 130,
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: [
                                  const CustomIconRating(),
                                  Text(
                                    '${meal.rating} (${meal.reviewCount} ratings)',
                                    style: const TextStyle(
                                      fontSize: 14,
                                      fontWeight: FontWeight.w500,
                                      decoration: TextDecoration.underline,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        Row(
                          children: [
                            const Text(
                              'By Chef ',
                              style: TextStyle(
                                fontSize: 15,
                                fontWeight: FontWeight.w500,
                                color: Color.fromARGB(255, 101, 101, 101),
                              ),
                            ),
                            Text(
                              meal.chiefName!,
                              style: const TextStyle(
                                fontSize: 15,
                                fontWeight: FontWeight.w500,
                                color: Color.fromARGB(255, 101, 101, 101),
                                decoration: TextDecoration.underline,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(
                          height: 15,
                        ),
                        CustomReadMoreText(text: meal.description!),
                        const SizedBox(
                          height: 15,
                        ),
                        Row(
                          children: [
                            Text(
                              'Spice Level: ${meal.mealSpiceLevel} ',
                              style: const TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w500,
                                color: Color.fromARGB(255, 101, 101, 101),
                              ),
                            ),
                            Image.asset('icons/not_spicy.png'),
                          ],
                        ),
                        const SizedBox(
                          height: 15,
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            SizedBox(
                              width: 208,
                              child: Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: [
                                  buildSmallButton('Small', () {
                                    var mealOption = meal.getMealOptionsRequest
                                        ?.where((element) =>
                                            element.mealSizeOption == 0);
                                    if (mealOption != null) {
                                      selectedMealSizeOption = 'Small';
                                      image = mealOption.first.fullScreenImage;
                                      price = mealOption.first.price;
                                      
                                      sideDishOptions.clear();

                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isFree == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            sideDishOptions.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      toppings.clear();
                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isTopping == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            toppings.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      // print(sideDishOptions.length);
                                      // print(toppings.length);
                                    } else {}
                                    setState(() {});
                                  }),
                                  buildSmallButton('Medium', () {
                                    var mealOption = meal.getMealOptionsRequest
                                        ?.where((element) =>
                                            element.mealSizeOption == 1);
                                    if (mealOption!.isNotEmpty) {
                                      selectedMealSizeOption = 'Medium';
                                      image = mealOption.first.fullScreenImage;
                                      price = mealOption.first.price;
                                      sideDishOptions.clear();
                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isFree == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            sideDishOptions.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      toppings.clear();
                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isTopping == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            toppings.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      // print(sideDishOptions.length);
                                      // print(toppings.length);
                                    } else {}
                                    setState(() {});
                                  }),
                                  buildSmallButton('Large', () {
                                    var mealOption = meal.getMealOptionsRequest
                                        ?.where((element) =>
                                            element.mealSizeOption == 2);
                                    if (mealOption!.isNotEmpty) {
                                      selectedMealSizeOption = 'Large';
                                      image = mealOption.first.fullScreenImage;
                                      price = mealOption.first.price;
                                      sideDishOptions.clear();
                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isFree == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            sideDishOptions.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      toppings.clear();
                                      for (var e in mealOption) {
                                        e.getMealSideDishesRequest!
                                            .where((e) => e.isTopping == true)
                                            .forEach((a) {
                                          for (var v in a
                                              .getMealSideDishOptionsRequest!) {
                                            toppings.add(
                                              GetMealSideDishOptionsRequest(
                                                name: v.name,
                                                price: v.price,
                                                sideDishID: v.sideDishID,
                                                sideDishSizeOption:
                                                    v.sideDishSizeOption,
                                                quantity: v.quantity,
                                              ),
                                            );
                                          }
                                        });
                                      }
                                      // print(sideDishOptions.length);
                                      // print(toppings.length);
                                    } else {}
                                    setState(() {});
                                  }),
                                  /*...List.generate(meal.getMealOptionsRequest!.length, (index) => buildSmallButton(mealSizeOption[meal.getMealOptionsRequest!.elementAt(index).mealSizeOption!], () {
                                    setState(() {
                                      isSelected = mealSizeOption[meal.getMealOptionsRequest!.elementAt(index).mealSizeOption!];
                                    });
                                  }), )*/
                                ],
                              ),
                            ),
                            buildTextMediumBold(
                                '${price ?? meal.getMealOptionsRequest!.first.price} EGP')
                          ],
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(
                    height: 15,
                  ),
                  buildQuantity(),
                  const SizedBox(
                    height: 15,
                  ),
                ],
              ),
            ),
          ),
          SliverToBoxAdapter(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.only(
                      left: 16, right: 18, top: 20, bottom: 16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'Toppings',
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.w500),
                          ),
                          Container(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 8, vertical: 5),
                            decoration: BoxDecoration(
                              color: const Color.fromARGB(255, 234, 247, 232),
                              borderRadius: BorderRadius.circular(15),
                            ),
                            child: const Text(
                              'Required',
                              style: TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          )
                        ],
                      ),
                      const SizedBox(
                        height: 5,
                      ),
                      const SizedBox(
                        height: 10,
                      ),
                      for (var t in toppings)
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              '${t!.name} (${sideDishOptionSize[t.sideDishSizeOption!]})',
                              style: const TextStyle(
                                  fontSize: 15, fontWeight: FontWeight.w400),
                            ),
                            Row(
                              children: [
                                Text(
                                  '(+${t.price} EGP)   ',
                                  style: const TextStyle(
                                    fontSize: 12,
                                    fontWeight: FontWeight.w400,
                                    color: Color.fromARGB(255, 101, 101, 101),
                                  ),
                                ),
                                SizedBox(
                                  width: 20,
                                  height: 30,
                                  child: Radio(
                                    activeColor:
                                        const Color.fromARGB(255, 42, 145, 21),
                                    value: t.sideDishID,
                                    groupValue: toppingsGroupValue,
                                    onChanged: (vlue) {
                                      setState(() {
                                        toppingsGroupValue = vlue;
                                        print(toppingsGroupValue);
                                      });
                                    },
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                    ],
                  ),
                ),
                const Divider(
                  color: Color.fromARGB(255, 244, 244, 244),
                  thickness: 6,
                )
              ],
            ),
          ),
          SliverToBoxAdapter(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.only(
                      left: 16, right: 18, top: 20, bottom: 16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'Side Dishes',
                            style: TextStyle(
                                fontSize: 14, fontWeight: FontWeight.w500),
                          ),
                          Container(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 8, vertical: 5),
                            decoration: BoxDecoration(
                              color: const Color.fromARGB(255, 234, 247, 232),
                              borderRadius: BorderRadius.circular(15),
                            ),
                            child: const Text(
                              'Required',
                              style: TextStyle(
                                fontSize: 12,
                                fontWeight: FontWeight.w500,
                              ),
                            ),
                          )
                        ],
                      ),
                      const SizedBox(
                        height: 5,
                      ),
                      buildSupTitleChoose1(),
                      const SizedBox(
                        height: 10,
                      ),
                      //for (int i = 0; i < sideDishOptions.length; i++)
                      for (var s in sideDishOptions)
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              '${s!.name!} (${sideDishOptionSize[s.sideDishSizeOption!]})',
                              style: const TextStyle(
                                  fontSize: 15, fontWeight: FontWeight.w400),
                            ),
                            Row(
                              children: [
                                Text(
                                  '(+${s.price})   ',
                                  style: const TextStyle(
                                    fontSize: 12,
                                    fontWeight: FontWeight.w400,
                                    color: Color.fromARGB(255, 101, 101, 101),
                                  ),
                                ),
                                SizedBox(
                                  width: 20,
                                  height: 30,
                                  child: Radio(
                                    activeColor:
                                        const Color.fromARGB(255, 42, 145, 21),
                                    value: s.sideDishID! +
                                        sideDishOptionSize[
                                            s.sideDishSizeOption!],
                                    groupValue: sideDishOptionsGroupValue,
                                    onChanged: (vlue) {
                                      setState(() {
                                        sideDishOptionsGroupValue = vlue;
                                      });
                                    },
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                    ],
                  ),
                ),
                const Divider(
                  color: Color.fromARGB(255, 244, 244, 244),
                  thickness: 6,
                )
              ],
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 20),
              child: Column(
                children: [
                  const Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        'Got any notes?',
                        style: TextStyle(
                            fontSize: 14, fontWeight: FontWeight.w400),
                      ),
                      SizedBox(
                        width: 65,
                        child: Text(
                          'Add a note',
                          style: TextStyle(
                            fontSize: 12,
                            fontWeight: FontWeight.w400,
                            color: Color.fromARGB(255, 42, 145, 21),
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(
                    height: 15,
                  ),
                  MaterialButton(
                    shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(10),
                        side: const BorderSide(
                            color: Color.fromARGB(255, 42, 145, 21))),
                    color: const Color.fromARGB(255, 42, 145, 21),
                    textColor: Colors.white,
                    minWidth: 400,
                    height: 50,
                    onPressed: () {},
                    child: const Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          'Add to basket',
                          style: TextStyle(
                            color: Color.fromARGB(255, 248, 249, 248),
                            fontWeight: FontWeight.w500,
                            fontSize: 16,
                          ),
                        ),
                        Text(
                          '155 EGP',
                          style: TextStyle(
                            color: Color.fromARGB(255, 248, 249, 248),
                            fontWeight: FontWeight.w600,
                            fontSize: 16,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          )
        ],
      ),
    );
  }

  Text buildSupTitleChoose1() {
    return const Text(
      'Choose 1',
      style: TextStyle(
        fontSize: 12,
        fontWeight: FontWeight.w400,
        color: Color.fromARGB(255, 101, 101, 101),
      ),
    );
  }

  Padding buildQuantity() {
    return Padding(
      padding: const EdgeInsets.only(left: 16, right: 5),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          const Text(
            'Quantity',
            style: TextStyle(fontSize: 15, fontWeight: FontWeight.w500),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: [
              IconButton(
                onPressed: () {
                  setState(() {
                    if (quantity > 1) quantity--;
                  });
                },
                icon: const Icon(
                  Icons.maximize_rounded,
                ),
                color: const Color.fromARGB(255, 42, 145, 21),
                padding: const EdgeInsets.only(top: 15, right: 0),
                iconSize: 20,
              ),
              Text(
                '$quantity',
                style:
                    const TextStyle(fontSize: 14, fontWeight: FontWeight.w500),
              ),
              IconButton(
                padding: const EdgeInsets.only(top: 3),
                onPressed: () {
                  setState(() {
                    quantity++;
                  });
                },
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
    );
  }

  Text buildTextMediumBold(String text) {
    return Text(
      text,
      style: const TextStyle(
        fontSize: 18,
        fontWeight: FontWeight.w700,
      ),
    );
  }

  Widget buildSmallButton(String text, void Function() onTap) {
    return InkWell(
      onTap: onTap,
      child: Container(
        alignment: Alignment.center,
        height: 28,
        width: 64,
        decoration: BoxDecoration(
          color: (selectedMealSizeOption == text)
              ? const Color.fromARGB(255, 158, 227, 146)
              : Colors.white,
          border: Border.all(
              color: const Color.fromARGB(255, 42, 145, 21), width: 0.92),
          borderRadius: BorderRadius.circular(9.1),
        ),
        child: Text(
          text,
          style: TextStyle(
            //fontFamily: 'Poppins',
            fontWeight: FontWeight.w400,
            fontSize: 13,
            color: (selectedMealSizeOption == text)
                ? Colors.black
                : const Color.fromARGB(255, 101, 101, 101),
          ),
        ),
      ),
    );
  }
}
