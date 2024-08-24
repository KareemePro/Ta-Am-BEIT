import 'package:flutter/material.dart';
import 'package:taambeit/features/chife/views/chife_view.dart';
import 'package:taambeit/features/meal/models/meal.dart';

import 'package:taambeit/features/meal/views/meal_view.dart';
import 'package:taambeit/widgets/custom_circle_avatar.dart';

import 'package:taambeit/widgets/custom_icon_rating.dart';

class CustomBigContainerMeal extends StatelessWidget {
  final MealModel meal;
  const CustomBigContainerMeal({super.key, required this.meal});

  @override
  Widget build(BuildContext context) {
    return meal.getMealOptionsRequest!.isNotEmpty
        ? InkWell(
            onDoubleTap: () =>
                Navigator.pushNamed(context, MealView.id, arguments: meal),
            child: Padding(
              padding: const EdgeInsets.symmetric(
                horizontal: 6,
                vertical: 10,
              ),
              child: Container(
                height: 272,
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(10),
                  boxShadow: const [
                    BoxShadow(
                      color: Color.fromARGB(51, 49, 172, 24),
                      blurRadius: 9.6,
                    ),
                  ],
                ),
                child: Column(
                  children: [
                    Stack(
                      clipBehavior: Clip.none,
                      children: [
                        Container(
                          height: 156,
                          clipBehavior: Clip.antiAlias,
                          decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(10),
                          ),
                          child: FadeInImage.assetNetwork(
                            width: double.maxFinite,
                            placeholder: 'images/loding.gif',
                            image: meal
                                .getMealOptionsRequest!.first.thumbnailImage,
                            fit: BoxFit.fill,
                            placeholderFit: BoxFit.none,
                            placeholderScale: 1,
                          ),
                        ),
                        Positioned(
                          top: 115,
                          right: 15,
                          child: InkWell(
                            autofocus: true,
                            onTap: () async {
                              Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                      builder: (context) => ChefView(
                                            chifeId: meal.chiefID,
                                          )));
                            },
                            child: CustomCircleAvatar(chefImage: meal.chiefImage!,radius: 35,)
                          ),
                        )
                      ],
                    ),
                    Padding(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 18, vertical: 14),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            meal.title!,
                            style: const TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.w400,
                            ),
                          ),
                          Text(
                            meal.chiefName!,
                            style: const TextStyle(
                                fontSize: 14,
                                fontWeight: FontWeight.w400,
                                color: Color.fromARGB(255, 101, 101, 101)),
                          ),
                          const SizedBox(
                            height: 5,
                          ),
                          Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Row(
                                children: [
                                  const CustomIconRating(),
                                  Text('${meal.rating} (${meal.reviewCount})'),
                                ],
                              ),
                              Row(
                                children: [
                                  Padding(
                                    padding: const EdgeInsets.only(right: 14),
                                    child: Text(
                                      '${meal.getMealOptionsRequest!.first.price} EGP',
                                      style: const TextStyle(
                                        fontSize: 18,
                                        fontWeight: FontWeight.w600,
                                      ),
                                      textAlign: TextAlign.left,
                                    ),
                                  ),
                                  SizedBox(
                                    width: 81,
                                    height: 40,
                                    child: MaterialButton(
                                      onPressed: () {
                                        
                                      },
                                      //  focusColor:const Color.fromARGB(255, 42, 145, 21),
                                      color: const Color.fromARGB(
                                          255, 42, 145, 21),
                                      //  disabledColor: const Color.fromARGB(255, 42, 145, 21),
                                      //  disabledTextColor: Colors.white,
                                      textColor: Colors.white,
                                      shape: const RoundedRectangleBorder(
                                        borderRadius: BorderRadius.all(
                                          Radius.circular(10),
                                        ),
                                      ),
                                      //  padding: EdgeInsets.symmetric(horizontal: 8),
                                      child: const Row(
                                        children: [
                                          Icon(
                                            Icons.add,
                                            size: 16,
                                          ),
                                          Text(
                                            'Add',
                                            style: TextStyle(
                                              fontSize: 16,
                                              fontWeight: FontWeight.w600,
                                            ),
                                          )
                                        ],
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
          )
        : const SizedBox();
  }
}
