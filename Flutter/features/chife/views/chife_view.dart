import 'package:flutter/material.dart';
import 'package:taambeit/enums/view_type.dart';
import 'package:taambeit/features/chife/models/chife_meals_model.dart';
import 'package:taambeit/features/chife/services/chife_api.dart';
import 'package:taambeit/features/meal/views/widgets/custom_read_more_text.dart';
import 'package:taambeit/features/meal/views/widgets/meals_graid_view.dart';
import 'package:taambeit/features/meal/views/widgets/meals_list_view.dart';

import 'package:taambeit/widgets/custom_circle_avatar.dart';
import 'package:taambeit/widgets/custom_circular_loding.dart';
import 'package:taambeit/widgets/custom_icon_background.dart';

import 'package:taambeit/widgets/custom_text_title.dart';
import 'package:taambeit/widgets/custom_toggle_buttons.dart';

class ChefView extends StatefulWidget {
  static String id = 'chefView';
  final String chifeId;
  const ChefView({super.key, required this.chifeId});
  @override
  State<ChefView> createState() => _ChefViewState();
}

class _ChefViewState extends State<ChefView> {
  List<String> category = [
    'Most Popular',
    'Most recent',
    'Offers',
    'Main dish',
    'Side dish',
  ];
  final List<bool> _isSelected = [true, false];
  // ignore: prefer_typing_uninitialized_variables
  var future;
  @override
  void initState() {
    super.initState();
    future = ChifeMealsApi().getChifeMeals(id: widget.chifeId);
  }

  ViewType viewType = ViewType.list;
  String selectedItem = 'Most Popular';

