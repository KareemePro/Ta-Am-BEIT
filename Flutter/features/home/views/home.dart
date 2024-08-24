import 'package:flutter/material.dart';



import 'package:taambeit/enums/view_type.dart';

import 'package:taambeit/features/meal/models/meal.dart';
import 'package:taambeit/features/meal/viewModel/meal_view_model.dart';
import 'package:taambeit/features/meal/views/widgets/meals_graid_view.dart';

import 'package:taambeit/features/meal/views/widgets/meals_list_view.dart';
import 'package:taambeit/widgets/custom_circular_loding.dart';

import 'package:taambeit/widgets/custom_search.dart';
import 'package:taambeit/widgets/custom_text_title.dart';
import 'package:taambeit/widgets/custom_toggle_buttons.dart';


class Home extends StatefulWidget {
  static String id = 'homeView';

  const Home({super.key});

  @override
  State<Home> createState() => _HomeViewState();
}

class _HomeViewState extends State<Home> {
  MealViewModel meals = MealViewModel();

  final List<String> sortBy = [
    'BestSelling',
    'NewlyAdded',
    'PriceAsc',
    'PriceDesc',
    'Discount',
  ];
  List<String> filter = [
    'Koshry',
    'SeaFood',
    'DeepFried',
    'Vegan',
    'DietFriendly',
    'KetoFriendly',
    'NaturalButter',
    'Sweeats',
    'GlutenFree',
    'SlowCooked',
    'NaturalColors',
    'Biscuits',
    'Cookies',
    'Cake',
    'Beef',
    'Lamb',
    'Ribs',
  ];

  List<String> mealSize = [
    'Small',
    'Medium',
    'Large',
  ];
  List<String> spiceLevel = [
    'NotSpicy',
    'Mild',
    'Medium',
    'Hot',
    'VeryHot',
  ];
  List<String> mealStyle = [
    'Egyption',
    'Italian',
    'Moroccan',
    'American',
    'Syrian',
    'Asian',
  ];

