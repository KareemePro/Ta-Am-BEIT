import 'package:flutter/material.dart';

class CustomCircleAvatar extends StatelessWidget {
  final String chefImage;
  final double radius;

   const CustomCircleAvatar({super.key, required this.chefImage, required this.radius});
  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
          border: Border.all(color: Colors.white, width: 5),
          borderRadius: BorderRadius.circular(50)),
      child: CircleAvatar(
        radius: radius,
        backgroundImage: NetworkImage(chefImage),
        backgroundColor: Colors.white,
      ),
    );
  }
}
