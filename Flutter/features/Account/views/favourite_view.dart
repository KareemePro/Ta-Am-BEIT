import 'package:flutter/material.dart';
import 'package:taambeit/enums/view_type.dart';





import 'package:taambeit/widgets/custom_search.dart';
import 'package:taambeit/widgets/custom_text_title.dart';
import 'package:taambeit/widgets/custom_toggle_buttons.dart';

class FavouriteView extends StatefulWidget {
  static String id = 'favouriteView';
  const FavouriteView({super.key});

  @override
  State<FavouriteView> createState() => _FavouriteViewState();
}

class _FavouriteViewState extends State<FavouriteView> {
  final List<bool> _isSelected = [true, false];
  ViewType viewType = ViewType.list;


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        scrolledUnderElevation: 0,
        toolbarHeight: 50,
        leadingWidth: 40,
        title: const Text(
          'Favourites',
          style: TextStyle(fontSize: 20, fontWeight: FontWeight.w600),
        ),
        centerTitle: true,
        leading: InkWell(
          onTap: ()=> Navigator.maybePop(context),
          child: const Icon(
            Icons.arrow_back_ios_outlined,
            size: 20,
          ),
        ),
        backgroundColor: Colors.white,
      ),
      body: Container(
        padding: const EdgeInsets.symmetric(horizontal: 16,),
        child: Column(
          children: [
            const SizedBox(height: 10,),
            const CustomSearch(),
            const SizedBox(height: 10,),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                const CustomTextTitle(title: 'Favourites',fontWeight: FontWeight.w400,),
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
            /*  const SizedBox(height: 5,),
            SizedBox(
              height: 655,
              child: (viewType == ViewType.list)
                  ? ListView.builder(
                      itemCount: meal.length,
                      itemBuilder: (context, index) => CustomContainerMealList(
                          mealImage: meal[index].mealImage,
                          mealName: meal[index].mealName,
                          chefName: meal[index].chefName,
                          chefImage: meal[index].chefImage,
                          rate: meal[index].rate,
                          price: meal[index].price,
                          onTapMeal: () => Navigator.pushNamed(context,MealView.id),
                          onTap: () {
                            setState(() {
                              Navigator.pushNamed(context, ChefView.id);
                            });
                          },
                          onPressed: null))
                  : GridView.builder(
                      gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                          crossAxisCount: 2,
                          crossAxisSpacing: 10,
                          mainAxisSpacing: 15,
                          mainAxisExtent: 300
                        ),
                      itemCount: meal.length,
                      itemBuilder: (context, index) => CustomContainerMealGraid(
                          mealImage: meal[index].mealImage2,
                          mealName: meal[index].mealName,
                          chefName: meal[index].chefName,
                          chefImage: meal[index].chefImage,
                          rate: meal[index].rate,
                          price: meal[index].price,
                          onTap: null,
                          onPressed: null),
                    ),
            ),*/
          ],
        ),
      ),
    );
  }
}
/*Wrap(
                        alignment: WrapAlignment.spaceBetween,
                        runSpacing: 20,
                        children: [
                          CustomContainerMealGraid(
                              mealImage: meal[index].mealImage2,
                              mealName: meal[index].mealName,
                              chefName: meal[index].chefName,
                              chefImage: meal[index].chefImage,
                              rate: meal[index].rate,
                              price: meal[index].price,
                              onTap: null,
                              onPressed: null),
                        ],
                      ),*/