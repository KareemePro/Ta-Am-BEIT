
import 'package:flutter/material.dart';

import 'package:taambeit/features/meal/models/meal.dart';
import 'package:taambeit/features/meal/services/meal_api.dart';
import 'package:taambeit/widgets/custom_circular_loding.dart';
import 'package:taambeit/features/meal/views/widgets/meals_graid_view.dart';

class MealsGraidViewBuilder extends StatefulWidget {
  const MealsGraidViewBuilder({super.key});

  @override
  State<MealsGraidViewBuilder> createState() => _MealsGraidViewBuilderState();
}

class _MealsGraidViewBuilderState extends State<MealsGraidViewBuilder> {
  // ignore: prefer_typing_uninitialized_variables
  var future;
  @override
  void initState() {
    super.initState();
    future = MealApi().getAllMeal();
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<MealModel>>(
        future: future,
        builder: (context, snapshot) {
          if (snapshot.hasData) {
            return MealsGraidView(meals: snapshot.data!);
          } else if (snapshot.hasError) {
            return const Text('erorr');
          }else{
            return const SliverToBoxAdapter(
              child:CustomCircularLoding(),
            );
          }
        });
  }
}