  String? selectedItem;
  final List<bool> _isSelected = [true, false];
  String? selectedValue;
  int? sortIndex;
  int? filterIndex;
  int? mealSizeIndex;
  int? spiceLevelIndex;
  int? mealStyleIndex;
  int? mealCategoryIndex;
  ViewType _viewType = ViewType.list;
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: SafeArea(
        child: CustomScrollView(
          slivers: [
            /*  SliverToBoxAdapter(
              child: Container(
                height: 30,
                margin: const EdgeInsets.symmetric(vertical: 6),
                padding: EdgeInsets.only(left: 12),
                child: Row(
                  children: [
                  //  IconButton(onPressed: ()=> scaffoldKey.currentState!.openDrawer() , icon:const Icon(Icons.menu),padding: EdgeInsets.symmetric(vertical: 0),),
                   
                    SizedBox(
                      width: 370,
                      child: DropdownButtonFormField(
                        decoration: const InputDecoration(
                            prefixIcon: Icon(
                              Icons.location_on,
                              color: Color.fromARGB(255, 42, 145, 21),
                            ),
                            hintText: 'Enter Location',
                            border: InputBorder.none,
                            fillColor: Colors.white,
                            filled: true,
                            contentPadding: EdgeInsets.only(top: -5, right: 12)),
                        isExpanded: true,
                        items: items
                            .map(
                              (item) => DropdownMenuItem(
                                value: item,
                                child: Text(item),
                              ),
                            )
                            .toList(),
                        onChanged: (item) => setState(() {
                          selectedItem = item;
                        }),
                      ),
                    ),
                  ],
                ),
              ),
            ),*/

            SliverToBoxAdapter(
              child: Container(
                padding: const EdgeInsets.only(bottom: 12, left: 12, right: 12),
                margin: const EdgeInsets.only(top: 15),
                child: const Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    SizedBox(
                      width: 350,
                      height: 50,
                      child: CustomSearch(),
                    ),
                    Icon(
                      Icons.notifications_none_sharp,
                      color: Color.fromARGB(255, 101, 101, 101),
                      size: 30,
                    )
                  ],
                ),
              ),
            ),
            SliverToBoxAdapter(
              child: SingleChildScrollView(
                padding: const EdgeInsets.symmetric(horizontal: 6),
                scrollDirection: Axis.horizontal,
                child: Row(
                  children: [
                    customDrobdownFormField(
                      width: 90,
                      prefexIcon: const Icon(
                        Icons.sort,
                        size: 20,
                        color: Colors.black,
                      ),
                      hintText: 'Sort',
                      items: sortBy,
                      onChanged: (value) {
                        for (int i = 0; i < sortBy.length; i++) {
                          if (value == sortBy[i]) {
                            sortIndex = i;
                            setState(() {});
                          }
                        }
                      },
                    ),
                    customDrobdownFormField(
                      width: 90,
                      prefexIcon: const Icon(
                        Icons.filter_list,
                        size: 20,
                        color: Colors.black,
                      ),
                      hintText: 'Filters',
                      items: filter,
                      onChanged: (value) {
                        for (int i = 0; i < filter.length; i++) {
                          if (value == filter[i]) {
                            filterIndex = i;
                            setState(() {});
                          }
                        }
                      },
                    ),
                    customDrobdownFormField(
                      width: 90,
                      prefexIcon: const Icon(
                        Icons.sort,
                        size: 20,
                        color: Colors.black,
                      ),
                      hintText: 'Sizes',
                      items: mealSize,
                      onChanged: (value) {
                        for (int i = 0; i < mealSize.length; i++) {
                          if (value == mealSize[i]) {
                            mealSizeIndex = i;
                            setState(() {});
                          }
                        }
                      },
                    ),
                    customDrobdownFormField(
                      width: 120,
                      prefexIcon: const Icon(
                        Icons.sort,
                        size: 20,
                        color: Colors.black,
                      ),
                      hintText: 'Spice Level',
                      items: spiceLevel,
                      onChanged: (value) {
                        for (int i = 0; i < spiceLevel.length; i++) {
                          if (value == spiceLevel[i]) {
                            spiceLevelIndex = i;
                            setState(() {});
                          }
                        }
                      },
                    ),
                    customDrobdownFormField(
                      width: 120,
                      prefexIcon: const Icon(
                        Icons.sort,
                        size: 20,
                        color: Colors.black,
                      ),
                      hintText: 'Meal Style',
                      items: mealStyle,
                      onChanged: (value) {
                        for (int i = 0; i < mealStyle.length; i++) {
                          if (value == mealStyle[i]) {
                            mealStyleIndex = i;
                            setState(() {});
                          }
                        }
                      },
                    ),
                  ],
                ),
              ),
            ),
            const SliverToBoxAdapter(
              child: SizedBox(
                height: 10,
              ),
            ),
            SliverToBoxAdapter(
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceAround,
                children: [
                  category(
                    "images/main_dish.png",
                    "Main Dish",
                    () => setState(() {
                      mealCategoryIndex = 0;
                    }),
                  ),
                  category(
                    "images/side_dish.png",
                    "Side Dish",
                    () => setState(() {
                      mealCategoryIndex = 1;
                    }),
                  ),
                  category(
                    "images/appetizer.png",
                    "Appetizer",
                    () => setState(() {
                      mealCategoryIndex = 2;
                    }),
                  ),
                ],
              ),
            ),
            SliverToBoxAdapter(
              child: Padding(
                padding:
                    const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
                child: Container(
                  padding: const EdgeInsets.symmetric(vertical: 5),
                  height: 84,
                  decoration: BoxDecoration(
                    color: const Color.fromARGB(255, 88, 179, 70),
                    borderRadius: BorderRadius.circular(12.39),
                  ),
                  child: const Column(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: [
                      Text(
                        'Get',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 14,
                            fontWeight: FontWeight.w500),
                      ),
                      Text(
                        '100 EGP OFF',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 23,
                            fontWeight: FontWeight.w700),
                      ),
                      Text(
                        ' Your First Order',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 14,
                            fontWeight: FontWeight.w500),
                      ),
                    ],
                  ),
                ),
              ),
            ),
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 14),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    const CustomTextTitle(
                      title: 'Menu',
                      fontWeight: FontWeight.w500,
                    ),
                    CustomToggleButtons(
                        isSelected: _isSelected,
                        onPressed: (index) {
                          setState(() {
                            if (index == 0) {
                              _isSelected[0] = true;
                              _isSelected[1] = false;
                              _viewType = ViewType.list;
                            } else {
                              _isSelected[0] = false;
                              _isSelected[1] = true;
                              _viewType = ViewType.grid;
                            }
                          });
                        })
                  ],
                ),
              ),
            ),
            FutureBuilder<List<MealModel>>(
                future: meals.fetchMeals(
                    sortBy: sortIndex,
                    tagFilter: filterIndex,
                    mealSpiceLevel: spiceLevelIndex,
                    mealStyle: mealSizeIndex,
                    sizeFilter: mealSizeIndex,
                    mealCategory: mealCategoryIndex,
                  ),
                builder: (context, snapshot) {
                  if (snapshot.hasData) {
                    return (_viewType == ViewType.list)
                        ? MealsListView(meals: snapshot.data!)
                        : MealsGraidView(meals: snapshot.data!);
                  } else if (snapshot.hasError) {
                    return const SliverToBoxAdapter(child: Text('erorr'));
                  } else {
                    return const SliverToBoxAdapter(
                      child: CustomCircularLoding(),
                    );
                  }
                })
          ],
        ),
      ),
    );
  }

  InkWell category(String imagUrl, String title, void Function() onTap) {
    return InkWell(
      onTap: onTap,
      child: SizedBox(
        height: 105,
        width: 83,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            SizedBox(
              height: 83,
              child: Image.asset(imagUrl),
            ),
            SizedBox(
              height: 22,
              child: Text(title),
            ),
          ],
        ),
      ),
    );
  }

  Padding customDrobdownFormField(
      {required double width,
      required Widget prefexIcon,
      required String hintText,
      required List<String> items,
      required Function(String?) onChanged}) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 6),
      child: FittedBox(
        //  fit: BoxFit.cover,
        child: Container(
          height: 30,
          width: width,
          constraints: const BoxConstraints(minWidth: 83),
          decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: const BorderRadius.all(Radius.circular(6)),
              border: Border.all(
                  color: const Color.fromARGB(255, 152, 150, 150), width: .1),
              boxShadow: const [
                BoxShadow(
                  blurRadius: 2,
                  color: Color.fromARGB(20, 0, 0, 0),
                  offset: Offset(0, 1),
                  spreadRadius: 0,
                )
              ]),
          child: DropdownButtonFormField(
            isExpanded: true,
            decoration: InputDecoration(
              prefixIcon: prefexIcon,
              hintText: hintText,
              hintStyle: const TextStyle(color: Colors.black, fontSize: 14),
              prefixIconConstraints: const BoxConstraints.expand(width: 25),
              contentPadding: const EdgeInsets.only(bottom: 13, left: 3),
              border: InputBorder.none,
            ),
            dropdownColor: Colors.white,
            style: const TextStyle(
                fontSize: 13, color: Colors.black, fontWeight: FontWeight.w500),
            icon: Padding(
              padding: const EdgeInsets.only(top: 6),
              child: Image.asset(
                'icons/arrow_down.png',
                width: 20,
                height: 30,
                color: Colors.black,
              ),
            ),
            menuMaxHeight: 200,
            value: selectedItem,
            items: items
                .map(
                  (item) => DropdownMenuItem(
                    value: item,
                    child: Text(item),
                  ),
                )
                .toList(),
            onChanged: onChanged,
          ),
        ),
      ),
    );
  }
}

