import 'package:flutter/material.dart';
import 'package:taambeit/features/meal/models/meal.dart';

import 'package:taambeit/features/meal/views/widgets/custom_small_container_meal.dart';

class MealsGraidView extends StatelessWidget {
  final List<MealModel> meals;

  const MealsGraidView({super.key, required this.meals});
  @override
  Widget build(BuildContext context) {
    return SliverGrid.builder(
        addAutomaticKeepAlives: true,
        gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
            crossAxisCount: 2,
            mainAxisExtent: 300,
            mainAxisSpacing: 10,
            crossAxisSpacing: 8),
        itemCount: meals.length,
        itemBuilder: (context, index){
          return CustomSmallContainerMeal(
            meal: meals[index],
          );
          
        });
  }
}
