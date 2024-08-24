
import 'package:flutter/material.dart';

import 'package:taambeit/features/meal/models/meal.dart';

import 'package:taambeit/features/meal/services/meal_api.dart';
import 'package:taambeit/widgets/custom_circular_loding.dart';
import 'package:taambeit/features/meal/views/widgets/meals_list_view.dart';

class MealsListViewBuilder extends StatefulWidget {
  final  String? tagFilter;
  final  String? sizeFilter;
  final  String? sortBy;
  final  String? mealSpiceLevel;
  final  String? mealCategory;
  final  String? mealStyle;
  final  String? chiefFilter;
  final  num? startPrice;
  final  num? endPrice;
  final  int? pageSize;
  final  int? pageNumber;
   const MealsListViewBuilder({super.key, this.tagFilter, this.sizeFilter, this.sortBy, this.mealSpiceLevel, this.mealCategory, this.mealStyle, this.chiefFilter, this.startPrice, this.endPrice, this.pageSize, this.pageNumber,});

  
  @override
  State<MealsListViewBuilder> createState() => _MealsListViewBuilder();
}

class _MealsListViewBuilder extends State<MealsListViewBuilder> {
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
          return MealsListView(meals: snapshot.data!);
        } else if (snapshot.hasError) {
          return const SliverToBoxAdapter(child: Text('erorr'));
        } else {
          return const SliverToBoxAdapter(
            child: CustomCircularLoding(),
          );
        }
      },
    );
  }
}