  @override
  Widget build(BuildContext context) {
    //ChifeMealsModel chife =
    //  ModalRoute.of(context)?.settings.arguments as ChifeMealsModel;

    return Scaffold(
        backgroundColor: Colors.white,
        body: FutureBuilder<ChifeMealsModel>(
          future: future,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const Center(child: CustomCircularLoding());
            } else if (snapshot.hasData) {
              return buildChifeVeiw(chife: snapshot.data!);
            } else {
              return const Text('error');
            }
          },
        ));
  }

  Padding buildTextInSingleChildScrollView(
      String text, void Function()? onTap) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 10),
      child: InkWell(
        onTap: onTap,
        child: Container(
          decoration: BoxDecoration(
            border: Border(
              bottom: BorderSide(
                width: 3,
                color: selectedItem == text
                    ? const Color.fromARGB(255, 42, 145, 21)
                    : const Color.fromARGB(0, 255, 255, 255),
              ),
            ),
          ),
          height: 25,
          child: Text(
            text,
            style: TextStyle(
              fontSize: 13,
              fontWeight:
                  selectedItem == text ? FontWeight.w500 : FontWeight.w400,
              color: selectedItem == text
                  ? const Color.fromARGB(255, 42, 145, 21)
                  : Colors.black,
            ),
          ),
        ),
      ),
    );
  }

  Widget buildChifeVeiw({required ChifeMealsModel chife}) {
    return CustomScrollView(
      shrinkWrap: true,
      slivers: [
        SliverAppBar(
          leadingWidth: 45,
          leading: Container(
            margin: const EdgeInsets.only(left: 15),
            child: CustomIconBackground(
              icon: const Icon(
                Icons.keyboard_arrow_left,
              ),
              onTap: () {
                Navigator.maybePop(context);
              },
            ),
          ),
          backgroundColor: const Color.fromARGB(255, 42, 145, 21),
          pinned: true,
          expandedHeight: chife.description != null ? 475 : 400,
          bottom: PreferredSize(
            preferredSize: const Size.fromHeight(0),
            child: Container(
              height: 50,
              //color: Colors.white,
              decoration: const BoxDecoration(
                  color: Colors.white,
                  border: Border(
                      top: BorderSide(
                          color: Color.fromARGB(255, 244, 244, 244),
                          width: 6))),
              padding: const EdgeInsets.only(left: 16, bottom: 10, top: 10),
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceAround,
                  children: [
                    const Padding(
                      padding: EdgeInsets.only(right: 10),
                      child: Icon(Icons.menu),
                    ),
                    for (int i = 0; i < category.length; i++)
                      buildTextInSingleChildScrollView(
                        category[i],
                        () {
                          setState(() {
                            selectedItem = category[i];
                          });
                        },
                      ),
                  ],
                ),
              ),
            ),
          ),
          flexibleSpace: FlexibleSpaceBar(
            background: Container(
              //height: 512,
              color: Colors.white,
              child: Stack(
                children: [
                  SizedBox(
                    width: double.infinity,
                    height: 189,
                    child: FadeInImage.assetNetwork(
                      placeholder: 'images/loding.gif',
                      image: chife.coverImage!,
                      fit: BoxFit.fill,
                      placeholderFit: BoxFit.none,
                    ),
                  ),
                  Positioned(
                    top: 140,
                    left: 16,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        CustomCircleAvatar(
                          chefImage: chife.prfileImage!,
                          radius: 40,
                        ),
                        SizedBox(
                          width: 185,
                          height: 50,
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Row(
                                children: [
                                  Text(
                                    '${chife.chiefName!}  ',
                                    style: const TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.w700,
                                    ),
                                  ),
                                  if (chife.isOnline == true)
                                    const Text(
                                      "Online",
                                      style: TextStyle(
                                          fontSize: 14,
                                          fontWeight: FontWeight.w500,
                                          color:
                                              Color.fromARGB(255, 42, 145, 21)),
                                      textAlign: TextAlign.left,
                                    ),
                                ],
                              ),
                              const Text(
                                'Italian food, bakery goods',
                                style: TextStyle(
                                  fontSize: 12,
                                  fontWeight: FontWeight.w500,
                                  color: Color.fromARGB(255, 101, 101, 101),
                                ),
                              ),
                            ],
                          ),
                        ),
                        if (chife.description != null)
                          const SizedBox(
                            height: 10,
                          ),
                        SizedBox(
                          height: 170,
                          child: SingleChildScrollView(
                            //clipBehavior: Clip.none,
                            scrollDirection: Axis.vertical,
                            child: Column(
                              children: [
                                SizedBox(
                                  width: 359,
                                  child: CustomReadMoreText(
                                      text: chife.description ?? ''),
                                ),
                                if (chife.description != null)
                                  const SizedBox(
                                    height: 15,
                                  ),
                                SizedBox(
                                  width: 380,
                                  child: Row(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.center,
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceAround,
                                    children: [
                                      buildContainer(
                                        icon: const Icon(
                                            Icons.star_border_rounded),
                                        image: null,
                                        rate: '${chife.rating}',
                                        title: '${chife.reviewCount} Reviews',
                                        textDecoration:
                                            TextDecoration.underline,
                                      ),
                                      buildContainer(
                                        icon: null,
                                        image: "icons/iconMeal.png",
                                        rate: ' 6.1k+',
                                        title: 'Meals Prepared',
                                        textDecoration: null,
                                      ),
                                      buildContainer(
                                        icon: null,
                                        title: 'Food Safety',
                                        rate: ' certified',
                                        image: 'icons/safety.png',
                                      ),
                                    ],
                                  ),
                                ),
                                const SizedBox(
                                  height: 10,
                                )
                              ],
                            ),
                          ),
                        )
                      ],
                    ),
                  ),
                  if (chife.isOnline == true)
                    Positioned(
                      top: 205,
                      left: 80,
                      child: Container(
                        height: 16,
                        width: 16,
                        decoration: const BoxDecoration(
                            borderRadius: BorderRadius.all(Radius.circular(10)),
                            color: Color.fromARGB(255, 90, 189, 70)),
                      ),
                    ),
                ],
              ),
            ),
          ),
        ),
        SliverToBoxAdapter(
          child: Padding(
            padding: const EdgeInsets.only(left: 16, right: 16, bottom: 10),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const CustomTextTitle(
                  title: 'Most Popular',
                  fontWeight: FontWeight.w400,
                ),
                CustomToggleButtons(
                    isSelected: _isSelected,
                    onPressed: (index) {
                      setState(() {
                        if (index == 0) {
                          _isSelected[0] = true;
                          _isSelected[1] = false;
                          viewType = ViewType.list;
                        } else {
                          _isSelected[0] = false;
                          _isSelected[1] = true;
                          viewType = ViewType.grid;
                        }
                      });
                    })
              ],
            ),
          ),
        ),
        (viewType == ViewType.list)
            ? MealsListView(meals: chife.meals!)
            : MealsGraidView(meals: chife.meals!)
      ],
    );
  }

  Container buildContainer({
    required Icon? icon,
    required String? image,
    required String rate,
    required String title,
    TextDecoration? textDecoration,
  }) {
    return Container(
      width: 98,
      height: 72,
      decoration: BoxDecoration(
        color: const Color.fromARGB(255, 253, 253, 253),
        borderRadius: BorderRadius.circular(10),
        boxShadow: const [
          BoxShadow(blurRadius: 9, color: Color.fromARGB(40, 49, 172, 24))
        ],
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              (icon == null)
                  ? Image.asset(
                      image!,
                      color: Colors.black,
                    )
                  : icon,
              Text(
                rate,
                style: const TextStyle(
                  fontSize: 14,
                  fontWeight: FontWeight.w600,
                  color: Colors.black,
                ),
              ),
            ],
          ),
          Text(
            title,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w400,
              color: const Color.fromARGB(255, 101, 101, 101),
              decoration: textDecoration,
            ),
          ),
        ],
      ),
    );
  }
}