/*customDrobdownFormField(
                        const Icon(
                          Icons.sort,
                          size: 20,
                          color: Colors.black,
                        ),
                        'Sort',
                        items),
                    customDrobdownFormField(
                        const Icon(
                          Icons.sort,
                          size: 20,
                          color: Colors.black,
                        ),
                        'Filters',
                        items),
                    customDrobdownFormField(
                        const Icon(
                          Icons.sort,
                          size: 20,
                          color: Colors.black,
                        ),
                        'Sort',
                        items),
                    customDrobdownFormField(
                        const Icon(
                          Icons.sort,
                          size: 20,
                          color: Colors.black,
                        ),
                        'Sort',
                        items),
                        
                          DropdownButtonFormField2<String>(
                      isExpanded: true,
                      decoration: InputDecoration(
                        contentPadding:
                            const EdgeInsets.symmetric(vertical: 10),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(12),
                        ),
                        focusedBorder: const OutlineInputBorder(
                          borderSide: BorderSide(color: Colors.green),
                          borderRadius: BorderRadius.all(Radius.circular(12)),
                        ),
                      ),
                      hint: const Text(
                        'Sort By',
                        style: TextStyle(fontSize: 14),
                      ),
                      items: sortBy
                          .map((item) => DropdownMenuItem<String>(
                                value: item,
                                child: Text(
                                  item,
                                  style: const TextStyle(
                                    fontSize: 14,
                                  ),
                                ),
                              ))
                          .toList(),
                      onChanged: (value) {
                        for (int i = 0; i < sortBy.length; i++) {
                          if (value == sortBy[i]) {
                            index = i;
                            print(index);
                            setState(() {});
                          }
                        }
                      },
                      onSaved: (value) {
                        selectedValue = value.toString();
                        print(selectedValue);
                      },
                      buttonStyleData: const ButtonStyleData(
                          padding: EdgeInsets.only(right: 8),
                          decoration: BoxDecoration()),
                      iconStyleData: const IconStyleData(
                        icon: Icon(
                          Icons.arrow_drop_down,
                          color: Colors.black45,
                        ),
                        iconSize: 24,
                      ),
                      dropdownStyleData: DropdownStyleData(
                        decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(12),
                            color: Colors.white),
                      ),
                      menuItemStyleData: const MenuItemStyleData(
                        padding: EdgeInsets.symmetric(horizontal: 16),
                      ),
                    ),
                        */
