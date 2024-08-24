import 'package:flutter/material.dart';
import 'package:taambeit/features/meal/models/meal.dart';
import 'package:taambeit/features/meal/views/widgets/custom_big_container_meal.dart';

class MealsListView extends StatelessWidget {
  final  List<MealModel> meals;

   const MealsListView({super.key, required this.meals});

  @override
  Widget build(BuildContext context) {
    return SliverList.builder(
      itemCount: meals.length,
      itemBuilder: (context, index) {
        return Padding(
            padding: const EdgeInsets.symmetric(horizontal: 8),
            child: CustomBigContainerMeal(meal: meals[index],));
      },
    );
  }
}
