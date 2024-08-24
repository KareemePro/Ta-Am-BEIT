import 'package:flutter/material.dart';
import 'package:taambeit/features/chife/models/chife_meals_model.dart';
import 'package:taambeit/features/chife/services/chife_api.dart';
import 'package:taambeit/features/chife/views/chife_view.dart';
import 'package:taambeit/features/meal/models/meal.dart';
import 'package:taambeit/features/meal/views/meal_view.dart';

class CustomSmallContainerMeal extends StatelessWidget {
  final MealModel meal;
  const CustomSmallContainerMeal({
    super.key,
    required this.meal,
  });

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onDoubleTap: () => Navigator.pushNamed(
        context,
        MealView.id,
        arguments:meal
        
      ),
      child: Container(
        width: 190,
        height: 300,
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
                  width: 190,
                  height: 145,
                  clipBehavior: Clip.antiAlias,
                  decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(10),
                      image: DecorationImage(
                          image: NetworkImage(
                              meal.getMealOptionsRequest!.first.thumbnailImage),
                          fit: BoxFit.fill)),
                ),
                Positioned(
                    top: 110,
                    right: 8,
                    child: InkWell(
                      onTap: ()async{
                        ChifeMealsModel chife = await ChifeMealsApi().getChifeMeals(id: meal.chiefID);
                        
                         // ignore: use_build_context_synchronously
                         Navigator.pushNamed(context, ChefView.id,arguments:chife ,);
                      }, 
                      child: Container(
                        decoration: BoxDecoration(
                            border: Border.all(color: Colors.white, width: 5),
                            borderRadius: BorderRadius.circular(50)),
                        child: CircleAvatar(
                          radius: 27,
                          backgroundImage: NetworkImage(meal.chiefImage!),
                        ),
                      ),
                    ))
              ],
            ),
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: 10),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const SizedBox(
                    height: 27,
                  ),
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
                    height: 10,
                  ),
                  Text('${meal.rating}'),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text(
                        '100 EGP',
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.w600,
                        ),
                        textAlign: TextAlign.left,
                      ),
                      SizedBox(
                        width: 32,
                        child: MaterialButton(
                          onPressed: () {},
                          color: const Color.fromARGB(255, 42, 145, 21),
                          disabledColor: const Color.fromARGB(255, 42, 145, 21),
                          textColor: Colors.white,
                          shape: const RoundedRectangleBorder(
                            borderRadius: BorderRadius.all(
                              Radius.circular(10),
                            ),
                          ),
                          padding: const EdgeInsets.symmetric(horizontal: 8),
                          child: const Icon(
                            Icons.add,
                            size: 18,
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
